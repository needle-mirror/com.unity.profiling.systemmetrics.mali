using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using Unity.Profiling.SystemMetrics;
using UnityEditor;
using UnityEditor.Profiling;
using UnityEngine.UIElements;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    internal class MaliProfilerModuleDetailsViewController : ProfilerModuleViewController
    {
        const string k_NotSupported = "Not Supported";

        const string k_UxmlResourcePath = "Packages/com.unity.profiling.systemmetrics.mali/Editor/ProfilerModule/MaliProfilerModuleDetailsView.uxml";
        const string k_UxmlIdentifier_ZKillPercent_Label = "z-kill__label";
        const string k_UxmlIdentifier_ShaderParallelism_Label = "shader-parallelism__label";

        // Each value counter in m_ValueCounters has a corresponding value-label and value-name-label for display.
        ProfilerRecorderHandle[] m_ValueCounters;
        List<ValueLabel> m_ValueLabels;
        List<Label> m_ValueNameLabels;

        // Each percent-value counter in m_PercentValueCounters has a corresponding percent-value-element for display.
        ProfilerRecorderHandle[] m_PercentValueCounters;
        List<PercentValueElement> m_PercentValueElements;

        // Derived (non-backed) statistics are not backed by a single counter and must be computed individually for display.
        Label m_ZKillLabel;
        Label m_ShaderParallelismLabel;

        public MaliProfilerModuleDetailsViewController(ProfilerWindow profilerWindow, ProfilerRecorderHandle[] valueCounters, ProfilerRecorderHandle[] percentValueCounters) : base(profilerWindow)
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

            m_ValueNameLabels = view.Query<Label>(className: "counter-name-label").ToList();
            UnityEngine.Debug.Assert(m_ValueNameLabels.Count == m_ValueCounters.Length, $"Expected {m_ValueCounters.Length} labels, but got {m_ValueNameLabels.Count}");

            m_PercentValueElements = view.Query<PercentValueElement>().ToList();
            UnityEngine.Debug.Assert(m_PercentValueElements.Count == m_PercentValueCounters.Length);

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
                var hasValidFrameDataView = (frameDataView != null && frameDataView.valid);

                // Raw value statistics.
                for (int i = 0; i < m_ValueCounters.Length; ++i)
                {
                    var counterId = m_ValueCounters[i];
                    var counterDescription = ProfilerRecorderHandle.GetDescription(counterId);
                    m_ValueNameLabels[i].text = counterDescription.Name;

                    var counterValue = 0L;
                    if (hasValidFrameDataView)
                        counterValue = GetCounterValueAsLong(frameDataView, counterId);

                    m_ValueLabels[i].SetValue(counterValue);
                }

                // Percentage value statistics.
                for (int i = 0; i < m_PercentValueCounters.Length; ++i)
                {
                    var counterId = m_PercentValueCounters[i];
                    var counterPercentage = 0f;
                    if (hasValidFrameDataView)
                        counterPercentage = GetCounterValueAsLong(frameDataView, counterId);
                    m_PercentValueElements[i].SetPercent(counterPercentage);
                }

                // Derived/non-backed statistics.
                var zKillPercentage = 0f;
                if (hasValidFrameDataView)
                {
                    var zTestCount = GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuEarlyZTests) +
                        GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuLateZTests);
                    var zTestKillsCount = GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuEarlyZKills) +
                        GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuLateZKills);
                    if (zTestCount > 0)
                        zKillPercentage = zTestKillsCount * 100f / zTestCount;
                }
                m_ZKillLabel.text = ProfilerCounterFormatter.FormatPercentage(zKillPercentage);

                var shaderParallelismPercentage = 0f;
                if (hasValidFrameDataView)
                {
                    var gpuInstructions = GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuInstructions);
                    if (gpuInstructions > 0)
                    {
                        var gpuNonDivergedInstructions = gpuInstructions - GetCounterValueAsLong(frameDataView, SystemMetricsMali.Instance.GpuDivergedInstructions);
                        shaderParallelismPercentage = gpuNonDivergedInstructions * 100f / gpuInstructions;
                    }
                }
                m_ShaderParallelismLabel.text = ProfilerCounterFormatter.FormatPercentage(shaderParallelismPercentage);
            }
        }

        long GetCounterValueAsLong(RawFrameDataView frameDataView, ProfilerRecorderHandle counterId)
        {
            var description = ProfilerRecorderHandle.GetDescription(counterId);
            var markerId = frameDataView.GetMarkerId(description.Name);
            return frameDataView.GetCounterValueAsLong(markerId);
        }

        float GetCounterValueLongAsPercentageOfTotal(RawFrameDataView frameDataView, ProfilerRecorderHandle valueCounterId, ProfilerRecorderHandle totalCounterId)
        {
            var value = GetCounterValueAsLong(frameDataView, valueCounterId);
            var total = GetCounterValueAsLong(frameDataView, totalCounterId);
            if (total == 0)
                return 0;

            return value * 100f / total;
        }
    }
}