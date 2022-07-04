# Tips


## Unsubscribe to events
Remember to unsubscribe -= to your events. Forgetting to do so can cause memory leaks 🤯 

```csharp
public class MyLevelModule : LevelModule
{
        public override IEnumerator OnLoadCoroutine() {
            EventManager.onCreatureKill += EventManager_onCreatureKill;
            EventManager.onPossess += EventManager_onPossess;
            EventManager.onUnpossess += EventManager_onUnpossess;
        }

        public override void OnUnload()
        {
            EventManager.onCreatureKill -= EventManager_onCreatureKill;
            EventManager.onPossess -= EventManager_onPossess;
            EventManager.onUnpossess -= EventManager_onUnpossess;
        }
}
```

## To get custom weapons to sit properly on a weapon rack

Create a second `HolderPoint` (Transform) like in the pictures below.
Then add an additional `HolderPoint` on the item script named **HolderRackTopAnchor** for the weapon rack and **HolderRackTopAnchorBow** for the bow rack and reference your new `HolderPoint`.

![]({{ site.baseurl }}/assets/tips/sword_on_rack.jpg)
![]({{ site.baseurl }}/assets/tips/bow_on_rack.jpg)
![]({{ site.baseurl }}/assets/tips/sword_holderpoint_script.PNG)
![]({{ site.baseurl }}/assets/tips/bow_holderpoint_script.PNG)

## How to create an audio container


<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/tips/create-audio-container.mp4" type="video/mp4">
</video>

## Use regions in C# code to collapse regions of code together

```csharp
    public class MyLevelModule : LevelModule {
        #region onLoadlogic
        public override IEnumerator OnLoadCoroutine() {
        
            return base.OnLoadCoroutine();
        }
        #endregion
    }
```

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/tips/csharp-regions.mp4" type="video/mp4">
</video>

