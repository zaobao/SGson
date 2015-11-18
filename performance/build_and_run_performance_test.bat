set file=%1
set name=%~n1
copy /y ..\\bin\\SGson.dll bin\\
csc -warn:4 /reference:bin\\SGson.dll -main:SGson.Performance.%name% -out:bin\\%name%.exe %file%
bin\\%name%.exe
@echo off
pause