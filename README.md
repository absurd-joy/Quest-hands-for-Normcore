# Quest Hands for Normcore

This is a sample project that showcases how to network hand-tracked states from the Oculus Quest. This utilizes the very-easy-to-use Normcore2.0 library; an account with [NormalVR](http://normalvr.com) is required to run this project.

**Requirements:**
- Oculus Quest
- Unity 2019.4.8+
- Normcore 2.0.0 preview 15
- Oculus plugin of some kind??? SDK? What version, what?

**Note:** As of this writing, hand detection doesn't display when running via link cable; you must make an Android build and push it to device.

---

## Installing and Running...

### Project Download:
1. Clone this git repository
1. Open the resulting folder in Unity
1. Navigate to `/Assets/Normal/Internal/Resources/` and view the added `NormcoreAppSettings` file in the Unity inspector. You will need to add your own app key to this, which you can obtain from http://NormalVR.com.
1. Plug in an Oculus Quest.
1. Hit play. If you did everything correctly, you shouldn't see any errors or warnings in the console, but your hands won't show up. (Hands only work on device builds, not via link cable)
1. Click "build and run" with Android as your target device. Once this completes your device should be online with hands showing. Connect with yet another device for online hand extravaganzas.

### Release Download:
1. todo!

---

## Credit

This project was written by @SnugglePilot for absurd:joy.

Inspired by GitHub project [SpeakGeek-Normcore-Quest-Hand-Tracking](https://github.com/dylanholshausen/SpeakGeek-Normcore-Quest-Hand-Tracking). Thanks, Dylan Holshausen! Click that link and buy Dylan a beer.