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
            GetDescriptorProfilerCounterHandle(SystemMetricsMali.Instance.GpuShaderCoreCycles),
        };

        static private ProfilerRecorderHandle[] DetailValueCounterNames = new ProfilerRecorderHandle[]
        {
            SystemMetricsMali.Instance.GpuCycles,
            SystemMetricsMali.Instance.GpuVertexAndComputeCycles,
            SystemMetricsMali.Instance.GpuFragmentCycles,

            SystemMetricsMali.Instance.GpuShaderCoreCycles,
            SystemMetricsMali.Instance.GpuShaderArithmeticCycles,
            SystemMetricsMali.Instance.GpuShaderLoadStoreCycles,
            SystemMetricsMali.Instance.GpuShaderTextureCycles,

            SystemMetricsMali.Instance.GpuTiles,
            SystemMetricsMali.Instance.GpuUnchangedEliminatedTiles,
            SystemMetricsMali.Instance.GpuPixels,

            SystemMetricsMali.Instance.GpuCacheReadLookups,
            SystemMetricsMali.Instance.GpuMemoryReadAccesses,
            SystemMetricsMali.Instance.GpuMemoryReadStalledCycles,
            SystemMetricsMali.Instance.GpuMemoryReadBytes,

            SystemMetricsMali.Instance.GpuCacheWriteLookups,
            SystemMetricsMali.Instance.GpuMemoryWriteAccesses,
            SystemMetricsMali.Instance.GpuMemoryWriteStalledCycles,
            SystemMetricsMali.Instance.GpuMemoryWriteBytes,

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

        static private System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>[] DetailPercentValueCounterNames = new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>[]
        {
            new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>(DetailValueCounterNames[1], DetailValueCounterNames[0]),
            new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>(DetailValueCounterNames[2], DetailValueCounterNames[0]),

            new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>(DetailValueCounterNames[4], DetailValueCounterNames[3]),
            new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>(DetailValueCounterNames[5], DetailValueCounterNames[3]),
            new System.Tuple<ProfilerRecorderHandle, ProfilerRecorderHandle>(DetailValueCounterNames[6], DetailValueCounterNames[3]),
        };

        public MaliProfilerModule() : base(ChartCounters) { }

        public override ProfilerModuleViewController CreateDetailsViewController()
        {
            return new MaliProfilerModuleDetailsViewController(ProfilerWindow, DetailValueCounterNames, DetailPercentValueCounterNames);
        }
    }
}