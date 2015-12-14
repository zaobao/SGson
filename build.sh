mkdir -vp bin
mcs -target:library -warn:4 -recurse:src/*.cs /out:bin/SGson.dll
