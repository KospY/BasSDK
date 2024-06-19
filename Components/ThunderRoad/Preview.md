---
parent: ThunderRoad
---
# Preview

The Preview script generates an image for the item, which is used to preview what the weapon looks like inside the Item Spawner and is generated from the [Item][Item] script. This script generates a Preview Image for when the item is selected, and an "Up-close" image which is shown for the small preview in the book.

![PreviewScript][PreviewScript]

>Note: Full preview is used as a fallback if the Close-up image is not present in JSON.

>Note: Generated previews need to be put in addressables and referenced in JSON.

## Buttons

| Button                            | Description
| ---                               | ---
|Generate Icon                      | Generates the Icon. Ensure that this is done in Prefab mods, does not work in scene.
|Align Camera                       | Aligns camera to the preview square to view the preview.
|Align from Camera                  | Aligns the preview to where the camera is looking at.

## Components

| Fields                            | Description
| ---                               | ---
|Close Up Preview                   | Changes to creating a Close up preview, shown for the small preview in the book/
|Size                               | Adjust the size to fit the weapon. Scales with X Scale of the Preview Script.
|Icon Resolution                    | Adjust the resolution of the image. `Recommended Resolution: 512`
|Temp Layer                         | Temporarily changes layer when taking a picture. Change this if the layers are already in use/changed.
| Renderers                         | Select Renderers which can used in the preview. Not neccessary for the weapon mesh.
| Generated Icon                    | Generated Icon goes here. User can create their own preview and put it here too.

![PreviewExamples][PreviewExamples]


[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}
[PreviewExamples]: {{ site.baseurl }}/assets/components/Preview/PreviewExamples.png
[PreviewScript]: {{ site.baseurl }}/assets/components/Preview/PreviewScript.PNG