﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Home.Ranker.Services;
using Home.Ranker.Views;
using Plugin.SharedTransitions;

namespace Home.Ranker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new SharedTransitionNavigationPage(new ItemsPage());
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
