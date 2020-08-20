using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Home.Ranker.Models;
using Home.Ranker.ViewModels;
using Home.Ranker.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SetCriteriaPage : ContentPage
    {
        public Criteria Criteria { get; set; }

        public event EventHandler<CustomEventArgs> CriteriaValidated;


        public SetCriteriaPage(Criteria criteria)   
        {
            InitializeComponent();
            Criteria = criteria;

            BindingContext = this;
            CriteriaImportanceSlider.ValueChanged += (sender, args) =>
            {
                CriteriaImportanceSlider.Value = Math.Round(args.NewValue);

            };
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CriteriaNameEntry.Text) && CriteriaImportanceSlider.Value != 0)
            {
                CriteriaValidated?.Invoke(this, new CustomEventArgs(Criteria));

                Shell.Current.Navigation.PopAsync();

            }

        }

        public async void NavBackImplementation()
        {
            CriteriaValidated?.Invoke(this, new CustomEventArgs(Criteria));

            await Shell.Current.Navigation.PopAsync();

        }



    }

    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(Criteria message)
        {
            Criteria = message;
        }

        public Criteria Criteria { get; set; }
    }
}