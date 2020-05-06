# Example of IoT hub to do notification to a WPF application

Simple example of using **Azure Iot Hub** to do large scale notifications. I highly recommend visiting [this developer guide for Azure IoT Hub](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide).

This example enables you to send notifications to Windows clients from Azure Web App and when clients responds, see the response in realtime with the use of [SignalR](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview). It uses [Azure IoT Hub MQTT](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-mqtt-support) protocol.

Technology used:
1. ASP.NET Core 3.1
2. Azure SignalR
3. Azure Iot Hub
4. Windows WPF application

## Configuration and settings

In the Iot branch you have 2 applications:
1. Web application to send the messages [SbNotifierDashboard](https://github.com/bovrhovn/sb-notification-sample/tree/iothub/source/SbNotifier/SbNotifierDashboard)
2. WPF application to receive and acknowledges the message [SbNotifierUw](https://github.com/bovrhovn/sb-notification-sample/tree/iothub/source/SbNotifier/SbNotifierDevice)

### Web application

To setup web application, you need to configure [appSettings.json](https://github.com/bovrhovn/sb-notification-sample/blob/iothub/source/SbNotifier/SbNotifierDashboard/appsettings.json) file. All the information is available via portal or via [Azure IOT Cli](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-using-cli).

In order to continue, you will need [Azure IoT Hub](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal). After that is done, you can copy values from Azure Iot Hub into configuration files.
![Azure IoT Hub policies](https://csacoresettings.blob.core.windows.net/public/IotAccessPolicies_1.png)

The connection string is available, when you select a policy.
![Connection String](https://csacoresettings.blob.core.windows.net/public/IotAccessPolicies_2.png)

In order to use Signalr, you will need to setup Azure SignalR. Check [this tutorial](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core) on how to do that. Replace the connection string as explained [here](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core#add-azure-signalr-to-the-web-app).

### WPF application
To use WPF app, you will need to enter the connection string for Azure IoT Hub [here](https://github.com/bovrhovn/sb-notification-sample/blob/iothub/source/SbNotifier/SbNotifierDevice/Helpers/Constants.cs).

You can find the connection string in Azure Policy (the same as you did for web app).
![Connection String](https://csacoresettings.blob.core.windows.net/public/IotAccessPolicies_2.png)

## Conclusion
If you find any challenges, submit an issue.


