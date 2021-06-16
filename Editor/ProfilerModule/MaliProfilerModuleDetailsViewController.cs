using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Profiling;
using UnityEngine.UIElements;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    public class MaliProfilerModuleDetailsViewController : ProfilerModuleViewController
    {
        const string k_UxmlResourcePath = "Packages/com.unity.profiling.systemmetrics.mali/Editor/ProfilerModule/MaliProfilerModuleDetailsView.uxml";
        const string k_UxmlIdentifier_CyclesPerPixel_Label = "cycles-per-pixel__label";
        const string k_UxmlIdentifier_ZKillPercent_Label = "z-kill__label";
        const string k_UxmlIdentifier_ShaderParallelism_Label = "shader-parallelism__label";

        string[] m_ValueCounters;
        System.Tuple<string, string>[] m_PercentValueCounters;

        List<ValueLabel> m_ValueLabels;
        List<PercentValueElement> m_PercentValueElements;
        Label m_CyclesPerPixelLabel;
        Label m_ZKillLabel;
        Label m_ShaderParallelismLabel;

        public MaliProfilerModuleDetailsViewController(ProfilerWindow profilerWindow, string[] valueCounters, System.Tuple<string, string>[] percentValueCounters) : base(profilerWindow)
        {
            m_ValueCounters = valueCounters;
            m_PercentValueCounters = percentValueCounters;
        }

        protected override VisualElement CreateView()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(k_UxmlResourcePath);
            var view = template.Instantiate();
            CollectElementsInView(view);

            ReloadData();
            SubscribeToEvents();

            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            UnsubscribeFromEvents();
            base.Dispose(disposing);
        }

        void CollectElementsInView(VisualElement view)
        {
            m_ValueLabels = view.Query<ValueLabel>().ToList();
            UnityEngine.Debug.Assert(m_ValueLabels.Count == m_ValueCounters.Length);

            m_PercentValueElements = view.Query<PercentValueElement>().ToList();
            UnityEngine.Debug.Assert(m_PercentValueElements.Count == m_PercentValueCounters.Length);

            m_CyclesPerPixelLabel = view.Q<Label>(k_UxmlIdentifier_CyclesPerPixel_Label);
            m_ZKillLabel = view.Q<Label>(k_UxmlIdentifier_ZKillPercent_Label);
            m_ShaderParallelismLabel = view.Q<Label>(k_UxmlIdentifier_ShaderParallelism_Label);
        }

        void SubscribeToEvents()
        {
            ProfilerWindow.SelectedFrameIndexChanged += OnSelectedFrameIndexChanged;
        }

        void UnsubscribeFromEvents()
        {
            ProfilerWindow.SelectedFrameIndexChanged -= OnSelectedFrameIndexChanged;
        }

        void OnSelectedFrameIndexChanged(long selectedFrameIndex)
        {
            ReloadData();
        }

        void ReloadData()
        {
            var selectedFrameIndexInt32 = System.Convert.ToInt32(ProfilerWindow.selectedFrameIndex);
            using (var frameDataView = UnityEditorInternal.ProfilerDriver.GetRawFrameDataView(selectedFrameIndexInt32, 0))
            {
                if (frameDataView != null && frameDataView.valid)
                {
                    // Raw value statistics.
                    for (int i = 0; i < m_ValueCounters.Length; ++i)
                    {
                        var counterName = m_ValueCounters[i];
                        var counterValue = GetCounterValueAsLong(frameDataView, counterName);
                        m_ValueLabels[i].SetValue(counterValue);
                    }

                    // Percentage value statistics.
                    for (int i = 0; i < m_PercentValueCounters.Length; ++i)
                    {
                        var counterNamePair = m_PercentValueCounters[i];
                        var counterPercentage = GetCounterValueLongAsPercentageOfTotal(frameDataView, counterNamePair.Item1, counterNamePair.Item2);
                        m_PercentValueElements[i].SetPercent(counterPercentage);
                    }

                    // Derived statistics.
                    var cyclesPerPixel = (double)GetCounterValueAsLong(frameDataView, "GPU Active Cycles") / GetCounterValueAsLong(frameDataView, "GPU Pixels");
                    m_CyclesPerPixelLabel.text = ProfilerCounterFormatter.FormatCount(cyclesPerPixel);

                    var zKillPercentage = 0f;
                    var zTestCount = GetCounterValueAsLong(frameDataView, "GPU Early Z Tests") + GetCounterValueAsLong(frameDataView, "GPU Late Z Tests");
                    var zTestKillsCount = GetCounterValueAsLong(frameDataView, "GPU Early Z Killed") + GetCounterValueAsLong(frameDataView, "GPU Late Z Killed");
                    if (zTestCount > 0)
                        zKillPercentage = zTestKillsCount * 100f / zTestCount;
                    m_ZKillLabel.text = ProfilerCounterFormatter.FormatPercentage(zKillPercentage);

                    var shaderParallelismPercentage = 0f;
                    var gpuInstructions = GetCounterValueAsLong(frameDataView, "GPU Instructions");
                    if (gpuInstructions > 0)
                    {
                        var gpuNonDivergedInstructions = gpuInstructions - GetCounterValueAsLong(frameDataView, "GPU Diverged Instructions");
                        shaderParallelismPercentage = gpuNonDivergedInstructions * 100f / gpuInstructions;
                    }
                    m_ShaderParallelismLabel.text = ProfilerCounterFormatter.FormatPercentage(shaderParallelismPercentage);
                }
            }
        }

        long GetCounterValueAsLong(RawFrameDataView frameDataView, string counterName)
        {
            var markerId = frameDataView.GetMarkerId(counterName);
            return frameDataView.GetCounterValueAsLong(markerId);
        }

        float GetCounterValueLongAsPercentageOfTotal(RawFrameDataView frameDataView, string valueCounterName, string totalCounterName)
        {
            var value = GetCounterValueAsLong(frameDataView, valueCounterName);
            var total = GetCounterValueAsLong(frameDataView, totalCounterName);
            if (total == 0)
                return 0;

            return value * 100f / total;
        }
    }
}