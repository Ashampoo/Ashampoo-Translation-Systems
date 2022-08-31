# TsProj
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