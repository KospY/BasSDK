# Modder Update Guide

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

