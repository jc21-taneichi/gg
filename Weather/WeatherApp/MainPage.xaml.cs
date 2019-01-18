using System;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;
        string uri = "https://p48-calendars.icloud.com/published/2/G_HUbAYckYLwD04btDaS_e1RMbDBCc8CTxGF4LVCIowXvW4mp8BxmnNoccTIxgr7C7HUxrCb2yFKMSNT_p3BKlGiRShOE0XjEK6uUkQp0AM";



        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();

            Button1.Clicked += ButtonClickEvent;
        }

        private void ButtonClickEvent(object sender, EventArgs e)
        {
            
            Navigation.PushModalAsync(new NavigationPage(new MainPage2()));
            
        }
        protected override async void OnAppearing()
        {
            var result = await VEvent.GetStringAsync(uri);
            foreach(VEvent ev in result){
                //await DisplayAlert("VEvent", "[" + ev.DTSTART + "]["+ev.SUMMARY+"]["+ev.ADDRESS+"]", "OK");


            }

        }

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                BindingContext = weatherData;
            
        }

        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            
            requestUri += "&units=metric"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;

        
        }

    }

}
    

    

