using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using SbNotifierDevice.Helpers;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace SbNotifierDevice.ViewModels
{
    public class MainPageViewModel : BasePageViewModel
    {
        public MainPageViewModel()
        {
            InfoMessage = String.Empty;
            RegisterCommand = new RelayCommand(async () => await RegisterDeviceAsync());
        }

        private string infoMessage;
        private string deviceId;

        public async Task RegisterDeviceAsync()
        {
            var registryManager = RegistryManager.CreateFromConnectionString(Constants.IotHubConnectionString);
            if (string.IsNullOrEmpty(DeviceId))
            {
                InfoMessage = $"Device ID not specified! Enter name and press Connect!{Environment.NewLine}";
            }
            else
            {
                var device = await registryManager.GetDeviceAsync(deviceId) ??
                             await registryManager.AddDeviceAsync(new Device(deviceId));
                InfoMessage =
                    $"Device registered and is {(device.ConnectionState == DeviceConnectionState.Connected ? "connected" : "disconnected")}{Environment.NewLine}";

                var deviceClient = DeviceClient.CreateFromConnectionString(Constants.IotHubConnectionString, DeviceId,
                    TransportType.Mqtt);
                await deviceClient.SetMethodHandlerAsync("UpdateRequested", UpdateRequestedMethod, null);
            }
        }

        public string DeviceId
        {
            get => deviceId;
            set
            {
                if (value == deviceId) return;
                deviceId = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; private set; }

        private Task<MethodResponse> UpdateRequestedMethod(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);

            if (string.IsNullOrEmpty(data))
            {
                InfoMessage += $"No data received from server.{Environment.NewLine}";
                var result = "{\"result\":\"Invalid parameter\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
            }
            else
            {
                InfoMessage = $"Data received: {data}{Environment.NewLine}";
                var result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
            }
        }

        public string InfoMessage
        {
            get => infoMessage;
            set
            {
                if (value == infoMessage) return;
                infoMessage = value;
                OnPropertyChanged();
            }
        }
    }
}