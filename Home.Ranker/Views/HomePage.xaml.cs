﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Home.Ranker.Data;
using Home.Ranker.Services;
using System.Collections.ObjectModel;
using System.Reflection;
using Plugin.SharedTransitions;
using System.Collections.Immutable;
namespace Home.Ranker.Views
{

    public partial class HomePage : ContentPage
    {
        private HomeRankerService HomeRankerService;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;




        }

        public ObservableCollection<Apartment> Apartments { get; set; }

        protected override void OnBindingContextChanged()
        {
            HomeRankerService = new HomeRankerService();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            LoadItemsCommand.Execute(null);
            MessagingCenter.Subscribe<SetApartmentPage, Apartment>(this, "AddItem", async (obj, item) =>
            {

                if (Apartments == null)
                {
                    Apartments = new ObservableCollection<Apartment>();
                }

                var existingIndex = Apartments.IndexOf(item);
                if (existingIndex != -1)
                {
                    Apartments[existingIndex].UpdateInstance(item, resetFirstImageUrl: Apartments[existingIndex].FirstPictureUrl != item.FirstPictureUrl);

                }
                else
                {
                    Apartments.Add(item);
                }

                Apartments.Sort();

                await Shell.Current.Navigation.PopAsync();


            });
            base.OnBindingContextChanged();
        }



        async void OnItemSelected(object sender, EventArgs args)
        {

            var layout = (BindableObject)sender;



            var item = (Apartment)layout.BindingContext;
            SharedTransitionShell.SetTransitionSelectedGroup(this, item.Name);

            var other = new Apartment
            {
                Adresse = item.Adresse,
                Id = item.Id,
                Name = item.Name
            };
            var newPage = new SetApartmentPage(other);
            try
            {



                newPage.LoadData();
                newPage.BindingContext = newPage;
                await Shell.Current.Navigation.PushAsync(newPage);


            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.Message, AppResources.Ok);
            }

           

        }

        async void Delete_Apartment_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (Apartment)layout.BindingContext;

            try
            {
                HomeRankerService.DeleteApartment(item);
                Apartments.Remove(item);
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message, AppResources.Ok);
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {



            var newApartmentLabel = await DisplayPromptAsync(AppResources.NewEvaluationTitle, AppResources.EnterNamePrompt, AppResources.Ok, AppResources.Cancel);

            if (!string.IsNullOrEmpty(newApartmentLabel))
            {
                var newPage = new SetApartmentPage(newApartmentLabel);

                try
                {
                   


                    newPage.LoadData();
                    newPage.BindingContext = newPage;
                    await Shell.Current.Navigation.PushAsync(newPage);


                }
                catch (Exception ex)
                {
                    await DisplayAlert(string.Empty,ex.Message, AppResources.Ok);
                }

               




            }
        }






        void ExecuteLoadItemsCommand()
        {

            try
            {


                var items = HomeRankerService.GetAllApartments();

                if (items != null)
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

        }

        public Command LoadItemsCommand { get; set; }

    }
}