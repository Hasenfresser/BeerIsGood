using System.Collections.Generic;

namespace BeerIsGood.Classes
{
    public class Image
    {
        public string imageid { get; set; }
        public string directurl { get; set; }
        public string imageurl { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string caption { get; set; }
        public string credit { get; set; }
        public string crediturl { get; set; }
        public string imagedate { get; set; }
        public string thumburl { get; set; }
        public string score { get; set; }
    }

    public class Images
    {
        public List<Image> ImageList { get; set; }

        public Images()
        {
            ImageList = new List<Image>();
        }
    }
}
