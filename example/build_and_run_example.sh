#!/bin/bash

example_file=$1
example_name=$(basename $example_file .cs)

mkdir -vp bin
cp ../bin/SGson.dll bin/SGson.dll
mcs -warn:4 $example_file /reference:bin/SGson.dll /main:SGson.Example."$example_name" /out:bin/"$example_name".exe
mono bin/"$example_name".exe
