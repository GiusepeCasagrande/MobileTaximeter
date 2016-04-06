using System;
using FreshMvvm;
using Taxi.PageModels;
using Taxi.Pages;
using Xamarin.Forms;

namespace Taxi
{
    public class App : Application
    {
        public App()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MapPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

