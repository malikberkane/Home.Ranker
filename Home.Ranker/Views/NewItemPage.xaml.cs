using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Home.Ranker.Models;
using Home.Ranker.Data;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;
using System.IO;
using Home.Ranker.ViewModels;
using Home.Ranker.Services;
using System.Collections;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Home.Ranker.Views
{
    
    public partial class NewItemPage : ContentPage
    {
        public Apartment Apartment { get; set; }

        private  HomeRankerService HomeRankerService;

        public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();

        public ObservableCollection<CriteriaViewModel> Criterias { get; set; } = new ObservableCollection<CriteriaViewModel>();




        public NewItemPage(string name)
        {
            InitializeComponent();


            Apartment = new Apartment
            {
                Name = name
            };

            BindingContext = this;

        }

        protected override void OnBindingContextChanged()
        {
            HomeRankerService = new HomeRankerService();


            var photos = HomeRankerService.GetPhotos(Apartment);

            if (photos != null)
            {

                foreach (var item in photos)
                {
                    Photos.Add(item);
                }
            }

            


            var criterias = HomeRankerService.GetCriteriasAndRates(Apartment);

            if (criterias != null)
            {
                Criterias = new ObservableCollection<CriteriaViewModel>(criterias);
            }
            base.OnBindingContextChanged();
        }


        public NewItemPage(Apartment appartment)
        {
            InitializeComponent();

            Apartment = appartment;
            BindingContext = this;


        }




        async void Save_Clicked(object sender, EventArgs e)
        {


            HomeRankerService.InsertApartment(Apartment, Photos, Criterias);

           

            Apartment.FirstPictureImageSource = Photos.FirstOrDefault()?.Source;

            MessagingCenter.Send(this, "AddItem", Apartment);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await TakePicture();

        }

        private async Task TakePicture()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 100,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;







            var newPhoto = new Photo
            {
                PhotoUrl = file.Path,
                ApartmentId = Apartment.Id,
                //Base64=base64,
                Source = ImageSource.FromStream(() =>
                {
                    var fileStream = file.GetStream();
                    return fileStream;
                })

            };

            Photos.Add(newPhoto);

            try
            {
                TheCarousel.Position = ((IList)TheCarousel.ItemsSource).Count - 1;

            }
            catch (Exception ex)
            {

            }
        }



        async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current = (e.CurrentSelection.FirstOrDefault() as CriteriaViewModel);

            await Navigation.PushModalAsync(new AboutPage() { CurrentCriteria= current});

        }



        protected override void OnAppearing()
        {
            var currentCarousel = collectionView.ItemsSource;
            base.OnAppearing();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            AddCriteriaModalPage = new ItemDetailPage();
            AddCriteriaModalPage.CriteriaValidated += this.NewCriteriaValidatedInModal;
            await Navigation.PushModalAsync(AddCriteriaModalPage);

        }

        private ItemDetailPage AddCriteriaModalPage;

        private async void NewCriteriaValidatedInModal(object sender, CustomEventArgs e)
        {

            try
            {
                HomeRankerService.InsertCriteria(e.Criteria);

                Criterias.Add(new CriteriaViewModel
                {
                    Criteria = e.Criteria
                });

            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                AddCriteriaModalPage.CriteriaValidated -= this.NewCriteriaValidatedInModal;

            }



        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (CriteriaViewModel)layout.BindingContext;

            await Navigation.PushModalAsync(new AboutPage() { CurrentCriteria = item, CurrentApartment= Apartment });
        }
    }
}