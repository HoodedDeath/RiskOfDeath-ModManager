# RiskOfDeath-ModManager
 Mod manager for Risk Of Rain 2, using ThunderStore.io api
 
 I built this mod manager because the other ones I tried were not installing some mods properly, so I made this one to do its best to pay attention to how mods need to be installed (since the managers I tried before just copied every mod into the plugins folder regardless of anything)
 
 - If you come across a mod that doesn't install properly, please message me on Discord (HoodedDeath#5391) and alert me to which mod it is
 
- This currently only works with BepInEx mods. Support for the SeikoML mods is planned, thanks to yoshisman8 - SeikoML Compat Layer (though this feature is on the back burner)

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
- Main priority work currently will be fixing the listing of the mods, making the uninstallation system, and backing up any files that would get overwritten
- Style work
- Keeping up with any special case mods that surface

# Current Issues
- List of installed mods is not currently persistent (WILL BE FIXED SOON)
- No uninstallation functionality currently (WILL BE FIXED SOON)

# Changlog
- 1.0.0 -> Initial release
- 1.0.1 -> Update RuleSet to not try installing itself to RoR2
- 1.0.2 -> Fix Readme