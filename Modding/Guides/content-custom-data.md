# Content Custom Data

## What is Content Custom Data?

`ContentCustomData` allows you to attach custom data classes to `Item` objects.

```tip
It could be used to keep track of how much liquid is in a potion, or how many bullets are in a magazine.
```

`ContentCustomData` is also used in conjuction with `Container` objects. 

When an `Item` is added to a `Container` the `ContentCustomData` associated with that `Item` can also be stored in the `Container`. 

`Container` objects on the player's `Creature` are saved and serialized, so data can persist when changing levels or quitting the game and starting up back up.

```tip
For example if you partially drink a potion, store it in your inventory, it will add the potion `Item` to the players `Container` along with the `ContentCustomData` which stores how much liquid is left in the potion. If you change maps the `Container` is saved so it will remember how much liquid is left in the potion.
```

## How to use Content Custom Data?

`ContentCustomData` is an `abstract` class. You must create your own class to extend it.

```csharp

public class MyData : ContentCustomData
{
    public int myValue;
    public MyData(int initialValue) {
        myValue = initialValue;
    }
}
```

To use this custom data with an `Item` you need to attach it to the `Item` somehow.

One way is to use an `ItemModule` to add the customData onto the item

```csharp
public class AddMyData : ItemModule
{
    public override void OnItemLoaded(Item item)
    {
        base.OnItemLoaded(item);
        item.AddCustomData(new MyData(10));        
    }
}
```

You can then retrive the custom data from the item in other parts for your code to read and update the value.

```csharp
item.TryGetCustomData<MyData>(out myData);
```

The customData will be persisted when the item is added to the players holsters or inventory.
