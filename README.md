# MSI-LED-Tool
This is a tiny project to allow modification of the LEDs of a MSI Gaming X cards without the requirement of all the bloatware that the MSI tooling includes. Other MSI cards are very likely to work as well, but right now the code performs a check specifically for the device code of Pascal (GTX 1060/GTX 1070/GTX 1080), Maxwell (GTX 980Ti/GTX 960) and Polaris cards as well as the MSI brand subvendor code.

It supports a fixed color, breathing mode, flashing mode and double flashing mode. If you desire a different effect or any custom mode and you aren't a developer, just [create a new issue](https://github.com/Vipeax/MSI-LED-Tool/issues/new) describing what you want.

# Installation
* To install the tool you place the "MSI LED Tool" executable, the "Settings.json" file and the included "Lib" folder in whichever folder you desire to be its location.
* You have to point the path in the "regedit.reg" accordingly to your newly created and selected folder.
* Open the Settings.json file to change the R(ed), G(reen), B(lue) variables to your desired color scheme and set the AnimationType variable to your desired animation type. "AnimationType":1 means no animation. You can disable device-ID checks by setting the "OverwriteSecurityChecks" setting to "true". When selecting the "Temperature Based" mode. The Upper- and LowerLimit options define the full green (lower) and full red (upper) numbers. So 45-85 means that anything from 45C and lower is 100% green and anything from 85C and upper is 100% red. It will then colorize from green to yellow to orange to red as your GPU temperature changes.
	* 1 = No Animation
	* 2 = Breathing
	* 3 = Flashing
	* 4 = Double Flashing
	* 5 = Disable LEDs
	* 6 = Temperature Based
* Run the "MSI LED Tool" executable and if you like what you see, run the "regedit.reg" file to register the tool to startup upon Windows boot, after which it will automatically apply your configured settings.

# Releases
For bundled releases, see [the releases area](https://github.com/Vipeax/MSI-LED-Tool/releases).