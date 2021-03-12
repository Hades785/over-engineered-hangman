# Over-engineered hangman

A game of Hangman.

This was supposed to be a collection of somewhat clean OOP concepts for a
friend...  
Yeah, this has not gone well.

## Branch: `main`

Cleaned up my mess, to try and come back to the original objective.

### Dependencies

Uses [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) to work
with simple word lists in JSON files.

## Branch: `madness`

Unaltered madness, why do things cleanly, when you can have fun making things
dynamically modular right?

Definitely not clean :)

### Dependencies

Uses [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) to work
with simple word lists in JSON files.

Uses [Lib.Harmony](https://www.nuget.org/packages/Lib.Harmony) to patch the
base behaviour 

### Specifics

Only a simple word list implementation is built-in, and two extensions are
included, that can be loaded by adding the required `.dll` files to the `mods/`
directory in the directory the program runs in. If it does not exist, it is
created at the first launch.

Each individual mod should be in its own directory with its dependencies.

The two included extensions are:
- `hangman-static-wordlist`: Four static word lists, one for each included
  language.
- `hangman-dynamic-wordlist`: Loads word lists in JSON format from the
  `mods/<mod>/data/` directory.
