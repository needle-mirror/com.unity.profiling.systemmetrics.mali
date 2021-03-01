# Metrics provided by System Metrics Mali package

| SystemMetricsMali property        | Counter name                           | Description                                                |
|-----------------------------------|----------------------------------------|------------------------------------------------------------|
| GpuActive                         | GPU Active Cycles                      | Number of cycles GPU was active |
| GpuNonFragmentActive              | GPU Vertex And Compute Active Cycles   | Number of cycles GPU was busy with *vertex/compute* workload |
| GpuFragmentActive                 | GPU Fragment Active Cycles             | Number of cycles GPU was busy with *fragment* workload |
| GpuTilerActive                    | GPU Tiler Active Cycles                | Number of cycles GPU *tiler* was active |
|                                   |                                        |                                                            |
| GpuVertexComputeJobs              | GPU Vertex And Compute Jobs            | Number of vertex/compute jobs                              |
| GpuTiles                          | GPU Tiles                              | Number of physical tiles written                           |
| GpuTransactionEliminations        | GPU Transaction Eliminations           | Number of transaction eliminations                         |
| GpuFragmentJobs                   | GPU Fragment Jobs                      | Number of fragment jobs                                    |
| GpuPixels                         | GPU Pixels                             | Number of pixels shaded                                    |
|                                   |                                        |                                                            |
| GpuEarlyZTests                    | GPU Early Z Tests                      | Early-Z tests performed                                    |
| GpuEarlyZKilled                   | GPU Early Z Killed                     | Early-Z tests resulting in a kill                          |
| GpuLateZTests                     | GPU Late Z Tests                       | Late-Z tests performed                                     |
| GpuLateZKilled                    | GPU Late Z Killed                      | Late-Z tests resulting in a kill                           |
|                                   |                                        |                                                            |
| GpuInstructions                   | GPU Instructions                       | Number of shader instructions                              |
| GpuDivergedInstructions           | GPU Diverged Instructions              | Number of diverged shader instructions                     |
|                                   |                                        |                                                            |
| GpuShaderCycles                   | GPU Shader Cycles                      | Shader total cycles                                        |
| GpuShaderArithmeticCycles         | GPU Shader Arithmetic Cycles           | Shader arithmetic cycles                                   |
| GpuShaderLoadStoreCycles          | GPU Shader Load Store Cycles           | Shader load/store cycles                                   |
| GpuShaderTextureCycles            | GPU Shader Texture Cycles              | Shader texture cycles                                      |
|                                   |                                        |                                                            |
| GpuCacheReadLookups               | GPU Cache Read Lookups                 | Cache read lookups                                         |
| GpuCacheWriteLookups              | GPU Cache Write Lookups                | Cache write lookups                                        |
| GpuExternalMemoryReadAccesses     | GPU External Memory Read Accesses      | Reads from external memory                                 |
| GpuExternalMemoryWriteAccesses    | GPU External Memory Write Accesses     | Writes to external memory                                  |
| GpuExternalMemoryReadStalls       | GPU External Memory Read Stalls        | Stalls when reading from external memory                   |
| GpuExternalMemoryWriteStalls      | GPU External Memory Write Stalls       | Stalls when writing to external memory                     |
| GpuExternalMemoryReadBytes        | GPU External Memory Read Bytes         | Bytes read to external memory                              |
| GpuExternalMemoryWriteBytes       | GPU External Memory Write Bytes        | Bytes written to external memory                           |
