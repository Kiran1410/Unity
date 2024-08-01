
#import "Bridge.h"

@interface Bridge()

- (void)messageFromGE:(NSString *)message;

@end

@implementation Bridge

+ (instancetype)shared {
    static id instance;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [[self alloc] init];
    });
    return instance;
}

- (void)messageFromGE:(NSString *)message {
    
    if (_delegate != nil) {
        [_delegate didRecievedMessage:message];
    }
}

- (void)send:(NSString *)message {
    
    UnitySendMessage("GameManager", "messageFromNative", [message UTF8String]);
}

@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {

    void messageFromGE(const char* message)
    {
        NSString *ns_message = CreateNSString(message);
        
        [[Bridge shared] messageFromGE:ns_message];
    }
}

