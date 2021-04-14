# LSEEDEE

A little tool to drive a Crystalfonz CFA643 display.

Docs: https://www.crystalfontz.com/products/document/342/CFA-634_v2.0.pdf

## Building a Windows exe

    dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:IncludeNativeLibrariesForSelfExtract=true

## Driver

The drivers for the display can be downloaded here: https://www.crystalfontz.com/product/usblcddriver