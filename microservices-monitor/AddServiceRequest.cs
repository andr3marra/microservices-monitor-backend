namespace microservices_monitor
{
    public class AddServiceRequest
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public uint PoolingInterval { get; set; }
    }
}
