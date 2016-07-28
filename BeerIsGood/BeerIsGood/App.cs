using Xamarin.Forms;

namespace BeerIsGood
{
    public class App : Application
    {
        public static Connection.ConnectionService ConnectionService = new Connection.ConnectionService();
        public static Plugin.Geolocator.Abstractions.Position Position = new Plugin.Geolocator.Abstractions.Position();

        

        public App()
        {
            MainPage = new Views.RootPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
