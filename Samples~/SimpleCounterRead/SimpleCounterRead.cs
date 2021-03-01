using UnityEngine;
using Unity.Profiling;
using Unity.Profiling.SystemMetrics;

public class SimpleCounterRead : MonoBehaviour
{
    private ProfilerRecorder m_GpuActiveRecorder;

    private GUIStyle m_Style;

    private void OnEnable()
    {
        // Create and activate recorder
        m_GpuActiveRecorder = new ProfilerRecorder(SystemMetricsMali.Instance.GpuActive);
        if (m_GpuActiveRecorder.Valid)
            m_GpuActiveRecorder.Start();

        // UI Style for data visualization
        m_Style = new GUIStyle();
        m_Style.fontSize = 15;
        m_Style.normal.textColor = Color.white;
    }

    private void OnDisable()
    {
        // Deactivate and destroy
        if (m_GpuActiveRecorder.Valid)
            m_GpuActiveRecorder.Stop();
        m_GpuActiveRecorder.Dispose();
    }

    void OnGUI()
    {
        string reportMsg = $"GpuActive {m_GpuActiveRecorder.LastValue} / {m_GpuActiveRecorder.IsRunning}";

        GUI.color = new Color(1, 1, 1, 1);
        float offset = 50;
        float w = 300, h = 500;
        offset += h + 50;

        GUILayout.BeginArea(new Rect(32, 50, w, h), "GPU Counter", GUI.skin.window);
        GUILayout.Label(reportMsg, m_Style);
        GUILayout.EndArea();
    }
}