using System;
using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public partial class Credits : ContentPage
    {
        ScrollView ScrollView_Credits = new ScrollView();
        StackLayout StackLayout_Credits = new StackLayout();

        Label Label_Titel = new Label();
        Label Label_SubTitel = new Label();
        Label Label_Developer = new Label();
        Label Label_EMail = new Label();
        Label Label_DisclaimerTitel = new Label();
        Label Label_Disclaimer = new Label();

        TapGestureRecognizer Label_Email_Clicked = new TapGestureRecognizer();

        public Credits()
        {
            Title = "BeerIsGood - Impressum";

            StackLayout_Credits.Spacing = 10;
            StackLayout_Credits.Orientation = StackOrientation.Vertical;

            Label_Titel.TextColor = Color.Silver;
            Label_Titel.FontAttributes = FontAttributes.Bold;
            Label_Titel.FontSize = 25;
            Label_Titel.HorizontalOptions = LayoutOptions.Start;
            Label_Titel.VerticalOptions = LayoutOptions.Start;
            Label_Titel.Text = "TU Ilmenau";

            Label_SubTitel.TextColor = Color.White;
            Label_SubTitel.FontSize = 18;
            Label_SubTitel.HorizontalOptions = LayoutOptions.Start;
            Label_SubTitel.VerticalOptions = LayoutOptions.Start;
            Label_SubTitel.Text = "Ein Projekt des Fachs:" + Environment.NewLine + "Content-Verwertungsmodelle und ihre Umsetzung in mobilen Systemen";

            Label_Developer.TextColor = Color.White;
            Label_Developer.FontAttributes = FontAttributes.Italic;
            Label_Developer.FontSize = 14;
            Label_Developer.HorizontalOptions = LayoutOptions.Start;
            Label_Developer.VerticalOptions = LayoutOptions.Start;
            Label_Developer.Text = "Entwickelt von Robert Werner. Betreut durch Dozent Jürgen Nützel.";

            Label_EMail.TextColor = Color.Blue;
            Label_EMail.FontSize = 14;
            Label_EMail.HorizontalOptions = LayoutOptions.Start;
            Label_EMail.VerticalOptions = LayoutOptions.Start;
            Label_EMail.Text = "Robert.Werner@tu-ilmenau.de";
            Label_EMail.GestureRecognizers.Add(Label_Email_Clicked);

            Label_Email_Clicked.Tapped += (s, e) =>
            {
                Uri EMail = new Uri("mailto:Robert.Werner@tu-ilmenau.de");

                Device.OpenUri(EMail);
            };

            Label_DisclaimerTitel.TextColor = Color.Gray;
            Label_DisclaimerTitel.FontSize = 14;
            Label_DisclaimerTitel.FontAttributes = FontAttributes.Bold;
            Label_DisclaimerTitel.HorizontalOptions = LayoutOptions.Start;
            Label_DisclaimerTitel.VerticalOptions = LayoutOptions.Start;
            Label_DisclaimerTitel.Text = "Haftungsausschluss:";

            Label_Disclaimer.TextColor = Color.White;
            Label_Disclaimer.FontSize = 12;
            Label_Disclaimer.HorizontalOptions = LayoutOptions.Start;
            Label_Disclaimer.VerticalOptions = LayoutOptions.Start;
            Label_Disclaimer.Text = "Die TU Ilmenau übernimmt keine Gewähr für die Aktualität, Korrektheit oder Qualität der von der App bereitgestellten Informationen. Grundsätzlich ausgeschlossen sind Haftungsansprüche, welche sich auf Schäden materieller oder ideeller Art beziehen, die durch die Nutzung oder Nichtnutzung bzw. durch die Nutzung fehlerhafter oder unvollständiger Informationen verursacht wurden, sofern die Studierenden der TU Ilmenau nicht nachweislich vorsätzlich oder grob fahrlässig gehandelt haben.";

            StackLayout_Credits.Children.Add(Label_Titel);
            StackLayout_Credits.Children.Add(Label_SubTitel);
            StackLayout_Credits.Children.Add(Label_Developer);
            StackLayout_Credits.Children.Add(Label_EMail);
            StackLayout_Credits.Children.Add(Label_DisclaimerTitel);
            StackLayout_Credits.Children.Add(Label_Disclaimer);

            ScrollView_Credits.Content = StackLayout_Credits;

            Content = ScrollView_Credits;
        }
    }
}
