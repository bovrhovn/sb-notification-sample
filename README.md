# Example of using Notification Hub and IoT hub to do notification

Simple example of using **Azure Notification Hub** to do large scale notifications. 
This example enables you to send notifications to Windows clients from Azure Web App and when clients responds, see the response in realtime with the use of SignalR.

Technology used:
1. ASP.NET Core 3.1
2. Azure SignalR
3. Azure Notification Hub
4. Windows 32 App (Windows Service)

Since we will be communicating with WPF or Windows Service, we need to use [interprocess communication](https://docs.microsoft.com/en-us/windows/uwp/communication/interprocess-communication).

![.NET Core](https://github.com/bovrhovn/sb-notification-sample/workflows/.NET%20Core/badge.svg?branch=master)
