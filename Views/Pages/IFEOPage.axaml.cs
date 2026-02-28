using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;

namespace MultiOperationExecutioner;

public partial class IFEOPage : UserControl
{
    public IFEOPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender,RoutedEventArgs e)
    {
        var s = RegHelper.GetSubRegistryKeyNames(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options");
        
    }

    private static StackPanel EditStackPanel()
    {

    }

}