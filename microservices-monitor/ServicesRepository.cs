
using Newtonsoft.Json;
using RestSharp;
using Timer = System.Timers.Timer;
namespace microservices_monitor
{
    public class ServicesRepository
    {
        public List<ServiceData> services = new List<ServiceData>();
        //private readonly List<ServiceInformation> _data = new List<ServiceInformation>();
        public Dictionary<Timer, ServiceData> timersDictionary = new Dictionary<Timer, ServiceData>();


        public ServicesRepository()
        {
            //services.Add(new ServiceData()
            //{
            //    Name = "LocalApi",
            //    Uri = new Uri("https://localhost:7211/status"),
            //    PoolingInterval = 10000,
            //});

            //foreach (var service in services)
            //{
            //    service.Timer = new System.Timers.Timer(service.PoolingInterval);
            //    service.Timer.Elapsed += Timer_Elapsed;
            //    timersDictionary.Add(service.Timer, service);
            //    service.Timer.Start();
            //    Timer_Elapsed(service.Timer, null);
            //}
        }
        public List<ServiceData> GetAllServices()
        {
            return services;
        }
        public bool TryGetService(Guid id, out ServiceData serviceData)
        {
            serviceData = services.FirstOrDefault(x => x.Id == id);

            if (serviceData != default)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TryAddService(AddServiceRequest addServiceRequest, out Guid? id)
        {
            id = null;
            try
            {
                var newService = new ServiceData(addServiceRequest.Name, addServiceRequest.Uri, addServiceRequest.PoolingInterval);
                newService.Timer.Elapsed += Timer_Elapsed;

                services.Add(newService);
                timersDictionary.Add(newService.Timer, newService);
                Timer_Elapsed(newService.Timer, null);

                id = newService.Id;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TryRemoveService(Guid id)
        {
            try
            {
                ServiceData? itemToBeRemoved = services.FirstOrDefault(x => x.Id == id);

                if(itemToBeRemoved != default)
                {
                    services.Remove(itemToBeRemoved);

                    timersDictionary.Remove(itemToBeRemoved.Timer);

                    itemToBeRemoved.Dispose();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddDependencyInformation(Guid serviceId, AddEdgeInformationRequest addEdgeInformationRequest)
        {
            try
            {
                if (TryGetService(serviceId, out ServiceData serviceData))
                {
                    var healthCheckToBeEdited = serviceData.HealthChecks.Last;

                    if(healthCheckToBeEdited.Value.DependencyHealthChecks.TryGetValue(addEdgeInformationRequest.DependencyName, out var dependencyHealthCheck))
                    {
                        dependencyHealthCheck.TargetNodeId = addEdgeInformationRequest.TargetNodeId;
                    }
                    else { return false; }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (timersDictionary.TryGetValue(sender as Timer, out ServiceData service))
            {
                RestClient restClient = new RestClient(service.Uri);

                var signInRequest = new RestRequest(Method.GET);

                var response = restClient.Execute(signInRequest);

                var desserializedResponse = JsonConvert.DeserializeObject<HealthCheckResponse>(response.Content);

                var lastHealthCheckResponse = service.HealthChecks.LastOrDefault();

                if (response.StatusCode == 0 || desserializedResponse == null)
                {
                    if(lastHealthCheckResponse != null)
                    {
                        var newHealthCheckResponse = new HealthCheckStatus(lastHealthCheckResponse);
                        service.HealthChecks.AddLast(newHealthCheckResponse);
                    }
                    else
                    {
                        var newHealthCheckResponse = new HealthCheckStatus();
                        service.HealthChecks.AddLast(newHealthCheckResponse);
                    }
                    
                }
                else
                {
                    var HealthCheckStatusFromResponse = new HealthCheckStatus(desserializedResponse, lastHealthCheckResponse);
                    service.HealthChecks.AddLast(HealthCheckStatusFromResponse);
                }
            }
        }

    }
}
