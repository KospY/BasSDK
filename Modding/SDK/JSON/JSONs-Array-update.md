# JSONs: Array Update

By default, when you create a JSON file used to override the default one, each array will be appended with the data from the custom JSON. 
It's ok for simple arrays, however for some of them it prevent overriding an existing item as the JSON serializer will rather create another item instead, causing duplicates.

To prevent this, an identifier field as been set in some arrays, and the serializer will instead update the corresponding array item if an item in the custom JSON have the same ID.

Below you will find the list of the current arrays concerned with the name of their identifier field:

## Level JSON
* modes (ID field: name)
* modules (ID field: $type)

## Text JSON
* pagegroups (ID field: id), 
* strings (ID field: id)

## Item JSON
* displayNames (ID field: language)
* descriptions (ID field: language)
* modules (ID field: $type)
* damagers (ID field: transformName)
* interactables (ID field: transformName)
* whooshs (ID field: transformName)
* customSnaps (ID field: holderName)

## Creature JSON
* spells (ID field: spellID)
* holders (ID field: name)


## JSON example

For example, if you want to add a tip in the house as well as changing the default help text in the book, you can do the following:

```
{
  "$type": "BS.TextData, Assembly-CSharp",
  "id": "English",
  "version": 1,
  "pageGroups": [
    {
      "id": "Tips",
      "pages": [
        {
          "text": "Don't die"
        }
      ]
    }
  ],
  "strings": [
    {
      "id": "Notes",
      "text": "Hello mates!"
    }
  ]
}
```

Also please note that if you don't put the corresponding ID field in the custom JSON, an error will be thrown in the console and the JSON will be ignored.
