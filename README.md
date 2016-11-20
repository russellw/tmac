## Tiny macro processor

Macro substitution on text files, with built-in macros or specified regular expressions.

Usage:

```
tmac [options] <files>
```

Options:

```
-b
```

Use built-in macros:

Macro|Meaning
---|---
$file|Filename minus extension

```
-ci
```

Culture-invariant regex match as in:

https://msdn.microsoft.com/en-us/library/yd1hzczs(v=vs.110).aspx#Invariant

```
-i
```

Ignore case

```
-r <pattern> <replacement>
```

Replace text using regular expressions as in:

https://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx

This option can be used more than once.

```
-t
```

Show the temporary folder to which original versions of changed files will be moved.

```
@file
```

Read options and filenames from a response file.

TMac automatically skips binary files, and lists the names of files to which it actually makes changes. In case something goes wrong, the original versions of changed files are not overwritten but moved to a temporary folder.
