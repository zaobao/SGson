mkdir -vp bin
cp ../bin/SGson.dll bin/
mcs -target:exe -warn:4 -recurse:*.cs /out:bin/GsonTest.exe /reference:bin/SGson.dll
mono bin/GsonTest.exe
