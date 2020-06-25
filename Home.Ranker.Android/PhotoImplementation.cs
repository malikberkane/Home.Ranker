using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Home.Ranker.Data;
using Home.Ranker.Droid;
using Home.Ranker;

[assembly: Xamarin.Forms.Dependency(typeof(PhotoImplementation))]

namespace Home.Ranker.Droid
{
   
        public class PhotoImplementation : Java.Lang.Object, IPhoto
        {
            public  Stream GetPhoto(string path)
            {
                // Open the photo and put it in a Stream to return       
                var memoryStream = new MemoryStream();

                using (var source = System.IO.File.OpenRead(path))
                {
                     source.CopyToAsync(memoryStream);
                }

                return memoryStream;
            }
        }
    
}