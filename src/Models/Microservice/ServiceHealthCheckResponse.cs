using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace microservices_monitor.Models.Microservice
{
    public class ServiceHealthCheckResponse
    {
        public HealthStatus? OverallStatus { get; set; }
        public string TotalChecksDuration { get; set; }
        public IReadOnlyDictionary<string, DependencyHealthChecks> DependencyStatus { get; set; }
    }

}