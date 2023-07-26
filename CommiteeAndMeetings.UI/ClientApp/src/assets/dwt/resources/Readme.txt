// @2018/06/28
* Product: Dynamsoft Web TWAIN SDK v14
* Summary: this Readme.txt is to help you understand the files under the Resources folder

====== Dynamsoft JavaScript Libraries ======

- dynamsoft.webtwain.config.js
This file is used to make basic configuration of Dynamic Web TWAIN. It's where you enter the product key for the product and to change the initial viewer size, etc.

- dynamsoft.webtwain.initiate.js
This file is the core of the Dynamic Web TWAIN JavaScript Library. You're not supposed to change it without consulting the Dynamsoft Support Team.

- dynamsoft.webtwain.install.js
This file is used to configure the dialogs which are shown when Dynamic Web TWAIN is not installed or needs to be upgraded. This file is already referenced inside the dynamsoft.webtwain.initiate.js

- dynamsoft.webtwain.css
This file contains the style definitions for all the elements of built-in image viewer, progress bar, dialogs, etc.

- addon/dynamsoft.webtwain.addon.pdf.js
This file contains the functionalities of the PDF Rasterizer addon. You're not supposed to change it without consulting the Dynamsoft Support Team.

====== End-user Distribution files ======

Under dist/win/

* for end users who use IE 10/11, Edge, Chrome or Firefox on Windows (Windows XP/7/8/2008/2012/2016 and 10; 32-bit and 64-bit)

- DynamsoftServiceSetup.msi
This Dynamsoft Service needs to be manually installed on end-user machine. For controlled environment, you can also use the MSI to silently deploy the service to all end-user machines, refer to: https://developer.dynamsoft.com/dwt/kb/2866

- WinDWT_*_*.*.*.*.zip
- WinDWT_*-*.*.*.*_x64.zip
These .zip files contain the core scanning library which is TWAIN-based. Please keep it in the Resources folder on your HTTP server. The file will be automatically and silently deployed to the end-user machine once the Dynamsoft Service is installed. You must make sure that your HTTP server is able to serve .zip files.

- DynamicWebTWAINModule.msi or DynamicWebTWAINModuleTrial.msi 
In case the automatic deplyment of the .zip files fails, a prompt will come up and provide this .msi file for end users to download and install the core scanning library.

* for Windows end users who use IE 6/7/8/9

These legacy browsers don't support HTML5, as a result, they still rely on the old ActiveX technology. The ActiveX uses the following packages to install.

- ActiveX/DynamicWebTWAINActiveX.exe
This is the default package to be downloaded in an automatic prompt, the end user needs to install it manually.

- ActiveX/WebTwainMSIX64.msi or WebTwainMSITrialX64.msi
- ActiveX/WebTwainMSIX86.msi or WebTwainMSITrialX86.msi
These .msi files are mainly designed for silent group deployment in a controlled environment (https://developer.dynamsoft.com/dwt/kb/2866).

- ActiveX/DynamicWebTWAIN.cab and ActiveX/DynamicWebTWAINx64.cab
These .cab files are Microsoft's legacy way to install ActiveX in Internet Explorer. If you prefer using them, you can set ActiveXInstallWithCAB to true in dynamsoft.webtwain.config.js.

- Pdf.zip and Pdfx64.zip
These files are used to install the PDF Rasterizer on the client machine. For more info, check out https://developer.dynamsoft.com/dwt/guide/how-to-use-the-pdf-rasterizer.

-----------------------------------------

Under dist/mac/

* for end users who use Safari, Chrome or Firefox on mac OS (OS X 10.6.8+)

- DynamsoftServiceSetup.pkg
This Dynamsoft Service needs to be manually installed on end-user machine.

- MacDWT_*.*.*.*.zip
These .zip files contain the core scanning library which is TWAIN|ICA-based. Please keep it in the Resources folder on your HTTP server. The file will be automatically and silently deployed to the end-user machine once the Dynamsoft Service is installed. You must make sure that your HTTP server is able to serve .zip files.

- MacPdf.zip
This file is used to install the PDF Rasterizer on the client machine. For more info, check out https://developer.dynamsoft.com/dwt/guide/how-to-use-the-pdf-rasterizer.

-----------------------------------------

Under dist/linux
* for end users who use Chrome or Firefox on Linux (Ubuntu 12.0.4+, Debian 8+, Fedora 24+, mint 18.3; 64-bit)

- DynamsoftServiceSetup.deb or DynamsoftServiceSetup.rpm
This Dynamsoft Service needs to be manually installed on Debian/Ubuntu/mint or Fedora end-user machine.

- LinuxDWT_*.*.*.*.zip
These .zip files contain the core scanning library which is SANE-based. Please keep it in the Resources folder on your HTTP server. The file will be automatically and silently deployed to the end-user machine once the Dynamsoft Service is installed. You must make sure that your HTTP server is able to serve .zip files.

- LinuxPdf.zip
This file is used to install the PDF Rasterizer on the client machine. For more info, check out https://developer.dynamsoft.com/dwt/guide/how-to-use-the-pdf-rasterizer.

====== Dynamsoft Service Update Packages ======

Under dist/serviceupdate

- WinDSUpdate_14.0.0.0618.zip
- WinDSUpdate_14.0.0.0618_x64.zip
- MacDSUpdate_14.0.0.0618.zip
- LinuxDSUpdate_14.0.0.0618.zip
These files are used to update the Dynamsoft Service. The update is not enabled by default but can be configured to be automated by setting IfUpdateService to true in dynamsoft.webtwain.config.js.
