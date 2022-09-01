[![build and test](https://github.com/RealAshampoo/Ashampoo-Translation-Systems/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/RealAshampoo/Ashampoo-Translation-Systems/actions/workflows/build-and-test.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/RealAshampoo/Ashampoo-Translation-Systems/blob/main/LICENSE)
![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/RealAshampoo/Ashampoo-Translation-Systems?display_name=tag&include_prereleases&sort=semver&style=flat-square)
# Ashampoo-Translation-Systems

## Description
This repository contains the source code for a system for working with translation files.

## Installation
To use the system, you need to install the following dependencies from NuGet:

[Ashampoo.Translation.Systems.Formats.Abstractions](https://www.nuget.org/packages/Ashampoo.Translation.Systems/) if you only want to use the interfaces and base classes, and not hte format implementations.\
[Ashampoo.Translation.Systems.Formats](https://www.nuget.org/packages/Ashampoo.Translation.Systems.Formats/) if you want to use the complete system, including the format implementations.\
If you only want to use specific formats, you can install the corresponding packages directly.

To register all your installed formats with the IServiceCollection, you need to add the following line to the `Startup.cs` file:
```c#
services.RegisterFormats();
```
If you want to register only specific formats, you can add the following line to the `Startup.cs` file:
```c#
services.AddFormatFactory();
services.RegisterFormat<MyFormat>();
```
> NOTE: If you want to register multiple formats individually, you only need to call `services.AddFormatFactory()` once.

## Supported Formats

### AshLang
AshLang is a file format for storing a translations with their english original text.\
An AshLang file is a binary file with the following structure:

TODO: complete readme

### Gengo
The Gengo format is a simple format for storing translations.\
It is meant to be used for sending it to Gengo for translating.\
Gengo is based on Excel (.xlsx) and has a specific structure:\

|    [[[ ID ]]]     |    source     |    target     |
|:-----------------:|:-------------:|:-------------:|
| [[[ first id ]]]  | first source  | first target  |
| [[[ second id ]]] | second source | second target |
| [[[ third id ]]]  | third source  | third target  |

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

```xml
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

## Contributing
Contributions are always welcome!\
Please read the [contribution guidelines](CONTRIBUTING.md) first.

## Code of Conduct
This project and everyone participating in it is governed by the [Code of Conduct](CODE_OF_CONDUCT.md).