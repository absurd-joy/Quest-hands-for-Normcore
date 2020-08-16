# Quest Hands for Normcore

This is a sample project that showcases how to network hand-tracked states from the Oculus Quest. This utilizes the very-easy-to-use Normcore2.0 library; an account with [NormalVR](http://normalvr.com) is required to run this project.

This is a "get you started" sort of package that demonstrates how to accomplish networked real-hand tracking using established methods and design patterns. Feel free to fork this project and modify the files to suit your needs; this is not intended to be an out-of-the-box solution for every project under the sun.

## Requirements:
- A physical Oculus Quest
- Unity 2019.4.8+
- A Normcore account
- Oculus Integration 19.1.0 (Asset Store SDK -- Included in this project)

It's much easier to test if you also have an Oculus Link Cable to run your HMD from your computer.

## Installing and Running...

### Project Download:
1. Clone this git repository
1. Open the resulting folder in Unity
1. Navigate to `/Assets/Normal/Internal/Resources/` and view the added `NormcoreAppSettings` file in the Unity inspector. You will need to add your own app key to this, which you can obtain from [NormalVR](http://NormalVR.com) (free).
1. Plug in an Oculus Quest and make sure you can see your hands in the Quest lobby. (You might have to enable them in settings)
1. (Optional, for testing:) If using a link cable, then activate "Link Mode". Once link mode is active your hands will stop working in the lobby.
1. (Optional, for testing:) Open this scene in Unity: `/Assets/QuestHandsForNormcore/Samples/Sample`
1. (Optional, for testing:) Hit play. If you did everything correctly, you shouldn't see any errors or warnings in the console, and you should see your hands in the HMD.
1. Click "build and run" with Android as your target device. Once this completes your device should be online with hands showing. Connect with yet another device for online hand extravaganzas.

This project is configured so that your local avatar (the "true" representation by Oculus) is shown as grey hands, and your remote (online) avatar is showcased with pink hands.

If you see your grey hands inverted and fingers not animating properly in the editor - this is a known problem, I'm not sure what the source is. The data seems valid in OVR (pink hands show up fine)... The current workaround is to make a build and push it to your device.

### Release Download:
If you already have all the required software installed in your own project, you can just grab the unique files from the Releases page here on GitHub. If you go this route you'll have to hook things up yourself, though our example scene is still included. If your Normcore and Oculus SDK versions are correct it should "just work."

[Latest Release](https://github.com/absurd-joy/Quest-hands-for-Normcore/releases/latest)

## Known Issues

Currently this Works(tm) but has a handful of minor annoyances/issues. Check them out on the [Issues Page](https://github.com/absurd-joy/Quest-hands-for-Normcore/issues) (and feel free to comment, submit fixes, or suggest new ways of doing things!)

## Credits

This project was written by me, Andy Moore for absurd:joy. you can chat with me at [andy@absurdjoy.com](mailto:andy@abusrdjoy.com)!

Collaborators welcome! Please send pull requests and/or issues here on gitHub.

Inspired by the GitHub project [SpeakGeek-Normcore-Quest-Hand-Tracking](https://github.com/dylanholshausen/SpeakGeek-Normcore-Quest-Hand-Tracking). Thanks, Dylan Holshausen! Especially for the first iteration of the Skeletal Serialization, which I cribbed heavily from. Click that link and buy Dylan a beer.

### Special thanks:
- The entire Normal team for their networking tool. It's _too_ easy to use.
- Oculus for getting hand tracking working at such a high quality despite the device not being built with fancy sensors.
