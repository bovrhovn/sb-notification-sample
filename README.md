![.NET Core](https://github.com/bovrhovn/sb-notification-sample/workflows/.NET%20Core/badge.svg?branch=master)

# Example of using Notification Hub and IoT hub to do notification

Simple example of using **Azure Notification Hub** to do large scale notifications. 
This example enables you to send notifications to Windows clients from Azure Web App and when clients responds, see the response in realtime with the use of SignalR.

Technology used:
1. ASP.NET Core 3.1
2. Azure SignalR
3. Azure Notification Hub

Since we will be communicating with WPF or Windows Service, you can use [interprocess communication](https://docs.microsoft.com/en-us/windows/uwp/communication/interprocess-communication).

## Configuration and settings

In the master branch you have 2 applications:
1. Web application [SbNotifierDashboard](https://github.com/bovrhovn/sb-notification-sample/tree/master/source/SbNotifier/SbNotifierDashboard)
2. UWP applications [SbNotifierUwp](https://github.com/bovrhovn/sb-notification-sample/tree/master/source/SbNotifier/SbNotifierUwp)

### Web Application Settings
In order to use Web Application, you will need to setup [Azure Notification Hub](https://docs.microsoft.com/en-us/azure/notification-hubs/create-notification-hub-portal). 

When that is finished, you will need to replace connection string in the [appSettings.json](https://github.com/bovrhovn/sb-notification-sample/blob/master/source/SbNotifier/SbNotifierDashboard/appsettings.json). 
![appSettings.json - image](https://webeudatastorage.blob.core.windows.net/web/public/AzureNotificationHub_0.png)

Go to Notification Hub and select Access Policies.
![Access Policies](https://webeudatastorage.blob.core.windows.net/web/NotificationHubAccessPolicies_1.png)

Select appropriate policy and copy connection string. Replace the value in [appSettings.json](https://github.com/bovrhovn/sb-notification-sample/blob/master/source/SbNotifier/SbNotifierDashboard/appsettings.json) with copied value.
![access policy connection string](https://webeudatastorage.blob.core.windows.net/web/NotificationHubAccessPolicies_2.png)

### UWP settings
To receive notifications, you will need to follow [this instructions](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-windows-store-dotnet-get-started-wns-push-notification). After success. replace the values in [Secrets.cs](https://github.com/bovrhovn/sb-notification-sample/blob/master/source/SbNotifier/SbNotifierUwp/Helpers/Secrets.cs) with NotificationHub name and connection string (which you already used in the web page part)

## Conclusion
If you find any challenges, submit an issue.
