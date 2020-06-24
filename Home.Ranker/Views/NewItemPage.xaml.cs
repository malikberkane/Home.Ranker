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

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Apartment Item { get; set; }

        public ObservableCollection<Photo> Photos { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            ApartmentRepository = new ApartmentRepository(new HomeRankerContext());

            Item = new Apartment
            {
                Name = "Item name",
                Adresse = "Item adress"
            };

            BindingContext = this;
        }


        private readonly ApartmentRepository ApartmentRepository;

        async void Save_Clicked(object sender, EventArgs e)
        {
            ApartmentRepository.InsertAppartment(Item);

            ApartmentRepository.Save();
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
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
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");


            if (Photos == null)
            {
                Photos = new ObservableCollection<Photo>();
            }

            var newPhoto = new Photo { PhotoUrl = file.Path, ApartmentId = Item.Id };

            newPhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });


            Photos.Add(newPhoto);


        }

        public ImageSource Source { get; set; }
    }
}