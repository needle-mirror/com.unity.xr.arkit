#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

using Inputs = UnityEngine.InputSystem.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.ARKit
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class InputLayoutLoader
    {
#if UNITY_EDITOR
        static InputLayoutLoader()
        {
#if ENABLE_INPUT_SYSTEM && (ENABLE_VR || UNITY_GAMECORE) && !UNITY_FORCE_INPUTSYSTEM_XR_OFF || PACKAGE_DOCS_GENERATION
            RegisterLayouts();
#endif // ENABLE_INPUT_SYSTEM && (ENABLE_VR || UNITY_GAMECORE) && !UNITY_FORCE_INPUTSYSTEM_XR_OFF || PACKAGE_DOCS_GENERATION
        }
#endif // UNITY_EDITOR

#if ENABLE_INPUT_SYSTEM && (ENABLE_VR || UNITY_GAMECORE) && !UNITY_FORCE_INPUTSYSTEM_XR_OFF || PACKAGE_DOCS_GENERATION
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterLayouts()
        {
#if !UNITY_EDITOR
            if (!Api.AtLeast11_0())
                return;
#endif // !UNITY_EDITOR

            Inputs.RegisterLayout<HandheldARInputDevice>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct("(ARKit)")
                );
        }
#endif // ENABLE_INPUT_SYSTEM && (ENABLE_VR || UNITY_GAMECORE) && !UNITY_FORCE_INPUTSYSTEM_XR_OFF || PACKAGE_DOCS_GENERATION
    }
}
#endif
