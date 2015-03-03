//
//  iOSScreenshot.m
//  
//
//  Created by Ryan on 20/03/2013.
//
//

@implementation ViewController : UIViewController

-(void) shareMethod: (const char *) path: (const char *) shareMessage
{
    NSString *imagePath = [NSString stringWithUTF8String:path];
    
    //    UIImage *image      = [UIImage imageNamed:imagePath];
    UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
    NSString *message   = [NSString stringWithUTF8String:shareMessage];
    NSArray *postItems  = @[message,image];
    
    UIActivityViewController *activityVc = [[UIActivityViewController alloc]initWithActivityItems:postItems applicationActivities:nil];
    
    if ( [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
        
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
    [activityVc release];
}
-(void) shareOnlyTextMethod: (const char *) shareMessage
{
    
    NSString *message   = [NSString stringWithUTF8String:shareMessage];
    NSArray *postItems  = @[message];
    
    UIActivityViewController *activityVc = [[UIActivityViewController alloc] initWithActivityItems:postItems applicationActivities:nil];
    
   
    if ( [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
    
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
    [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
    [activityVc release];
}

@end



extern "C"{
    void sampleMethod(const char * path, const char * message){
        ViewController *vc = [[ViewController alloc] init];
        [vc shareMethod: path: message];
        [vc release];
    }
}
extern "C"{
    void sampleTextMethod(const char * message){
        ViewController *vc = [[ViewController alloc] init];
        [vc shareOnlyTextMethod: message];
        [vc release];
    }
}
