

using System.ComponentModel;

namespace Home.Ranker.Data
{
    public class Rate
    {
        public int CriteriaId { get; set; }
       
        public int ApartmentId { get; set; }

        public double RateValue { get; set; }

        public string Note { get; set; }


    }
}