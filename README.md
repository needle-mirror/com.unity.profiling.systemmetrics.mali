# System Metrics Mali
The System Metrics Mali package provides a simple and extensible interface for reading hardware performance metrics from Arm Mali GPUs. Hardware counters provide a low-level view of application performance. These metrics can provide insight into what impact your changes produce on the hardware level.  System Metrics Mali package exposes a set of hardware metrics via Unity Profiler window and Unity ProfilerRecorder API.

Disclaimer: This package is still under development and anything could change on the API side.

# Installation
The System Metrics Mali package can be installed via Package Manager:
* Download Unity 2021.2.0a17 or newer through the Hub.
* Create or open a Unity project.
* Go to Window > Package Manager.
* Follow “Adding a registry package by name” instructions and add “com.unity.profiling.systemmetrics.mali”

# Supported platforms
The System Metrics Mali package has following limitations:
* The minimum required Unity version is 2021.2
* Platforms: Android
* CPU: Any Arm v7 or v8
* GPU: Mali-T88x or newer
** Supports Samsung Exynos, HiSilicon Kirin, or MediaTek CPUs  (as these use Mali GPUs)
** Doesn’t support Qualcomm Snapdragon CPUs (as these use Adreno GPUs)

# Resources
For more information, please have a look at:
* [Package documentation page](https://docs.unity3d.com/Packages/com.unity.profiling.systemmetrics.mali@0.1/changelog/CHANGELOG.html)
* [Mali Performance Counters documentation](https://community.arm.com/developer/tools-software/graphics/b/blog/posts/mali-midgard-family-performance-counters)
* [HWCP library documentation page](https://github.com/ARM-software/HWCPipe)
