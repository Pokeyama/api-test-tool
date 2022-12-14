#!/bin/sh
mkdir Template/APITestTool
cp -r APITestTool Template
rm -rf Template/APITestTool/bin
rm -rf Template/APITestTool/obj
dotnet new --install Template
dotnet new --list
