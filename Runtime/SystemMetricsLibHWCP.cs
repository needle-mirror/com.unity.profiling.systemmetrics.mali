using System.Runtime.InteropServices;
using Unity.Profiling.LowLevel.Unsafe;

namespace Unity.Profiling.SystemMetrics
{
    internal static class SystemMetricsLibHWCP
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        [DllImport("AndroidHWCP")]
        public static extern bool IsActive();

        [DllImport("AndroidHWCP")]
        public static extern void Sample();

        [DllImport("AndroidHWCP")]
        public static extern ulong GetCounterHandle(string id);
#else
        public static bool IsActive()
        {
            return false;
        }

        public static void Sample()
        {
        }

        public static ulong GetCounterHandle(string id)
        {
            return 0;
        }
#endif

        public static unsafe ProfilerRecorderHandle GetProfilerRecorderHandle(string id)
        {
            var handle = GetCounterHandle(id);
            return *((ProfilerRecorderHandle*)&handle);
        }
    }
}