using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Home.Ranker.Services;
using Home.Ranker.Views;
using Plugin.SharedTransitions;
using Plugin.Media;

namespace Home.Ranker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            CrossMedia.Current.Initialize();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
