# NAME

build - an unsophiscated tool to glue together csharp sources for space engineer scripts  

# SYNOPSIS

    perl build INFILE
    perl build --infile INFILE [--outfile OUTFILE] [--verbose] [--libdirs DIR1 --libdirs DIR2 ]
            
    If INFILE is - STDIN will be used.
    If OUTFILE is - STDOUT will be used.
    If INFILE is - and OUTFILE ist not specified STDOUT will be used.
    If INFILE is not - and OUTFILE is not specified INFILE.out will be used

    Libdirs default to './lib' and '../lib'
          

# DESCRIPTION

build scans INFILE for '#use Bla' lines. Each matching line is prepended with '//', so the comiler won't panic, and the requested lib will be remembered.

Every lib requested in INFILE will be searched for in LIBDIRS (default to './lib' and '../lib'), with filename LIB.cs . If found the lib will be searched for '#use' statements too, and the source code will be inserted after the last line of INFILEs source. If the lib can't be found in any LIBDIR build aborts with an error. 

Once all libs have been processed the resulting code will be written to OUTFILE.  

Every lib will only be added once, no matter how many times a matching use statement is encountered, this holds true even if the lib is requested by multiple other libs.

## Libs and Filenames

Libnames may only consist of word characters \[0-9A-Za-z\_\] and dots, and must begin and end in a word character. A sample lib named LIBNAME will be searched in files named LIBNAME.cs in all libdirs (the first one found is used). Libs with dots in their name are treated a bit differently, the dots are replaced with slashes, so the lib will be searched in subdirectories of the libdirs, so one can organize libs neatly.

### Examples

    #use Foo     => LIBDIR/Foo.cs
    #use Foo.Bar => LIBDIR/Foo/Bar.cs

### Libdirs

build has two builtin libdirs, './lib' and '../lib' . Additional libdirs can be supplied with --libdir . Libdirs are searched in the order they are given, the builtin first.

## Troubleshooting

If build bails out with any error, or misbehaves elsewise the --verbose flag might help. It enables debugging output. 

# CAVEATS

Currently there is no way to specify additional libdirs beside the --libdir commandline option, and no way to remove './lib' and '../lib' from search path, beside editing the source code. 

# AUTHOR

blade 

# LICENSE

This code isn't intended for public use, and not licensed in any way, to nobody. Until it is, use at your own risk, technical, legal or otherwise.
