using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage2 : ContentPage
    {

        RestService _restService;
        

        public MainPage2()
        {
            InitializeComponent();
            _restService = new RestService();

            Button2.Clicked += ButtonClickEvent;
            

        }

        

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Searchcity.Text))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                BindingContext = weatherData;

               
            }
        }
     

        private void ButtonClickEvent(object sender, EventArgs e)
        {
            
            Navigation.PopModalAsync();
        }



        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={Searchcity.Text}";
            requestUri += "&units=metric&lang=ja"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;


        }
    }
}