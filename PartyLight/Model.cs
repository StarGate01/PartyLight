using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml.Media;

namespace PartyLight.Models
{

    namespace Main
    {

        public class MainView : INotifyPropertyChanged
        {

            private int _red;
            public int Red
            {
                get
                {
                    return _red;
                }
                set
                {
                    if (value > 255) value = 255;
                    if (value < 0) value = 0;
                    _red = value;
                    RaisePropertyChanged("Red");
                }
            }

            private int _green;
            public int Green
            {
                get
                {
                    return _green;
                }
                set
                {
                    if (value > 255) value = 255;
                    if (value < 0) value = 0;
                    _green = value;
                    RaisePropertyChanged("Green");
                }
            }

            private int _blue;
            public int Blue
            {
                get
                {
                    return _blue;
                }
                set
                {
                    if (value > 255) value = 255;
                    if (value < 0) value = 0;
                    _blue = value;
                    RaisePropertyChanged("Blue");
                }
            }

            private int _modeIndex;
            public int ModeIndex
            {
                get
                {
                    return _modeIndex;
                }
                set
                {
                    _modeIndex = value;
                    RaisePropertyChanged("ModeIndex");
                }
            }

            private int _numLEDs;
            public int NumLEDs
            {
                get
                {
                    return _numLEDs;
                }
                set
                {
                    _numLEDs = value;
                    RaisePropertyChanged("NumLEDs");
                }
            }

            private string _address;
            public string Address
            {
                get
                {
                    return _address;
                }
                set
                {
                    _address = value;
                    RaisePropertyChanged("Address");
                }
            }

            public MainView() : this(0, 100, 200, 50, 12, "10.0.0.67")
            {
                if (DesignMode.DesignModeEnabled) return;
                ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
                foreach (PropertyInfo property in GetType().GetRuntimeProperties())
                {
                    if (property.CanWrite && Settings.Values.ContainsKey(property.Name)) property.SetValue(this, Settings.Values[property.Name]);
                }
            }
            public MainView(int mode, int r, int g, int b, int numLEDs, string address)
            {
                _modeIndex = mode;
                _red = r;
                _green = g;
                _blue = b;
                _numLEDs = numLEDs;
                _address = address;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void RaisePropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                if (DesignMode.DesignModeEnabled) return;
                ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
                PropertyInfo property = GetType().GetRuntimeProperty(propertyName);
                if (property.CanWrite) Settings.Values[propertyName] = property.GetValue(this);
            }

            public MainView GetCopy()
            {
                MainView copy = (MainView)this.MemberwiseClone();
                return copy;
            }

        }

        namespace Converters
        {

            public class ModeToEnabledConverter : IValueConverter
            {

                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value.GetType() != typeof(int)) return null;
                    return (int)value == 0; 
                }

                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    return null;
                }

            }

            public class ModeToIconConverter : IValueConverter
            {

                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value.GetType() != typeof(int)) return null;
                    switch ((int)value)
                    {
                        case 0:
                            return Symbol.TouchPointer.ToString();
                        case 1:
                            return Symbol.Favorite.ToString();
                        default:
                            return null;
                    }
                }

                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    return null;
                }

            }

            public class ModeToNameConverter : IValueConverter
            {

                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value.GetType() != typeof(int)) return null;
                    switch((int)value)
                    {
                        case 0:
                            return "Manual";
                        case 1:
                            return "Blink";
                        default:
                            return null;
                    }
                }

                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    return null;
                }

            }

        }

    }

}
