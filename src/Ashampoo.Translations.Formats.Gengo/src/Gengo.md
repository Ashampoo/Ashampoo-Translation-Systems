# Gengo
The Gengo format is a simple format for storing translations.\
It is meant to be used for sending it to Gengo for translating.\
Gengo is based on Excel (.xlsx) and has a specific structure:\
\
| [[[ ID ]]]        | source         | target          |
|:-----------------:|:--------------:|:---------------:|
| [[[ first id ]]]  | first source   | first target    |
| [[[ second id ]]] | second source  | second target   |
| [[[ third id ]]]  | third source   | third target    |

As you can see, the ids are always in the first column and the source and target are in the second and third columns.
Also, the ids are inside of 3 square brackets.\
This is because Gengo doesn't translates strings that are in three square brackets.

