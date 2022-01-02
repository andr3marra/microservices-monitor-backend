namespace microservices_monitor
{
    public class AddEdgeInformationRequest
    {
        public string DependencyName  { get; set; }
        public Guid TargetNodeId { get; set; }
    }
}
