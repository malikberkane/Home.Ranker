using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Home.Ranker.Models;
using Home.Ranker.ViewModels;
using Home.Ranker.Data;
using System.Linq;

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        public Criteria Criteria { get; set; }

        public event EventHandler<CustomEventArgs> CriteriaValidated;


        public ItemDetailPage(Criteria criteria)
        {
            InitializeComponent();
            Criteria = criteria;

            BindingContext = this;
            CriteriaImportanceSlider.ValueChanged += (sender, args) =>
            {
                if (Enumerable.Range(1, 2).Contains((int)args.NewValue))
                {
                    ImportanceDescriptionLabel.Text = "très peu important";

                }
                else if (Enumerable.Range(2, 5).Contains((int)args.NewValue))
                {
                    ImportanceDescriptionLabel.Text = "Peu important";

                }

                else if (Enumerable.Range(5, 7).Contains((int)args.NewValue))
                {
                    ImportanceDescriptionLabel.Text = "Important";

                }
                else if (Enumerable.Range(7, 10).Contains((int)args.NewValue))
                {
                    ImportanceDescriptionLabel.Text = "Très important";

                }
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CriteriaNameEntry.Text) && CriteriaImportanceSlider.Value != 0)
            {
                CriteriaValidated?.Invoke(this, new CustomEventArgs(Criteria));

                Shell.Current.Navigation.PopAsync();

            }

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