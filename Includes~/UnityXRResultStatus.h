#pragma once

// Must match UnityEngine.XR.ARSubsystems.XRResultStatus
struct UXRResultStatus
{
public:
    typedef enum UXRStatusCode
    {
        kStatusPlatformQualifiedSuccess = 1,
        kStatusUnqualifiedSuccess = 0,
        kStatusPlatformError = -1,
        kStatusUnknownError = -2,
        kStatusProviderUninitialized = -3,
        kStatusProviderNotStarted = -4,
        kStatusValidationFailure = -5,
    } UXRStatusCode;

    UXRStatusCode statusCode;
    int nativeStatusCode;

    bool IsError()
    {
        return statusCode < 0;
    }

    bool IsSuccess()
    {
        return statusCode >= 0;
    }
};

class UXRResultStatusUtils
{
public:
    inline static UXRResultStatus Create(UXRResultStatus::UXRStatusCode statusCode, int nativeStatusCode)
    {
        return UXRResultStatus{ statusCode, nativeStatusCode };
    }

    inline static UXRResultStatus Create(UXRResultStatus::UXRStatusCode statusCode)
    {
        return UXRResultStatus{ statusCode, 0 };
    }
};
