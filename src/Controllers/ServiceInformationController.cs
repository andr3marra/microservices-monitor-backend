using microservices_monitor.Models.ServiceInformationController.Requests;
using microservices_monitor.Models.ServiceInformationController.Responses;
using microservices_monitor.Models.ServiceRepository;
using microservices_monitor.Modules;
using Microsoft.AspNetCore.Mvc;

namespace microservices_monitor.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ServiceInformationController : ControllerBase
    {
        private readonly ILogger<ServiceInformationController> _logger;
        readonly ServicesRepository _servicesRepository;

        public ServiceInformationController(ILogger<ServiceInformationController> logger, ServicesRepository servicesRepository)
        {
            _logger = logger;
            _servicesRepository = servicesRepository;
        }

        [HttpGet(Name = "GetServices")]
        public IEnumerable<GetServicesResponse> GetServices()
        {
            var response = new LinkedList<GetServicesResponse>();
            foreach (var service in _servicesRepository.GetAllServices())
            {
                GetServicesResponse getServicesResponse = new GetServicesResponse();

                getServicesResponse.Id = service.Id;
                getServicesResponse.Name = service.Name;
                getServicesResponse.Uri = service.Uri;
                getServicesResponse.PoolingInterval = service.PoolingInterval;
                getServicesResponse.LastHealthCheck = service.HealthChecks.Last();      // verificar
                response.AddLast(getServicesResponse);
            }

            return response;
        }

        [HttpGet("{serviceId}", Name = "GetDetailedService")]
        public IActionResult GetDetailedService(Guid serviceId)
        {
            try
            {
                if (_servicesRepository.TryGetService(serviceId, out ServiceData? service))
                {
                    GetDetailedServiceResponse response = new GetDetailedServiceResponse();

                    response.Id = service.Id;
                    response.Name = service.Name;
                    response.Uri = service.Uri;
                    response.PoolingInterval = service.PoolingInterval;
                    response.HealthChecks = service.HealthChecks;

                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddService")]
        public IActionResult AddService([FromBody] AddServiceRequest addServiceRequest)
        {
            try
            {
                string Name = addServiceRequest.Name;
                string Uri = addServiceRequest.Uri;
                uint PoolingInterval = addServiceRequest.PoolingInterval;

                if (_servicesRepository.TryAddService(Name, Uri, PoolingInterval, out Guid? id))
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

        [HttpDelete("{id}", Name = "DeleteService")]
        public IActionResult DeleteService(Guid id)
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

        [HttpPut("{serviceId}", Name = "LinkServices")]
        public IActionResult LinkServices(Guid serviceId, [FromBody] AddEdgeInformationRequest addEdgeInformationRequest)
        {
            try
            {
                if (_servicesRepository.AddDependencyInformation(serviceId, addEdgeInformationRequest.DependencyName, addEdgeInformationRequest.TargetNodeId))
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