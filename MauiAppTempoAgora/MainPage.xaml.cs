using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Threading.Tasks;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

      

        private async void Button_Clicked_1(object sender, EventArgs e)
        {

            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat}\n" +
                                         $"Longitude: {t.lon}\n" +
                                         $"Nascer do Sol: {t.sunrise}\n" +
                                         $"Por do Sol: {t.sunset}\n" +
                                         $"Temp. Máx: {t.temp_max}\n" +
                                         $"Temp. Min: {t.temp_min}\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Cidade não encontrada ou sem dados de previsão.";
                    }
                }
                else
                {
                    lbl_res.Text = "Informe uma cidade válida.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
   }
