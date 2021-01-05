#if USE_XR_MANAGEMENT
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.XR.ARKit;

using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;

namespace UnityEditor.XR.ARKit
{
    class XRPackage : IXRPackage
    {
        class ARKitLoaderMetadata : IXRLoaderMetadata
        {
            public string loaderName { get; set; }
            public string loaderType { get; set; }
            public List<BuildTargetGroup> supportedBuildTargets { get; set; }
        }

        class ARKitPackageMetadata : IXRPackageMetadata
        {
            public string packageName { get; set; }
            public string packageId { get; set; }
            public string settingsType { get; set; }
            public List<IXRLoaderMetadata> loaderMetadata { get; set; }
        }

        static IXRPackageMetadata s_Metadata = new ARKitPackageMetadata()
        {
                packageName = "ARKit XR Plugin",
                packageId = "com.unity.xr.arkit",
                settingsType = typeof(ARKitLoaderSettings).FullName,
                loaderMetadata = new List<IXRLoaderMetadata>() {
                new ARKitLoaderMetadata() {
                        loaderName = "ARKit",
                        loaderType = typeof(ARKitLoader).FullName,
                        supportedBuildTargets = new List<BuildTargetGroup>()
                        {
                            BuildTargetGroup.iOS
                        }
                    },
                }
            };

        public IXRPackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            return true;
        }
    }
}
#endif
