# PO
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
