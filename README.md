# Example of using IoT hub to query and generate devices with connection string

Simple example of using **Azure IotHub** to do large scale notifications to different devices, separated by tag, via [ASP.NET Core 3.1](https://asp.net). To run the app, you need [ASP.NET Core 3.1](https://asp.net) installed.

This branch (devices) focuses on generating and query devices (with custom defined tag via html form).

When you open solution in your favorite editor, navigate to **Pages** folder and inside you will find 2 pages - **Generate** (generate device with custom tag) and **Index** (query devices with different tags and getting connection string to connect to that specific device).

![pages for generation](https://webeudatastorage.blob.core.windows.net/web/iothub-generate-device.png)

In order for making it work, you need [Azure Iothub](https://azure.microsoft.com/en-us/services/iot-hub/) account and you need to add connection string (setting **ConnectionString**) and name (setting **Name**) in the appSettings.json file.

![settings](https://webeudatastorage.blob.core.windows.net/web/iothub-generate-settings.png)

After you have done that, you run the application (either via favorite editor or via [CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run)), select Generate or Query devices in menu.

![menu options](https://webeudatastorage.blob.core.windows.net/web/iothub-generate-menu-option.png)

Technology used:
1. [ASP.NET Core 3.1](https://asp.net)
2. [Azure Iothub](https://azure.microsoft.com/en-us/services/iot-hub/)

![.NET Core](https://github.com/bovrhovn/sb-notification-sample/workflows/.NET%20Core/badge.svg?branch=master)
