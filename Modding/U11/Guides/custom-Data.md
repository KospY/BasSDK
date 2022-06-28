# Custom Data

## What is Custom Data?

Custom Data is the replacement to Saved Values introduced in U11 of Blade and Sorcery. Saved Values were pairs of strings that could be saved between level loads, allowing you to save important data. An example of this could be tracking the ID of an item stored in a holder so that you could respawn that item into that holder after a level load.

Custom Data is, in essence, an expansion on the idea of Saved Values. Where Saved Values had limitations due to only being able to save Strings (or serialised data as Strings), Custom Data allows you to save an entire unique class, populated with any kind of variable type you want.

## So how do I *use* Custom Data?

First, you should start with declaring a new class that Extends the `ContentCustomData` class found in ThunderRoad. you should also make a Constructor for the class, though it's unnecessary to add anything to the constructor.

```csharp

public class CustomDataClass : ContentCustomData
{
    public CustomDataClass() {}
}
```

From here, you can populate it with any manner of variable types and names that you might need to use for an Item MonoBehaviour. an example below is some variables you might save for a kind of modular firearm.

```csharp

public class CustomDataClass : ContentCustomData
{
    int bulletCount {get; set;}
    string SightHolderID {get; set;}
    BulletType bulletType {get; set;} //assume BulletType is an Enum
    bool safetyOn {get; set;}

    public CustomDataClass() {}
}
```

Now that you've set up your Custom Data Class, you can try to get any saved Data from it during Start(), or create a new empty one if there is no Data.

```csharp
Item item;
CustomDataClass customData;
int bulletCount;

public void Start()
{
    item = getComponent(Item);

    item.TryGetCustomData<CustomDataClass>(out customData);
    if (customData != null)
    {
        //retrieve data for use
        bulletCount = customData.bulletCount;
        //... etc
    }
    else
    {
        //if no CustomData is found, instantiate a new CustomData
        customData = new CustomDataClass();
        customData.bulletCount = 0; //populate with initial values
        //... etc
    }
}
```

Now, this is for *Loading* CustomData, but what about saving CustomData? Saving is simple, and lets assume we defined a Method `Save()` for whenever we wanted to save values.

```csharp

public void Save()
{
    customData.bulletCount = bulletCount; //necessary if you haven't updated the information inside the class yet
    //...

    item.RemoveCustomData<ContentCustomData>();
    item.AddCustomData<ContentCustomData>(customData);
}
```

### Why did you remove CustomData it before adding it?

Because `AddCustomData()` won't save your class if it's already storing CustomData of the same type (presumedly to prevent duplicates or overwriting of the original by mistake), thus, `RemoveCustomData()` is called first.

## Methods

These methods are found on the Item Class:

### `public bool TryGetCustomData<T>(out T customData)`
Searches a list of `ContentCustomData`'s, returns True if it finds one that matches your extended class and can output the class, Else returns False.

### `public bool HasCustomData<T>()`
Searches the list of `ContestCustomData`'s, returns True if it finds one that matches your extended class, else returns False.

### `public void AddCustomData<T>(T customData)`
If `HasCustomData<T>()` returns False, adds customData to the list.

### `public void RemoveCustomData<T>()`
Removes any instance of Type T from the list of `ContentCustomData`'s

### `public void OverrideCustomData(List<ContentCustomData> newContentCustomData)`
Completely replaces the list of `ContentCustomData`'s to the new variable