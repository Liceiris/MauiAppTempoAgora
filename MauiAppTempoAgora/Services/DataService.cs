using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "899f87be92f57829dcfa36883ce308bb";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&lang=pt_br&appid={chave}";

            try
            {
                using (HttpClient Client = new HttpClient())
                {
                    HttpResponseMessage resp = await Client.GetAsync(url);

                    //  Tratamento de erros
                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        
                        return null;
                    }
                    else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new Exception("Chave da API inválida ou não autorizada.");
                    }
                    else if (!resp.IsSuccessStatusCode)
                    {
                        throw new Exception($"Erro da API: {resp.StatusCode}");
                    }

                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    // converter sunrise/sunset
                    DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]).ToLocalTime().DateTime;
                    DateTime sunset = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]).ToLocalTime().DateTime;

                 
                    t = new Tempo
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString("HH:mm"),
                        sunset = sunset.ToString("HH:mm")
                    };
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("Sem conexão com a internet. Verifique sua rede.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }

            return t;
        }
    }
}