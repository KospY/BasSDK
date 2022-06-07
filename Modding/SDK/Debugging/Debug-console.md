# Debug Console

Blade and Sorcery has a console which can be used to debug mods, as well as allow custom commands and easy-access commands like loading levels and spawning items. This console can be viewed by pressing F8.

![](https://i.imgur.com/0QXvQ9h.png)

This console is pretty useful to check if your mod is loading correctly, and it's also possible to use the console to type commands by clicking on the commands line. To get the list of available commands, just type `help`.

The console is also useful for debugging mods, as it displays if a mod json is broken, if the bundle has an errors and so on. At the start of the game, the console will display what JSONs have been loaded, overwritten and display all .DLL files that have been loaded successfully. Red errors are often null references, or a message to display that a certain mod has not been loaded correctly, which it will state.

Plugins can also add their own commands. To do that, just add the `ConsoleMethod` attribute to any static method in your plugin.
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

For more information about the debug console, you can check this Github page: https://github.com/yasirkula/UnityIngameDebugConsole

---

Notes: If the mouse click is not working, check if the controller pointer is not looking at an UI and if the game is paused (in this case just close the console and press Space to unpause it).

