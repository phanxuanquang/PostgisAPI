using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class ApiHelper
    {
        public string baseUrl {  get; set; }
        /// <summary>
        /// Create a new API helper with defalt base URL https://localhost:7186
        /// </summary>
        public ApiHelper()
        {
            baseUrl = $"https://localhost:7186";
        }
        /// <summary>
        /// Get data from the database
        /// </summary>
        /// <param name="header">Header of the request.</param>
        /// <returns>Requested data in JSON format, need to be deserialized in to proper object.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Get(string header)
        {
            string endpoint = $"{baseUrl}{header}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        /// <summary>
        /// Post data to the database
        /// </summary>
        /// <param name="header">Header of the request.</param>
        /// <param name="bodyAsJson">Data to be posted, in proper JSON format.</param>
        public async void Post(string header, string bodyAsJson)
        {
            string endpoint = $"{baseUrl}{header}";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
                try
                {
                    await client.PostAsync(endpoint, content);
                    MessageBox.Show("Post data to the database successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post data to the database failed.", $"Failed: {header}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
