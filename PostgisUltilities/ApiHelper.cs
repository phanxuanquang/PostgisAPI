using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class ApiHelper
    {
        private string baseUrl;
        public ApiHelper(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }
        private async Task<string> Get(string header)
        {
            string endpoint = baseUrl + header;

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

        private async void Post(string header, string bodyAsJson)
        {
            string endpoint = baseUrl + header;
            using (HttpClient client = new HttpClient())
            {
                StringContent body = new StringContent(bodyAsJson, Encoding.UTF8, "application/json-patch+json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(endpoint, body);
                    MessageBox.Show("Post data to the database successfully.", "Post data successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Failed: {endpoint}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
