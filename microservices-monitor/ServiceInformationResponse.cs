namespace microservices_monitor
{

    public class ServiceInformationResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public IEnumerable<EdgeResponse> edges { get; set; }
    }

    public class EdgeResponse
    {
        public string id { get; set;}
        public string? targetId { get; set; }
        public string status { get; set; }
    }
}
