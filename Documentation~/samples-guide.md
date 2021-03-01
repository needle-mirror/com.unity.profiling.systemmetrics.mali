# Using System Metrics Mali samples

System Metrics Mali ships with a sample to help you integrate provided counters into your Project.

When you install the System Metrics package, Unity automatically downloads its associated samples. Import these samples into your Project to see example uses of the System Metrics Mali APIs.

While running, samples generate information about their current state. You can use the [Profiler Module Editor](https://docs.unity3d.com/Manual/profiler-module-editor.html) to monitor the state of plug-in counters. You can add System Metrics Mali counters and record updates in the same way as usual profiler data.

## Installation

Install System Metrics Mali samples from the **Package Manager** window. The following samples are available:

- [Read GPU Metric](#sample-counterread)

## Read GPU Metric
**Note:** You need an Android phone with a Mali GPU to see effects for this sample.

The Read GPU Metric sample shows how GPU metrics can be accessed at runtime.

The sample demonstrates how to create a ProfilerRecorder associated with GPU Metric, how to read it, and how to dispose of created ProfilerRecorder after use.
