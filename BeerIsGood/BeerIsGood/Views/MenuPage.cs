using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            Title = "Menü";
            BackgroundColor = Color.FromHex("333333");

            Menu = new MenuListView();

            var MenuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Color.FromHex("AAAAAA"),
                    Text = "Menü"
                }
            };

            var Layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Layout.Children.Add(MenuLabel);
            Layout.Children.Add(Menu);

            Content = Layout;
        }
    }
}
