# Performance counters provided by System Metrics Mali package

## GPU Utilization

### GPU Active Cycles
> **ID:** [`GpuCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCycles) | **Units:** `Cycles`

This counter shows overall GPU processing load requested by the application. The counter increments every clock cycle that the GPU has any pending workload active, queued or stalled. Any of these states are counted as "active" time even if no forward progress was made.

### Vertex And Compute Cycles
> **ID:** [`GpuVertexAndComputeCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexAndComputeCycles) | **Units:** `Cycles`

The number of GPU clock cycles where the GPU was busy computing one of the following: vertex shaders, tessellation shaders, geometry shaders, fixed function tiling, compute shaders. This counter will include GPU cycles for the specified workloads even if the GPU is stalled waiting for external memory to return data; this is still counted towards the load even though no forward progress is being made. 

### Vertex And Compute Utilization
> **ID:** [`GpuVertexAndComputeUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexAndComputeUtilization) | **Units:** `Percentage`

This counter indicates percentage of time GPU were busy processing vertex shaders, tessellation shaders, geometry shaders, fixed function tiling, or compute shaders. This counter will include GPU cycles for the specified workloads even if the GPU is stalled waiting for external memory to return data; this is still counted as towards the load even though no forward progress is being made.

### Fragment Cycles
> **ID:** [`GpuFragmentCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentCycles) | **Units:** `Cycles`

The number of GPU clock cycles where the GPU calculates any type of fragment shader operation. This counter increments every clock cycle where the GPU has any workload present in the fragment queue. This counter will include GPU cycles where a workload is loaded into a queue even if the GPU is stalled waiting for external memory to return data; this is still counted as active time even though no forward progress is being made. For most content there are orders of magnitude more fragments than vertices, so this counter will usually be the highest processing load. 

### Fragment Utilization
> **ID:** [`GpuFragmentUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentUtilization) | **Units:** `Percentage`

This counter indicates percentage of time GPU were busy processing fragment shaders. This counter increments every clock cycle where the GPU has any workload present in the fragment queue. This counter will include GPU cycles where a workload is loaded into a queue even if the GPU is stalled waiting for external memory to return data; this is still counted as active time even though no forward progress is being made. For most content there are orders of magnitude more fragments than vertices, so this counter will usually be the highest processing load. 


## Scene Composition

### Input Primitives
> **ID:** [`GpuInputPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuInputPrimitives) | **Units:** `Primitives`

Number of primitives sent to GPU for rendering.

### Culled Primitives
> **ID:** [`GpuCulledPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCulledPrimitives) | **Units:** `Primitives`

Number of primitives that are culled by GPU due to XY clipping. GPU culls a primitive if it is outside the visible region of the render surface horizontally or vertically. These primitives are ignored by the rasterizer. 

### % Culled Primitives
> **ID:** [`GpuCulledPrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCulledPrimitivesPercentage) | **Units:** `Percentage`

Percentage of primitives that are culled by GPU due to XY clipping. GPU culls a primitive if it is outside the visible region of the render surface horizontally or vertically. These primitives are ignored by the rasterizer. 

### Clipped Primitives
> **ID:** [`GpuClippedPrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuClippedPrimitives) | **Units:** `Primitives`

Number of primitives discarded by the GPU due to Z clipping. GPU rasterizer ignores a primitive if it is outside the near and far clip planes region specified for the render surface.

### % Clipped Primitives
> **ID:** [`GpuClippedPrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuClippedPrimitivesPercentage) | **Units:** `Percentage`

Percentage of primitives discarded by the GPU due to Z clipping. GPU rasterizer ignores a primitive if it is outside the near and far clip planes region specified for the render surface.

### Visible Primitives
> **ID:** [`GpuVisiblePrimitives`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVisiblePrimitives) | **Units:** `Primitives`

The number of primitives that are visible after culling.

### % Visible Primitives
> **ID:** [`GpuVisiblePrimitivesPercentage`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVisiblePrimitivesPercentage) | **Units:** `Percentage`

The percentage of primitives that are visible after culling.


## Shader Core Utilization

### Shader Vertex And Compute Cycles
> **ID:** [`GpuShaderComputeCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderComputeCycles) | **Units:** `Cycles`

The number of cycles compute shader pipeline was active. The counter increases whenever shader core is processing any compute workload. Compute workload includes vertex shaders, tessellation shaders, geometry shaders, fixed function tiling and compute shaders. Active processing includes any cycle where compute work is running anywhere in the fixed-function front-end or the in the programmable core.

### Shader Vertex And Compute Utilization
> **ID:** [`GpuShaderComputeUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderComputeUtilization) | **Units:** `Percentage`

This counter indicates overall utilization of compute shader pipeline. The counter increases whenever shader core is processing any compute workload. Compute workload includes vertex shaders, tessellation shaders, geometry shaders, fixed function tiling and compute shaders. Active processing includes any cycle where compute work is running anywhere in the fixed-function front-end or the in the programmable core.

### Shader Fragment Cycles
> **ID:** [`GpuShaderFragmentCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderFragmentCycles) | **Units:** `Cycles`

The number of cycles fragment pipeline was active. The counter includes all cycles when at least one fragment task is active anywhere inside the shader core, including the fixed-function fragment frontend, the programmable tripipe, or the fixed-function fragment backend.

### Shader Fragment Utilization
> **ID:** [`GpuShaderFragmentUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderFragmentUtilization) | **Units:** `Percentage`

This counter indicates overall utilization of fragment pipeline. The counter includes all cycles when at least one fragment task is active anywhere inside the shader core, including the fixed-function fragment frontend, the programmable tripipe, or the fixed-function fragment backend.

### Shader Arithmetic Cycles
> **ID:** [`GpuShaderArithmeticCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderArithmeticCycles) | **Units:** `Cycles`

The number of cycles shader core arithmetic unit was active. The counter increases whenever shader core is processing any arithmetic operations. Active processing includes any cycle where arithmetic work is running anywhere in the fixed-function front-end or in the programmable core.
### Shader Arithmetic Utilization
> **ID:** [`GpuShaderArithmeticUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderArithmeticUtilization) | **Units:** `Percentage`

This counter shows shader core arithmetic unit load. The counter increases whenever shader core is processing any arithmetic operations. Active processing includes any cycle where arithmetic work is running anywhere in the fixed-function front-end or the in the programmable core.

### Shader Texture Cycles
> **ID:** [`GpuShaderTextureCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderTextureCycles) | **Units:** `Cycles`

The number of cycles shader core texturing unit was active. Different instructions might take more than one cycle due to multi-cycle data access and filtering operations:
 - 2D bilinear filtering takes two cycles per quad. 
 - 2D trilinear filtering takes four cycles per quad. 
 - 3D bilinear filtering takes four cycles per quad. 
 - 3D trilinear filtering takes eight cycles per quad.

### Shader Texture Utilization
> **ID:** [`GpuShaderTextureUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderTextureUtilization) | **Units:** `Percentage`

This counter indicates shader core texturing unit load. Different instructions might take more than one cycle due to multi-cycle data access and filtering operations:
 - 2D bilinear filtering takes two cycles per quad. 
 - 2D trilinear filtering takes four cycles per quad. 
 - 3D bilinear filtering takes four cycles per quad. 
 - 3D trilinear filtering takes eight cycles per quad.

### Shader Load/Store Cycles
> **ID:** [`GpuShaderLoadStoreCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderLoadStoreCycles) | **Units:** `Cycles`

The number of cycles shader core load/store cache unit was active. This counter increments for every load/store cache read

### Shader Load/Store Utilization
> **ID:** [`GpuShaderLoadStoreUtilization`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuShaderLoadStoreUtilization) | **Units:** `Percentage`

This counter indicates shader core load/store cache unit load. This counter increments for every load/store cache read


## Z and Stencil Test Stats

### Early ZS Tests
> **ID:** [`GpuEarlyZTests`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuEarlyZTests) | **Units:** `Pixels`

The number of depth and/or stencil tests performed before fragments processing.

### Early ZS Kills
> **ID:** [`GpuEarlyZKills`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuEarlyZKills) | **Units:** `Pixels`

The number of depth and/or stencil tests performed before fragments processing that resulted in a kill.

### Late ZS Tests
> **ID:** [`GpuLateZTests`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuLateZTests) | **Units:** `Pixels`

The number of depth and/or stencil tests performed after fragments processing.

### Late ZS Kills
> **ID:** [`GpuLateZKills`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuLateZKills) | **Units:** `Pixels`

The number of depth and/or stencil tests performed after fragments processing that resulted in a kill.


## Memory Stats

### Cache Read Lookups
> **ID:** [`GpuCacheReadLookups`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCacheReadLookups) | **Units:** `Count`

The number of cache read lookups the GPU performs.

### Cache Write Lookups
> **ID:** [`GpuCacheWriteLookups`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuCacheWriteLookups) | **Units:** `Count`

The number of cache write lookups the GPU performs.

### Memory Read Accesses
> **ID:** [`GpuMemoryReadAccesses`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadAccesses) | **Units:** `Count`

The number of time the GPU reads from external memory

### Memory Write Accesses
> **ID:** [`GpuMemoryWriteAccesses`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteAccesses) | **Units:** `Count`

The number of time the GPU writes to external memory

### Memory Read Stalled Cycles
> **ID:** [`GpuMemoryReadStalledCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadStalledCycles) | **Units:** `Cycles`

The number of cycles L2 cache is stalled waiting for data to be read from system memory

### % Memory Read Stalled
> **ID:** [`GpuMemoryReadStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryReadStallRate) | **Units:** `Percentage`

This counter indicates percentage of cycles L2 cache is stalled waiting for data to be read from system memory

### Memory Write Stalled Cycles
> **ID:** [`GpuMemoryWriteStalledCycles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStalledCycles) | **Units:** `Cycles`

The number of cycles L2 cache is stalled waiting for data to be written to system memory

### % Memory Write Stalled
> **ID:** [`GpuMemoryWriteStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStallRate) | **Units:** `Percentage`

This counter indicates percentage of cycles L2 cache is stalled waiting for data to be written to system memory

### Memory Read Bytes
> **ID:** [`GpuMemoryWriteStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStallRate) | **Units:** `Bytes`

The number of bytes the GPU reads to external memory

### Memory Write Bytes
> **ID:** [`GpuMemoryWriteStallRate`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuMemoryWriteStallRate) | **Units:** `Bytes`

The number of bytes the GPU writes to external memory

## Other stats

### Vertex And Compute Jobs
> **ID:** [`GpuVertexComputeJobs`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuVertexComputeJobs) | **Units:** `Jobs`

The number of vertex and compute jobs executed.

### Fragment Jobs
> **ID:** [`GpuFragmentJobs`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuFragmentJobs) | **Units:** `Jobs`

The number of fragment jobs executed.

### Tiles
> **ID:** [`GpuTiles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuTiles) | **Units:** `Tiles`

The number of physical tiles written by the GPU.

### Unchanged Eliminated Tiles
> **ID:** [`GpuUnchangedEliminatedTiles`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuUnchangedEliminatedTiles) | **Units:** `Tiles`

The number of physical tile writes that are skipped because the content of the tile matches the content already in memory.

### Pixels
> **ID:** [`GpuPixels`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuPixels) | **Units:** `Pixels`

The number of pixels the GPU shaded.

### Instructions
> **ID:** [`GpuInstructions`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuInstructions) | **Units:** `Instructions`

The number of shader instructions the GPU executes.

### Diverged Instructions
> **ID:** [`GpuDivergedInstructions`](../api/Unity.Profiling.SystemMetrics.SystemMetricsMali.html#Unity_Profiling_SystemMetrics_SystemMetricsMali_GpuDivergedInstructions) | **Units:** `Instructions`

The number of diverged shader instructions the GPU executes.
