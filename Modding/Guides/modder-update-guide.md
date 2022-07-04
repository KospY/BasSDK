# Modder Update Guide

# SDK Changes

## Handles

Handles have been drastically changed. They no longer have a hand pose inside the JSON, and can be adjusted in editor instead! 

![posing]({{ site.baseurl }}/assets/u11-modder-update-guide/handpose.gif)

Handposes now need to be adjusted themselves, and can be blended between the provided hand poses in the SDK. This allows you to adjust each hand pose for each pose, and has a nice new handle interface.


![posing]({{ site.baseurl }}/assets/u11-modder-update-guide/handpose-editor.gif)

This also means that in JSON, the hand pose section is no longer present in the handle, and **needs to be removed**.

## Parry Target

ParryTargets are now also used for splash size. Be sure to add them so your weapon splashes in water.


## Previews

There is now a close-up preview checkbox, which allows you to generate used for the weapon icon.

![preview]({{ site.baseurl }}/assets/u11-modder-update-guide/preview.JPG)


## Json Changes

### Handle Poses

`handPoseId` and `overrideHandPose` has been removed from the Interactables section

### Previews

Close Up Icon address has been added
```json
"closeUpIconAddress": "Path.to.my.Icon"
```

### Inventory Sounds

```json
  "inventoryHoverSounds": null,
  "inventorySelectSounds": null,
  "inventoryStoreSounds": null,
```

### Category

This has changed to a simple field

```json
  "category": "Daggers",
```

To help mass change your categories, you can use notepad++
and this regex to find and replace to fix your json category

find : `^\s*"categoryPath": \[\s*("[a-zA-Z0-9 ]+")\s*]`

replace: `"category": $1`

Make sure you check `Regular expresssion` AND `. matches newline`
you can do `Find in files` in notepad++ to do it for all the jsons

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/tips/category-regex.mp4" type="video/mp4">
</video>

