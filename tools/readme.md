# tools

## compile
small perl program that takes a space engineers script, wraps some code around it to make it standalone compileable and uses csc.exe to compile it to a dll. The dll isn't of much use (afaik). However the compiler does full error checking, with nice reporting, without irrelvant warnings and other crap.Won't output anything except errors.

### install
adapt the paths to your system.

### usage
perl compile FILE.cs   

### known issues
FILE.cs must be in current directory. Paths won't work 

## build
an unsophiscated tool to glue together csharp sources for space engineer scripts

### install
copy it somewhere

### usage
perl build FILE.cs
see build.readme.md for documentation

### known issues
refer to documentation
