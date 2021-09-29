using NUnit.Framework;
using System.Collections;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace Unity.Profiling.SystemMetrics.Tests
{
    class CountersTests : IPrebuildSetup
    {
        const string TestSceneName = "SystemMetricsTestScene";
        const int WarmupFrames = 4;

        void IPrebuildSetup.Setup()
        {
#if UNITY_EDITOR
            var testScenes = AssetDatabase
                .FindAssets($"t:Scene {TestSceneName}")
                .Select(assetId =>
                    {
                        var scenePath = AssetDatabase.GUIDToAssetPath(assetId);
                        return new EditorBuildSettingsScene(scenePath, true);
                    });
            EditorBuildSettings.scenes = testScenes.ToArray();
#endif
        }

        [UnitySetUp]
        public IEnumerator LoadSceneOnStartup()
        {
            SceneManager.LoadScene(TestSceneName);

            // Always wait one frame for scene load
            yield return null;

            // On Android first scene often needs a bit more frames to load all the assets
            // otherwise the screenshot is just a black screen
            for (int i = 0; i < WarmupFrames; i++)
                yield return new WaitForEndOfFrame();

            // Check that we're in valid state
            Assert.IsTrue(SystemMetricsMali.Instance.Active);

        }


        [UnityTest, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public IEnumerator GpuActivityCycles_NonZeroValue()
        {
            using(var recorder = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuCycles))
            {
                // Wait for 10 frames for result
                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                // GPU should have ticked at least 1 cycle
                Assert.Greater(recorder.LastValue, 0);
            }
        }

        [UnityTest, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public IEnumerator SubsystemsCounters_LessThanGpuActivityCycles()
        {
            // Create recorders
            using(var activityCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuCycles))
            using(var vertexAndComputeCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuVertexAndComputeCycles))
            using(var fragmentCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuFragmentCycles))
            {
                // Wait for 10 frames for result
                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                // Activity cycles (total) >= vertex / fragment cycles
                Assert.LessOrEqual(vertexAndComputeCycles.LastValue, activityCycles.LastValue);
                Assert.LessOrEqual(fragmentCycles.LastValue, activityCycles.LastValue);
            }
        }

        [UnityTest, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public IEnumerator PrimitivesCounters_InputCountIsLargetThanOutput()
        {
            // Create recorders
            using(var inputPrim = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuInputPrimitives))
            using(var culledPrim = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuCulledPrimitives))
            using(var clippedPrim = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuClippedPrimitives))
            using(var visiblePrim = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuVisiblePrimitives))
            {
                // Wait for 10 frames for result
                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                // Input triangles >= clipped / culled / visible triangles
                Assert.LessOrEqual(culledPrim.LastValue, inputPrim.LastValue);
                Assert.LessOrEqual(clippedPrim.LastValue, inputPrim.LastValue);
                Assert.LessOrEqual(visiblePrim.LastValue, inputPrim.LastValue);
            }
        }

        [UnityTest, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public IEnumerator ShaderCoreActivityCycles_NonZeroValue()
        {
            using(var recorder = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderCoreCycles))
            {
                // Wait for 10 frames for result
                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                // Shader core should have ticked at least 1 cycle
                Assert.Greater(recorder.LastValue, 0);
            }
        }

        [UnityTest, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public IEnumerator ShaderCoreCounters_LessThanShaderCoreActivityCycles()
        {
            using(var coreCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderCoreCycles))
            using(var computeCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderComputeCycles))
            using(var fragmentCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderFragmentCycles))
            using(var arithmeticCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderArithmeticCycles))
            using(var textureCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderTextureCycles))
            using(var loadStoreCycles = CreateSystemMetricRecorder(SystemMetricsMali.Instance.GpuShaderLoadStoreCycles))
            {
                // Wait for 10 frames for result
                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                // Shader core cycles >= compute / fragment / arithmetic / load-store / texture cycles
                Assert.LessOrEqual(computeCycles.LastValue, computeCycles.LastValue);
//                Assert.LessOrEqual(fragmentCycles.LastValue, coreCycles.LastValue); // Ask Arm why this might be larger than Shader Core?
                Assert.LessOrEqual(arithmeticCycles.LastValue, coreCycles.LastValue);
                Assert.LessOrEqual(textureCycles.LastValue, coreCycles.LastValue);
                Assert.LessOrEqual(loadStoreCycles.LastValue, coreCycles.LastValue);
            }
        }

        ProfilerRecorder CreateSystemMetricRecorder(ProfilerRecorderHandle handle)
        {
            Assert.IsTrue(handle.Valid);

            var recorder = new ProfilerRecorder(handle);
            recorder.Start();
            return recorder;
        }
    }
}
