copy /y ..\\bin\\SGson.dll bin\\
csc -target:exe -warn:4 -out:bin\\GsonTest.exe -reference:bin\\SGson.dll -recurse:*.cs
bin\\GsonTest.exe
@echo off
pause