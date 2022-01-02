namespace microservices_monitor
{
    public class DependencyHealthCheck
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public Guid? TargetNodeId { get; set; }
        //public DependencyHealthCheck()
        //{
        //    Id = Guid.NewGuid();
        //    Status = "Unknown";
        //}
        public DependencyHealthCheck(string name, string status, string description, Guid? targetNodeId = null)
        {
            Name = name;
            Status = status;
            Description = description;
            TargetNodeId = targetNodeId;
        }
    }
}
