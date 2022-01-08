using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace microservices_monitor.Models.Microservice
{
    public class DependencyHealthChecks
    {
        public IReadOnlyDictionary<string, object> Data;
        public string? Description;
        public TimeSpan Duration;
        public Exception? Exception;
        public HealthStatus Status;
        public IEnumerable<string> Tags;
    }
}
