using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Profiling.SystemMetrics.Tests
{
    class InstanceTests
    {
        [Test]
        public void Instance_NotNull()
        {
            Assert.NotNull(SystemMetricsMali.Instance);
        }

        [Test, UnityPlatform(exclude = new[] { RuntimePlatform.Android })]
        public void Active_ReturnsFalseEverywhereButAndroid()
        {
            Assert.IsFalse(SystemMetricsMali.Instance.Active);
        }

        [Test, UnityPlatform(include = new[] { RuntimePlatform.Android })]
        public void Active_ReturnsTrueOnAndroid()
        {
            Assert.IsTrue(SystemMetricsMali.Instance.Active);
        }
    }
}