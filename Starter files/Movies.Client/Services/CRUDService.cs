using Movies.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using System.IO;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        private static HttpClient _httpCLient = new HttpClient();

        public CRUDService()
        {
            _httpCLient.BaseAddress = new Uri("http://localhost:57863");
            _httpCLient.Timeout = new TimeSpan(0, 0, 30);
            _httpCLient.DefaultRequestHeaders.Clear();
            _httpCLient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpCLient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));

        }
        public async Task Run()
        {
            await GetResource();
        }

        public async Task GetResource()
        {
            var response = await _httpCLient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var movies = new List<Movie>();
           var content = await response.Content.ReadAsStringAsync();
            if(response.Content.Headers.ContentType.MediaType == "application/json")
            {
                 movies = (List<Movie>)JsonSerializer.Deserialize<List<Movie>>(content,
                   new JsonSerializerOptions()
                   {
                       PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                   });
            } else if (response.Content.Headers.ContentType.MediaType == 'application/xml')
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>)serializer.Deserialize(new StringReader(content));

            }
     
        }
    }
}