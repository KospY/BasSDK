---
parent: SDK-HowTo
grand_parent: Guides
---
# Retrieve your game log file

## Nomad (Android)

---

{: .tip}
Since Android version V63, accessing the Android file system is now much more difficult. Follow [This guide](https://youtu.be/7H3pfTvzDBc?si=GBgbL1ltAPg3k4lv) to learn how to access the Oculus/Meta Quest 2/3 file system.


1. Power up your headset and plug it into your PC
2. There may be a notification asking you to `Allow access to data`. Click “Allow”
    
    ![AllowAccessData][AllowAccessData]
    
3. Navigate to `This PC\Quest 2\Internal shared storage\Android\data\com.Warpfrog.BladeAndSorcery\files\Logs`
4. Copy the Player.log and/or Player-prev.log to your desktop
5. Open the log file with notepad, notepad++, wordpad, or drag it into discord to send it

<video src="/assets/components/Guides/GameLogFile/nomad-log-windows.mp4" width="880" height="440" controls></video>

{: .important}
The cable which comes with the Quest 2 should work.
You must use a cable that is for charging and data, if you use a charging only cable it won't work.


## PCVR (Steam)

---

On PC, logs can be found in `C:\Users\<YourUsername>\AppData\LocalLow\Warpfrog\BladeAndSorcery` 

File are named: `Player.log` and `Player-prev.log`

[NomadLog]: {{ site.baseurl }}/assets/components/Guides/GameLogFile/nomad-log-windows.mp4
[AllowAccessData]: {{ site.baseurl }}/assets/components/Guides/GameLogFile/AllowAccessData.png