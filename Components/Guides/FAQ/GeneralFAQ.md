---
parent: FAQ
grand_parent: Guides
---
# General FAQ

This page container frequently asked questions about general problems reported by the community.

{: .tip}
Question not here, or any of the solutions did not work? Ask for help in the [Blade and Sorcery Discord](https://discord.gg/atdUuvd6).

## The mod I downloaded doesn't work

This could be caused by a plethora of problems.
- Make sure that the mod uses the correct version of Blade and Sorcery. If you downloaded the mod from mod.io/in-game mod loader, and the manifest states the wrong version, this may be an error on the mod creator's behalf.
- Ensure that you are on the correct version of Blade and Sorcery. 1.0 Mods will not work with Update 12 mods, and vice versa
- Ensure that the mod is not located in a sub-folder (like ModFolder>Mod>Mod Files)
- If your mod has been installed with the modloader, and still does not work, restart the game to see if that resolves the issue.
- Pirated versions of the game will not function correctly with mods and can cause many issues. Buy the game [Here](https://store.steampowered.com/app/629730/Blade_and_Sorcery/)

## My game is infinite loading

- Verify your files on Steam to ensure your files are correctly installed
- When you're loaded in to the main menu, before character selection, go to the Mods page. Disable mods 1-by-1 until you can load in correctly. If you have installed mods manually, try deleting the mods 1-by-1 until your game loads in. 
- Re-install the game.
- If all does not work, delete your save file (make a backup, just in case).

## When is Nomad 1.0 release?

The update is coming to Nomad in 2024. The exact date is unknown but there will be announcements and news about it when Warpfrog have news.

## How much of the PCVR content will be in the Nomad Crystal Hunt update?

The aim is to make the Nomad update as close to 1:1 in content as PCVR but it will not be fully known until the update is almost complete. The Nomad update will obviously have different graphics compared to PCVR.

## Is Blade and Sorcery coming to PCVR2?

We would love to do this but currently we have not announced any plans. We will need to investigate and see if a port is possible.

## Is there going to be any more updates after 1.0?

Blade & Sorcery will continue to receive patches where the main focus is on bugfixing, QOL and stabilization. We are now complete with the roadmap, so while it is possible that there may be content updates, they will surely be minor content updates and nothing as major as the massive feature-laden previous updates like U10, U11, etc. We also do not want to break the modding scene.

## Will there be Blade & Sorcery DLC or expansions?

It is unlikely there would be expansions for Blade & Sorcery as the Warpfrog team are very eager to move onto our new game framework, Thunder Road 2. This is a much improved game framework that would let us bypass many issues we had on Blade & Sorcery development. Although there are no current plans, a sequel to Blade & Sorcery on Thunder Road 2 would be more likely than an expansion to the original game.

## How do I start modding?

There are many guides for modding! You can check the [SDK How-to][SDKHowTo] to see all the written guides on creating mods, as well as the general guides section. Additionally, you can join the [Discord Server](https://discord.gg/atdUuvd6) and follow the community-written guides located in the #modding-resources forum.

## The mod I installed has no mod options

The mod options section of the options will only show mods that have added their own mod options to it. If the mod is supposed to have mod options, consult the mod description or see if the mod was installed correctly. 

## A mod I installed has weapons which are question marks in the book

This usually means that the mod is out of date, or the mod was installed incorrectly.

## I find climbing really difficult, what do I do?

The Sandbox/Cheats tab in Options contains "free climb" as an option. It will make your climbing much easier. 

## I set the game graphics tab to high and now I can't get in to the game without it crashing

You can delete the Options file to reset the settings back to the default. 

You can find this file in `Documents > MyGames > BladeAndSorcery > Saves > Default`

The file should be called `Options.opt`

## I skipped linking my email to mod.io and now I cannot link my email

You can reset this in the options file. On PCVR open the Options.opt file with a text editor and set `"linkedEmail": false.`
You can find this file in `Documents > MyGames > BladeAndSorcery > Saves > Default`
The file should be called `Options.opt`

## When I try to see downloadable mods using the in-game launcher with Mod.Io, it stays there is a server connection issue and try again later

Unfortunatly, this is an issue with the mod.io servers in your region. Usually, this will resolve itself by refreshing the page after a minute or so. If the problem persists, you can contact mod.io support, or give it more time. In the meantime, you can still install mods manually, using the mod.io or Nexus page. 

## My mod is not appearing in the Crystal Hunt game mode

For a mod to appear in Crystal Hunt, it would require the modder to have added support for Crystal Hunt in their mod. If not, then the mod would only be available in Sandbox mode.

[SDKHowTo]: {{ site.baseurl }}{% link Components/Guides/SDK-HowTo/index.md %}