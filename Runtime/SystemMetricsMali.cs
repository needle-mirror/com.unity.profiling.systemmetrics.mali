using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.LowLevel;

namespace Unity.Profiling.SystemMetrics
{
    public class SystemMetricsMali
    {
        public static SystemMetricsMali Instance { get; private set; }

        public bool Active { get { return SystemMetricsLibHWCP.IsActive(); } }

        public ProfilerRecorderHandle GpuActive { get; private set; }
        public ProfilerRecorderHandle GpuNonFragmentActive { get; private set; }
        public ProfilerRecorderHandle GpuFragmentActive { get; private set; }
        public ProfilerRecorderHandle GpuTilerActive { get; private set; }

        public ProfilerRecorderHandle GpuVertexComputeJobs { get; private set; }
        public ProfilerRecorderHandle GpuTiles { get; private set; }
        public ProfilerRecorderHandle GpuTransactionEliminations { get; private set; }
        public ProfilerRecorderHandle GpuFragmentJobs { get; private set; }
        public ProfilerRecorderHandle GpuPixels { get; private set; }

        public ProfilerRecorderHandle GpuEarlyZTests { get; private set; }
        public ProfilerRecorderHandle GpuEarlyZKilled { get; private set; }
        public ProfilerRecorderHandle GpuLateZTests { get; private set; }
        public ProfilerRecorderHandle GpuLateZKilled { get; private set; }

        public ProfilerRecorderHandle GpuInstructions { get; private set; }
        public ProfilerRecorderHandle GpuDivergedInstructions { get; private set; }

        public ProfilerRecorderHandle GpuShaderCycles { get; private set; }
        public ProfilerRecorderHandle GpuShaderArithmeticCycles { get; private set; }
        public ProfilerRecorderHandle GpuShaderLoadStoreCycles { get; private set; }
        public ProfilerRecorderHandle GpuShaderTextureCycles { get; private set; }

        public ProfilerRecorderHandle GpuCacheReadLookups { get; private set; }
        public ProfilerRecorderHandle GpuCacheWriteLookups { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryReadAccesses { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryWriteAccesses { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryReadStalls { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryWriteStalls { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryReadBytes { get; private set; }
        public ProfilerRecorderHandle GpuExternalMemoryWriteBytes { get; private set; }

        static SystemMetricsMali()
        {
            Instance = new SystemMetricsMali();
        }

        SystemMetricsMali()
        {
            if (!SystemMetricsLibHWCP.IsActive())
                return;

            if (!InstallUpdateCallback())
            {
                Debug.LogError("SystemMetricsMali: Failed to inject callback");
                return;
            }

            // Set properties only if initialization is successeful
            GpuActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuActive));
            GpuNonFragmentActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuNonFragmentActive));
            GpuFragmentActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuFragmentActive));
            GpuTilerActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuTilerActive));

            GpuVertexComputeJobs = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuVertexComputeJobs));
            GpuTiles = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuTiles));
            GpuTransactionEliminations = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuTransactionEliminations));
            GpuFragmentJobs = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuFragmentJobs));
            GpuPixels = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuPixels));

            GpuEarlyZTests = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuEarlyZTests));
            GpuEarlyZKilled = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuEarlyZKilled));
            GpuLateZTests = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuLateZTests));
            GpuLateZKilled = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuLateZKilled));

            GpuInstructions = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuInstructions));
            GpuDivergedInstructions = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuDivergedInstructions));

            GpuShaderCycles = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderCycles));
            GpuShaderArithmeticCycles = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderArithmeticCycles));
            GpuShaderLoadStoreCycles = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderLoadStoreCycles));
            GpuShaderTextureCycles = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderTextureCycles));

            GpuCacheReadLookups = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCacheReadLookups));
            GpuCacheWriteLookups = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCacheWriteLookups));
            GpuExternalMemoryReadAccesses = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryReadAccesses));
            GpuExternalMemoryWriteAccesses = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryWriteAccesses));
            GpuExternalMemoryReadStalls = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryReadStalls));
            GpuExternalMemoryWriteStalls = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryWriteStalls));
            GpuExternalMemoryReadBytes = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryReadBytes));
            GpuExternalMemoryWriteBytes = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuExternalMemoryWriteBytes));
        }

        static bool InstallUpdateCallback()
        {
            var root = PlayerLoop.GetCurrentPlayerLoop();
            var injectionPoint = typeof(UnityEngine.PlayerLoop.PreLateUpdate);
            var res = InjectPlayerLoopCallback(ref root, injectionPoint, SystemMetricsLibHWCP.Sample);
            PlayerLoop.SetPlayerLoop(root);
            return res;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate uint PlayerLoopDelegate();

        static bool InjectPlayerLoopCallback(ref PlayerLoopSystem system, System.Type injectedSystem, PlayerLoopSystem.UpdateFunction injectedFnc)
        {
            // Have we found the system we're looking for?
            if (system.type == injectedSystem)
            {
                // If system has updateFunction set, updateDelegate won't be called
                // We wrap native update function in something we can call in c#
                // Reset updateFunction and call wrapped updateFunction in our delegate
                PlayerLoopDelegate systemDelegate = null;
                if (system.updateFunction.ToInt64() != 0)
                {
                    var intPtr = Marshal.ReadIntPtr(system.updateFunction);
                    if (intPtr.ToInt64() != 0)
                        systemDelegate = (PlayerLoopDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PlayerLoopDelegate));
                }

                // Install the new delegate and keep the system function call
                system.updateDelegate = () => { injectedFnc(); if (systemDelegate != null) systemDelegate(); };
                system.updateFunction = new System.IntPtr(0);

                return true;
            }

            if (system.subSystemList == null)
                return false;

            // Iterate all subsystems
            for (int i = 0; i < system.subSystemList.Length; ++i)
            {
                if (InjectPlayerLoopCallback(ref system.subSystemList[i], injectedSystem, injectedFnc))
                    return true;
            }

            return false;
        }
    }
}