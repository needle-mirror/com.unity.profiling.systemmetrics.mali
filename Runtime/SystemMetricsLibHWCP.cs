using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

namespace Unity.Profiling.SystemMetrics
{
    internal static class SystemMetricsLibHWCP
    {
#if UNITY_ANDROID || UNITY_EDITOR
        [DllImport("AndroidHWCP")]
        public static extern bool IsActive();

        [DllImport("AndroidHWCP")]
        public static extern void Sample();

        [DllImport("AndroidHWCP")]
        public static extern ulong GetCounterHandle(string id);

        [DllImport("AndroidHWCP")]
        public static extern ushort GetProfilerCategoryId();

        [DllImport("AndroidHWCP")]
        public static extern int GetAvailableCounters(System.IntPtr names, int maxCount);

        [DllImport("AndroidHWCP")]
        public static extern int GetAvailableCounterHandles(System.IntPtr handles, int maxCount);
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

        public static ushort GetProfilerCategoryId()
        {
            return ProfilerUnsafeUtility.CategoryOther;
        }

        public static int GetAvailableCounters(System.IntPtr names, int maxCount)
        {
            return 0;
        }

        public static int GetAvailableCounterHandles(System.IntPtr handles, int maxCount)
        {
            return 0;
        }
#endif

        public static unsafe ProfilerRecorderHandle GetProfilerRecorderHandle(string id)
        {
            var handle = GetCounterHandle(id);
            if (handle == 0)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"SystemMetrics: Failed to get handle for '{id}' id, check that native plugin id match");
#endif
            }

            return *((ProfilerRecorderHandle*)&handle);
        }

        public static unsafe void GetAvailableCounterHandles(List<ProfilerRecorderHandle> handlesList)
        {
            handlesList.Clear();

            int maxCounters = GetAvailableCounterHandles(System.IntPtr.Zero, 0);
            if (maxCounters == 0)
                return;

            var handles = stackalloc ulong[maxCounters];
            int count = GetAvailableCounterHandles((System.IntPtr)handles, maxCounters);

            for (int i = 0; i < count; i++)
            {
                handlesList.Add(*((ProfilerRecorderHandle*)&handles[i]));
            }
        }
    }
}