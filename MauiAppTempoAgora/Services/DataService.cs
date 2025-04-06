using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "9d99cb7f4b6bc931b4b0fdc01b4e02c8";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage resp = await httpClient.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min  = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"], 
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),

                    }; //Fecha objeto do tempo
                } //Fecha if se o status do servidor foi um sucesso

                else if (resp.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new Exception("Você está sem conexão com a internet!");

                }
                else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Erro, Cidade não encontrada!");
                }

            } // Fecha o laço using

            return t;

        }
    }
}