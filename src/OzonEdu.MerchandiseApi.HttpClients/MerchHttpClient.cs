using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public class MerchHttpClient : IMerchHttpClient
    {
        private const string BaseRoute = "v1/api/merch";
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GiveOutMerch(GiveOutMerchRequest request, CancellationToken token)
        {
            var json = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(BaseRoute, stringContent, token);
        }

        public async Task<GetMerchDeliveryStatusResponse?> GetMerchDeliveryStatusRequest(
            GetMerchDeliveryStatusRequest request, CancellationToken token)
        {
            var requestUri = BaseRoute 
                             + $"/delivery?employeeId={request.EmployeeId}&merchPackTypeId={request.MerchPackTypeId}";
            using var response = await _httpClient.GetAsync(requestUri, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<GetMerchDeliveryStatusResponse>(body, options);
        }
    }
}