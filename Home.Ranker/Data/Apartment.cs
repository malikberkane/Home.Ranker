using Xamarin.Forms;

namespace Home.Ranker.Data
{
    public class Apartment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string   Adresse { get; set; }


        public string FirstPictureUrl { get; set; }

        public ImageSource FirstPictureImageSource { get; set; }

    }
}