namespace SbNotifierDashboard.Options
{
    public class IotOptions : Options
    {
        public string EventHubName { get; set; }
        public string EventHubsCompatibleEndpoint { get; set; }
        public string SasKey { get; set; }
        public int PageSize { get; set; } = 100;
    }
}