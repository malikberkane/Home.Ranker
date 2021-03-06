﻿using System;
using Xamarin.Forms;
using Home.Ranker.Data;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;
using Home.Ranker.ViewModels;
using Home.Ranker.Services;
using System.Collections;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Linq;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel;
using Plugin.SharedTransitions;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Home.Ranker.Views
{

    public partial class SetApartmentPage : ContentPage
    {
        public Apartment Apartment { get; set; }

        private HomeRankerService HomeRankerService;

        public ObservableCollection<Photo> Photos { get; set; }

        public ICollection<Photo> PhotosToDelete { get; set; }

        public ObservableCriterias Criterias { get; set; }

        private string _initialApartmentStateHash;

        public SetApartmentPage(string name)
        {

            InitializeComponent();

            HeaderView.SizeChanged += HeaderView_SizeChanged;

            collectionView.Scrolled += this.CollectionView_Scrolled;
            Apartment = new Apartment
            {
                Name = name
            };


        }

        private void HeaderView_SizeChanged(object sender, EventArgs e)
        {

            HeaderContentView.HeightRequest = HeaderView.Height;
        }

        private async void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (!_defaultVisibleItemsMesured)
            {
                _defaultVisibleItemsNumber = e.LastVisibleItemIndex - e.FirstVisibleItemIndex;
                _defaultVisibleItemsMesured = _defaultVisibleItemsNumber <= Criterias.Count;

            }

            if (_defaultVisibleItemsMesured)
            {
                
                await CustomAnimation(e.LastVisibleItemIndex, e.FirstVisibleItemIndex, HeaderView);

            }
        }

        private int _defaultVisibleItemsNumber;
        private bool _defaultVisibleItemsMesured;
        private bool _performAnimation;
        public  async Task CustomAnimation(int lastIndexVisible,int firstIndexVisible, StackLayout name)
        {

            double ecart = lastIndexVisible - firstIndexVisible;
           
            var fadeTarget =  1 / Math.Pow((ecart/ _defaultVisibleItemsNumber), 5);
            await name.FadeTo(fadeTarget, 30, Easing.SinOut);
            if (fadeTarget < 0.6)
            {
                HeaderView.InputTransparent = true;
            }
            else
            {
                HeaderView.InputTransparent = false;

            }
            
        }

        public SetApartmentPage(Apartment appartment)
        {

            InitializeComponent();
            HeaderView.SizeChanged += HeaderView_SizeChanged;

            collectionView.Scrolled += this.CollectionView_Scrolled;

            Apartment = appartment;
            AdressEditor.Text = Apartment.Adresse;




        }


        public void LoadData()
        {
            HomeRankerService = new HomeRankerService();


            var photos = HomeRankerService.GetPhotos(Apartment);

            if (photos != null && photos.Any())
            {

                Photos = new ObservableCollection<Photo>(photos);
            }

            PhotosToDelete = new List<Photo>();





            var criterias = HomeRankerService.GetCriteriasAndRates(Apartment);

            if (criterias != null)
            {
                Criterias = new ObservableCriterias(criterias);
            }
            else
            {
                Criterias = new ObservableCriterias();

            }

            _initialApartmentStateHash = GetApartmentStateHash();

          

          
        }

        private string GetApartmentStateHash()
        {
            StringBuilder sb = new StringBuilder();
            if (Photos!=null)
            {
                foreach (var item in Photos)
                {
                    sb.Append(item.PhotoUrl);
                    
                } 
            }

            if (Criterias!=null)
            {
                foreach (var item in Criterias)
                {
                    sb.Append($"{item.Criteria.Id}{item.Note}{item.RateValue}");
                } 
            }

            sb.Append($"{Apartment.Adresse}{Apartment.Name}");

            return sb.ToString();
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            Save();

        }

        private void Save()
        {
            var photosTaken = Photos != null && Photos.Any();
            if (photosTaken)
            {
                Apartment.FirstPictureUrl = Photos.First().PhotoUrl;
                Apartment.FirstPictureImageSource = Photos.First().Source;

            }
            HomeRankerService.InsertApartment(Apartment, Criterias);

            if (photosTaken && Apartment.Id != 0)
            {
                HomeRankerService.PersistPhotos(Apartment.Id, Photos.Where(n => n.PhotoId == 0), PhotosToDelete);
            }





            MessagingCenter.Send(this, "AddItem", Apartment);
        }

        async void BackButton_Clicked(object sender, EventArgs e)
        {
            await NavBackImplementation();

        }

        public async Task NavBackImplementation()
        {
            if (_initialApartmentStateHash == GetApartmentStateHash())
            {
                await Shell.Current.Navigation.PopAsync();

            }
            else
            {
                var actionSheetResult = await Shell.Current.DisplayActionSheet(string.Empty, AppResources.Cancel, string.Empty, new string[] { AppResources.Save, AppResources.Discard });

                if (actionSheetResult == AppResources.Save)
                {
                    Save();
                    return;
                }

                await Shell.Current.Navigation.PopAsync();

               
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await TakePicture();

        }

        private async void OnGetAdressFromLocationClicked(object sender, EventArgs e)
        {

            if (await DisplayAlert(string.Empty, AppResources.GetCurrentAdressPrompt, AppResources.AcceptGetCurrentAdress, AppResources.Cancel))    
            {

                LoadingAdressIndicator.IsVisible = true;
                LoadAdressButton.IsVisible = false;
                var location = await Geolocation.GetLocationAsync();



                var info = await Geocoding.GetPlacemarksAsync(location);



                var adress = info.FirstOrDefault();

                Apartment.Adresse = AdressEditor.Text = $"{adress.SubThoroughfare} {adress.Thoroughfare}{Environment.NewLine}{adress.PostalCode} {adress.Locality}";


                LoadingAdressIndicator.IsVisible = false;
                LoadAdressButton.IsVisible = true;

            }


        }

        private async Task TakePicture()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Shell.Current.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                
                CompressionQuality = 100,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,

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


            if (Photos==null)
            {
                Photos = new ObservableCollection<Photo>() { newPhoto};
                OnPropertyChanged(nameof(Photos));

            }
            else
            {
                Photos.Add(newPhoto);

            }





        }





      


        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            AddCriteriaModalPage = new SetCriteriaPage(new Criteria());
            AddCriteriaModalPage.Criteria = new Criteria();
            AddCriteriaModalPage.CriteriaValidated += this.NewCriteriaValidatedInModal;
            await Shell.Current.Navigation.PushAsync(AddCriteriaModalPage);

        }

        private async void DeleteCriteria_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (CriteriaViewModel)layout.BindingContext;

            try
            {
                HomeRankerService.DeleteCriteria(item.Criteria);
                Criterias.Remove(item);
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private SetCriteriaPage AddCriteriaModalPage;
        private string initialPhotoSetHash;
        private string initialCriteriaSetHash;

        private async void NewCriteriaValidatedInModal(object sender, CustomEventArgs e)
        {

            try
            {
                HomeRankerService.InsertCriteria(e.Criteria);

                var newCriteria = new CriteriaViewModel
                {
                    Criteria = e.Criteria
                };

                var existingIndex = Criterias.IndexOf(newCriteria);
                if (existingIndex != -1)
                {
                    newCriteria.RateValue = Criterias[existingIndex].RateValue;
                    Criterias[existingIndex] = newCriteria;

                }
                else
                {
                    Criterias.Add(newCriteria);
                }


                Criterias.Sort();


            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
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

            await Shell.Current.Navigation.PushAsync(new RateCriteriaPage(item, Apartment));
        }

        private async void EditItem_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (CriteriaViewModel)layout.BindingContext;

            AddCriteriaModalPage = new SetCriteriaPage(item.Criteria);
            AddCriteriaModalPage.CriteriaValidated += this.NewCriteriaValidatedInModal;
            await Shell.Current.Navigation.PushAsync(AddCriteriaModalPage);
        }

        private async void PhotoTapped(object sender, EventArgs e)
        {
            var item = (sender as View).BindingContext as Photo;

            var photosPage = new DetailPhotosPage();
            photosPage.Photos = this.Photos;

            var index = Photos.IndexOf(item);
            photosPage.SelectedIndex = index == -1 ? 0 : index;
            photosPage.PhotosToDelete = PhotosToDelete;
            photosPage.BindingContext = photosPage;
            SharedTransitionShell.SetTransitionSelectedGroup(this, item.PhotoUrl);

            await Shell.Current.Navigation.PushAsync(photosPage);


        }

    }
}