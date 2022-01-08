namespace microservices_monitor.Models.ServiceInformationController.Requests {
    public class AddEdgeInformationRequest {
        public string DependencyName { get; set; }
        public Guid TargetNodeId { get; set; }
    }
}
