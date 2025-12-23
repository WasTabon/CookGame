#import <UIKit/UIKit.h>
#import <AudioToolbox/AudioToolbox.h>

static UIImpactFeedbackGenerator *lightGenerator = nil;
static UIImpactFeedbackGenerator *mediumGenerator = nil;
static UIImpactFeedbackGenerator *heavyGenerator = nil;
static UISelectionFeedbackGenerator *selectionGenerator = nil;
static UINotificationFeedbackGenerator *notificationGenerator = nil;

void InitializeHaptics() {
    if (@available(iOS 10.0, *)) {
        if (lightGenerator == nil) {
            lightGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
            [lightGenerator prepare];
        }
        if (mediumGenerator == nil) {
            mediumGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
            [mediumGenerator prepare];
        }
        if (heavyGenerator == nil) {
            heavyGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
            [heavyGenerator prepare];
        }
        if (selectionGenerator == nil) {
            selectionGenerator = [[UISelectionFeedbackGenerator alloc] init];
            [selectionGenerator prepare];
        }
        if (notificationGenerator == nil) {
            notificationGenerator = [[UINotificationFeedbackGenerator alloc] init];
            [notificationGenerator prepare];
        }
    }
}

extern "C" {
    bool _SupportsHaptics() {
        if (@available(iOS 10.0, *)) {
            return YES;
        }
        return NO;
    }
    
    void _PlayHapticLight() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [lightGenerator impactOccurred];
            [lightGenerator prepare];
        }
    }
    
    void _PlayHapticMedium() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [mediumGenerator impactOccurred];
            [mediumGenerator prepare];
        }
    }
    
    void _PlayHapticHeavy() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [heavyGenerator impactOccurred];
            [heavyGenerator prepare];
        }
    }
    
    void _PlayHapticSelection() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [selectionGenerator selectionChanged];
            [selectionGenerator prepare];
        }
    }
    
    void _PlayHapticSuccess() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [notificationGenerator notificationOccurred:UINotificationFeedbackTypeSuccess];
            [notificationGenerator prepare];
        }
    }
    
    void _PlayHapticWarning() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [notificationGenerator notificationOccurred:UINotificationFeedbackTypeWarning];
            [notificationGenerator prepare];
        }
    }
    
    void _PlayHapticError() {
        if (@available(iOS 10.0, *)) {
            InitializeHaptics();
            [notificationGenerator notificationOccurred:UINotificationFeedbackTypeError];
            [notificationGenerator prepare];
        }
    }
}
