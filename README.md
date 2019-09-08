# RiskOfDeath-ModManager
 Mod manager for Risk Of Rain 2, using ThunderStore.io api
 
 I built this mod manager because the other ones I tried were not installing some mods properly, so I made this one to do its best to pay attention to how mods need to be installed (since the managers I tried before just copied every mod into the plugins folder regardless of anything)
 
 - If you come across a mod that doesn't install properly, please message me on Discord (HoodedDeath#5391) and alert me to which mod it is
 
- This currently only works with BepInEx mods. Support for the SeikoML mods is planned, thanks to yoshisman8 - SeikoML Compat Layer (though this feature is on the back burner)

# How to install
If you're downloading the manager for the first time, simply extract the 'Risk of Death' folder to wherever you'd like to, then run 'RiskOfDeath ModManager.exe'

If you're updating from an older version, simply launch the manager and if there's a new version available, it will popup and ask if you want to update. When you select yes, the updater program will launch and update the application for you.

# How to use
This is a simple mod manager, with very simple steps, like the download button and uninstall button for mods. All dependencies are taken care of automatically!

If you want to use the 'Install with Mod Manager' button on thunderstore.io, that's been made simple! From within Risk of Death, click the 'Options' button at the top, then click the 'Link Manager Protocol' button. This will prompt you for administrator priveleges, as writing the data for the protocol requires accessing restricted areas of the Windows Registry. Allow administrator rights, and a command window will pop up for a moment. After that window closes, you're ready to use the Mod Manager button on thunderstore!

# For Mod Developers
 
If you want to guarantee that your mod will install properly with my manager, there are a couple options.
- You can set up your mod folder system such that the structure mimics the folder system in BepInEx. If the dll or other files for your mod just need to go into the plugins folder, no need to worry! You can leave the files in the top-level of the archive, or in a folder that doesn't have the name same as a BepInEx folder
- There is a button on the top of the main form of the RoD Mod Manager ("For Mod Devs" button) that opens up a form where you can input and export rules for your mod (See next section)

# Making Custom Rules For Your Mods

When you click the "For Mod Devs" button, it will open a form that starts with two text boxes. Here's how to use this form:
- "Mod Folder" represents a folder in the top level of the archive for your mod
- "Game Folder" represents the folder within the RoR2 file system where the contents of the mod folder should be copied to. There are limited options for folders to copy into:
  - BepInEx -> The BepInEx folder inside the RoR2 base folder. If you use this, you should have the file system as the RoR2/BepInEx folder (example: if your mod needs to go to patchers and you use this option, your mod needs to be in a system like "MyMod/patchers/MyMod.dll")
  - plugins -> The BepInEx plugins folder
  - patchers -> The BepInEx patchers folder
  - monomod -> The BepInEx monomod folder
  - core -> The BepInEx core folder (You probably don't want to ever use this one, but it's in here for now)
  - data -> "Risk of Rain 2_data" folder. Contains folders like language and managed
  - language -> RoR2_data/language folder. For text replacement mods
  - EN-US -> RoR2_data/language/en folder. The folder containing the english language text files
  - managed -> RoR2_data/managed folder

# Future update plans
- Style work
- Keeping up with any special case mods that surface
- Next major version (V3.0.0) will (likely) be the update to include mod profiles

# Current Issues
- Stupidly slow download for any mods over about 10MB (about 0.5MB/s download speed)

# Changlog
- 1.0.0 -> Initial release
- 1.0.1 -> Update RuleSet to not try installing itself to RoR2
- 1.0.2 -> Fix Readme
- 2.0.0 -> Implemented Uninstall and Update functionality, as well as update check for application
- 2.1.0 -> Implemmented "Install with Mod Manager" url protocol for downloading directly from thunderstore.io
- 2.1.1 -> Added How To Use section to readme
- 2.1.2 -> Fixed dependencies listing to not list dependencies under Installed Mods, Uninstalls dependencies when all mods depending on them are uninstalled, Removes folders created for mods when uninstalling
- 2.2.0 -> Update code in preparation for upcoming V3 update. Added updater sister program for automating RoD updates