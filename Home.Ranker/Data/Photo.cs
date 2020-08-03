using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Home.Ranker.Data
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public int ApartmentId { get; set; }

        public ImageSource Source { get;set; }

        public string Base64 { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Photo other)
            {
                return other.PhotoUrl == PhotoUrl;
            }
            return false;
        }


    }
}