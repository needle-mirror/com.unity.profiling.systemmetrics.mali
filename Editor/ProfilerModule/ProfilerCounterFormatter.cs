namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    internal static class ProfilerCounterFormatter
    {
        public static string FormatValue(in long data, ProfilerMarkerDataUnit unit)
        {
            switch (unit)
            {
                case ProfilerMarkerDataUnit.Count:
                {
                    return FormatCount(data);
                }

                case ProfilerMarkerDataUnit.Bytes:
                {
                    return UnityEditor.EditorUtility.FormatBytes(data);
                }

                default:
                {
                    return FormatCount(data);
                }
            }
        }

        public static string FormatCount(in long count)
        {
            if (count < 1000)
                return string.Format("{0:D}", count);
            else if (count < 1000000) // 1e6
                return string.Format("{0:F2}k", count * 1.0e-3);
            else
                return string.Format("{0:F2}M", count * 1.0e-6);
        }

        public static string FormatCount(in double count)
        {
            // We only see the fractional component when not scaling the value for display (when it's < 1000).
            if (count < 1.0e3)
                return string.Format("{0:F2}", count);
            else
                return FormatCount((long)count);
        }

        public static string FormatPercentage(in float percent)
        {
            return string.Format("{0:F2}%", percent);
        }
    }
}