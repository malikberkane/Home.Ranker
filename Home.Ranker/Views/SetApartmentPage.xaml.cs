using System;
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

namespace Home.Ranker.Views
{

    public partial class SetApartmentPage : ContentPage
    {
        public Apartment Apartment { get; set; }

        private HomeRankerService HomeRankerService;

        public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();

        public ObservableCollection<CriteriaViewModel> Criterias { get; set; } = new ObservableCollection<CriteriaViewModel>();




        public SetApartmentPage(string name)
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


        public SetApartmentPage(Apartment appartment)
        {
            InitializeComponent();

            Apartment = appartment;
            AdressEditor.Text = Apartment.Adresse;
            BindingContext = this;


        }




        void Save_Clicked(object sender, EventArgs e)
        {


            HomeRankerService.InsertApartment(Apartment, Photos, Criterias);




            MessagingCenter.Send(this, "AddItem", Apartment);



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

            if (await DisplayAlert(string.Empty, "Would you like to get adress from current location?", "Get adress", "Cancel"))
            {
                var location = await Geolocation.GetLocationAsync();



                var info = await Geocoding.GetPlacemarksAsync(location);



                var adress = info.FirstOrDefault();

                Apartment.Adresse = AdressEditor.Text = $"{adress.SubThoroughfare} {adress.Thoroughfare}, {adress.PostalCode} {adress.Locality}";
            }


        }

        private async Task TakePicture()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Shell.Current.DisplayAlert("No Camera", ":( No camera available.", "OK");
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }





        protected override void OnAppearing()
        {
            var currentCarousel = collectionView.ItemsSource;
            base.OnAppearing();
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
    }
}