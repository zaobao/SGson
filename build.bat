if not exist bin (md bin)
csc -target:library -warn:4  -out:bin\\SGson.dll -recurse:src\\*.cs
@echo off
pause