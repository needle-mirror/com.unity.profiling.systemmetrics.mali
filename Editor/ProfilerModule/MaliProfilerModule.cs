using Unity.Profiling.LowLevel.Unsafe;
using Unity.Profiling.SystemMetrics;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    [System.Serializable]
    [ProfilerModuleMetadata("Mali System Metrics")]
    internal class MaliProfilerModule : ProfilerModule
    {
        static private ProfilerCounterDescriptor[] ChartCounters = new ProfilerCounterDescriptor[]
        {
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuCycles),
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuVertexAndComputeCycles),
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuFragmentCycles),
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuVertexQueueActiveCycles),
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuComputeQueueActiveCycles),
        };

        static private ProfilerRecorderHandle[] DetailValueCounterNames = new ProfilerRecorderHandle[]
        {
            SystemMetricsMali.Instance.GpuCycles,
            SystemMetricsMali.Instance.GpuFragmentCycles,
            SystemMetricsMali.Instance.GpuVertexAndComputeCycles,
            SystemMetricsMali.Instance.GpuVertexQueueActiveCycles,
            SystemMetricsMali.Instance.GpuComputeQueueActiveCycles,
            SystemMetricsMali.Instance.GpuTilerCycles,

            SystemMetricsMali.Instance.GpuShaderCoreCycles,
            SystemMetricsMali.Instance.GpuShaderArithmeticCycles,
            SystemMetricsMali.Instance.GpuShaderLoadStoreCycles,
            SystemMetricsMali.Instance.GpuShaderTextureCycles,
            SystemMetricsMali.Instance.GpuFragmentFPKActiveCycles,

            SystemMetricsMali.Instance.GpuTiles,
            SystemMetricsMali.Instance.GpuUnchangedEliminatedTiles,
            SystemMetricsMali.Instance.GpuPixels,
            SystemMetricsMali.Instance.GpuCyclesPerPixels,

            SystemMetricsMali.Instance.GpuCacheReadLookups,
            SystemMetricsMali.Instance.GpuMemoryReadAccesses,
            SystemMetricsMali.Instance.GpuMemoryReadBytes,
            SystemMetricsMali.Instance.GpuMemoryReadStalledCycles,

            SystemMetricsMali.Instance.GpuCacheWriteLookups,
            SystemMetricsMali.Instance.GpuMemoryWriteAccesses,
            SystemMetricsMali.Instance.GpuMemoryWriteBytes,
            SystemMetricsMali.Instance.GpuMemoryWriteStalledCycles,

            SystemMetricsMali.Instance.GpuInputPrimitives,
            SystemMetricsMali.Instance.GpuVisiblePrimitives,

            SystemMetricsMali.Instance.GpuEarlyZTests,
            SystemMetricsMali.Instance.GpuEarlyZKills,
            SystemMetricsMali.Instance.GpuLateZTests,
            SystemMetricsMali.Instance.GpuLateZKills,

            SystemMetricsMali.Instance.GpuInstructions,
            SystemMetricsMali.Instance.GpuDivergedInstructions,
            SystemMetricsMali.Instance.GpuVertexComputeJobs,
            SystemMetricsMali.Instance.GpuFragmentJobs,
        };

        static private ProfilerCounterDescriptor GetDescriptorProfilerCounterHandle(ProfilerRecorderHandle handle)
        {
            var description = ProfilerRecorderHandle.GetDescription(handle);
            return new ProfilerCounterDescriptor(description.Name, description.Category);
        }

        static private ProfilerRecorderHandle[] DetailPercentValueCounterNames = new ProfilerRecorderHandle[]
        {
            SystemMetricsMali.Instance.GpuFragmentUtilization,
            SystemMetricsMali.Instance.GpuVertexAndComputeUtilization,
            SystemMetricsMali.Instance.GpuVertexQueueUtilization,
            SystemMetricsMali.Instance.GpuComputeQueueUtilization,
            SystemMetricsMali.Instance.GpuTilerUtilization,

            SystemMetricsMali.Instance.GpuShaderCoreUtilization,
            SystemMetricsMali.Instance.GpuShaderArithmeticUtilization,
            SystemMetricsMali.Instance.GpuShaderLoadStoreUtilization,
            SystemMetricsMali.Instance.GpuShaderTextureUtilization,

            SystemMetricsMali.Instance.GpuMemoryReadStallRate,
            SystemMetricsMali.Instance.GpuMemoryWriteStallRate,

            SystemMetricsMali.Instance.GpuVisiblePrimitivesPercentage,
        };

        public MaliProfilerModule() : base(ChartCounters) { }

        public override ProfilerModuleViewController CreateDetailsViewController()
        {
            return new MaliProfilerModuleDetailsViewController(ProfilerWindow, DetailValueCounterNames, DetailPercentValueCounterNames);
        }
    }
}