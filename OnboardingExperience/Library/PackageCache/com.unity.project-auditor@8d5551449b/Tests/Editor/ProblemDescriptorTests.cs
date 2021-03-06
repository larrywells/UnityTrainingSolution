using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unity.ProjectAuditor.Editor;
using Unity.ProjectAuditor.Editor.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.ProjectAuditor.EditorTests
{
    class ProblemDescriptorTests
    {
        [Test]
        public void ProblemDescriptor_Comparison_Works()
        {
            var a = new ProblemDescriptor
                (
                102001,
                "test",
                Area.CPU,
                "this is not actually a problem",
                "do nothing"
                );
            var b = new ProblemDescriptor
                (
                102001,
                "test",
                Area.CPU,
                "this is not actually a problem",
                "do nothing"
                );

            Assert.True(a.Equals(a));
            Assert.True(a.Equals((object)a));
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
            b = null;
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
        }

        [Test]
        public void ProblemDescriptor_Hash_IsId()
        {
            var p = new ProblemDescriptor
                (
                102001,
                "test",
                Area.CPU,
                "this is not actually a problem",
                "do nothing"
                );

            Assert.True(p.GetHashCode() == p.id);
        }

        [Test]
        public void ProblemDescriptor_Version_IsCompatible()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test",
                Area.CPU,
                "this is not actually a problem",
                "do nothing"
                );

            // check default values
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = string.Empty;
            desc.maximumVersion = string.Empty;
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = "0.0";
            desc.maximumVersion = null;
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = null;
            desc.maximumVersion = "0.0";
            Assert.False(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = null;
            desc.maximumVersion = "9999.9";
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = "9999.9";
            desc.maximumVersion = null;
            Assert.False(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = InternalEditorUtility.GetUnityVersion().ToString();
            desc.maximumVersion = null;
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = null;
            desc.maximumVersion = InternalEditorUtility.GetUnityVersion().ToString();
            Assert.True(ProblemDescriptorLoader.IsVersionCompatible(desc));

            desc.minimumVersion = "1.1";
            desc.maximumVersion = "1.0";
            var result = ProblemDescriptorLoader.IsVersionCompatible(desc);
            LogAssert.Expect(LogType.Error, "Descriptor (102001) minimumVersion (1.1) is greater than maximumVersion (1.0).");
            Assert.False(result);
        }

        [Test]
        public void ProblemDescriptor_Area_IsDefault()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test"
                );
            Assert.AreEqual(1, desc.GetAreas().Length);
            Assert.Contains(Area.Info, desc.GetAreas());
        }

        [Test]
        public void ProblemDescriptor_MultipleAreas_AreCorrect()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test",
                new[] {Area.CPU, Area.Memory},
                "this is not actually a problem",
                "do nothing"
                );
            Assert.AreEqual(2, desc.GetAreas().Length);
            Assert.Contains(Area.CPU, desc.GetAreas());
            Assert.Contains(Area.Memory, desc.GetAreas());
        }

        [Test]
        public void ProblemDescriptor_AnyPlatform_IsCompatible()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test",
                new[] {Area.CPU}
                );

            Assert.True(ProblemDescriptorLoader.IsPlatformCompatible(desc));
        }

        [Test]
        public void ProblemDescriptor_Platform_IsCompatible()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test",
                new[] {Area.CPU}
                )
            {
#if UNITY_EDITOR_WIN
                platforms = new[] { BuildTarget.StandaloneWindows64.ToString() }
#else
                platforms = new[] { BuildTarget.StandaloneOSX.ToString() }
#endif
            };

            Assert.True(ProblemDescriptorLoader.IsPlatformCompatible(desc));
        }

        [Test]
        public void ProblemDescriptor_Platform_IsNotCompatible()
        {
            var desc = new ProblemDescriptor
                (
                102001,
                "test",
                new[] {Area.CPU}
                )
            {
                platforms = new[] { BuildTarget.Android.ToString() }  // assuming Android is not installed by default
            };

            Assert.False(ProblemDescriptorLoader.IsPlatformCompatible(desc));
        }

        [Test]
        [TestCase("ApiDatabase")]
        [TestCase("ProjectSettings")]
        public void ProblemDescriptor_Descriptors_AreCorrect(string jsonFilename)
        {
            var descriptors = ProblemDescriptorLoader.LoadFromJson(Editor.ProjectAuditor.DataPath, jsonFilename);
            foreach (var descriptor in descriptors)
            {
                Assert.Positive(descriptor.id);
                Assert.NotNull(descriptor.areas);
            }
        }

        [Test]
        [TestCase("ApiDatabase")]
        [TestCase("ProjectSettings")]
        public void ProblemDescriptor_TypeAndMethods_Exist(string jsonFilename)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).ToArray();
            var skippableMethodNames = new[]
            {
                "*",
                "OnGUI",
                "OnTriggerStay",
                "OnTriggerStay2D",
                "OnCollisionStay"
            };

            var descriptors = ProblemDescriptorLoader.LoadFromJson(Editor.ProjectAuditor.DataPath, jsonFilename);
            foreach (var desc in descriptors)
            {
                var type = types.FirstOrDefault(t => t.FullName.Equals(desc.type));

                Assert.True(desc.method.Equals("*") || type != null, desc.type);

                if (skippableMethodNames.Contains(desc.method))
                    continue;

                try
                {
                    Assert.True(type.GetMethod(desc.method) != null || type.GetProperty(desc.method) != null, "{0} does not belong to {1}", desc.method, desc.type);
                }
                catch (AmbiguousMatchException)
                {
                    // as long as there is a match, this is fine
                }
            }
        }

#if UNITY_2019_1_OR_NEWER
        [Test]
        [TestCase("ApiDatabase")]
        [TestCase("ProjectSettings")]
        public void ProblemDescriptor_Areas_Exist(string jsonFilename)
        {
            var descriptors = ProblemDescriptorLoader.LoadFromJson_Internal(Editor.ProjectAuditor.DataPath, jsonFilename);
            foreach (var desc in descriptors)
            {
                for (int i = 0; i < desc.areas.Length; i++)
                {
                    Area area;
                    Assert.True(Enum.TryParse(desc.areas[i], out area), "Invalid area {0} for descriptor {1}", desc.areas[i], desc.id);
                }
            }
        }

#endif
    }
}
