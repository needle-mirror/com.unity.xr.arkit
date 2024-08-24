#pragma once

// Various AR Subsystems have GetNativePtr methods on them, which return
// pointers to the following structs. The first field will always
// be a version number, so code which tries to interpret the native
// pointers can safely check the version prior to casting to the
// appropriate struct.

// XRSessionExtensions.GetNativePtr
typedef struct UnityXRNativeSession_1
{
    int version;
    void* sessionPtr;
} UnityXRNativeSession_1;

typedef struct UnityXRNativeFrame_1
{
    int version;
    void* framePtr;
} UnityXRNativeFrame_1;

// XRPlaneExtensions.GetNativePtr
typedef struct UnityXRNativePlane_1
{
    int version;
    void* planePtr;
} UnityXRNativePlane_1;

// XRReferencePointExtensions.GetNativePtr
typedef struct UnityXRNativeAnchor_1
{
    int version;
    void* anchorPtr;
} UnityXRNativeAnchor_1;

typedef struct UnityXRNativePointCloud_1
{
    int version;
    void* pointCloud;
} UnityXRNativePointCloud_1;

typedef struct UnityXRNativeEnvironmentProbe_1
{
    int version;
    void* environmentProbeAnchor;
} UnityXRNativeEnvironmentProbe_1;

typedef struct UnityXRNativeImage_1
{
    int version;
    void* imageAnchor;
} UnityXRNativeImage_1;

typedef struct UnityXRNativeTrackedObject_1
{
    int version;
    void* objectAnchor;
} UnityXRNativeTrackedObject_1;

typedef struct UnityXRNativeHumanBody_1
{
    int version;
    void* humanBodyPtr;
} UnityXRNativeHumanBody_1;

typedef struct UnityXRNativeParticipant_1
{
    int version;
    void* participantAnchor;
} UnityXRNativeParticipant_1;

typedef struct UnityXRNativeRaycast_1
{
    int version;
    void* raycastQuery;
    void* trackedRaycast;
    void* raycastResults;
} UnityXRNativeRaycast_1;

static const int kUnityXRNativeSessionVersion = 1;
static const int kUnityXRNativeFrameVersion = 1;
static const int kUnityXRNativePlaneVersion = 1;
static const int kUnityXRNativeAnchorVersion = 1;
static const int kUnityXRNativePointCloudVersion = 1;
static const int kUnityXRNativeEnvironmentProbeVersion = 1;
static const int kUnityXRNativeImageVersion = 1;
static const int kUnityXRNativeTrackedObjectVersion = 1;
static const int kUnityXRNativeHumanBodyVersion = 1;
static const int kUnityXRNativeParticipantVersion = 1;
static const int kUnityXRNativeRaycastVersion = 1;

typedef UnityXRNativeSession_1 UnityXRNativeSession;
typedef UnityXRNativeFrame_1 UnityXRNativeFrame;
typedef UnityXRNativePlane_1 UnityXRNativePlane;
typedef UnityXRNativeAnchor_1 UnityXRNativeAnchor;
typedef UnityXRNativePointCloud_1 UnityXRNativePointCloud;
typedef UnityXRNativeEnvironmentProbe_1 UnityXRNativeEnvironmentProbe;
typedef UnityXRNativeImage_1 UnityXRNativeImage;
typedef UnityXRNativeTrackedObject_1 UnityXRNativeTrackedObject;
typedef UnityXRNativeHumanBody_1 UnityXRNativeHumanBody;
typedef UnityXRNativeParticipant_1 UnityXRNativeParticipant;
typedef UnityXRNativeRaycast_1 UnityXRNativeRaycast;
