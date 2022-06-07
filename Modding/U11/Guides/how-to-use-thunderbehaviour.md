# How to use ThunderBehaviour

This extension of `Monobehaviour` is used to bypass unity's magic update methods.
For more information on why this is useful, read [1k update calls](https://blog.unity.com/technology/1k-update-calls)


- `ThunderBehaviour` is available as a baseclass to use, you can use this instead of your own `MonoBehaviours` by doing the following
    - instead of `void Update()` use `protected override void ManagedUpdate()`
    - instead of `void LateUpdate()` use `protected override void ManagedLateUpdate()`
    - instead of `void FixedUpdate()` use `protected override void ManagedFixedUpdate()`
    - instead of `void OnEnable()` use `protected override void ManagedOnEnable()`
    - instead of `void OnDisable()` use `protected override void ManagedOnDisable()`
    - Implement `protected override ManagedLoops ManagedLoops => ManagedLoops.Update | ManagedLoops.LateUpdate | ManagedLoops.FixedUpdate`
        - Specify one or more of the `ManagedLoops` enum to indicate to the UpdateManager which loops you want to run on your ThunderBehaviour


## Example Classes

```csharp
using ThunderRoad;
public class ExampleUpdate : ThunderBehaviour
{
    //Still use awake as normal
    void Awake(){}

    //Use the ManagedOnEnable and ManagedOnDisable instead of the unity ones
    protected override void ManagedOnEnable() {}

    protected override void ManagedOnDisable() {}

    //This tells the update manager to only call your ManagedUpdate() function
    protected override ManagedLoops ManagedLoops => ManagedLoops.Update;

    //This will get called
    protected internal override void ManagedUpdate() {}

    //Late update is not called because it is not specified in the ManagedLoops
    protected internal override void ManagedLateUpdate() {}
}
```

```csharp
using ThunderRoad;
public class ExampleFixedAndUpdate : ThunderBehaviour
{
    //This tells the update manager to only call your ManagedUpdate() AND ManagedFixedUpdate() functions
    protected override ManagedLoops ManagedLoops => ManagedLoops.Update | ManagedLoops.FixedUpdate;

    //This will get called
    protected internal override void ManagedUpdate() {}

    //This will get called
    protected internal override void ManagedFixedUpdate() {}
}
```