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
using Home.Ranker.Services;
using System.Collections.ObjectModel;
using System.Collections;
using System.Reflection;
using Plugin.SharedTransitions;

namespace Home.Ranker.Views
{
    
    public partial class ItemsPage : ContentPage
    {
        private  HomeRankerService HomeRankerService;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = this;

            homeImage.Source = ImageSource.FromResource(
             "Home.Ranker.Images.HomeImage.png",
                typeof(ItemsPage).GetTypeInfo().Assembly);


        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            homeImage.Source = ImageSource.FromResource(
           "Home.Ranker.Images.HomeImage.png",
              typeof(ItemsPage).GetTypeInfo().Assembly);
        }
        public ObservableCollection<Apartment> Apartments { get; set; }

        protected override void OnBindingContextChanged()
        {
            HomeRankerService = new HomeRankerService();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            LoadItemsCommand.Execute(null);
            MessagingCenter.Subscribe<NewItemPage, Apartment>(this, "AddItem", async (obj, item) =>
            {

                if (Apartments == null)
                {
                    Apartments = new ObservableCollection<Apartment>();
                }

                if (!Apartments.Contains(item))
                {

                    Apartments.Add(item);

                }

                await Navigation.PopAsync();


            });
            base.OnBindingContextChanged();
        }



        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Apartment)layout.BindingContext;
            SharedTransitionNavigationPage.SetTransitionSelectedGroup(this, item.Name);

            await Navigation.PushAsync(new NewItemPage(item));

        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var newApartmentLabel = await DisplayPromptAsync("New visit","Enter name or adress");

            if (!string.IsNullOrEmpty(newApartmentLabel))
            {
                await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(newApartmentLabel)));

            }
        }

       


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {


                var items = HomeRankerService.GetAllApartments();

                if(items!=null)
                {
                    if (Apartments == null)
                    {
                        Apartments = new ObservableCollection<Apartment>();

                    }
                    else
                    {
                        Apartments.Clear();
                    }
                }

                foreach (var item in items)
                {
                    Apartments.Add(item);
                }


               
            }
            catch (Exception ex)
            {
                //Do nothing
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LoadItemsCommand { get; set; }

        private void refreshView_Refreshing(object sender, EventArgs e)
        {
            refreshView.IsRefreshing = true;
            LoadItemsCommand.Execute(null);
            refreshView.IsRefreshing = false;

        }
    }
}