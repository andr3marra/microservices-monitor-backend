using microservices_monitor.Models.Microservice;

namespace microservices_monitor.Models.ServiceRepository {
    public class ServiceData : IDisposable {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public uint PoolingInterval { get; set; }
        public System.Timers.Timer Timer { get; set; }
        private readonly LinkedList<ServiceHealthCheckResponse> healthChecks;
        public IEnumerable<ServiceHealthCheckResponse> HealthChecks { get { return healthChecks; } }
        public Dictionary<string, Guid> Links { get { return _links; } }
        private readonly Dictionary<string, Guid> _links;

        public ServiceData(string name, string uri, uint poolingInterval) {
            Id = Guid.NewGuid();
            Name = name;
            Uri = new Uri(uri);
            healthChecks = new LinkedList<ServiceHealthCheckResponse>();
            _links = new Dictionary<string, Guid>();
            if (poolingInterval < 1000) {
                poolingInterval = 1000;
            }
            PoolingInterval = poolingInterval;
            Timer = new System.Timers.Timer(PoolingInterval);
            Timer.Start();
        }
        public void AddHealthCheck(ServiceHealthCheckResponse serviceHealthCheckResponse) {
            healthChecks.AddLast(serviceHealthCheckResponse);
            foreach (string key in serviceHealthCheckResponse.DependencyStatus.Keys) {
                if (!Links.ContainsKey(key)) {
                    Links.Add(key, Guid.NewGuid());
                }
            }
        }

        public void Dispose() {
            Timer.Stop();
            Timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
