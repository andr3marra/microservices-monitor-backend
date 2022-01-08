using microservices_monitor.Models.Microservice;

namespace microservices_monitor.Models.ServiceInformationController.Responses {
    public class GetDetailedServiceResponse {
        // Service Register Information
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public uint PoolingInterval { get; set; }

        // Service Current Status
        public IEnumerable<ServiceHealthCheckResponse> HealthChecks { get; set; }
    }
}
