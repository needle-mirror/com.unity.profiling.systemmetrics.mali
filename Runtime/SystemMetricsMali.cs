using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.LowLevel;

namespace Unity.Profiling.SystemMetrics
{
    /// <summary>
    /// System Metrics Mali class provides access to all Profiler Counters related to the device performance. You can
    /// use exposed <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to create relevant <see cref="Unity.Profiling.ProfilerRecorder"/> to access
    /// collected performance data.
    /// </summary>
    public class SystemMetricsMali
    {
        /// <summary>
        /// Global SystemMetricsMali class instance to access hardware counters.
        /// </summary>
        public static SystemMetricsMali Instance { get; private set; }

        /// <summary>
        /// Returns true if System Metrics Mali was initialized successfully, false otherwise.
        /// This means that System Metrics Mali runs on a device that supports hardware counters.
        /// </summary>
        /// <value>True when System Metrics Mali is available, false otherwise.</value>
        public bool Active { get { return SystemMetricsLibHWCP.IsActive(); } }

        /// <summary>
        /// Returns `GPU Active Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuCycles { get; private set; }

        /// <summary>
        /// Returns `Vertex And Compute Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuVertexAndComputeCycles { get; private set; }

        /// <summary>
        /// Returns `Vertex And Compute Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuVertexAndComputeUtilization { get; private set; }

        /// <summary>
        /// Returns `Fragment Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuFragmentCycles { get; private set; }

        /// <summary>
        /// Returns `Fragment Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuFragmentUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Core Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderCoreCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Core Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderCoreUtilization { get; private set; }


        /// <summary>
        /// Returns `Input Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuInputPrimitives { get; private set; }

        /// <summary>
        /// Returns `Culled Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuCulledPrimitives { get; private set; }

        /// <summary>
        /// Returns `% Culled Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuCulledPrimitivesPercentage { get; private set; }

        /// <summary>
        /// Returns `Clipped Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuClippedPrimitives { get; private set; }

        /// <summary>
        /// Returns `% Clipped Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuClippedPrimitivesPercentage { get; private set; }

        /// <summary>
        /// Returns `Visible Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuVisiblePrimitives { get; private set; }

        /// <summary>
        /// Returns `% Visible Primitives` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuVisiblePrimitivesPercentage { get; private set; }


        /// <summary>
        /// Returns `Shader Fragment Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderComputeCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Fragment Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderComputeUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Vertex And Compute Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderFragmentCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Vertex And Compute Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderFragmentUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Arithmetic Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderArithmeticCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Arithmetic Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderArithmeticUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Texture Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderTextureCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Texture Unit Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderTextureUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Load/Store Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderLoadStoreCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Load/Store Utilization` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuShaderLoadStoreUtilization { get; private set; }


        /// <summary>
        /// Returns `Early ZS Tests` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuEarlyZTests { get; private set; }

        /// <summary>
        /// Returns `Early ZS Kills` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuEarlyZKills { get; private set; }

        /// <summary>
        /// Returns `Late ZS Tests` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuLateZTests { get; private set; }

        /// <summary>
        /// Returns `Late ZS Kills` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuLateZKills { get; private set; }


        /// <summary>
        /// Returns `Vertex And Compute Jobs` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuVertexComputeJobs { get; private set; }

        /// <summary>
        /// Returns `Fragment Jobs` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuFragmentJobs { get; private set; }

        /// <summary>
        /// Returns `Tiles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuTiles { get; private set; }

        /// <summary>
        /// Returns `Unchanged Eliminated Tiles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuUnchangedEliminatedTiles { get; private set; }

        /// <summary>
        /// Returns `Pixels` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuPixels { get; private set; }

        /// <summary>
        /// Returns `Instructions` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuInstructions { get; private set; }

        /// <summary>
        /// Returns `Diverged Instructions` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuDivergedInstructions { get; private set; }


        /// <summary>
        /// Returns `Cache Read Lookups` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuCacheReadLookups { get; private set; }

        /// <summary>
        /// Returns `Cache Write Lookups` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuCacheWriteLookups { get; private set; }

        /// <summary>
        /// Returns `Memory Read Accesses` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryReadAccesses { get; private set; }

        /// <summary>
        /// Returns `Memory Write Accesses` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryWriteAccesses { get; private set; }

        /// <summary>
        /// Returns `Memory Read Stalled Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryReadStalledCycles { get; private set; }

        /// <summary>
        /// Returns `% Memory Read Stalled` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryReadStallRate { get; private set; }

        /// <summary>
        /// Returns `Memory Write Stalled Cycles` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryWriteStalledCycles { get; private set; }

        /// <summary>
        /// Returns `% Memory Write Stalled` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryWriteStallRate { get; private set; }

        /// <summary>
        /// Returns `Memory Read Bytes` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryReadBytes { get; private set; }

        /// <summary>
        /// Returns `Memory Write Bytes` performance counter handler
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/>.</value>
        public ProfilerRecorderHandle GpuMemoryWriteBytes { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        static void InitializeSystemMetricsMali()
        {
            if (!SystemMetricsMali.Instance.Active)
                Debug.Log("SystemMetricsMali: Initialization failed");
        }

        static SystemMetricsMali()
        {
            Instance = new SystemMetricsMali();
        }

        SystemMetricsMali()
        {
            // Associate all properties with counter handles created in native plugin
            var classType = this.GetType();
            var properties = classType.GetProperties();
            foreach (var propInfo in properties)
            {
                if (propInfo.PropertyType != typeof(ProfilerRecorderHandle))
                    continue;

                propInfo.SetValue(this, SystemMetricsLibHWCP.GetProfilerRecorderHandle(propInfo.Name));
            }

            // Don't do anything else on non-target platforms
            if (!SystemMetricsLibHWCP.IsActive())
                return;

            if (!InstallUpdateCallback())
            {
                Debug.LogError("SystemMetricsMali: Failed to inject callback");
                return;
            }
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