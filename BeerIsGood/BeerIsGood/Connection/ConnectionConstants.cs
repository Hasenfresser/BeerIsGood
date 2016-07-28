namespace BeerIsGood.Connection
{
    class ConnectionConstants
    {
        static string APIKey = "24e921af2e99533b1d3961e912cde8c3"; 
        static string URL = "http://beermapping.com/webservice/loc";

        static string City = "city";    // recieves name of city as string, returns facility
        static string State = "state";  // receives name of state as string, returns facility
        static string Query = "query";  // recieves unique ID as int or name as string, returns facility
        static string Map = "map";      // receives unique ID, returns map
        static string Score = "score";  // receives unique ID, returns score
        static string Image = "image";  // receives unique ID, returns image

        public static string SearchQuery = URL + Query + '/' + APIKey + '/';
        public static string SearchCity = URL + City + '/' + APIKey + '/';
        public static string SearchState = URL + State + '/' + APIKey + '/';
        public static string SearchMap = URL + Map + '/' + APIKey + '/';
        public static string SearchScore = URL + Score + '/' + APIKey + '/';
        public static string SearchImage = URL + Image + '/' + APIKey + '/';
    }
}
