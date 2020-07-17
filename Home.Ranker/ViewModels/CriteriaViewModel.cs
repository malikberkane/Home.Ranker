using Home.Ranker.Data;
using System;
using System.Text;

namespace Home.Ranker.ViewModels
{
    public class CriteriaViewModel : BaseViewModel, IComparable<CriteriaViewModel>
    {
        private double? rateValue;

        public Criteria Criteria { get; set; }



        public double? RateValue
        {
            get => rateValue; set
            {
                SetProperty(ref rateValue, value);
            }
        }

        public int CompareTo(CriteriaViewModel other)
        {
            if (this.Criteria.ImportanceLevel < other.Criteria.ImportanceLevel)
            {
                return 1;
            }
            else if (this.Criteria.ImportanceLevel > other.Criteria.ImportanceLevel)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CriteriaViewModel otherCriteria)
            {
                return this.Criteria.Id == otherCriteria.Criteria.Id;
            }
            return false;
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(this.rateValue, this.Criteria, this.RateValue);
        }
    }

}
