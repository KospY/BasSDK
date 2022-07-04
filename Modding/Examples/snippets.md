# Snippets


## U10


#### Spawning a prefab asynchronously from an asset bundle


```csharp
Catalog.InstantiateAsync<GameObject>("myPrefabGameObject", myGameObject => {
  myGameObject.transform.position = whatever;
}, "SpawningMyGameObject");
```

#### Is Player holding an Item with Telekinesis
```csharp
/// <summary>
/// Returns true if the player is TK holding the item
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
public static bool IsPlayerTkHolding( Item item ) {
    if ( item != null ) {
        foreach ( SpellCaster spellCaster in item.tkHandlers ) {
            if ( spellCaster.ragdollHand.creature.isPlayer ) {
                return true;
            }
        }
    }
    return false;
}
```

#### Disable culling for an Item (uses reflection)
```csharp
item.Set("cullingDetectionEnabled", false);
 
public static void Set<T>( this object source, string fieldName, T val ) {
    source.GetType()
        .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
        .SetValue(source, val);
}
```


#### Basic U10 LevelModule json
```json
{
  "$type": "ThunderRoad.LevelData, ThunderRoad",
  "id": "Master",
  "version": 3,
  "modes": [
    {
      "name": "Default",
      "modules": [
        {
          "$type": "Namespace.Classname, DLL-Name"
        }
      ]
    }
  ]
}
```

## Adding custom tips

Create a level module which on load adds your tips to the catalog at runtime.
This will add them to the base game tips without overwriting them or other modders ones.

```csharp
    public class AddLoadingScreenTips : LevelModule {
        public List<string> tips = new List<string> {
            "Tip1",
            "Tip2",
            "Tip3"
        };
        public override IEnumerator OnLoadCoroutine() {
            AddTips();
            return base.OnLoadCoroutine();
        }
        private void AddTips() {
            var textData = Catalog.GetTextData();
            if ( textData.GetGroup("Tips") != null ) {
                var tips = textData.GetGroup("Tips");
                int id = tips.texts.Count;
                foreach ( string t in this.tips ) {
                    id++;
                    var tip = new TextData.TextID {
                        id = id.ToString(),
                        text = t
                    };
                    tips.texts.Add(tip);
                }
            }
        }
    }
```

### Extension method to get the mod path of an object

```csharp
public static string GetModPath( this Object thisObject ) {
    Type type = thisObject.GetType();
    string assemblyFullName = type.Assembly.GetName().Name;
    return FileManager.GetFullPath(FileManager.Type.JSONCatalog,
        FileManager.Source.Mods, assemblyFullName);
}
```

### Extension method to get the manifest.json file for a Type (assuming the type is from a mod dll)

```csharp
public static string GetManifest(Type type) {
    string assemblyFullName = type.Assembly.GetName().Name;
    string manifestName = assemblyFullName + "\\manifest.json";
    string manifest = File.ReadAllText(FileManager.GetFullPath(FileManager.Type.JSONCatalog,
        FileManager.Source.Mods, manifestName));
    return manifest;
}
```

### How to access the items in the players holders

```csharp
foreach(Holder holder in Player.local.creature.equipment.holders)
{
    holder.items[0]
}
```