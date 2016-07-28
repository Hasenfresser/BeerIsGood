using System;

using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public class RootPage : MasterDetailPage
    {
        MenuPage MenuPage;

        public RootPage()
        {
            MenuPage = new MenuPage();

            MenuPage.Menu.ItemSelected +=
                (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = MenuPage;
            Detail = new NavigationPage(new Home());
        }

        async void NavigateTo(MenuItem Menu)
        {
            if (Menu == null)
                return;

            Page DisplayPage = (Page)Activator.CreateInstance(Menu.TargetTyp);

            try
            {
                Detail = new NavigationPage(DisplayPage);
            }
            catch (Exception Exception)
            {
                await App.Current.MainPage.DisplayAlert("FEHLER", "Fehler " + Exception.Message, "OK");
            }

            MenuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}
