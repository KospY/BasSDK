# Debug Console

Blade and Sorcery has an ingame console which can be used to debug mods and find errors and run built-in and custom commands.

```tip
This console can be viewed by pressing F8.
```

![console]({{ site.baseurl }}/assets/debug/console.png)


## Adding custom commands

Scripting mods can also add their own commands. 

To do that, just add the `ConsoleMethod` attribute to any static method in your plugin.

Here is an example for a new command that spawn a cube at a position:

```
using UnityEngine;
using IngameDebugConsole;

public class TestScript : MonoBehaviour
{
	[ConsoleMethod( "cube", "Creates a cube at specified position" )]
	public static void CreateCubeAt( Vector3 position )
	{
		GameObject.CreatePrimitive( PrimitiveType.Cube ).transform.position = position;
	}
}
```

For more information about the debug console, you can find it here:
[https://github.com/yasirkula/UnityIngameDebugConsole](https://github.com/yasirkula/UnityIngameDebugConsole)


