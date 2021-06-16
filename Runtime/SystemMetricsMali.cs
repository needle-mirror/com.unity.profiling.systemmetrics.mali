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
        public ProfilerRecorderHandle GpuShaderCoreActive { get; private set; }
        public ProfilerRecorderHandle GpuNonFragmentActive { get; private set; }
        public ProfilerRecorderHandle GpuFragmentActive { get; private set; }
        public ProfilerRecorderHandle GpuTilerActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderCoreUtilization { get; private set; }
        public ProfilerRecorderHandle GpuNonFragmentUtilization { get; private set; }
        public ProfilerRecorderHandle GpuFragmentUtilization { get; private set; }

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

        public ProfilerRecorderHandle GpuShaderArithmeticActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderLoadStoreActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderTextureUnitActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderFragmentActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderComputeActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderTripipeActive { get; private set; }
        public ProfilerRecorderHandle GpuShaderFragmentUtilization { get; private set; }
        public ProfilerRecorderHandle GpuShaderComputeUtilization { get; private set; }
        public ProfilerRecorderHandle GpuShaderTripipeUtilization { get; private set; }
        public ProfilerRecorderHandle GpuShaderArithmeticUtilization { get; private set; }
        public ProfilerRecorderHandle GpuShaderLoadStoreUtilization { get; private set; }
        public ProfilerRecorderHandle GpuShaderTextureUtilization { get; private set; }

        public ProfilerRecorderHandle GpuCacheReadLookups { get; private set; }
        public ProfilerRecorderHandle GpuCacheWriteLookups { get; private set; }
        public ProfilerRecorderHandle GpuMemoryReadAccesses { get; private set; }
        public ProfilerRecorderHandle GpuMemoryWriteAccesses { get; private set; }
        public ProfilerRecorderHandle GpuMemoryReadStalls { get; private set; }
        public ProfilerRecorderHandle GpuMemoryWriteStalls { get; private set; }
        public ProfilerRecorderHandle GpuMemoryReadBytes { get; private set; }
        public ProfilerRecorderHandle GpuMemoryWriteBytes { get; private set; }
        public ProfilerRecorderHandle GpuReadStallRate { get; private set; }
        public ProfilerRecorderHandle GpuWriteStallRate { get; private set; }

        public ProfilerRecorderHandle GpuInputPrimitives { get; private set; }
        public ProfilerRecorderHandle GpuCulledPrimitives { get; private set; }
        public ProfilerRecorderHandle GpuClippedPrimitives { get; private set; }
        public ProfilerRecorderHandle GpuVisiblePrimitives { get; private set; }
        public ProfilerRecorderHandle GpuCulledPrimitivesPercentage { get; private set; }
        public ProfilerRecorderHandle GpuClippedPrimitivesPercentage { get; private set; }
        public ProfilerRecorderHandle GpuVisiblePrimitivesPercentage { get; private set; }

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
            GpuShaderCoreActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderCoreActive));
            GpuNonFragmentActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuNonFragmentActive));
            GpuFragmentActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuFragmentActive));
            GpuTilerActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuTilerActive));
            GpuShaderCoreUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderCoreUtilization));
            GpuNonFragmentUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuNonFragmentUtilization));
            GpuFragmentUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuFragmentUtilization));

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

            GpuShaderArithmeticActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderArithmeticActive));
            GpuShaderLoadStoreActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderLoadStoreActive));
            GpuShaderTextureUnitActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderTextureUnitActive));
            GpuShaderFragmentActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderFragmentActive));
            GpuShaderComputeActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderComputeActive));
            GpuShaderTripipeActive = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderTripipeActive));
            GpuShaderFragmentUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderFragmentUtilization));
            GpuShaderComputeUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderComputeUtilization));
            GpuShaderTripipeUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderTripipeUtilization));
            GpuShaderArithmeticUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderArithmeticUtilization));
            GpuShaderLoadStoreUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderLoadStoreUtilization));
            GpuShaderTextureUtilization = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuShaderTextureUtilization));

            GpuCacheReadLookups = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCacheReadLookups));
            GpuCacheWriteLookups = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCacheWriteLookups));
            GpuMemoryReadAccesses = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryReadAccesses));
            GpuMemoryWriteAccesses = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryWriteAccesses));
            GpuMemoryReadStalls = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryReadStalls));
            GpuMemoryWriteStalls = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryWriteStalls));
            GpuMemoryReadBytes = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryReadBytes));
            GpuMemoryWriteBytes = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuMemoryWriteBytes));
            GpuReadStallRate = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuReadStallRate));
            GpuWriteStallRate = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuWriteStallRate));

            GpuInputPrimitives = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuInputPrimitives));
            GpuCulledPrimitives = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCulledPrimitives));
            GpuClippedPrimitives = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuClippedPrimitives));
            GpuVisiblePrimitives = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuVisiblePrimitives));
            GpuCulledPrimitivesPercentage = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuCulledPrimitivesPercentage));
            GpuClippedPrimitivesPercentage = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuClippedPrimitivesPercentage));
            GpuVisiblePrimitivesPercentage = SystemMetricsLibHWCP.GetProfilerRecorderHandle(nameof(GpuVisiblePrimitivesPercentage));
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