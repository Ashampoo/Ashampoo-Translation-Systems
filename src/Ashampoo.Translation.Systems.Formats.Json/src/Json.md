# Json
The Json format is a simple format for storing translations.\
It is a JSON file with the following structure:

```json
{
  "first id": "first target",
  "second id": "second target",
  "third id": "third target"
}
```
Json also supports nested objects:

```json
{
  "first id": "first target",
  "second id": "second target",
  "third id": {
    "first nested id": "first nested target",
    "second nested id": "second nested target",
    "third nested id": "third nested target"
  }
}
```
The id for translations in nested objects is constructed by concatenating the ids of the parent objects with a `/` .\
For example, the id for the first nested id is `third id/first nested id`.

Additionally, arrays are supported:

```json
{
  "first id": [
    "first target",
    "second target",
    "third target"
  ]
}
```
The id for translations in arrays is constructed by concatenating the id of the parent object with a `/` and the index of the array.\
For example, the id for the first target is `first id/0`.