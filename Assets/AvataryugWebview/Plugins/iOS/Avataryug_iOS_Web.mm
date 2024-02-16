#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>

@interface MyPlugin: NSObject <WKNavigationDelegate>
{
    NSDate *creationDate;
    UIPopoverController *popover;
    WKWebView *webView;
}
@end

@implementation MyPlugin

static MyPlugin *_sharedInstance;

+(MyPlugin*) sharedInstance
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _sharedInstance = [[MyPlugin alloc] init];
    });
    return _sharedInstance;
}

+(NSString*)createNSString:(const char*) string {
    if (string!=nil)
        return [NSString stringWithUTF8String:string];
    else
        return @"";
}

-(id)init
{
    self = [super init];
    if (self)
        [self initHelper];
    return self;
}

-(void)initHelper
{
    creationDate = [NSDate date];
}

-(void)showWebView:(const char*)URL_in pixelSpace:(int)pixelSpace
{
    UIView *mainView = UnityGetGLView();
    NSString *URL = [MyPlugin createNSString:URL_in];
    pixelSpace /= [UIScreen mainScreen].scale;
    
    WKWebViewConfiguration *configuration = [[WKWebViewConfiguration alloc] init];
    CGRect frame = mainView.frame;
    frame.origin.y += pixelSpace;
    frame.size.height -= pixelSpace;
    webView = [[WKWebView alloc] initWithFrame:frame configuration:configuration];
    webView.navigationDelegate = self;
    
    NSURLRequest *nsrequest=[NSURLRequest requestWithURL:[NSURL URLWithString:URL]];
    [webView loadRequest:nsrequest];
    [mainView addSubview:webView];
}

-(void)hideWebView
{
    if (webView!=nil)
    {
        [webView removeFromSuperview];
        webView = nil;
    }
}

@end

extern "C"
{
    void IOSshowWebView(const char* URL, int pixelSpace)
    {
        [[MyPlugin sharedInstance] showWebView:URL pixelSpace:pixelSpace];
    }
    
    void IOShideWebView()
    {
        [[MyPlugin sharedInstance] hideWebView];
    }
    
    void IOSClearCache()
    {
        NSSet *websiteDataTypes = [NSSet setWithArray:@[
            WKWebsiteDataTypeDiskCache,
            WKWebsiteDataTypeOfflineWebApplicationCache,
            WKWebsiteDataTypeMemoryCache,
            WKWebsiteDataTypeLocalStorage,
            WKWebsiteDataTypeCookies,
            WKWebsiteDataTypeSessionStorage,
            WKWebsiteDataTypeIndexedDBDatabases,
            WKWebsiteDataTypeWebSQLDatabases
        ]];
    
        NSDate *dateFrom = [NSDate dateWithTimeIntervalSince1970:0];
        [[WKWebsiteDataStore defaultDataStore] removeDataOfTypes:websiteDataTypes modifiedSince:dateFrom completionHandler:^{
        }];
    }
}
