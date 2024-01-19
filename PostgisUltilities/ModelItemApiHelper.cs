using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PostgisUltilities
{
    public class ModelItemApiHelper
    {
        private readonly HttpClient client;

        public ModelItemApiHelper(string serviceEndpoint = "https://localhost:7214")
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(serviceEndpoint)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get all model items of an model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">/api/modelitems</param>
        /// <returns>A list of model item of the input model</returns>
        public async Task<T> Get<T>(string endpoint, Guid ModelID)
        {
            HttpResponseMessage response = await client.GetAsync($"{endpoint}/{ModelID}");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        /// <summary>
        /// Get the model item of an model by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="ModelID"></param>
        /// <param name="ModelItemID"></param>
        /// <returns>The model item that has the input ID</returns>
        public async Task<T> GetById<T>(string endpoint, Guid ModelID, Guid ModelItemID)
        {
            HttpResponseMessage response = await client.GetAsync($"{endpoint}/{ModelID}/{ModelItemID}");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Create a new model item for a model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="ModelID"></param>
        /// <param name="data"></param>
        /// <returns>Created model item</returns>
        public async Task<T> Post<T>(string endpoint, Guid ModelID, ModelItemDB data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{endpoint}/{ModelID}", content);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
