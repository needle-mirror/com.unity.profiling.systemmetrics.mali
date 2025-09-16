using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.LowLevel;
using UnityEngine.Scripting;
using Unity.Collections.LowLevel.Unsafe;

[assembly: AlwaysLinkAssembly]

namespace Unity.Profiling.SystemMetrics
{
    /// <summary>
    /// System Metrics Mali class provides access to a wide variety of GPU performance Profiler Counters related to various aspects of
    /// device performance. You can
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
        /// Gets all available Mali performance counters on this device.
        /// </summary>
        /// <param name="handles">The list to be filled with <see cref="ProfilerRecorderHandle"/> instances representing available Mali hardware counters.</param>
        public void GetAvailableCounters(List<ProfilerRecorderHandle> handles)
        {
            SystemMetricsLibHWCP.GetAvailableCounterHandles(handles);
        }

        /// <summary>
        /// Returns the ProfilerCategory for all System Metrics Mali counters
        /// </summary>
        /// <value>The <see cref="Unity.Profiling.ProfilerCategory"/> that all Mali counters belong to.</value>
        public unsafe ProfilerCategory Category
        {
            get
            {
                ushort categoryId = SystemMetricsLibHWCP.GetProfilerCategoryId();
                return *(ProfilerCategory*)(&categoryId);
            }
        }

        /// <summary>
        /// Returns `GPU Active Cycles` performance counter handler (Mali counter: MaliGPUActiveCy).
        /// </summary>
        /// <remarks>
        /// Shows overall GPU processing load. High values indicate GPU-bound content.
        /// If high but individual queues are low, may indicate serialization or dependency issues preventing parallel processing.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCycles { get; private set; }

        /// <summary>
        /// Returns `Fragment Queue Cycles` performance counter handler (Mali counter: MaliFragQueueActiveCy).
        /// </summary>
        /// <remarks>
        /// Indicates fragment workload pressure. For good parallelism, should be similar to GPU active cycles.
        /// Low values with high GPU cycles may indicate vertex/compute bottleneck. Includes stall time waiting for external memory.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuFragmentCycles { get; private set; }

        /// <summary>
        /// Returns `Fragment Queue Utilization` performance counter handler (Mali counter: MaliFragQueueUtil).
        /// </summary>
        /// <remarks>
        /// Fragment queue utilization as percentage of GPU active time. For GPU-bound content,
        /// the dominant queue should be close to 100%. Low utilization may indicate dependency problems or insufficient fragment work.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuFragmentUtilization { get; private set; }

        /// <summary>
        /// Returns `Non Fragment Queue Cycles` performance counter handler (Mali counter: MaliNonFragQueueActiveCy).
        /// </summary>
        /// <remarks>
        /// Indicates vertex/compute workload pressure. Used for vertex shaders, tessellation, geometry shaders,
        /// tiling, and compute shaders. For good parallelism, should be similar to GPU active cycles when vertex/compute bound.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVertexAndComputeCycles { get; private set; }

        /// <summary>
        /// Returns `Non Fragment Queue Utilization` performance counter handler (Mali counter: MaliNonFragQueueUtil).
        /// </summary>
        /// <remarks>
        /// Non-fragment queue utilization as percentage of GPU active time. For vertex/compute-bound content,
        /// should be close to 100%. Low utilization may indicate insufficient vertex/compute work or dependency issues.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVertexAndComputeUtilization { get; private set; }

        /// <summary>
        /// Returns `Vertex Queue Cycles` performance counter handler (Mali counter: MaliVertQueueActiveCy).
        /// </summary>
        /// <remarks>
        /// Number of cycles the vertex processing queue has any workload present, including vertex shaders,
        /// tessellation, and geometry shaders. This provides more granular analysis than the combined
        /// non-fragment queue metrics, allowing separation of vertex and compute workloads.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVertexQueueActiveCycles { get; private set; }

        /// <summary>
        /// Returns `Vertex Queue Utilization` performance counter handler (Mali counter: MaliVertQueueUtil).
        /// </summary>
        /// <remarks>
        /// Vertex queue utilization as percentage of GPU active time, measuring vertex processing pressure.
        /// This enables analysis of vertex-specific bottlenecks separate from compute shader workloads.
        /// High utilization indicates vertex-bound performance.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVertexQueueUtilization { get; private set; }

        /// <summary>
        /// Returns `Compute Queue Cycles` performance counter handler (Mali counter: MaliCompQueueActiveCy).
        /// </summary>
        /// <remarks>
        /// Number of cycles the compute processing queue has any workload present, including compute shaders.
        /// This provides granular analysis of compute shader workloads separate from vertex processing,
        /// enabling targeted optimization of compute-heavy applications.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuComputeQueueActiveCycles { get; private set; }

        /// <summary>
        /// Returns `Compute Queue Utilization` performance counter handler (Mali counter: MaliCompQueueUtil).
        /// </summary>
        /// <remarks>
        /// Compute queue utilization as percentage of GPU active time, measuring compute shader processing pressure.
        /// This enables analysis of compute-specific bottlenecks separate from vertex shader workloads.
        /// High utilization indicates compute-bound performance.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuComputeQueueUtilization { get; private set; }

        /// <summary>
        /// Returns `Tiler Cycles` performance counter handler (Mali counter: MaliTilerActiveCy).
        /// </summary>
        /// <remarks>
        /// Tiler coordinates geometry processing and provides fixed-function tiling for tile-based rendering.
        /// High cycle count doesn't necessarily indicate bottleneck unless non-fragment shader core cycles are comparatively low.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuTilerCycles { get; private set; }

        /// <summary>
        /// Returns `Tiler Utilization` performance counter handler (Mali counter: MaliTilerUtil).
        /// </summary>
        /// <remarks>
        /// Tiler utilization as percentage of GPU active time. Measures overall tiler geometry pipeline
        /// processing time including aspects of vertex shading and fixed-function tiling. High values may indicate geometry complexity issues.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuTilerUtilization { get; private set; }

        /// <summary>
        /// Returns `GPU Interrupt Cycles` performance counter handler (Mali counter: MaliGPUIRQActiveCy).
        /// </summary>
        /// <remarks>
        /// Cycles when GPU has interrupt pending, waiting for CPU processing. High percentage of GPU active cycles
        /// may indicate system integration issues preventing efficient CPU interrupt handling (not typically application-resolvable).
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuInterruptCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Vertex And Compute Cycles` performance counter handler (Mali counter: MaliNonFragActiveCy).
        /// </summary>
        /// <remarks>
        /// Number of cycles the compute pipeline is active processing vertex shaders, tessellation, geometry shaders,
        /// fixed-function tiling, and compute shaders. This counter includes any cycle where compute work runs
        /// in the fixed-function front-end or programmable core. High values indicate vertex/compute-bound workload.
        /// For vertex/compute-bound content, should be similar to GPU active cycles.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderComputeCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Vertex And Compute Utilization` performance counter handler (Mali counter: MaliNonFragUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of time the compute pipeline is active processing vertex and compute workloads.
        /// This counter indicates overall utilization of the compute shader pipeline including vertex shaders,
        /// tessellation, geometry shaders, fixed-function tiling, and compute shaders. For vertex/compute-bound content,
        /// should approach 100%. Low utilization may indicate insufficient vertex/compute work or dependency issues.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderComputeUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Fragment Cycles` performance counter handler (Mali counter: MaliFragActiveCy).
        /// </summary>
        /// <remarks>
        /// Number of cycles the fragment pipeline is active processing fragment workloads. This counter includes
        /// all cycles when at least one fragment task is active anywhere inside the shader core, including the
        /// fixed-function fragment frontend, programmable tripipe, or fixed-function fragment backend.
        /// For most content, fragments outnumber vertices by orders of magnitude, making this typically the highest processing load.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderFragmentCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Fragment Utilization` performance counter handler (Mali counter: MaliFragUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of time the fragment pipeline is active processing fragment shaders. This counter indicates
        /// overall utilization of the fragment pipeline including the fixed-function fragment frontend,
        /// programmable tripipe, and fixed-function fragment backend. High utilization indicates fragment-bound performance.
        /// For GPU-bound content, the dominant queue should approach 100%. Low utilization may suggest vertex/compute bottleneck.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderFragmentUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Core Cycles` performance counter handler (Mali counter: MaliCoreActiveCy)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderCoreCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Core Utilization` performance counter handler (Mali counter: MaliCoreUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of time any shader core is executing instructions. Lower values indicate GPU utilization
        /// bottlenecks - check for CPU serialization, bandwidth limits, texture cache pressure. Higher values suggest compute efficiency.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderCoreUtilization { get; private set; }

        /// <summary>
        /// Returns `Input Primitives` performance counter handler (Mali counter: MaliGeomTotalPrim).
        /// </summary>
        /// <remarks>
        /// Total number of input primitives to rendering process. High complexity geometry is expensive
        /// as vertices are larger than compressed texels (~32 bytes each). Use dynamic LOD and normal maps to reduce primitive count.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuInputPrimitives { get; private set; }

        /// <summary>
        /// Returns `% Culled Primitives` performance counter handler (Mali counter: MaliGeomZPlaneCullRate)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCulledPrimitivesPercentage { get; private set; }

        /// <summary>
        /// Returns `Z Plane Culled Primitives` performance counter handler (Mali counter: MaliGeomZPlaneCullPrim).
        /// </summary>
        /// <remarks>
        /// Primitives culled by Z plane test (closer than near clip or further than far clip).
        /// Significant culling at this stage indicates insufficient application software culling - optimize view frustum culling.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCulledPrimitives { get; private set; }

        /// <summary>
        /// Returns `% Clipped Primitives` performance counter handler (Mali counter: MaliGeomFaceXYPlaneCullRate)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuClippedPrimitivesPercentage { get; private set; }

        /// <summary>
        /// Returns `Facing/XY Plane Culled Primitives` performance counter handler (Mali counter: MaliGeomFaceXYPlaneCullPrim).
        /// </summary>
        /// <remarks>
        /// Primitives culled by facing and XY plane tests (back-facing or out-of-frustum in XY axis).
        /// More than 50% culled may indicate out-of-frustum geometry - optimize with better software culling or batching granularity.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuClippedPrimitives { get; private set; }

        /// <summary>
        /// Returns `Visible Primitives` performance counter handler (Mali counter: MaliGeomVisiblePrim).
        /// </summary>
        /// <remarks>
        /// Primitives surviving all culling stages. All fragments might still be occluded by closer primitives.
        /// For efficient 3D content, expect ~50% visibility due to back-face culling. Much higher may indicate disabled facing test.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVisiblePrimitives { get; private set; }

        /// <summary>
        /// Returns `% Visible Primitives` performance counter handler (Mali counter: MaliGeomVisibleRate).
        /// </summary>
        /// <remarks>
        /// Percentage of primitives visible after culling. Efficient 3D content expects ~50% due to back-face culling.
        /// Significantly higher rates may indicate disabled facing test. Lower rates suggest optimization opportunities with better software culling.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVisiblePrimitivesPercentage { get; private set; }

        /// <summary>
        /// Returns `Shader Arithmetic Cycles` performance counter handler (Mali counter: MaliALUIssueCy).
        /// </summary>
        /// <remarks>
        /// Shader core cycles used on arithmetic operations (ALU). High usage indicates compute-bound shaders.
        /// Optimize by reducing complex calculations, using precision lowering (mediump/lowp), or moving computations to vertex stage when possible.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderArithmeticCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Arithmetic Utilization` performance counter handler (Mali counter: MaliALUUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of shader core cycles spent on arithmetic operations. High utilization indicates compute-bound shaders.
        /// Low utilization suggests stalls from memory access or texture sampling. Balance workload distribution and optimize shader complexity.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderArithmeticUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Texture Cycles` performance counter handler (Mali counter: MaliTexIssueCy).
        /// </summary>
        /// <remarks>
        /// Shader core cycles used on texture filtering workloads. High usage suggests texture bandwidth bottleneck.
        /// Consider reducing texture resolution, using compressed formats, optimizing cache efficiency with texture atlases or tiled rendering.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderTextureCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Texture Unit Utilization` performance counter handler (Mali counter: MaliTexUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of shader core cycles spent on texture filtering operations. High utilization indicates texture bandwidth bottleneck.
        /// Optimize with compressed texture formats, reduced resolution, better cache locality, or simplified filtering requirements.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderTextureUtilization { get; private set; }

        /// <summary>
        /// Returns `Shader Load/Store Cycles` performance counter handler (Mali counter: MaliLSIssueCy).
        /// </summary>
        /// <remarks>
        /// Shader core cycles used on memory access (varying/uniform/image/storage buffer operations).
        /// High usage suggests memory access bottleneck. Optimize with better data locality, reduced varying count, or cached uniform buffers.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderLoadStoreCycles { get; private set; }

        /// <summary>
        /// Returns `Shader Load/Store Utilization` performance counter handler (Mali counter: MaliLSUtil).
        /// </summary>
        /// <remarks>
        /// Percentage of shader core cycles spent on memory load/store operations. High utilization indicates memory access bottleneck.
        /// Optimize with reduced varying interpolation, cached uniform buffers, better data locality, or simplified memory access patterns.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuShaderLoadStoreUtilization { get; private set; }


        /// <summary>
        /// Returns `Early Z Tests` performance counter handler (Mali counter: MaliFragEZSTestQd).
        /// </summary>
        /// <remarks>
        /// Fragment quads subjected to early depth/stencil testing. High early test rates are efficient
        /// as they remove redundant work before shading. Enable depth testing and avoid modifiable coverage for better early test rates.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuEarlyZTests { get; private set; }

        /// <summary>
        /// Returns `Early Z Kills` performance counter handler (Mali counter: MaliFragEZSKillQd).
        /// </summary>
        /// <remarks>
        /// Fragment quads killed by early depth/stencil testing before shading. High percentage is generally good
        /// as it avoids redundant processing. May indicate opportunity for software culling (portal/occlusion culling) to avoid submitting occluded geometry.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuEarlyZKills { get; private set; }

        /// <summary>
        /// Returns `Late Z Tests` performance counter handler (Mali counter: MaliFragLZSTestTd).
        /// </summary>
        /// <remarks>
        /// Fragment tiles tested by late depth/stencil testing. High percentage can cause slow performance
        /// as younger fragments can't complete early ZS until older fragments complete late ZS. Avoid modifiable coverage and shader depth writes.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuLateZTests { get; private set; }

        /// <summary>
        /// Returns `Late Z Kills` performance counter handler (Mali counter: MaliFragLZSKillTd).
        /// </summary>
        /// <remarks>
        /// Fragment tiles killed by late depth/stencil testing after running some fragment program.
        /// High percentage indicates redundant processing. Avoid modifiable coverage and shader depth writes. Always start from cleared values when possible.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuLateZKills { get; private set; }


        /// <summary>
        /// Returns `Vertex And Compute Jobs` performance counter handler (Mali counter: MaliNonFragThread).
        /// </summary>
        /// <remarks>
        /// Vertex and compute shader tasks executed by GPU. High counts may indicate vertex processing bottleneck.
        /// Optimize with reduced vertex complexity, better LOD systems, or moving calculations to fragment stage when appropriate.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuVertexComputeJobs { get; private set; }

        /// <summary>
        /// Returns `Fragment Jobs` performance counter handler (Mali counter: MaliFragThread).
        /// </summary>
        /// <remarks>
        /// Fragment shader tasks executed by GPU. High counts indicate fragment processing bottleneck.
        /// Optimize with simplified fragment shaders, reduced overdraw, better early-Z utilization, or precision lowering (mediump/lowp).
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuFragmentJobs { get; private set; }

        /// <summary>
        /// Returns `Tiles` performance counter handler (Mali counter: MaliFragTile)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuTiles { get; private set; }

        /// <summary>
        /// Returns `Unchanged Eliminated Tiles` performance counter handler (Mali counter: MaliFragTileKill)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuUnchangedEliminatedTiles { get; private set; }

        /// <summary>
        /// Returns `Pixels` performance counter handler (Mali counter: MaliGPUPix)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuPixels { get; private set; }

        /// <summary>
        /// Returns `Cycles Per Pixel` performance counter handler (Mali counter: MaliGPUCyPerPix)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCyclesPerPixels { get; private set; }

        /// <summary>
        /// Returns `Instructions` performance counter handler (Mali counter: MaliEngInstr)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuInstructions { get; private set; }

        /// <summary>
        /// Returns `Diverged Instructions` performance counter handler (Mali counter: MaliEngDivergedInstr)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuDivergedInstructions { get; private set; }


        /// <summary>
        /// Returns `Cache Read Lookups` performance counter handler (Mali counter: MaliL2CacheRdLookup).
        /// </summary>
        /// <remarks>
        /// L2 cache read lookup activity. High values relative to external memory reads indicate good cache efficiency.
        /// Low cache hit rates may indicate poor data locality or working sets larger than cache capacity.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCacheReadLookups { get; private set; }

        /// <summary>
        /// Returns `Cache Write Lookups` performance counter handler (Mali counter: MaliL2CacheWrLookup).
        /// </summary>
        /// <remarks>
        /// L2 cache write lookup activity. High values relative to external memory writes indicate good cache efficiency.
        /// Monitors effectiveness of write combining and cache utilization for output data.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuCacheWriteLookups { get; private set; }

        /// <summary>
        /// Returns `Memory Read Accesses` performance counter handler (Mali counter: MaliExtBusRd)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryReadAccesses { get; private set; }

        /// <summary>
        /// Returns `Memory Write Accesses` performance counter handler (Mali counter: MaliExtBusWr)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryWriteAccesses { get; private set; }

        /// <summary>
        /// Returns `Memory Read Stalled Cycles` performance counter handler (Mali counter: MaliExtBusRdStallCy)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryReadStalledCycles { get; private set; }

        /// <summary>
        /// Returns `% Memory Read Stalled` performance counter handler (Mali counter: MaliExtBusRdStallRate).
        /// </summary>
        /// <remarks>
        /// Percentage of GPU cycles stalled on external read transactions. High stall rates indicate
        /// content requesting more data than memory system can provide. Reduce by minimizing data resource sizes (buffers, textures).
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryReadStallRate { get; private set; }

        /// <summary>
        /// Returns `Memory Write Stalled Cycles` performance counter handler (Mali counter: MaliExtBusWrStallCy)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryWriteStalledCycles { get; private set; }

        /// <summary>
        /// Returns `% Memory Write Stalled` performance counter handler (Mali counter: MaliExtBusWrStallRate).
        /// </summary>
        /// <remarks>
        /// Percentage of GPU cycles stalled on external write transactions. High stall rates indicate
        /// memory bandwidth bottleneck. Reduce by minimizing geometry complexity or framebuffer sizes in memory.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryWriteStallRate { get; private set; }

        /// <summary>
        /// Returns `Fragment FPK Active Cycles` performance counter handler (Mali counter: MaliFragFPKActiveCy)
        /// </summary>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuFragmentFPKActiveCycles { get; private set; }

        /// <summary>
        /// Returns `Memory Read Bytes` performance counter handler (Mali counter: MaliExtBusRdBt).
        /// </summary>
        /// <remarks>
        /// External memory read bandwidth usage. High values indicate memory bandwidth bottleneck.
        /// On mobile, ~100MB per frame at 60FPS is sustainable. Reduce texture sizes, buffer sizes, or use compression to optimize.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryReadBytes { get; private set; }

        /// <summary>
        /// Returns `Memory Write Bytes` performance counter handler (Mali counter: MaliExtBusWrBt).
        /// </summary>
        /// <remarks>
        /// External memory write bandwidth usage. High values indicate memory bandwidth bottleneck.
        /// Accessing external DRAM costs 80-100mW per GB/s. Reduce framebuffer sizes or geometry complexity to optimize.
        /// </remarks>
        /// <value>Use <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle"/> to access counter data with <see cref="Unity.Profiling.ProfilerRecorder"/> and <see cref="Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.Valid"/> to check if counter is supported.</value>
        public ProfilerRecorderHandle GpuMemoryWriteBytes { get; private set; }

        [Preserve]
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
                // Only initialize counters which are exposed via properties.
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
            var injectAfter = typeof(UnityEngine.PlayerLoop.PreLateUpdate);
            var res = InjectPlayerLoopCallback(ref root, injectAfter, SystemMetricsLibHWCP.Sample);
            PlayerLoop.SetPlayerLoop(root);
            return res;
        }

        private struct SystemMetricsPlayerLoopUpdateSystem {};

        static bool InjectPlayerLoopCallback(ref PlayerLoopSystem rootSystem, System.Type injectAfter, PlayerLoopSystem.UpdateFunction injectedFnc)
        {
            if (rootSystem.subSystemList == null)
                return false;

            var insertAfterIndex = Array.FindIndex(rootSystem.subSystemList, x => x.type == injectAfter);
            if (insertAfterIndex >= 0)
            {
                // Check if we're not already injected
                if (Array.IndexOf(rootSystem.subSystemList, typeof(SystemMetricsPlayerLoopUpdateSystem)) > 0)
                    return true;

                InsertAt(ref rootSystem.subSystemList, insertAfterIndex, new PlayerLoopSystem
                {
                    type = typeof(SystemMetricsPlayerLoopUpdateSystem),
                    updateDelegate = injectedFnc
                });

                return true;
            }

            // Iterate all subsystems
            for (int i = 0; i < rootSystem.subSystemList.Length; ++i)
            {
                if (InjectPlayerLoopCallback(ref rootSystem.subSystemList[i], injectAfter, injectedFnc))
                    return true;
            }

            return false;
        }

        static void InsertAt<TValue>(ref TValue[] array, int index, TValue value)
        {
            var oldLength = array.Length;
            Array.Resize(ref array, oldLength + 1);

            // Make room for element.
            if (index != oldLength)
                Array.Copy(array, index, array, index + 1, oldLength - index);

            array[index] = value;
        }
    }
}