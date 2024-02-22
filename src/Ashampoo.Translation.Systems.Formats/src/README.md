# Ashampoo-Translation-Systems-Formats

This package provides the implementation of several translation file formats,
such as PO and Json, for the Ashampoo Translation Studio.

## Installation
To use the system, you need to install the following dependencies from NuGet:

[Ashampoo.Translation.Systems.Formats](https://www.nuget.org/packages/Ashampoo.Translation.Systems.Formats/)\
If you only want to use specific formats, you can install the corresponding packages directly.\
\
To register all your installed formats with the `IServiceCollection`, you need to call the following methods:
```c#
using Ashampoo.Translation.Systems.Formats.Abstractions;

services.RegisterFormats();
```

## Supported Formats

### AshLang
AshLang is a file format for storing translations with their english original text.\
An AshLang file is a binary file.\
We recommend not to use this format for storing translations, because it is an internal format of Ashampoo,\
and is not human-readable.

### Gengo
The Gengo format is a simple format for storing translations.\
It is meant to be used for sending it to Gengo for translating.\
Gengo is based on Excel (.xlsx) and has a specific structure:

| [[[ ID ]]]        | source         | target          |
|:-----------------:|:--------------:|:---------------:|
| [[[ first id ]]]  | first source   | first target    |
| [[[ second id ]]] | second source  | second target   |
| [[[ third id ]]]  | third source   | third target    |

As you can see, the ids are always in the first column and the source and target are in the second and third columns.
Also, the ids are inside of 3 square brackets.\
This is because Gengo doesn't translates strings that are in three square brackets.

### Json
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

### NLang
NLang is a simple key-value format.
```
firstId=firstTarget
secondId=secondTarget
thirdId=thirdTarget
```

### PO
The [PO](https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html) format is a simple format for storing translations.\
It is a text file with the following structure:

```po
msgid "first source"
msgstr "first target"

msgid "second source"
msgstr "second target"

msgid "third source"
msgstr "third target"
```

Po files can also contain comments, and context.\
Both can be used to store additional information about the translation.\
At the moment, context and comments in files are accepted as inputs, but are not properly handled.\
If a translation has a context, the id of the translation is constructed by concatenating the context with a `/` and the id.\
For the full documentation of the PO format, see the [GNU gettext manual](https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html).

### Resx
The Resx format is a simple format for storing translations.\
It is a XML file with the following structure:

```
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="first id">
    <value>first target</value>
  </data>
  <data name="second id">
    <value>second target</value>
  </data>
  <data name="third id">
    <value>third target</value>
  </data>
</root>
```
For more information about the Resx format, see the [Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/framework/resources/creating-resource-files-for-desktop-apps#resx-files).

### TsProj
TsProj is a file format for storing translation strings in a .tsproj file.\
It is a xml based format, intended to use with the Ashampoo Translation Studio,
with the following structure:

```xml
<?xml version="1.0" encoding="utf-8"?>
<project name="My Project" version="1.0" author="Author" mail="author@email.com" source_language="source_language"
target_language="target_language">
    <component pluginguid="{F0D8F625-2EE3-4C84-96EC-BFBDD4946878}">
        <translation id="first id">
            <comment>first comment</comment>
            <source>first source</source>
            <target>first target</target>
        </translation>
        <translation id="second id">
            <comment>second comment</comment>
            <source>second source</source>
            <target>second target</target>
        </translation>
        <translation id="third id">
            <comment>third comment</comment>
            <source>third source</source>
            <target>third target</target>
        </translation>
    </component>
</project>
```
 


## License
This project is licensed under the MIT license.