using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class ApiHelper
    {
        public string baseUrl;
        public ApiHelper() { }
        public async Task<string> Get(string header)
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

        public async void Post(string header, string bodyAsJson)
        {
            string endpoint = $"{baseUrl}{header}";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Post data to the database successfully.", "Post data successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Post data to the database failed.", $"Failed: {endpoint}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
