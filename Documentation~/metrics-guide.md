# Mali GPU Performance Counters Reference Guide

This document provides comprehensive information about ARM Mali GPU performance counters exposed by the System Metrics Mali package. The document is based on the [ARM performance documentation](https://developer.arm.com/documentation#numberOfResults=48&q=Performance%20Counters&sort=relevancy&f:@navigationhierarchiesproducts=[IP%20Products,Graphics%20and%20Multimedia%20Processors,Mali%20GPUs]).

## Overview

The System Metrics Mali package exposes a set of essential Mali GPU performance counters for the initial GPU performance analysis. These counters help identify bottlenecks across GPU activity, content behavior, shader core utilization, and memory performance, providing Unity developers with powerful profiling capabilities similar to ARM's Streamline tool.

### Data Types and Usage

- **Data Type**: All counters return `long` (64-bit signed integer) values
- **Access**: Use `ProfilerRecorder` with the provided `ProfilerRecorderHandle` properties from `SystemMetricsMali.Instance`
- **Sampling**: All counters are automatically sampled when the Unity Profiler is active. When Unity Profiler is disabled only counters with attached `ProfilerRecorder` are sampled.
- **Performance Impact**: Minimal - ARM Mali hardware counters are designed for production use with negligible overhead
- **Platform Support**: Android devices with ARM Mali GPUs.
- **Category Access**: All counters belong to the same `ProfilerCategory` accessible via `SystemMetricsMali.Instance.Category`

### Performance Analysis Methodology

Effective Mali GPU performance analysis follows the following workflow:

1. **GPU Activity Analysis**: Identify overall GPU utilization, queue pressure, and processing bottlenecks
2. **Content Behavior Analysis**: Optimize geometry complexity, primitive culling efficiency, and rendering pipeline usage
3. **Shader Core Data Path Analysis**: Balance workloads across arithmetic, texture, and load/store functional units
4. **Memory Performance Analysis**: Minimize bandwidth usage, optimize cache efficiency, and reduce memory stalls

### Cycle Budget Planning

For mobile Mali GPUs, ARM recommends setting cycle budgets based on target resolution and frame rate to manage performance expectations:

```
Maximum Cycle Budget = (GPU Frequency × Core Count) / (Resolution × Target FPS)
Real-world Budget = 0.85 × Maximum Budget  // Assuming 85% utilization efficiency
```

**Example**: A 3-core 500MHz Mali GPU targeting 1080p60 has:
- Maximum budget: (500MHz × 3 cores) / (1920×1080 × 60fps) = ~12 cycles per pixel
- Realistic budget: 0.85 × 12 = ~10 cycles per pixel

This budget must cover all processing costs including vertex shading, fragment shading, and fixed-function operations. Effective optimization requires spending each cycle wisely within this constraint.

---

## GPU Activity Counters

GPU activity counters monitor the overall processing load and identify which GPU queues are under pressure. These counters help determine if your application is GPU-bound and which processing stages are bottlenecks.

### Core GPU Activity

#### GPU Active Cycles
> **Property:** [`GpuCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCycles) | **HWCPipe:** `MaliGPUActiveCy` | **Units:** `Cycles`

**Description**: Total number of GPU cycles where the GPU has any active workload, including processing, queued, or stalled work. This is the primary indicator of overall GPU utilization.

**Performance Impact**: High values relative to available GPU cycles indicate GPU-bound content. If high but individual queue utilizations are low, may indicate serialization issues or dependency problems preventing efficient parallel processing.

**Optimization**: Reduce geometry complexity, shader complexity, rendering resolution, or eliminate CPU-GPU synchronization stalls. Consider frame pacing to reduce peak workload.

### Queue Activity

#### Fragment Queue Cycles
> **Property:** [`GpuFragmentCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentCycles) | **HWCPipe:** `MaliFragQueueActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the fragment processing queue has any workload present, including stall time waiting for external memory.

**Performance Impact**: For GPU-bound content, this should be similar to GPU active cycles. Low values with high GPU cycles may indicate vertex/compute bottleneck limiting fragment processing.

**Optimization**: For fragment-bound scenarios, reduce fragment shader complexity, improve early-Z efficiency, or reduce overdraw. For vertex-bound scenarios, optimize geometry or vertex shaders.

#### Fragment Queue Utilization
> **Property:** [`GpuFragmentUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentUtilization) | **HWCPipe:** `MaliFragQueueUtil` | **Units:** `Percentage (0-100)`

**Description**: Fragment queue utilization as percentage of GPU active time, indicating fragment processing pressure.

**Performance Impact**: For GPU-bound content, the dominant queue should approach 100%. Low utilization may indicate dependency problems, insufficient work, or barriers preventing parallel execution.

**Optimization**: Ensure consistent fragment workload submission, minimize barriers between draw calls, and optimize for parallel fragment processing.

#### Non-Fragment Queue Cycles
> **Property:** [`GpuVertexAndComputeCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexAndComputeCycles) | **HWCPipe:** `MaliNonFragQueueActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the vertex/compute processing queue has workload present, covering vertex shaders, tessellation, geometry shaders, tiling, and compute shaders.

**Performance Impact**: High values indicate vertex/compute processing bottleneck. Should be similar to GPU active cycles when vertex/compute bound.

**Optimization**: Reduce vertex complexity, optimize vertex shaders, implement level-of-detail systems, or move calculations to fragment stage when appropriate.

#### Non-Fragment Queue Utilization
> **Property:** [`GpuVertexAndComputeUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexAndComputeUtilization) | **HWCPipe:** `MaliNonFragQueueUtil` | **Units:** `Percentage (0-100)`

**Description**: Non-fragment queue utilization as percentage of GPU active time, indicating vertex/compute processing pressure.

**Performance Impact**: For vertex/compute-bound content, should approach 100%. Low utilization indicates insufficient vertex/compute work or dependency issues.

**Optimization**: Balance workload between vertex and fragment stages, ensure efficient vertex data submission, and minimize vertex/compute pipeline stalls.

#### Vertex Queue Cycles
> **Property:** [`GpuVertexQueueActiveCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexQueueActiveCycles) | **HWCPipe:** `MaliVertQueueActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the vertex processing queue has any workload present, including vertex shaders, tessellation, and geometry shaders. This provides more granular analysis than the combined non-fragment queue metrics.

**Performance Impact**: High values indicate vertex processing bottleneck. Enables separation of vertex and compute workloads for targeted optimization analysis.

**Optimization**: Reduce vertex complexity, optimize vertex shaders, implement level-of-detail systems, or use vertex attribute compression to reduce vertex processing overhead.

#### Vertex Queue Utilization
> **Property:** [`GpuVertexQueueUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexQueueUtilization) | **HWCPipe:** `MaliVertQueueUtil` | **Units:** `Percentage (0-100)`

**Description**: Vertex queue utilization as percentage of GPU active time, measuring vertex processing pressure separate from compute shaders.

**Performance Impact**: High utilization indicates vertex-bound performance. Low utilization may suggest fragment bottleneck or inefficient vertex submission patterns.

**Optimization**: Balance vertex workload with fragment processing, ensure efficient vertex data submission, and minimize vertex pipeline stalls through better geometry organization.

#### Compute Queue Cycles
> **Property:** [`GpuComputeQueueActiveCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuComputeQueueActiveCycles) | **HWCPipe:** `MaliCompQueueActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the compute processing queue has any workload present, including compute shaders. This enables granular analysis of compute shader workloads separate from vertex processing.

**Performance Impact**: High values indicate compute shader bottleneck. Essential for applications using compute shaders for physics, post-processing, or other parallel computations.

**Optimization**: Optimize compute shader complexity, reduce compute workgroup sizes, improve memory access patterns, or balance compute workload with graphics rendering.

#### Compute Queue Utilization
> **Property:** [`GpuComputeQueueUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuComputeQueueUtilization) | **HWCPipe:** `MaliCompQueueUtil` | **Units:** `Percentage (0-100)`

**Description**: Compute queue utilization as percentage of GPU active time, measuring compute shader processing pressure separate from vertex and fragment workloads.

**Performance Impact**: High utilization indicates compute-bound performance. Low utilization may suggest graphics workload dominance or inefficient compute shader submission.

**Optimization**: Balance compute workload with graphics rendering, optimize compute shader efficiency, and ensure proper compute workload scheduling to avoid pipeline stalls.

### Specialized GPU Units

#### Tiler Cycles
> **Property:** [`GpuTilerCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuTilerCycles) | **HWCPipe:** `MaliTilerActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the tiler is active coordinating geometry processing and fixed-function tiling for tile-based rendering.

**Performance Impact**: High cycle count doesn't necessarily indicate bottleneck unless non-fragment shader core cycles are comparatively low. Indicates geometry pipeline complexity.

**Optimization**: Reduce geometry complexity, optimize vertex data layout, implement more aggressive level-of-detail, or improve spatial locality of geometry submission.

#### Tiler Utilization
> **Property:** [`GpuTilerUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuTilerUtilization) | **HWCPipe:** `MaliTilerUtil` | **Units:** `Percentage (0-100)`

**Description**: Tiler utilization as percentage of GPU active time, measuring overall geometry pipeline processing including vertex shading and fixed-function tiling.

**Performance Impact**: High values may indicate geometry complexity issues or inefficient vertex processing. Affects vertex throughput and geometry submission rates.

**Optimization**: Implement dynamic level-of-detail, reduce vertex attribute count, optimize index buffer layout, or use geometry instancing to reduce vertex processing overhead.

#### GPU Interrupt Cycles
> **Property:** [`GpuInterruptCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuInterruptCycles) | **HWCPipe:** `MaliGPUIRQActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the GPU has interrupts pending, waiting for CPU processing.

**Performance Impact**: High percentage relative to GPU active cycles may indicate system integration issues preventing efficient CPU interrupt handling. Generally not application-resolvable.

**Optimization**: System-level issue. Consider reducing GPU command submission frequency or implementing more efficient CPU-GPU synchronization patterns.

### Shader Core Activity

#### Shader Core Cycles
> **Property:** [`GpuShaderCoreCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderCoreCycles) | **HWCPipe:** `MaliCoreActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles any shader core is active processing workload, including all programmable shader stages.

**Performance Impact**: Primary indicator of shader core utilization. High values indicate compute-intensive workloads.

**Optimization**: Optimize shader complexity, use precision lowering (mediump/lowp), reduce complex calculations, or balance workload across functional units.

#### Shader Core Utilization
> **Property:** [`GpuShaderCoreUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderCoreUtilization) | **HWCPipe:** `MaliCoreUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of time any shader core is executing instructions across all functional units.

**Performance Impact**: Lower values indicate GPU utilization bottlenecks such as CPU serialization, memory bandwidth limits, or texture cache pressure. Higher values suggest efficient compute utilization.

**Optimization**: For low utilization, investigate memory access patterns, texture cache efficiency, and CPU-GPU synchronization. For high utilization, optimize shader complexity and instruction mix.

---

## Content Behavior Counters

Content behavior counters help optimize rendering efficiency by monitoring geometry usage, primitive culling, depth testing, and fragment processing. These counters identify redundant work and opportunities to improve content structure.

### Geometry Usage

#### Input Primitives
> **Property:** [`GpuInputPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuInputPrimitives) | **HWCPipe:** `MaliGeomTotalPrim` | **Units:** `Count`

**Description**: Total number of primitives submitted to the GPU rendering pipeline before any culling operations.

**Performance Impact**: High complexity geometry is expensive as vertices are larger than compressed texels (~32 bytes each). Excessive primitive counts directly increase vertex processing and bandwidth costs.

**Optimization**: Use dynamic level-of-detail systems, normal maps to reduce geometry detail, geometry instancing, and mesh optimization tools to minimize primitive count while maintaining visual quality.

### Primitive Culling Efficiency

#### Facing/XY Plane Culled Primitives
> **Property:** [`GpuClippedPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuClippedPrimitives) | **HWCPipe:** `MaliGeomFaceXYPlaneCullPrim` | **Units:** `Count`

**Description**: Number of primitives culled by facing test (back-facing) and XY plane test (outside view frustum horizontally/vertically).

**Performance Impact**: For efficient 3D content, expect ~50% back-face culling. More than 50% culled may indicate out-of-frustum geometry submission, wasting vertex processing on non-visible content.

**Optimization**: Implement better software frustum culling, improve batching granularity to avoid submitting out-of-view geometry, and ensure back-face culling is enabled where appropriate.

#### Clipped Primitives Percentage
> **Property:** [`GpuClippedPrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuClippedPrimitivesPercentage) | **HWCPipe:** `MaliGeomFaceXYPlaneCullRate` | **Units:** `Percentage (0-100)`

**Description**: Percentage of input primitives requiring clipping operations due to intersecting view frustum boundaries.

**Performance Impact**: High clipping rates indicate insufficient software culling, requiring expensive hardware clipping operations.

**Optimization**: Implement more aggressive software frustum culling, optimize clip plane distances, and use view-dependent geometry submission to minimize clipping requirements.

#### Z Plane Culled Primitives
> **Property:** [`GpuCulledPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCulledPrimitives) | **HWCPipe:** `MaliGeomZPlaneCullPrim` | **Units:** `Count`

**Description**: Number of primitives culled by Z plane test (closer than near clip plane or further than far clip plane).

**Performance Impact**: Significant culling at this stage indicates insufficient application-level culling, wasting vertex processing on geometry that will never be visible.

**Optimization**: Implement more aggressive software view frustum culling, optimize near/far clip plane distances, and use occlusion culling systems to avoid processing occluded geometry.

#### Z Plane Culled Primitives Percentage
> **Property:** [`GpuCulledPrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCulledPrimitivesPercentage) | **HWCPipe:** `MaliGeomZPlaneCullRate` | **Units:** `Percentage (0-100)`

**Description**: Percentage of input primitives culled by Z plane test.

**Performance Impact**: High culling rates indicate excessive out-of-frustum geometry submission or opportunities for better software culling.

**Optimization**: Target efficient culling rates through better software frustum culling and optimized near/far clip plane distances.

#### Visible Primitives
> **Property:** [`GpuVisiblePrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVisiblePrimitives) | **HWCPipe:** `MaliGeomVisiblePrim` | **Units:** `Count`

**Description**: Number of primitives surviving all culling stages and proceeding to rasterization.

**Performance Impact**: These primitives will generate fragments, but individual fragments may still be occluded by closer geometry. Represents the actual geometry workload for rasterization and fragment processing.

**Optimization**: This is the "useful" geometry after culling. Focus optimization efforts on reducing input primitives or improving culling efficiency to minimize the ratio of input to visible primitives.

#### Visible Primitive Percentage
> **Property:** [`GpuVisiblePrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVisiblePrimitivesPercentage) | **HWCPipe:** `MaliGeomVisibleRate` | **Units:** `Percentage (0-100)`

**Description**: Percentage of input primitives that survive all culling stages.

**Performance Impact**: For efficient 3D content, expect ~50% visibility due to back-face culling. Significantly higher rates may indicate disabled face culling. Lower rates suggest optimization opportunities.

**Optimization**: Target 50% visibility for typical 3D content. Much higher suggests missing back-face culling. Much lower suggests excessive out-of-frustum geometry or opportunities for better software culling.

### Fragment Depth and Stencil Testing

#### Early Z Tests
> **Property:** [`GpuEarlyZTests`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuEarlyZTests) | **HWCPipe:** `MaliFragEZSTestQd` | **Units:** `Count`

**Description**: Number of fragment quads subjected to early depth and stencil testing before fragment shading.

**Performance Impact**: High early test rates are efficient as they remove redundant work before expensive fragment shading. Maximize early testing for better performance.

**Optimization**: Enable depth testing, avoid shader depth writes and modifiable coverage, render opaque objects front-to-back to maximize early-Z efficiency.

#### Early Z Kills
> **Property:** [`GpuEarlyZKills`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuEarlyZKills) | **HWCPipe:** `MaliFragEZSKillQd` | **Units:** `Count`

**Description**: Number of fragment quads killed by early depth and stencil testing before fragment shading.

**Performance Impact**: High percentage of early kills is generally beneficial as it avoids expensive fragment shading. May indicate opportunity for portal/occlusion culling to avoid submitting occluded geometry entirely.

**Optimization**: While early kills save fragment processing, preventing occluded geometry submission through software culling is even more efficient.

#### Late Z Tests
> **Property:** [`GpuLateZTests`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuLateZTests) | **HWCPipe:** `MaliFragLZSTestTd` | **Units:** `Count`

**Description**: Number of fragment tiles subjected to late depth and stencil testing after fragment shading.

**Performance Impact**: High percentage of late ZS testing can cause performance problems as younger fragments cannot complete early ZS until older fragments complete late ZS operations, creating stalls.

**Optimization**: Minimize late ZS testing by avoiding shader depth writes, modifiable coverage, and memory-visible side effects in fragment shaders. Always start render passes from cleared depth values when possible.

#### Late Z Kills
> **Property:** [`GpuLateZKills`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuLateZKills) | **HWCPipe:** `MaliFragLZSKillTd` | **Units:** `Count`

**Description**: Number of fragment tiles killed by late depth and stencil testing after running at least some fragment program.

**Performance Impact**: High percentage indicates redundant fragment processing. These fragments consume expensive shader resources before being discarded, representing wasted computation.

**Optimization**: Minimize late ZS kills by avoiding shader depth writes, modifiable coverage, and side effects. Prefer early-Z testing through careful draw order and shader design.

### Fragment Processing Overview

#### Fragment Jobs
> **Property:** [`GpuFragmentJobs`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentJobs) | **HWCPipe:** `MaliFragThread` | **Units:** `Count`

**Description**: Number of fragment shader tasks executed by the GPU.

**Performance Impact**: High counts indicate fragment processing bottleneck. Fragment shaders typically dominate mobile GPU workload due to high fragment counts relative to vertices.

**Optimization**: Simplify fragment shaders, reduce overdraw, improve early-Z utilization, use precision lowering (mediump/lowp), and minimize complex calculations in fragment stage.

#### Vertex and Compute Jobs
> **Property:** [`GpuVertexComputeJobs`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexComputeJobs) | **HWCPipe:** `MaliNonFragThread` | **Units:** `Count`

**Description**: Number of vertex and compute shader tasks executed by the GPU.

**Performance Impact**: High counts may indicate vertex processing bottleneck. Balance vertex complexity against fragment complexity based on workload characteristics.

**Optimization**: Reduce vertex complexity, implement better level-of-detail systems, use vertex attribute compression, or move appropriate calculations to fragment stage.

#### Tiles
> **Property:** [`GpuTiles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuTiles) | **HWCPipe:** `MaliFragTile` | **Units:** `Count`

**Description**: Number of 32×32 pixel tiles processed by the GPU's tile-based rendering architecture.

**Performance Impact**: Directly related to resolution and rendered area. High tile counts increase memory bandwidth and fragment processing requirements.

**Optimization**: Reduce rendering resolution, implement variable rate shading, or use render area restrictions to minimize tile count when appropriate.

#### Unchanged Eliminated Tiles
> **Property:** [`GpuUnchangedEliminatedTiles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuUnchangedEliminatedTiles) | **HWCPipe:** `MaliFragTileKill` | **Units:** `Count`

**Description**: Number of tiles eliminated because they contain no visible changes, an optimization in Mali's tile-based rendering.

**Performance Impact**: High elimination rates indicate efficient redundant work removal. Represents processing savings from unchanged regions.

**Optimization**: Good tile elimination is beneficial. Consider scene organization and temporal coherence to maximize unchanged tile elimination opportunities.

#### Pixels
> **Property:** [`GpuPixels`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuPixels) | **HWCPipe:** `MaliGPUPix` | **Units:** `Count`

**Description**: Total number of pixels processed by the GPU, including on-screen and off-screen rendering.

**Performance Impact**: High pixel counts indicate resolution or overdraw pressure. Mobile GPUs have limited fill rate, making pixel count optimization crucial.

**Optimization**: Reduce rendering resolution, minimize overdraw through better draw order, implement level-of-detail for materials, and optimize multi-pass rendering.

#### Cycles Per Pixel
> **Property:** [`GpuCyclesPerPixels`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCyclesPerPixels) | **HWCPipe:** `MaliGPUCyPerPix` | **Units:** `Cycles/Pixel`

**Description**: Number of GPU cycles spent per pixel, providing efficiency metric for rendering performance analysis.

**Performance Impact**: Higher values indicate inefficient pixel processing, potentially from overdraw, complex fragment shaders, or memory bandwidth limitations. Good values depend on scene complexity but should remain consistent for similar content.

**Optimization**: Reduce fragment shader complexity, minimize overdraw, optimize texture access patterns, and consider reducing rendering resolution for complex scenes.


---

## Shader Core Data Path Counters

Shader core data path counters monitor utilization of Mali GPU's functional units within the shader core. These counters help identify workload imbalances and optimize shader instruction mix for better performance.

### Fragment and Non-Fragment Pipeline Utilization

#### Fragment Cycles
> **Property:** [`GpuShaderFragmentCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderFragmentCycles) | **HWCPipe:** `MaliFragActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the fragment pipeline is active, including fixed-function fragment frontend, programmable tripipe, and fixed-function fragment backend.

**Performance Impact**: For most content, fragments outnumber vertices by orders of magnitude, making this typically the highest processing load. High values indicate fragment-bound workload.

**Optimization**: Reduce fragment shader complexity, minimize overdraw, improve early-Z efficiency, use precision lowering (mediump/lowp), and optimize texture sampling patterns.

#### Fragment Utilization
> **Property:** [`GpuShaderFragmentUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderFragmentUtilization) | **HWCPipe:** `MaliFragUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of time the fragment pipeline is active processing fragment workloads.

**Performance Impact**: High utilization indicates fragment-bound performance. Low utilization may suggest vertex/compute bottleneck or inefficient fragment submission.

**Optimization**: Balance fragment workload with vertex/compute work. For high utilization, optimize fragment shaders. For low utilization, investigate vertex bottlenecks or submission patterns.

#### Vertex and Compute Cycles
> **Property:** [`GpuShaderComputeCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderComputeCycles) | **HWCPipe:** `MaliNonFragActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the compute pipeline is active processing vertex shaders, tessellation, geometry shaders, fixed-function tiling, and compute shaders.

**Performance Impact**: High values indicate vertex/compute-bound workload. Includes any cycle where compute work runs in fixed-function front-end or programmable core.

**Optimization**: Reduce vertex complexity, implement level-of-detail, optimize vertex attribute layouts, or move appropriate calculations to fragment stage when vertex-bound.

#### Vertex and Compute Utilization
> **Property:** [`GpuShaderComputeUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderComputeUtilization) | **HWCPipe:** `MaliNonFragUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of time the compute pipeline is active processing vertex and compute workloads.

**Performance Impact**: High utilization indicates vertex/compute-bound performance. Should approach 100% when vertex/compute is the bottleneck.

**Optimization**: For high utilization, optimize vertex/compute shaders. For low utilization with high fragment utilization, workload is properly balanced toward fragment processing.

#### Fragment FPK Active Cycles
> **Property:** [`GpuFragmentFPKActiveCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentFPKActiveCycles) | **HWCPipe:** `MaliFragFPKActiveCy` | **Units:** `Cycles`

**Description**: Number of cycles the Fragment Forward Pixel Kill (FPK) unit is active performing hidden surface removal.

**Performance Impact**: FPK activity indicates hidden surface removal efficiency. Higher activity means more effective occlusion culling at the hardware level.

**Optimization**: FPK efficiency is generally beneficial as it removes hidden fragments before shading. Focus on overall scene organization and depth complexity rather than FPK optimization.

### Functional Unit Utilization

#### Arithmetic Unit Cycles
> **Property:** [`GpuShaderArithmeticCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderArithmeticCycles) | **HWCPipe:** `MaliALUIssueCy` | **Units:** `Cycles`

**Description**: Number of cycles the arithmetic (ALU) unit is active processing mathematical operations.

**Performance Impact**: High usage indicates compute-intensive shaders with many mathematical operations. ALU-bound workloads benefit from instruction optimization.

**Optimization**: Use precision lowering (mediump/lowp), reduce complex mathematical operations, optimize loop structures, and consider lookup tables for expensive functions.

#### Arithmetic Unit Utilization
> **Property:** [`GpuShaderArithmeticUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderArithmeticUtilization) | **HWCPipe:** `MaliALUUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of shader core cycles spent on arithmetic operations.

**Performance Impact**: High utilization indicates compute-bound shaders. Low utilization suggests stalls from memory access, texture sampling, or instruction dependencies.

**Optimization**: Balance arithmetic with texture/memory operations. For high utilization, simplify calculations. For low utilization, investigate memory access patterns and instruction scheduling.

#### Texture Unit Cycles
> **Property:** [`GpuShaderTextureCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderTextureCycles) | **HWCPipe:** `MaliTexIssueCy` | **Units:** `Cycles`

**Description**: Number of cycles the texture unit is active performing texture filtering operations. Different filtering modes require multiple cycles:
- 2D bilinear filtering: 2 cycles per quad
- 2D trilinear filtering: 4 cycles per quad
- 3D trilinear filtering: 8 cycles per quad

**Performance Impact**: High usage suggests texture bandwidth bottleneck or excessive texture sampling complexity.

**Optimization**: Reduce texture resolution, use compressed texture formats, optimize texture cache locality with atlasing, and simplify filtering requirements when possible.

#### Texture Unit Utilization
> **Property:** [`GpuShaderTextureUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderTextureUtilization) | **HWCPipe:** `MaliTexUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of shader core cycles spent on texture filtering operations.

**Performance Impact**: High utilization indicates texture bandwidth bottleneck. Mobile GPUs have limited texture bandwidth, making efficient texture usage critical.

**Optimization**: Use compressed formats (ASTC, ETC2), reduce texture resolution, improve cache efficiency through atlasing, and minimize complex filtering operations.

#### Load/Store Unit Cycles
> **Property:** [`GpuShaderLoadStoreCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderLoadStoreCycles) | **HWCPipe:** `MaliLSIssueCy` | **Units:** `Cycles`

**Description**: Number of cycles the load/store cache unit is active processing memory operations including varying interpolation, uniform buffer access, and storage buffer operations.

**Performance Impact**: High usage suggests memory access bottleneck from excessive varying interpolation, uniform buffer access, or storage operations.

**Optimization**: Reduce varying count between vertex and fragment shaders, use uniform buffer objects efficiently, improve data locality, and minimize storage buffer access patterns.

#### Load/Store Unit Utilization
> **Property:** [`GpuShaderLoadStoreUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderLoadStoreUtilization) | **HWCPipe:** `MaliLSUtil` | **Units:** `Percentage (0-100)`

**Description**: Percentage of shader core cycles spent on memory load/store operations.

**Performance Impact**: High utilization indicates memory access bottleneck from varying interpolation, uniform access, or storage operations.

**Optimization**: Optimize varying usage between stages, implement efficient uniform buffer caching, improve memory access patterns, and reduce complex interpolation requirements.

### Shader Execution Metrics

#### Instructions
> **Property:** [`GpuInstructions`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuInstructions) | **HWCPipe:** `MaliEngInstr` | **Units:** `Count`

**Description**: Total number of shader instructions executed across all shader stages.

**Performance Impact**: High instruction counts indicate complex shaders. Mobile GPUs have limited instruction throughput, making instruction efficiency important.

**Optimization**: Simplify shader logic, use precision lowering, eliminate dead code, and optimize loop structures to reduce total instruction count.

#### Diverged Instructions
> **Property:** [`GpuDivergedInstructions`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuDivergedInstructions) | **HWCPipe:** `MaliEngDivergedInstr` | **Units:** `Count`

**Description**: Number of instructions where different threads in a warp execute different code paths due to branching.

**Performance Impact**: High divergence reduces shader efficiency as threads must execute both branch paths, essentially executing redundant instructions.

**Optimization**: Minimize dynamic branching in shaders, use uniform branches when possible, and design shaders to have consistent execution paths across fragment groups.

---

## Memory Performance Counters

Memory performance counters monitor external memory bandwidth usage, cache efficiency, and memory system stalls. These counters help identify memory bottlenecks and optimize data transfer efficiency.

### Cache Performance

#### Cache Read Lookups
> **Property:** [`GpuCacheReadLookups`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCacheReadLookups) | **HWCPipe:** `MaliL2CacheRdLookup` | **Units:** `Count`

**Description**: Number of L2 cache read lookup operations performed by the GPU.

**Performance Impact**: High values relative to external memory reads indicate good cache efficiency. Measures cache utilization effectiveness for read operations.

**Optimization**: Good cache hit rates are beneficial. Improve data locality, use appropriate data structures, and organize memory access patterns to maximize cache efficiency.

#### Cache Write Lookups
> **Property:** [`GpuCacheWriteLookups`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCacheWriteLookups) | **HWCPipe:** `MaliL2CacheWrLookup` | **Units:** `Count`

**Description**: Number of L2 cache write lookup operations performed by the GPU.

**Performance Impact**: High values relative to external memory writes indicate good cache efficiency. Monitors effectiveness of write combining and cache utilization for output data.

**Optimization**: Efficient write caching reduces external memory pressure. Organize render targets and output patterns to maximize write cache effectiveness.

### External Memory Bandwidth

#### Memory Read Accesses
> **Property:** [`GpuMemoryReadAccesses`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadAccesses) | **HWCPipe:** `MaliExtBusRd` | **Units:** `Count`

**Description**: Number of external memory read access requests initiated by the GPU.

**Performance Impact**: High values indicate memory bandwidth pressure from frequent read operations. Each external memory access has significant latency and power cost.

**Optimization**: Reduce texture sizes, improve cache locality through data organization, use compressed formats, and minimize buffer sizes to reduce read access frequency.

#### Memory Write Accesses
> **Property:** [`GpuMemoryWriteAccesses`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteAccesses) | **HWCPipe:** `MaliExtBusWr` | **Units:** `Count`

**Description**: Number of external memory write access requests initiated by the GPU.

**Performance Impact**: High values indicate memory bandwidth pressure from frequent write operations, typically from framebuffer writes or geometry output.

**Optimization**: Reduce framebuffer complexity and resolution, minimize geometry output complexity, and optimize multi-pass rendering to reduce write access frequency.

#### Memory Read Bytes
> **Property:** [`GpuMemoryReadBytes`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadBytes) | **HWCPipe:** `MaliExtBusRdBt` | **Units:** `Bytes`

**Description**: Total bytes read from external memory (DRAM) by the GPU.

**Performance Impact**: High values indicate memory bandwidth bottleneck. Mobile devices typically sustain ~100MB per frame at 60FPS before bandwidth limitations impact performance.

**Optimization**: Reduce texture sizes, use compressed texture formats (ASTC, ETC2), minimize buffer sizes, optimize vertex data layouts, and implement level-of-detail systems.

#### Memory Write Bytes
> **Property:** [`GpuMemoryWriteBytes`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteBytes) | **HWCPipe:** `MaliExtBusWrBt` | **Units:** `Bytes`

**Description**: Total bytes written to external memory (DRAM) by the GPU.

**Performance Impact**: High values indicate memory bandwidth bottleneck. Accessing external DRAM costs 80-100mW per GB/s, significantly impacting mobile power consumption.

**Optimization**: Reduce framebuffer sizes and complexity, minimize geometry complexity to reduce vertex buffer writes, and optimize multi-pass rendering to reduce total write bandwidth.

### Memory Stall Analysis

#### Memory Read Stalled Cycles
> **Property:** [`GpuMemoryReadStalledCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadStalledCycles) | **HWCPipe:** `MaliExtBusRdStallCy` | **Units:** `Cycles`

**Description**: Number of GPU cycles stalled waiting for external memory read transactions to complete.

**Performance Impact**: High stall cycles indicate content requesting more read data than the memory system can provide efficiently. Directly reduces effective GPU utilization.

**Optimization**: Reduce total texture and buffer sizes, improve cache hit rates through better data locality, and implement more efficient data compression strategies.

#### Memory Read Stall Rate
> **Property:** [`GpuMemoryReadStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadStallRate) | **HWCPipe:** `MaliExtBusRdStallRate` | **Units:** `Percentage (0-100)`

**Description**: Percentage of GPU cycles stalled waiting for external memory read transactions to complete.

**Performance Impact**: High stall rates indicate content requesting more data than the memory system can provide efficiently. Reduces effective GPU utilization.

**Optimization**: Reduce total data resource sizes (textures, buffers), improve cache hit rates through better data locality, and implement more efficient data compression strategies.

#### Memory Write Stalled Cycles
> **Property:** [`GpuMemoryWriteStalledCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStalledCycles) | **HWCPipe:** `MaliExtBusWrStallCy` | **Units:** `Cycles`

**Description**: Number of GPU cycles stalled waiting for external memory write transactions to complete.

**Performance Impact**: High stall cycles indicate memory bandwidth bottleneck on the write side, often from complex framebuffers or excessive geometry output.

**Optimization**: Reduce framebuffer complexity and resolution, minimize geometry output complexity, optimize multi-pass rendering patterns, and reduce overdraw to minimize write bandwidth requirements.

#### Memory Write Stall Rate
> **Property:** [`GpuMemoryWriteStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStallRate) | **HWCPipe:** `MaliExtBusWrStallRate` | **Units:** `Percentage (0-100)`

**Description**: Percentage of GPU cycles stalled waiting for external memory write transactions to complete.

**Performance Impact**: High stall rates indicate memory bandwidth bottleneck on the write side, often from complex framebuffers or excessive geometry output.

**Optimization**: Reduce framebuffer complexity and resolution, minimize geometry output complexity, optimize multi-pass rendering patterns, and reduce overdraw to minimize write bandwidth requirements. Implement more efficient data compression strategies.

---

## Performance Analysis Workflow

### Systematic Bottleneck Identification

1. **Start with GPU Activity**: Check `GpuCycles` and overall utilization to confirm GPU-bound performance
2. **Identify Primary Queue**: Compare `GpuFragmentQueueUtilization` vs `GpuNonFragmentQueueUtilization` to identify dominant workload
3. **Analyze Content Efficiency**: Review primitive culling rates and early-Z efficiency to identify content optimization opportunities
4. **Examine Functional Units**: Check arithmetic, texture, and load/store utilization to identify shader corec bottlenecks
5. **Validate Memory Performance**: Monitor bandwidth usage and stall rates to identify memory limitations

### Mobile-Specific Considerations

- **Power Efficiency**: Memory bandwidth has significant power cost (~80-100mW per GB/s)
- **Thermal Throttling**: Sustained high GPU utilization may trigger frequency reduction
- **Cycle Budgets**: Mobile GPUs have strict cycle budgets; optimize for efficiency over peak performance
- **Battery Life**: Balance visual quality with energy consumption for longer battery life

### Integration with Unity Profiler

Use `ProfilerRecorder` to capture counter data:

```csharp
using Unity.Profiling;
using Unity.Profiling.SystemMetrics;

var recorder = ProfilerRecorder.StartNew(
    SystemMetricsMali.Instance.Category,
    SystemMetricsMali.Instance.GpuCycles
);
```

Counters integrate seamlessly with Unity's profiling ecosystem, enabling automated performance analysis and regression detection.
