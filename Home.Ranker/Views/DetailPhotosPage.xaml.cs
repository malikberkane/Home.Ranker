using Home.Ranker.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Home.Ranker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPhotosPage : ContentPage
    {
        public DetailPhotosPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Photo> Photos { get; set; }

        public int SelectedIndex { get; set; }

        public int CurrentApartmentId { get; set; }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            if (SelectedIndex != -1)
            {
                Photos.RemoveAt(SelectedIndex);

            }

        }

        async void BackButton_Clicked(object sender, EventArgs e)
        {
           
         await Shell.Current.Navigation.PopAsync();
                

        }


        async void TakePhotoClicked(object sender, EventArgs e)
        {


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
                ApartmentId = CurrentApartmentId,
                //Base64=base64,
                Source = ImageSource.FromStream(() =>
                {
                    var fileStream = file.GetStream();
                    return fileStream;
                })

            };



            Photos.Add(newPhoto);
        }

       

    }
}