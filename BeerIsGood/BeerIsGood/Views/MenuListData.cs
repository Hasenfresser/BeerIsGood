using System.Collections.Generic;

namespace BeerIsGood.Views
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Title = "Home",
                TargetTyp = typeof(Home)
            });

            // eventuell später implementieren...
            //
            // this.Add(new MenuItem()
            // {
            //     Title = "Verlauf",
            //     TargetTyp = typeof(History)
            // });

            this.Add(new MenuItem()
            {
                Title = "Impressum",
                TargetTyp = typeof(Credits)
            });
        }
    }
}
