namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    [System.Serializable]
    [ProfilerModuleMetadata("Mali System Metrics")]
    public class MaliProfilerModule : ProfilerModule
    {
        // TODO Use final Mali counter category.
        static readonly ProfilerCategory k_MaliProfilerCategory = ProfilerCategory.Scripts;

        static readonly ProfilerCounterDescriptor[] k_ChartCounters = new ProfilerCounterDescriptor[]
        {
            new ProfilerCounterDescriptor("GPU Active Cycles", k_MaliProfilerCategory),
            new ProfilerCounterDescriptor("GPU Vertex And Compute Active Cycles", k_MaliProfilerCategory),
            new ProfilerCounterDescriptor("GPU Fragment Active Cycles", k_MaliProfilerCategory),
            new ProfilerCounterDescriptor("GPU Tiler Active Cycles", k_MaliProfilerCategory),
        };

        static readonly string[] k_DetailValueCounterNames = new string[]
        {
            "GPU Active Cycles",
            "GPU Vertex And Compute Active Cycles",
            "GPU Fragment Active Cycles",
            "GPU Tiler Active Cycles",

            "GPU Shader Cycles",
            "GPU Shader Arithmetic Cycles",
            "GPU Shader Load Store Cycles",
            "GPU Shader Texture Cycles",

            "GPU Tiles",
            "GPU Transaction Eliminations",
            "GPU Pixels",

            "GPU Cache Read Lookups",
            "GPU External Memory Read Accesses",
            "GPU External Memory Read Stalls",
            "GPU External Memory Read Bytes",

            "GPU Cache Write Lookups",
            "GPU External Memory Write Accesses",
            "GPU External Memory Write Stalls",
            "GPU External Memory Write Bytes",

            "GPU Early Z Tests",
            "GPU Early Z Killed",
            "GPU Late Z Tests",
            "GPU Late Z Killed",

            "GPU Instructions",
            "GPU Diverged Instructions",
            "GPU Vertex And Compute Jobs",
            "GPU Fragment Jobs",
        };

        static readonly System.Tuple<string, string>[] k_DetailPercentValueCounterNames = new System.Tuple<string, string>[]
        {
            new System.Tuple<string, string>(k_DetailValueCounterNames[1], k_DetailValueCounterNames[0]),
            new System.Tuple<string, string>(k_DetailValueCounterNames[2], k_DetailValueCounterNames[0]),
            new System.Tuple<string, string>(k_DetailValueCounterNames[3], k_DetailValueCounterNames[0]),

            new System.Tuple<string, string>(k_DetailValueCounterNames[5], k_DetailValueCounterNames[4]),
            new System.Tuple<string, string>(k_DetailValueCounterNames[6], k_DetailValueCounterNames[4]),
            new System.Tuple<string, string>(k_DetailValueCounterNames[7], k_DetailValueCounterNames[4]),
        };

        public MaliProfilerModule() : base(k_ChartCounters) { }

        public override ProfilerModuleViewController CreateDetailsViewController()
        {
            return new MaliProfilerModuleDetailsViewController(ProfilerWindow, k_DetailValueCounterNames, k_DetailPercentValueCounterNames);
        }
    }
}