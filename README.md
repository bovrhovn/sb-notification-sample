# Example of using Notification Hub and IoT hub to do notification

Simple example of using **Azure Notification Hub** to do large scale notifications. 
This example enables you to send notifications to Windows clients from Azure Web App and when clients responds, see the response in realtime with the use of SignalR.

Technology used:
ASP.NET Core 3.1
Azure SignalR
Azure Notification Hub
Windows 32 App

Since we will be communicating with WPF or Windows Service, we need to use interprocess communication (https://docs.microsoft.com/en-us/windows/uwp/communication/interprocess-communication).
