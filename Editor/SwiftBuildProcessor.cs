#if UNITY_IOS
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace UnityEditor.XR.ARKit
{
    static class ARKitSwiftBuildProcessor
    {
        class Postprocessor : IPostprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; } }

            public void OnPostprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                var buildOutputPath = report.summary.outputPath;

                // Read in the PBXProject
                var project = new PBXProject();
                var pbxProjectPath = PBXProject.GetPBXProjectPath(buildOutputPath);
                project.ReadFromString(File.ReadAllText(pbxProjectPath));

#if UNITY_2019_3_OR_NEWER
                var targetGuid = project.GetUnityFrameworkTargetGuid();
#else
                var targetGuid = project.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif

                project.AddBuildProperty(targetGuid, "SWIFT_VERSION", "5.0");
                project.AddBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");

                // Write out the updated Xcode project file
                File.WriteAllText(pbxProjectPath, project.WriteToString());
            }
        }
    }
}
#endif
