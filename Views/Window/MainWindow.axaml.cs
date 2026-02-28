using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Microsoft.Win32;
using System;

namespace MultiOperationExecutioner
{
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.TitleBar.ExtendsContentIntoTitleBar = true;
            this.TitleBar.Height = 48;
            TitleBarIcon.Source = Xiaowang0229.ImageLibrary.Avalonias.Image.ConvertByteArrayToAvaloniaImageSource(AppResource.ApplicationImage);
            RootNavi.SelectedItem = RootNavi.MenuItems[0];
            
        }
        private void RootNavi_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if(RootNavi.SelectedItem == RootNavi.MenuItems[0])
            {
                RootFrame.Navigate(typeof(IFEOPage));
            }
            else if (RootNavi.SelectedItem == RootNavi.MenuItems[1])
            {
                RootFrame.Navigate(typeof(HostsPage));
            }
            else if (RootNavi.SelectedItem == RootNavi.MenuItems[2])
            {
                RootFrame.Navigate(typeof(GPEditPage));
            }
            else if (RootNavi.SelectedItem == RootNavi.FooterMenuItems[0])
            {
                RootFrame.Navigate(typeof(SettingsPage));
            }
            else if (RootNavi.SelectedItem == RootNavi.FooterMenuItems[1])
            {
                RootFrame.Navigate(typeof(AboutPage));
            }
            

        }
    }
}