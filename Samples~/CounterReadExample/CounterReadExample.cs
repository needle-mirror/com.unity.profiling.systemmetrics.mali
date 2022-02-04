using UnityEngine;

namespace Unity.Profiling.SystemMetrics
{
    /// <summary>
    /// Simple example how System Metrics Mali counter can be read in run-time using <see cref="Unity.Profiling.ProfilerRecorder"/>
    /// </summary>
    public class CounterReadExample : MonoBehaviour
    {
        private ProfilerRecorder m_GpuCyclesRecorder;

        private GUIStyle m_Style;

        private void OnEnable()
        {
            // Create and activate recorder
            m_GpuCyclesRecorder = new ProfilerRecorder(SystemMetricsMali.Instance.GpuCycles);
            if (m_GpuCyclesRecorder.Valid)
                m_GpuCyclesRecorder.Start();

            // UI Style for data visualization
            m_Style = new GUIStyle();
            m_Style.fontSize = 15;
            m_Style.normal.textColor = Color.white;
        }

        private void OnDisable()
        {
            // Deactivate and destroy
            if (m_GpuCyclesRecorder.Valid)
                m_GpuCyclesRecorder.Stop();
            m_GpuCyclesRecorder.Dispose();
        }

        void OnGUI()
        {
            string reportMsg = $"GpuCycles {m_GpuCyclesRecorder.LastValue} / {m_GpuCyclesRecorder.IsRunning}";

            GUI.color = new Color(1, 1, 1, 1);
            float offset = 50;
            float w = 300, h = 500;
            offset += h + 50;

            GUILayout.BeginArea(new Rect(32, 50, w, h), "GPU Counter", GUI.skin.window);
            GUILayout.Label(reportMsg, m_Style);
            GUILayout.EndArea();
        }
    }
}