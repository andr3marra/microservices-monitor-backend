namespace microservices_monitor
{
    public class ServiceData : IDisposable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public uint PoolingInterval { get; set; }
        public System.Timers.Timer Timer { get; set; }

        public LinkedList<HealthCheckStatus> HealthChecks = new LinkedList<HealthCheckStatus>();
        public ServiceData(string name, string uri, uint poolingInterval) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Uri = new Uri(uri);
            PoolingInterval = poolingInterval;
            Timer = new System.Timers.Timer(PoolingInterval);
            Timer.Start();
        }

        public void Dispose()
        {
            Timer.Stop();
            Timer.Dispose();
        }
    }
}
