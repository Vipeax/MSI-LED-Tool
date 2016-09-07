# MSI-LED-Tool
This is a tiny project to allow modification of the LEDs of a MSI GTX 1080/1070 Gaming X card without the requirement of all the bloatware that the MSI tooling includes. Other MSI cards are very likely to work as well, but right now the code performs a check specifically for the device code of GTX 1070/GTX 1080 cards as well as the MSI brand subvendor code.

It supports a fixed color, breathing mode, flashing mode and double flashing mode. If you desire a different effect or any custom mode and you aren't a developer, just [create a new issue](https://github.com/Vipeax/MSI-LED-Tool/issues/new) describing what you want.

# Installation
* To install the tool you place the "MSI LED Tool" executable, the "Settings.json" file and the included "Lib" folder in whichever folder you desire to be its location.
* You have to point the path in the "regedit.reg" accordingly to your newly created and selected folder.
* Open the Settings.json file to change the R(ed), G(reen), B(lue) variables to your desired color scheme and set the AnimationType variable to your desired animation type. "AnimationType":1 means no animation.
	* 1 = No Animation
	* 2 = Breathing
	* 3 = Flashing
	* 4 = Double Flashing
* Run the "MSI LED Tool" executable and if you like what you see, run the "regedit.reg" file to register the tool to startup upon Windows boot, after which it will automatically apply your configured settings.

# Releases
For bundled releases, see [the releases area](https://github.com/Vipeax/MSI-LED-Tool/releases).