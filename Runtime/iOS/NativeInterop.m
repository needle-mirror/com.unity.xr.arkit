#import <ARKit/ARKit.h>

void* NSObject_get_description(void* self) {
    return (__bridge_retained void*)((__bridge NSObject*)self).description;
}

bool NSObject_isEqual_(void* self, void* other) {
    return [(__bridge NSObject*)self isEqual:(__bridge NSObject*)other];
}

uint64_t NSObject_get_hash(void* self) {
    return ((__bridge NSObject*)self).hash;
}

uint64_t NSString_lengthOfBytesUsingUTF16Encoding(void* self) {
    return [(__bridge NSString*)self lengthOfBytesUsingEncoding:NSUTF16StringEncoding];
}

void NSString_getBytes_maxLength_(void* self, void* buffer, uint64_t maxLength) {
    NSString* string = (__bridge NSString*)self;
    [string getBytes:buffer
           maxLength:maxLength
          usedLength:nil
            encoding:NSUTF16StringEncoding
             options:NSStringEncodingConversionAllowLossy
               range:NSMakeRange(0, string.length)
      remainingRange:nil];
}

uint64_t NSString_get_length(void* self) {
    return ((__bridge NSString*)self).length;
}

int64_t NSError_get_code(void* self) {
    return ((__bridge NSError*)self).code;
}

void* NSError_get_domain(void* self) {
    return (__bridge_retained void*)((__bridge NSError*)self).domain;
}

void* NSError_get_localizedDescription(void* self) {
    return (__bridge_retained void*)((__bridge NSError*)self).localizedDescription;
}

void* NSError_get_localizedRecoverySuggestion(void* self) {
    return (__bridge_retained void*)((__bridge NSError*)self).localizedRecoverySuggestion;
}

void* NSError_get_localizedFailureReason(void* self) {
    return (__bridge_retained void*)((__bridge NSError*)self).localizedFailureReason;
}

void* UnityARKit_GetARErrorDomain() {
    if (@available(iOS 11, *)) {
        return (__bridge void*)ARErrorDomain;
    }
    return NULL;
}

void* UnityARKit_GetCLErrorDomain() {
    return (__bridge void*)kCLErrorDomain;
}
