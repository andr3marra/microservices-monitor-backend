namespace microservices_monitor
{
    public class HealthCheckResponse
    {
        public string OverallStatus { get; set; }
        public string TotalChecksDuration { get; set; }
        public IDictionary<string, DependencyStatus> DependencyHealthChecks { get; set; }
        public HealthCheckResponse() {
            DependencyHealthChecks = new Dictionary<string, DependencyStatus>();
        }
        public HealthCheckResponse(HealthCheckResponse healthCheckResponse)
        {
            OverallStatus = healthCheckResponse.OverallStatus;
            TotalChecksDuration = healthCheckResponse.TotalChecksDuration;
            DependencyHealthChecks = healthCheckResponse.DependencyHealthChecks;
        }
    }

    public class DependencyStatus
    {
        public string Status { get; set; }
        public string Description { get; set; }
        //public IReadOnlyDictionary<string, object> Data { get; set; }
    }
}