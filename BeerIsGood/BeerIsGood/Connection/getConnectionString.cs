using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BeerIsGood.Connection
{
    partial class ConnectionService
    {
        public async Task<string> getConnectionString(string URL)
        {
            Classes.Facilities Data = new Classes.Facilities();

            string Content = "";

            // Rest URL für News Download
            var uri = new Uri(string.Format(URL, string.Empty));

            try
            {
                var response = await App.ConnectionService.Client.GetAsync(uri);    // GET Request an Server senden um Daten zu Empfangen
                if (response.IsSuccessStatusCode)                           // Nur bei HTTP OK (kein Fehler) werten wir die Daten aus
                    Content = await response.Content.ReadAsStringAsync();   // Inhalt als String dekodieren    
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Content;
        }
    }
}
