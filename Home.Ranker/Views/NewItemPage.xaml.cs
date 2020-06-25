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

namespace Home.Ranker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Apartment Item { get; set; }

        public ObservableCollection<Photo> Photos { get; set; }

        public NewItemPage(string name)
        {
            InitializeComponent();

            ApartmentRepository = new ApartmentRepository(new HomeRankerContext());

            PhotoRepository = new PhotoRepository(new HomeRankerContext());

            Item = new Apartment
            {
                Name = name
            };

            CriteriasRatings = new ObservableCollection<CriteriaViewModel>
            {
                new CriteriaViewModel{Criteria= new Criteria{Name= "Luminosité"}},
                new CriteriaViewModel{Criteria= new Criteria{Name= "Prix"}},
                new CriteriaViewModel{Criteria= new Criteria{Name= "Espace"}},
                new CriteriaViewModel{Criteria= new Criteria{Name= "Emplacement"}},
                new CriteriaViewModel{Criteria= new Criteria{Name= "Coup de coeur"}},
                new CriteriaViewModel{Criteria= new Criteria{Name= "Luminosité"}},
            };

            BindingContext = this;
        }

        public ObservableCollection<CriteriaViewModel> CriteriasRatings { get; set; }

        public NewItemPage(Apartment appartment)
        {
            InitializeComponent();

            var homeContext = new HomeRankerContext();
            ApartmentRepository = new ApartmentRepository(homeContext);

            PhotoRepository = new PhotoRepository(homeContext);


            Item = appartment;

            var photos = PhotoRepository.GetPhotos(p => p.ApartmentId == Item.Id);

            foreach (var photo in photos)
            {
                photo.Source = ImageSource.FromStream(() =>
                 {
                     var bytes = File.ReadAllBytes(photo.PhotoUrl);
                     return new MemoryStream(bytes);
                 });
                //photo.Source = ImageSource.FromStream(() => 
                //{
                //    var bytes = Convert.FromBase64String(photo.Base64);
                //    return new MemoryStream(bytes);
                //});

            }

            Photos = new ObservableCollection<Photo>(photos);

           

            BindingContext = this;
        }



        private readonly ApartmentRepository ApartmentRepository;
        private readonly PhotoRepository PhotoRepository;


        async void Save_Clicked(object sender, EventArgs e)
        {
            ApartmentRepository.InsertAppartment(Item);


            ApartmentRepository.Save();

            foreach (var photo in Photos)
            {
                photo.ApartmentId = Item.Id;
                PhotoRepository.InsertPhoto(photo);
            }

            PhotoRepository.Save();
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
                CompressionQuality = 100,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;



            if (Photos == null)
            {
                Photos = new ObservableCollection<Photo>();
            }

            //var stream = file.GetStream();
            //var bytes = new byte[stream.Length];
            //await stream.ReadAsync(bytes, 0, (int)stream.Length);
            //string base64 = System.Convert.ToBase64String(bytes);


            var newPhoto = new Photo
            {
                PhotoUrl = file.Path,
                ApartmentId = Item.Id,
                //Base64=base64,
                Source = ImageSource.FromStream(() =>
                {
                    var fileStream = file.GetStream();
                    return fileStream;
                })

            };


            //newPhoto.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    return stream;
            //});

            Photos.Add(newPhoto);

            OnPropertyChanged(nameof(Photos));

        }

    }
}