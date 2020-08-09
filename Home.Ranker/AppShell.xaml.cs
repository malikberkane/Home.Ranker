using Home.Ranker.Views;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Home.Ranker
{
    public partial class AppShell : SharedTransitionShell
    {
        public AppShell()
        {
            InitializeComponent();
        }


        protected override bool OnBackButtonPressed()
        {
            var page = (Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;

            if (!(page is SetApartmentPage setAptPage)) return base.OnBackButtonPressed();

            if (Current.Navigation.NavigationStack.Count > 1)
            {
                setAptPage.NavBackImplementation();
            }

            return true;

        }
    }
}
