using System;
using System.Collections.Generic;
using Plugin.Geolocator;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BeerIsGood.Views
{
    public partial class Home : ContentPage
    {
        // Frontend-Elemente werden angelegt
        ScrollView ScrollView_Direct_Search = new ScrollView();
        StackLayout StackLayout_Direct_Search = new StackLayout();

        Label Label_Status = new Label();
        Entry Entry_Keyword = new Entry();
        Picker Picker_SearchType = new Picker();
        Button Button_Search = new Button();
        Label Label_SearchResults = new Label();

        ListView ListView_SearchResults = new ListView();

        Classes.Facilities SearchedData = new Classes.Facilities();

        // Konstruktor der Home-Page, wird bei jedem Öffnen ausgeführt
        public Home()
        {
            // Page-Titel setzen
            Title = "BeerIsGood - Home";

            // StackLayout wird konfiguriert
            StackLayout_Direct_Search.Spacing = 10;
            StackLayout_Direct_Search.Orientation = StackOrientation.Vertical;

            // Status-Label wird konfiguriert
            Label_Status.TextColor = Color.White;
            Label_Status.FontSize = 15;
            Label_Status.Text = "Stadt eingeben:";

            // Suchbegriff-Entry wird konfiguriert
            Entry_Keyword.Text = string.Empty;
            Entry_Keyword.FontSize = 20;

            // Suchtyp-Picker wird konfiguriert
            Picker_SearchType.Items.Add("Entfernung (benötigt GPS)");
            Picker_SearchType.Items.Add("Bewertung");
            Picker_SearchType.SelectedIndex = 0;

            // Such-Button wird konfiguriert
            Button_Search.FontSize = 20;
            Button_Search.BackgroundColor = Color.Gray;
            Button_Search.TextColor = Color.White;
            Button_Search.Text = "Suchen";
            Button_Search.Clicked += new EventHandler(this.Button_Search_Clicked);

            // Ergebnis-Label wird konfiguriert
            Label_SearchResults.TextColor = Color.White;
            Label_SearchResults.FontSize = 15;

            // Ergebnis-ListView  wird konfiguriert
            ListView_SearchResults.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(ListView_SearchResults_Clicked);
            ListView_SearchResults.HasUnevenRows = true;
            ListView_SearchResults.VerticalOptions = LayoutOptions.StartAndExpand;
            ListView_SearchResults.HorizontalOptions = LayoutOptions.Fill;

            // alle Elemente werden dem StackLayout hinzugefügt
            StackLayout_Direct_Search.Children.Add(Label_Status);
            StackLayout_Direct_Search.Children.Add(Entry_Keyword);
            StackLayout_Direct_Search.Children.Add(Picker_SearchType);
            StackLayout_Direct_Search.Children.Add(Button_Search);
            StackLayout_Direct_Search.Children.Add(Label_SearchResults);
            StackLayout_Direct_Search.Children.Add(ListView_SearchResults);

            // Stacklayout wird dem ScrollView hinzugefügt
            ScrollView_Direct_Search.Content = StackLayout_Direct_Search;

            // ScrollView wird als Page-Content gesetzt
            Content = ScrollView_Direct_Search;
        }

        // Event, welches beim Drücken des Suchen-Buttons ausgeführt wird
        private async void Button_Search_Clicked(object Sender1, EventArgs EventArgs1)
        {
            // Ergebnis-Label wird geleert
            Label_SearchResults.Text = string.Empty;

            // Suchbegriff wird von unnötigen Leerzeichen gesäubert
            string Keyword = Entry_Keyword.Text.Trim();

            // Überprüfen, ob Suchbegriff nicht leer ist
            if (Keyword == string.Empty) // falls Suchbegriff leer, keine Suche
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Kein Suchbegriff eingegeben!";
            }
            else // falls Suchbegriff nicht leer, Suche starten
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Yellow;
                Label_Status.Text = "Es wird gesucht...";

                // alle Elemente der Suche vorrübergehend deaktivieren
                Entry_Keyword.IsEnabled = false;
                Button_Search.IsEnabled = false;
                Picker_SearchType.IsEnabled = false;

                // Ergebnis-ListView leeren
                ListView_SearchResults.ItemsSource = null;

                // Überprüfen, welcher Suchtyp im Picker ausgewählt wurde
                if (Picker_SearchType.SelectedIndex == 0) // Suche über Entfernung ausführen
                    SearchedData = await sortedDistanceList(Entry_Keyword.Text);
                else // Suche über Bewertung ausführen
                    SearchedData = await sortedScoreList(Entry_Keyword.Text);

                // Überprüfen, ob Ergebnisliste leer
                if (SearchedData.FacilityList.Count > 0) // falls nicht leer
                {
                    // Status-Label aktualisieren
                    Label_Status.TextColor = Color.Green;
                    Label_Status.Text = "Ergebnisse gefunden.";

                    // Ergebnis-Label aktualisieren
                    Label_SearchResults.Text = "Ergebnisse:";

                    // String-Liste für Ergebnisse anlegen
                    List<string> SearchedItems = new List<string>();

                    // Hinzufügen aller Ergebnis-Namen zur String-Liste
                    for (short Counter = 0; Counter < SearchedData.FacilityList.Count; ++Counter)
                        SearchedItems.Add(SearchedData.FacilityList[Counter].name);

                    // Ergebnis-ListView mit String-Liste befüllen
                    ListView_SearchResults.ItemsSource = SearchedItems;
                }
                else // falls leer
                {
                    // Ergebnis-Label aktualisieren
                    Label_SearchResults.Text = string.Empty;

                    // Ergebnis-ListView leeren
                    ListView_SearchResults.ItemsSource = null;
                }

                // alle Elemente der Suche reaktivieren
                Entry_Keyword.IsEnabled = true;
                Button_Search.IsEnabled = true;
                Picker_SearchType.IsEnabled = true;
            }
        }

        // Methode, welche bei der Suche über die Bewertung ausgeführt wird
        private async Task<Classes.Facilities> sortedScoreList(string Keyword)
        {
            // Ergebnis-Facilities wird angelegt
            Classes.Facilities Facilities = new Classes.Facilities();

            // Versuche Verbindung aufzubauen
            try
            {
                // Suche an BeerMapping-API mit Suchbegriff wird gestartet, Ergebnis wird in Ergebnis-Facilities gespeichert
                Facilities = await App.ConnectionService.parseXMLStringFacilities(Connection.ConnectionConstants.SearchCity, Keyword);
            }
            // falls Verbindung nicht aufgebaut werden kann
            catch (Exception)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Internetverbindung.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // Überprüfen, ob Ergebnis-Faclities leer ist
            if (Facilities.FacilityList[0].id == string.Empty)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // Ergebnisliste nach Bewertung sortieren
            Facilities.FacilityList.Sort(new Classes.FacilityScoreComparer());

            // sortierte Ergebnis-Facilities anlegen
            Classes.Facilities SortedFacilities = new Classes.Facilities();

            // nur Ergebnisse mit verfügbarer Bewertung zur sortierten Ergebnis-Facilities hinzufügen (maximal fünf)
            for (int Counter = 0; Counter < 5 && Counter < Facilities.FacilityList.Count; ++Counter)
                if (Facilities.FacilityList[Counter].overall != "0")
                    SortedFacilities.FacilityList.Add(Facilities.FacilityList[Counter]);

            // Überprüfen, ob sortierte Ergebnis-Facilities leer ist
            if (SortedFacilities.FacilityList.Count == 0)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // sortierte Ergebnis-Facilities zurückgeben
            return SortedFacilities;
        }

        // Methode, welche bei der Suche über die Entfernung ausgeführt wird
        private async Task<Classes.Facilities> sortedDistanceList(string Keyword)
        {
            // Ergebnis-Facilities wird angelegt
            Classes.Facilities Facilities = new Classes.Facilities();

            // Versuche Verbindung aufzubauen
            try
            {
                // Suche an BeerMapping-API mit Suchbegriff wird gestartet, Ergebnis wird in Ergebnis-Facilities gespeichert
                Facilities = await App.ConnectionService.parseXMLStringFacilities(Connection.ConnectionConstants.SearchCity, Keyword);
            }
            // falls Verbindung nicht aufgebaut werden kann
            catch (Exception)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Internetverbindung.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // Überprüfen, ob Ergebnis-Faclities leer ist
            if (Facilities.FacilityList[0].id == string.Empty)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // Versuche aktuellen Standort zu ermitteln
            try
            {
                // Aufrufen externer Methode, Speichern in Position (statisch) 
                App.Position = await getPosition();
            }
            // falls Standort nicht ermittelt werden kann
            catch (Exception)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Kein GPS-Signal!";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // Überprüfen, Standord nicht null ist
            if (App.Position == null)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Kein GPS-Signal!";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // sortierte Ergebnis-Facilities anlegen
            Classes.Facilities SortedFacilities = new Classes.Facilities();

            // alle Elemente der Ergebnis-Facilities durchlaufen
            for (int Counter = 0; Counter < Facilities.FacilityList.Count; ++Counter)
            {
                // externe Methode zur Berechnung der Entfernung aufrufen und in jedem Element abspeichern
                Facilities.FacilityList[Counter].Distance = await getDistance(Facilities.FacilityList[Counter].id);

                // wenn Entfernung ungleich -1 (Fehlerfall), Hinzufügen in sortierte Ergebnis-Facilities
                if (Facilities.FacilityList[Counter].Distance != -1)
                    SortedFacilities.FacilityList.Add(Facilities.FacilityList[Counter]);
            }

            // sortierte Ergebnisliste nach Entfernung sortieren
            SortedFacilities.FacilityList.Sort(new Classes.FacilityDistanceComparer());

            // nach dem fünften Element der Liste alle weiteren aus der Liste entfernen
            if (SortedFacilities.FacilityList.Count > 5)
                SortedFacilities.FacilityList.RemoveRange(5, SortedFacilities.FacilityList.Count - 5);

            // Überprüfen, ob sortierte Ergebnis-Facilities leer ist
            if (SortedFacilities.FacilityList.Count == 0)
            {
                // Status-Label aktualisieren
                Label_Status.TextColor = Color.Red;
                Label_Status.Text = "Keine Ergebnisse gefunden.";

                // leere Ergebnisliste zurückgeben
                return new Classes.Facilities();
            }

            // sortierte Ergebnis-Facilities zurückgeben
            return SortedFacilities;
        }

        // Methode, welche zum Ermitteln des aktuellen Standort ausgeführt wird
        private async Task<Plugin.Geolocator.Abstractions.Position> getPosition()
        {
            // Geolocator wird angelegt und ausgerichtet
            Plugin.Geolocator.Abstractions.IGeolocator Geolocator = CrossGeolocator.Current;

            // Suchgenauigkeit wird festgelegt
            Geolocator.DesiredAccuracy = 10;

            // Versuche aktuellen Standort zu ermitteln und zurückzugeben
            try
            {
                return await Geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            // falls nicht möglich, null zurückgeben
            catch (Exception)
            {
                return null;
            }
        }

        // Methode, welche zum Ermitteln der Entfernung ausgeführt wird, ID der Location muss übergeben werden
        private async Task<double> getDistance(string ID)
        {
            // Map wird angelegt
            Classes.Map Map = new Classes.Map();

            // Versuche Verbindung aufzubauen
            try
            {
                // Suche an BeerMapping-API (Map) mit ID wird gestartet, Ergebnis wird in Map gespeichert
                Map = await App.ConnectionService.parseXMLStringMap(ID);
            }
            // falls Verbindung nicht aufgebaut werden kann, -1 zurückgeben
            catch (Exception)
            {
                return -1;
            }

            // Zwischenspeicher für Latitude und Longitude werden angelegt
            double MapLatitude;
            double MapLongitude;

            // versuche Latitude von String in Double zu konvertieren
            try
            {
                // im String muss der '.' mit einem ',' ersetzt werden
                MapLatitude = Convert.ToDouble(Map.lat.Replace('.', ','));
            }
            // falls ein Fehler auftritt, -1 zurückgeben
            catch (Exception)
            {
                return -1;
            }

            // Überprüfen, ob Latitude nicht 0 ist, ansonsten -1 zurückgeben
            if (MapLatitude == 0)
                return -1;

            // Latitude in Radialwert umrechnen
            MapLatitude = MapLatitude.toRadians();

            // versuche Longitude von String in Double zu konvertieren
            try
            {
                // im String muss der '.' mit einem ',' ersetzt werden
                MapLongitude = Convert.ToDouble(Map.lng.Replace('.', ','));
            }
            // falls ein Fehler auftritt, -1 zurückgeben
            catch (Exception)
            {
                return -1;
            }

            // Überprüfen, ob Longitude nicht 0 ist, ansonsten -1 zurückgeben
            if (MapLongitude == 0)
                return -1;

            // Longitude in Radialwert umrechnen
            MapLongitude = MapLongitude.toRadians();

            // Zwischenspeicher für Differenzen zwischen Latitude und Longitude berechnen (Positionsdaten müssen jeweils noch in einen Radialwert umgerechnet werden)
            double DifferenceLatitude = MapLatitude - App.Position.Latitude.toRadians();
            double DifferenceLongitude = MapLongitude - App.Position.Longitude.toRadians();

            // Zwischenergebnis der Entfernungsberechnung
            double Cache = Math.Pow(Math.Sin(DifferenceLatitude / 2.0), 2) + Math.Pow(Math.Sin(DifferenceLongitude / 2.0), 2)
                         * Math.Cos(MapLatitude) * Math.Cos(App.Position.Latitude.toRadians());

            // Restliche berechnung und Rückgabe der Entfernung
            return Math.Round(12742 * Math.Asin(Math.Min(1, Math.Sqrt(Cache))), 2);
        }

        // Event, welches beim Auswählen eines Elements der Ergebnis-ListView ausgelöst wird
        private async void ListView_SearchResults_Clicked(object Sender, SelectedItemChangedEventArgs EventArgs)
        {
            // Überprüfen, dass ein Element angewählt ist, ansonsten sofort return
            if (EventArgs.SelectedItem == null)
                return;

            // Durchlaufen aller Elemente der Suchergebnisse
            for (short Counter = 0; Counter < 5 && Counter < SearchedData.FacilityList.Count; ++Counter)
            {
                // Überprüfen, ob aktuelles Element gleich dem angewählten Element ist (Überprüfung über Name)
                if (SearchedData.FacilityList[Counter].name == EventArgs.SelectedItem.ToString())
                {
                    // Map wird angelegt
                    Classes.Map Map = new Classes.Map();

                    // Boolean für Möglichkeit der Navigation wird angelegt
                    bool MapDataAvailable = true;

                    try
                    {
                        Map = await App.ConnectionService.parseXMLStringMap(SearchedData.FacilityList[Counter].id);
                    }
                    catch (Exception)
                    {
                        MapDataAvailable = false;
                    }

                    if (Map.lat != string.Empty && Map.lng != string.Empty)
                    {
                        double Latitide;
                        double Longitude;

                        try
                        {
                            Latitide = Convert.ToDouble(Map.lat);
                        }
                        catch (Exception)
                        {
                            MapDataAvailable = false;
                            Latitide = 0;
                        }


                        try
                        {
                            Longitude = Convert.ToDouble(Map.lng);
                        }
                        catch (Exception)
                        {
                            MapDataAvailable = false;
                            Longitude = 0;
                        }

                        if (Latitide == 0 || Longitude == 0)
                            MapDataAvailable = false;
                    }

                    string Text = string.Empty;

                    if (SearchedData.FacilityList[Counter].name != string.Empty)
                        Text += "Name: " + SearchedData.FacilityList[Counter].name + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].status != string.Empty)
                        Text += "Status: " + SearchedData.FacilityList[Counter].status + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].street != string.Empty)
                        Text += "Straße: " + SearchedData.FacilityList[Counter].street + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].city != string.Empty)
                        Text += "Ort: " + SearchedData.FacilityList[Counter].city + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].state != string.Empty)
                        Text += "Staat: " + SearchedData.FacilityList[Counter].state + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].zip != string.Empty)
                        Text += "Postleitzahl: " + SearchedData.FacilityList[Counter].zip + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].country != string.Empty)
                        Text += "Land: " + SearchedData.FacilityList[Counter].country + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].phone != string.Empty)
                        Text += "Telefon: " + SearchedData.FacilityList[Counter].phone + Environment.NewLine;

                    if (SearchedData.FacilityList[Counter].overall != "0")
                        Text += "Gesamtbewertung: " + SearchedData.FacilityList[Counter].overall + Environment.NewLine;

                    try
                    {
                        Classes.Score Score = await App.ConnectionService.parseXMLStringScore(SearchedData.FacilityList[Counter].id);

                        if (Score.selection != "0")
                            Text += "Auswahl: " + Score.selection + Environment.NewLine;

                        if (Score.service != "0")
                            Text += "Service: " + Score.service + Environment.NewLine;

                        if (Score.atmosphere != "0")
                            Text += "Atmosphäre: " + Score.atmosphere + Environment.NewLine;

                        if (Score.food != "0")
                            Text += "Essen: " + Score.food + Environment.NewLine;
                    }
                    catch (Exception)
                    {

                    }

                    if (SearchedData.FacilityList[Counter].Distance != 0)
                        Text += "Luftlinie (km): " + SearchedData.FacilityList[Counter].Distance + Environment.NewLine;

                    if (MapDataAvailable)
                    {
                        bool Navigation = await DisplayAlert("Ergebnis-ID: " + SearchedData.FacilityList[Counter].id, Text, "Navigation", "Zurück");

                        if (Navigation)
                        {
                            Uri GoogleMaps = new Uri("http://maps.google.com/?q=" + Map.lat + ',' + Map.lng);

                            Device.OpenUri(GoogleMaps);
                        }
                    }
                    else
                        await DisplayAlert("Ergebnis-ID: " + SearchedData.FacilityList[Counter].id, Text, "Zurück");

                    break;
                }
            }
        }
    }
}
