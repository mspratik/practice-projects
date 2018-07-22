#!/bin/bash

## Build program
make

## Provide file path for processing files
## -P or --path : To provide file path for the PPRO file
## -FP or --file-path : To provide file path for the Merchant files
## WARNING -- Please provide appropriate file paths!!
## Execute program
./ppro.exe -P F:/Cygwin/home/Pratik/ppro/Solution_2/master/ -FP F:/Cygwin/home/Pratik/ppro/Solution_2/slave/