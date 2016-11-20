## Tiny macro processor

Macro substitution on text files, with built-in macros or specified regular expressions.

Usage:

```
tmac [options] <files>
```

Options

```
-b
```

Use built-in macros:

|             |          Grouping           ||
First Header  | Second Header | Third Header |
 ------------ | :-----------: | -----------: |
Content       |          *Long Cell*        ||
Content       |   **Cell**    |         Cell |

New section   |     More      |         Data |
And more      | With an escaped '\|'         ||
[Prototype table]

|Macro|Meaning
---|---
$file|Filename minus extension
