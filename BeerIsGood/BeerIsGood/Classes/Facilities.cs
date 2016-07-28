using System.Collections.Generic;
using System;

namespace BeerIsGood.Classes
{
    public class Facility
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string reviewlink { get; set; }
        public string proxylink { get; set; }
        public string blogmap { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string overall { get; set; }
        public string imagecount { get; set; }

        public double Distance { get; set; }
    }

    public class Facilities
    {
        public List<Facility> FacilityList { get; set; }
        
        public Facilities()
        {
            FacilityList = new List<Facility>();
        } 
    }

    public class FacilityScoreComparer : IComparer<Facility>
    {
        public int Compare(Facility X, Facility Y)
        {
            Facility FacilityA = X;
            Facility FacilityB = Y;

            return string.Compare(FacilityB.overall, FacilityA.overall);
        }
    }

    public class FacilityDistanceComparer : IComparer<Facility>
    {
        public int Compare(Facility X, Facility Y)
        {
            Facility FacilityA = X;
            Facility FacilityB = Y;

            return string.Compare(FacilityA.Distance.ToString(), FacilityB.Distance.ToString());
        }
    }
}
