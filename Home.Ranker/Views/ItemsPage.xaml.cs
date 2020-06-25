using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Home.Ranker.Models;
using Home.Ranker.Views;
using Home.Ranker.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Home.Ranker.Data;

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Apartment)layout.BindingContext;
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(item)));
        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var newApartmentLabel = await DisplayPromptAsync("New visit","Enter name or adress");

            if (!string.IsNullOrEmpty(newApartmentLabel))
            {
                await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(newApartmentLabel)));

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}