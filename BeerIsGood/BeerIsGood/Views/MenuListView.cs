using System.Collections.Generic;
using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<MenuItem> Data = new MenuListData();

            ItemsSource = Data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            var Cell = new DataTemplate(typeof(MenuCell));
            Cell.SetBinding(MenuCell.TextProperty, "Title");

            ItemTemplate = Cell;
        }
    }
}
