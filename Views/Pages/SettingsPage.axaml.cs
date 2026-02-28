using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;
using System;
using System.Diagnostics;

namespace MultiOperationExecutioner;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void EnableMouseMenu_Toggled(object sender,RoutedEventArgs e)
    {
        if(EnableMouseMenu.IsChecked == true)
        {
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard", "MUIVerb", "IFEO 选项");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard", "Icon", $"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard", "SubCommands", "");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard\Item1", "", "启用 IFEO");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard\Item1\Command", "", $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"EnableIFEO\" \"%1\"");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard\Item2", "", "禁用 IFEO");
            RegHelper.WriteRegeditString(Registry.ClassesRoot, @"*\shell\RocketGuard\Item2\Command", "", $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"DisableIFEO\" \"%1\"");
        }
        else
        {

        }
    }

    private void Page_Loaded(object sender,RoutedEventArgs e)
    {
        if(RegHelper.RegKeyExists(Registry.ClassesRoot, @"*\shell\RocketGuard"))
        {
            EnableMouseMenu.IsChecked = true;
        }
    }
}