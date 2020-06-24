using System.IO;
using Home.Ranker.Data;
using Home.Ranker.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Home.Ranker.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetConnection()
        {
            var fileName = "homeRankerDb.db3";
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, fileName);

        }
    }
}