# How to retrieve your log file for Nomad

# Short Guide

1. Plug your Quest 2 into your PC
2. Navigate to `This PC\Quest 2\Internal shared storage\Android\data\com.Warpfrog.BladeAndSorcery\files\Logs`
3. Copy the Player.log and/or Player-prev.log to your desktop
4. Open the log file with notepad, notepad++, wordpad, or drag it into discord to send it

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/nomad-logs/nomad-log-windows.mp4" type="video/mp4">
</video>

## Retrieve logs on the Quest 2 (Windows)

### Power on the Quest 2
Switch it on by pressing the power button on the right side.

### Plug the Quest 2 into the PC
Plug in the USB C cable into the Quest 2 and into your PC or Laptop you downloaded the mod onto.

```tip
The cable which comes with the Quest 2 will work perfectly.

You must use a cable that is for charging and data, if you use a charging only cable it won't work.
```

### Put on the Quest 2 headset
There may be a notification asking you to `Allow access to data`. 

This will let your PC or Laptop access your Quest 2's files.

Select **Allow**

```warning
Do not click **Deny** or **Don't show again** 
```

```tip
If your Quest 2 is in Developer mode, you will not be able to access your Quest 2 internal storage this way. 
In Developer mode you can only access the internal storage over ADB. You will need to use **Side Quest** to transfer the files.

You must disable Developer mode, reboot your Quest 2 and plug in your cable again and it should prompt you to `Allow access to data`.
```

![mod folder]({{ site.baseurl }}/assets/mod-install-nomad/allow-access.JPG)

### Take off your Quest 2 headset
You dont need to wear the headset for the next part.

### Open the Quest 2 folder
On your PC or Laptop which you plugged the Quest 2 into, open `File Explorer` and go to `This PC`. Where you would normally see your hard drives.

You can do this many ways in Windows.
* Click Start, type `This PC`. Click the `This PC` icon.
* Click Start, click the Documents file Icon on the left side of the start menu. Click `This PC` on the left side of the window
* On your keyboard, press the `Windows Key` AND `E` at the same time. Click `This PC` on the left side of the window

In `This PC` check you have the `Quest 2` drive appearing.

![mod folder]({{ site.baseurl }}/assets/mod-install-nomad/quest2-drive.JPG)

### Go to the Blade and Sorcery: Nomad mod folder

The folder should be here:
`This PC\Quest 2\Internal shared storage\Android\data\com.Warpfrog.BladeAndSorcery\files\Mods`

There are two ways to get to the folder.

#### 1. Navigate to the logs folder
Open up the `Quest 2` drive by double clicking it.

Then you can go through each of the folders by double clicking on them:
* `Internal shared storage`
* `Android`
* `data`
* `com.Warpfrog.BladeAndSorcery`
* `files`
* `Logs`

```tip
Inside the Log folder is sometimes two files, `Player.log` and `Player-prev.log`. Sometimes they may just appear as `Player` and `Player-prev` depending on your Windows settings.
```

#### 2. Copy the log file out of the folder

You can copy the log file onto your PC by dragging and dropping it from the `Logs` folder Window to your desktop, where you can then send it on discord if needed.