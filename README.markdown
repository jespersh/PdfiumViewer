# PdfiumViewer

[![License](https://img.shields.io/github/license/jespersh/PdfiumViewer.svg)](https://github.com/jespersh/PdfiumViewer/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PdfiumViewer.Forms.svg)](http://nuget.org/packages/PdfiumViewer.Forms)

## Updated (1-6-2021)

This is an updated fork of the archived **[Bluegrams/PdfiumViewer](https://github.com/Bluegrams/PdfiumViewer)** project. It is available as `PdfiumViewer.Forms` via nuget and includes the following changes/ updates:

- Support for form fields (not XFA)
- Updated to newer PDFium binaries with specific patches for this library from https://github.com/jespersh/pdfium-binaries
- Added **.NET5.0** as the only target

## Introduction

PdfiumViewer is a PDF viewer based on the PDFium project.

PdfiumViewer provides a number of components to work with PDF files:

* PdfDocument is the base class used to render PDF documents;

* PdfRenderer is a WinForms control that can render a PdfDocument;

* PdfiumViewer is a WinForms control that hosts a PdfRenderer control and
  adds a toolbar to save the PDF file or print it.

## Compatibility

The PdfiumViewer library has been tested with Windows 10 and only supports .NET5.0-windows targetting currently.

## Using the library

The PdfiumViewer control requires native PDFium libraries. These are not included
in the PdfiumViewer NuGet package. See the [Installation instructions](https://github.com/pvginkel/PdfiumViewer/wiki/Installation-instructions)
Wiki page for more information on how to add these.

## Note on the `PdfViewer` control

The PdfiumViewer library primarily consists out of three components:

* The `PdfViewer` control. This control provides a host for the `PdfRenderer`
  control and has a default toolbar with limited functionality;
* The `PdfRenderer` control. This control implements the raw PDF renderer.
  This control displays a PDF document, provides zooming and scrolling
  functionality and exposes methods to perform more advanced actions;
* The `PdfDocument` class provides access to the PDF document and wraps
  the Pdfium library.

The `PdfViewer` control should only be used when you have a very simple use
case and where the buttons available on the toolbar provide enough functionality
for you. This toolbar will not be extended with new buttons or with functionality
to hide buttons. The reason for this is that the `PdfViewer` control is just
meant to get you started. If you need more advanced functionality, you should
create your own control with your own toolbar, e.g. by starting out with
the `PdfViewer` control. Also, because people currently are already using the
`PdfViewer` control, adding more functionality to this toolbar would be
a breaking change. See [issue #41](https://github.com/pvginkel/PdfiumViewer/issues/41)
for more information.

## Building PDFium

Instructions to build the PDFium library can be found on the [Building PDFium](https://github.com/pvginkel/PdfiumViewer/wiki/Building-PDFium)
wiki page. However, if you are just looking to use the PdfiumViewer component
or looking for a compiled version of PDFium, these steps are not required.
NuGet packages with precompiled PDFium libraries are made available for
usage with PdfiumViewer. See the chapter on **Using the library** for more
information.

Alternatively, the [PdfiumBuild](https://github.com/pvginkel/PdfiumBuild) project
is provided to automate building PDFium. This project contains scripts to
build PdfiumViewer specific versions of the PDFium library. This project
is configured on a build server to compile PDFium daily. Please refer to
the [PdfiumBuild](https://github.com/pvginkel/PdfiumBuild) project page
for the location of the output of the build server. The PdfiumViewer specific
libraries are located in the `PdfiumViewer-...` target directories.

## Bugs

Bugs should be reported through github at
[http://github.com/jespersh/PdfiumViewer/issues](http://github.com/jespersh/PdfiumViewer/issues).

## License

PdfiumViewer is licensed under the Apache 2.0 license. See the license details for how PDFium is licensed.
