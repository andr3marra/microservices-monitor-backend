namespace microservices_monitor.Models.ServiceInformationController.Requests {
    public class AddServiceRequest {
        public string Name { get; set; }
        public string Uri { get; set; }
        public uint PoolingInterval { get; set; }
    }
}
