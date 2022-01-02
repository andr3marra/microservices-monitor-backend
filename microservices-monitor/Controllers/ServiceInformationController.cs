using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace microservices_monitor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceInformationController : ControllerBase
    {
        private readonly ILogger<ServiceInformationController> _logger;
        ServicesRepository _servicesRepository;

        public ServiceInformationController(ILogger<ServiceInformationController> logger, ServicesRepository servicesRepository)
        {
            _logger = logger;
            _servicesRepository = servicesRepository;
        }

        [HttpGet(Name = "GetAllServiceInformation")]
        public IEnumerable<ServiceInformationResponse> GetAll()
        {
            var response = new LinkedList<ServiceInformationResponse>();
            foreach (var service in _servicesRepository.GetAllServices())
            {
                HealthCheckStatus? lastHealthCheck = service.HealthChecks.Last?.Value;
                response.AddLast(new ServiceInformationResponse()
                {
                    id = service.Id.ToString(),
                    name = service.Name,
                    status = lastHealthCheck?.OverallStatus ?? "Unknown",
                    edges = lastHealthCheck?.DependencyHealthChecks.Select(x => new EdgeResponse()
                    {
                        id = string.Join("|", new string[] { service.Id.ToString(), x.Key }),
                        targetId = x.Value.TargetNodeId?.ToString(),
                        status = x.Value.Status
                    })
                });
            }

            return response;
        }

        [HttpGet("{serviceId}", Name = "GetServiceInformation")]
        public IActionResult Get(Guid serviceId)
        {
            try{
                if(_servicesRepository.TryGetService(serviceId, out ServiceData service))
                {
                    HealthCheckStatus? lastHealthCheck = service.HealthChecks.Last?.Value;
                    ServiceInformationResponse response = new ServiceInformationResponse() {
                        id = service.Uri.ToString(),
                        name = service.Name,
                        status = lastHealthCheck?.OverallStatus ?? "Unknown",
                        edges = lastHealthCheck?.DependencyHealthChecks.Select(x => new EdgeResponse()
                        {
                            id = string.Join("|", new string[] { service.Id.ToString(), x.Key }),
                            targetId = x.Value.TargetNodeId?.ToString(),
                            status = x.Value.Status
                        })
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddServiceInformation")]

        public IActionResult Post([FromBody] AddServiceRequest addServiceRequest)
        {
            try
            {
                if (_servicesRepository.TryAddService(addServiceRequest, out Guid? id))
                {
                    return Ok(id);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "RemoveServiceInformation")]
        public IActionResult Put(Guid id)
        {
            try
            {
                if (_servicesRepository.TryRemoveService(id))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{serviceId}", Name = "AddDependencyInformation")]

        public IActionResult PostAddDependencyInformation(Guid serviceId, [FromBody] AddEdgeInformationRequest addEdgeInformationRequest)
        {
            try
            {
                if(_servicesRepository.AddDependencyInformation(serviceId, addEdgeInformationRequest))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                    //NotFound
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}