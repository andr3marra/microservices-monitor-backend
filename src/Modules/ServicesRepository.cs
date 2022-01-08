using microservices_monitor.Models.Microservice;
using microservices_monitor.Models.ServiceRepository;
using Newtonsoft.Json;
using RestSharp;
using Timer = System.Timers.Timer;
namespace microservices_monitor.Modules {
    public class ServicesRepository {
        public List<ServiceData> services = new List<ServiceData>();
        public Dictionary<Timer, ServiceData> timersDictionary = new Dictionary<Timer, ServiceData>();


        public ServicesRepository() {
            // Load data from database
            // Start timers
        }
        public List<ServiceData> GetAllServices() {
            return services;
        }

        public bool TryGetService(Guid id, out ServiceData? serviceData) {
            serviceData = services.FirstOrDefault(x => x.Id == id);

            if (serviceData != default) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool TryAddService(string name, string uri, uint poolingInterval, out Guid? id) {
            id = null;
            try {
                var newService = new ServiceData(name, uri, poolingInterval);
                newService.Timer.Elapsed += Timer_Elapsed;

                timersDictionary.Add(newService.Timer, newService);
                Timer_Elapsed(newService.Timer, null);
                services.Add(newService);

                id = newService.Id;
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public bool TryRemoveService(Guid id) {
            try {
                ServiceData? itemToBeRemoved = services.FirstOrDefault(x => x.Id == id);

                if (itemToBeRemoved != default) {
                    services.Remove(itemToBeRemoved);

                    timersDictionary.Remove(itemToBeRemoved.Timer);

                    itemToBeRemoved.Dispose();

                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }

        public bool AddDependencyInformation(Guid serviceId, string dependencyName, Guid targetId) {
            try {
                if (TryGetService(serviceId, out ServiceData? serviceData)
                    && serviceData.Links.TryGetValue(dependencyName, out Guid value)) {

                    serviceData.Links[dependencyName] = targetId;
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                return false;
            }

        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs? e) {
            if (timersDictionary.TryGetValue(sender as Timer, out ServiceData? service)) {
                RestClient restClient = new RestClient(service.Uri);

                var signInRequest = new RestRequest(Method.GET);

                var response = restClient.Execute(signInRequest);

                var desserializedResponse = JsonConvert.DeserializeObject<ServiceHealthCheckResponse>(response.Content);

                if (response.StatusCode == 0 || desserializedResponse == null) {
                    var newHealthCheckResponse = new ServiceHealthCheckResponse();
                    service.AddHealthCheck(newHealthCheckResponse);
                }
                else {
                    service.AddHealthCheck(desserializedResponse);
                }
            }
        }

    }
}
