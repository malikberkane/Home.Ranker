using Home.Ranker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Home.Ranker.ViewModels
{
    public class CriteriaViewModel : BaseViewModel
    {
        private double? rateValue;

        public Criteria Criteria { get; set; }



        public double? RateValue { get => rateValue; set { 
                SetProperty(ref rateValue, value);
            } }



    }
}
