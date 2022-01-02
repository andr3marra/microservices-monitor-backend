namespace microservices_monitor
{
    public class HealthCheckStatus
    {
        public string OverallStatus { get; set; }
        public string TotalChecksDuration { get; set; }
        public IDictionary<string, DependencyHealthCheck> DependencyHealthChecks { get; set; }
        public HealthCheckStatus()
        {
            OverallStatus = "Unknown";
            DependencyHealthChecks = new Dictionary<string, DependencyHealthCheck>();
        }
        public HealthCheckStatus(HealthCheckStatus healthCheckStatus)
        {
            OverallStatus = healthCheckStatus.OverallStatus;
            TotalChecksDuration = healthCheckStatus.TotalChecksDuration;
            DependencyHealthChecks = healthCheckStatus.DependencyHealthChecks;
        }
        public HealthCheckStatus(HealthCheckResponse healthCheckResponse, HealthCheckStatus previousHealthCheckStatus)
        {
            OverallStatus = healthCheckResponse.OverallStatus;
            TotalChecksDuration = healthCheckResponse.TotalChecksDuration;
            DependencyHealthChecks = new Dictionary<string, DependencyHealthCheck>();
            foreach (var item in healthCheckResponse.DependencyHealthChecks)
            {
                if(previousHealthCheckStatus != null)
                {
                    previousHealthCheckStatus.DependencyHealthChecks.TryGetValue(item.Key, out DependencyHealthCheck? healthCheck);
                    DependencyHealthChecks.Add(item.Key, new DependencyHealthCheck(item.Key, item.Value.Status, item.Value.Description, healthCheck?.TargetNodeId));
                }
                else
                {
                    DependencyHealthChecks.Add(item.Key, new DependencyHealthCheck(item.Key, item.Value.Status, item.Value.Description));
                }
            }
        }
    }
}
