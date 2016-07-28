using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using System;

namespace BeerIsGood.Connection
{
    partial class ConnectionService
    {   
        public async Task<XDocument> fillNodes(string URL)
        {
            string XMLString = await App.ConnectionService.getConnectionString(URL);

            return XDocument.Parse(XMLString);
        }
        
        public async Task<Classes.Facilities> parseXMLStringFacilities(string SearchType, string Keyword)
        {
            XDocument XDocument = await fillNodes(SearchType + Keyword);

            Classes.Facilities Faclilities = new Classes.Facilities();

            var Results = from Element in XDocument.Descendants("location")
                                      select new Classes.Facility
                                      {
                                          id = Element.Element("id").Value,
                                          name = Element.Element("name").Value,
                                          status = Element.Element("status").Value,
                                          reviewlink = Element.Element("reviewlink").Value,
                                          proxylink = Element.Element("proxylink").Value,
                                          blogmap = Element.Element("blogmap").Value,
                                          street = Element.Element("street").Value,
                                          city = Element.Element("city").Value,
                                          state = Element.Element("state").Value,
                                          zip = Element.Element("zip").Value,
                                          country = Element.Element("country").Value,
                                          phone = Element.Element("phone").Value,
                                          overall = Element.Element("overall").Value,
                                          imagecount = Element.Element("imagecount").Value
                                      };

            foreach (Classes.Facility Facility in Results)
                Faclilities.FacilityList.Add(Facility);
            
            return Faclilities;
        }
        
        public async Task<Classes.Images> parseXMLStringImages(string ID)
        {
            XDocument XDocument = await fillNodes(ConnectionConstants.SearchImage + ID);

            Classes.Images Images = new Classes.Images();

            var Results = from Element in XDocument.Descendants("location")
                          select new Classes.Image
                          {
                              imageid = Element.Element("imageid").Value,
                              directurl = Element.Element("directurl").Value,
                              imageurl = Element.Element("imageurl").Value,
                              width = Element.Element("width").Value,
                              height = Element.Element("height").Value,
                              caption = Element.Element("caption").Value,
                              credit = Element.Element("credit").Value,
                              crediturl = Element.Element("crediturl").Value,
                              imagedate = Element.Element("imagedate").Value,
                              thumburl = Element.Element("thumburl").Value,
                              score = Element.Element("score").Value
                          };

            foreach (Classes.Image Image in Results)
                Images.ImageList.Add(Image);

            return Images;
        }
        
        public async Task<Classes.Map> parseXMLStringMap(string ID)
        {
            XDocument XDocument = await fillNodes(ConnectionConstants.SearchMap + ID);

            Classes.Map Map = new Classes.Map();

            var Result = from Element in XDocument.Descendants("location")
                          select new Classes.Map
                          {
                              name = Element.Element("name").Value,
                              status = Element.Element("status").Value,
                              lat = Element.Element("lat").Value,
                              lng = Element.Element("lng").Value,
                              map = Element.Element("map").Value,
                              altmap = Element.Element("altmap").Value
                          };

            return Result.ElementAt(0);
        }

        public async Task<Classes.Score> parseXMLStringScore(string ID)
        {
            XDocument XDocument = await fillNodes(ConnectionConstants.SearchScore + ID);

            Classes.Score Score = new Classes.Score();

            var Result = from Element in XDocument.Descendants("location")
                         select new Classes.Score
                         {
                             overall = Element.Element("overall").Value,
                             selection = Element.Element("selection").Value,
                             service = Element.Element("service").Value,
                             atmosphere = Element.Element("atmosphere").Value,
                             food = Element.Element("food").Value,
                             reviewcount = Element.Element("reviewcount").Value,
                             fbscore = Element.Element("fbscore").Value,
                             fbcount = Element.Element("fbcount").Value
                         };

            return Result.ElementAt(0);
        }
    }
}
