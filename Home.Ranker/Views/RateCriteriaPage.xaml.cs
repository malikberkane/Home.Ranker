using Home.Ranker.Data;
using Home.Ranker.Services;
using Home.Ranker.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RateCriteriaPage : ContentPage
    {
        private bool hasRate;

        public CriteriaViewModel CurrentCriteria { get; set; }

        public Apartment CurrentApartment { get; set; }



        public bool HasRate
        {
            get => hasRate; set
            {

                hasRate = value;
                if (value == false)
                {
                    if (CurrentCriteria.RateValue.HasValue)
                    {
                        CurrentCriteria.RateValue = 0;
                        CurrentCriteria.RateValue = null;
                    }

                   


                }
                else
                {
                    RatingControl.Opacity=1;
                    if (!CurrentCriteria.RateValue.HasValue)
                    {
                        CurrentCriteria.RateValue = 0;
                    }


                }

                OnPropertyChanged(nameof(HasRate));
            }
        }

        public RateCriteriaPage(CriteriaViewModel criteria, Apartment appartment)
        {
            InitializeComponent();
            CurrentApartment = appartment;
            CurrentCriteria = criteria;
            HasRate = CurrentCriteria.RateValue.HasValue;
            BindingContext = this;
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync();


        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if(!hasRate && e.NewValue > 0)
            {
                HasRate = true;
            }
            
          
            Slider.Value = Math.Round(e.NewValue);
            RateDescriptionLabel.Text = String.Format("{0}/10", Slider.Value);
        }

    }


}