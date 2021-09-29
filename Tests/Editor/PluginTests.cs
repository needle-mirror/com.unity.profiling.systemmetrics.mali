using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UnityEditor.Profiling.SystemMetrics.Tests
{
    internal class PluginTests
    {
        [Test]
        public void libAndroidHWCP_IsImported()
        {
            bool pluginFound = false;

            PluginImporter[] importers = PluginImporter.GetImporters(BuildTarget.Android);
            foreach (PluginImporter importer in importers)
            {
                if (importer.assetPath.Contains("libAndroidHWCP"))
                {
                    pluginFound = true;
                    break;
                }
            }

            Assert.IsTrue(pluginFound, "Plugins failed to import.");
        }
    }
}