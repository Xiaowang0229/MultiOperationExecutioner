using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
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
        }
        private void RootNavi_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if(RootNavi.SelectedItem == RootNavi.MenuItems[0])
            {
                RootFrame.Navigate(typeof());
            }
            else if (RootNavi.SelectedItem == RootNavi.MenuItems[0])
            {
                RootFrame.Navigate(typeof());
            }
            else if (RootNavi.SelectedItem == RootNavi.MenuItems[0])
            {
                RootFrame.Navigate(typeof());
            }
            else if (RootNavi.SelectedItem == RootNavi.MenuItems[0])
            {
                RootFrame.Navigate(typeof());
            }
            

        }
    }
}