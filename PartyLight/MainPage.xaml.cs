using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Net;
using System.Net.Http.Headers;
using Windows.UI.Popups;
using System.Threading;
using System.Threading.Tasks;
using PartyLight.Models.Main;

namespace PartyLight
{


    public sealed partial class MainPage : Page
    {

        private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
        private MainView model;
        private bool numLEDsChanged = true;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            model = new MainView();
            DataContext = model;
            model.PropertyChanged += Model_PropertyChanged;
        }

        private async void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ModeIndex")
            {
                try
                {
                    WebRequest request;
                    if (model.ModeIndex == 0) request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=manual&r=" + model.Red + "&g=" + model.Green + "&bb=" + model.Blue);
                    else request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=blink");
                    request.Headers["Cache-Control"] = "no-cache";
                    await request.GetResponseAsync();
                }
                catch (Exception) { }
            }
            else if(e.PropertyName == "NumLEDs")
            {
                numLEDsChanged = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AppBarButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage), model);
        }

        private async void AppBarButtonPower_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=power&power=toggle");
                request.Headers["Cache-Control"] = "no-cache";
                await request.GetResponseAsync();
            }
            catch (Exception) { }
        }

        private void AppBarButtonMode_Click(object sender, RoutedEventArgs e)
        {
            if (model.ModeIndex == 0) model.ModeIndex = 1;
            else if (model.ModeIndex == 1) model.ModeIndex = 0;
        }

        private async void AppBarButtonApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebRequest request;
                if (numLEDsChanged)
                {
                    request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=ml&ml=" + model.NumLEDs);
                    request.Headers["Cache-Control"] = "no-cache";
                    await request.GetResponseAsync();
                    numLEDsChanged = false;
                }
                if (model.ModeIndex == 0)
                {
                    request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=manual&r=" + model.Red + "&g=" + model.Green + "&bb=" + model.Blue);
                    request.Headers["Cache-Control"] = "no-cache";
                    await request.GetResponseAsync();
                }
            }
            catch (Exception) { }
        }

    }

}
