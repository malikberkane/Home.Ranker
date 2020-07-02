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
    public partial class AboutPage : ContentPage
    {

        //public event EventHandler<RateValidatedEventArgs> RateValidated;

        public CriteriaViewModel CurrentCriteria { get; set; }

        public Apartment CurrentApartment { get; set; }

        private HomeRankerService HomeRankerService;


        public AboutPage()
        {
            InitializeComponent();
            HomeRankerService = new HomeRankerService();

            BindingContext = this;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Cri.Text) && CriteriaRateSlider.Value != 0)
            //{
            //    RateValidated?.Invoke(this, new RateValidatedEventArgs(new Rate
            //    {
            //        Name = CriteriaNameEntry.Text
            //    ,
            //        ImportanceLevel = (int)CriteriaImportanceSlider.Value
            //    }));




                Navigation.PopModalAsync();


            }

            //}

            //private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
            //{
            //    double value = e.NewValue;
            //    displayLabel.Text = String.Format("The Slider value is {0}", value);


            //}
        }

    //public class RateValidatedEventArgs : EventArgs
    //{
    //    public RateValidatedEventArgs(Rate rate)
    //    {
    //        Rate = rate;
    //    }

    //    public Rate Rate { get; set; }
    //}
}