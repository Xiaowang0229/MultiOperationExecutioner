using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;
using System;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard", "MUIVerb", "IFEO 选项");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard", "Icon", $"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard", "SubCommands", "");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard\shell\Item1", "", "启用 IFEO");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard\shell\Item1\Command", "", $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"EnableIFEO\" \"%1\"");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard\shell\Item2", "", "禁用 IFEO");
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard\shell\Item2\Command", "", $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"DisableIFEO\" \"%1\"");
        }
        else
        {
            RegHelper.DeleteRegeditKey(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard");
        }
    }

    private void Page_Loaded(object sender,RoutedEventArgs e)
    {
        if(RegHelper.RegKeyExists(Registry.CurrentUser, @"Software\Classes\*\shell\RocketGuard"))
        {
            EnableMouseMenu.IsChecked = true;
        }
        
        if (RegHelper.ReadRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "IsStartUpCheckUpdate") == bool.TrueString)
        {
            StartUpCheckUpdate.IsChecked = true;
        }

        if(string.IsNullOrEmpty(RegHelper.ReadRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password")))
        {
            DisablePassword.IsVisible = false;
        }
        else
        {
            EnablePassword.IsVisible = false;
        }
    }

    private void StartUpCheckUpdate_Toggled(object sender, RoutedEventArgs e)
    {

        if (StartUpCheckUpdate.IsChecked == true)
        {

            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "IsStartUpCheckUpdate", bool.TrueString);
        }
        else
        {

            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "IsStartUpCheckUpdate", bool.FalseString);
        }
    }

    private async void ResConfig_Click(object sender,RoutedEventArgs e)
    {
        if(RegHelper.ValidatePassword())
        {
            if (await Variables._MainWindow.ShowMessageAsync("提示", "确定要恢复配置文件至默认状态吗?此操作不可逆"))
            {
                RegHelper.Initialize();
                WindowHelper.Restart();
            }
        }
        else
        {
            Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
        }
    }
    

    private void EnablePassword_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new Ookii.Dialogs.Wpf.CredentialDialog
        {
            WindowTitle = "输入密码",
            MainInstruction = "请输入密码(用户名可置空).",
            Content = "用于进行敏感操作需要验证的密码",
            ShowSaveCheckBox = false,
            ShowUIForSavedCredentials = false,
            Target = "RocketGuard"
        };
        if(dlg.ShowDialog())
        {
            var res = dlg.Credentials.Password;
            if(!string.IsNullOrEmpty(dlg.Credentials.Password))
            {
                var dlg2 = new Ookii.Dialogs.Wpf.CredentialDialog
                {
                    WindowTitle = "确认密码",
                    MainInstruction = "请确认密码(用户名可置空).",
                    Content = "用于进行敏感操作需要验证的密码",
                    ShowSaveCheckBox = false,
                    ShowUIForSavedCredentials = false,
                    Target = "RocketGuard"
                };
                if (dlg2.ShowDialog())
                {
                    if (dlg2.Credentials.Password == dlg.Credentials.Password)
                    {
                        
                        RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password", dlg2.Credentials.Password);
                        EnablePassword.IsVisible = false;
                        DisablePassword.IsVisible = true;
                        Variables._MainWindow.ShowMessageAsync("提示", "密码设置成功");
                    }
                    else
                    {
                        Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
                    }
                }
                else
                {
                    Variables._MainWindow.ShowMessageAsync("提示", "操作已取消");
                }
            }
            else
            {
                Variables._MainWindow.ShowMessageAsync("提示", "密码不可置空,操作已取消");
            }
        }
        
    }

    private void DisablePassword_Click(object sender, RoutedEventArgs e)
    {
        
            if(RegHelper.ValidatePassword())
            {
                RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password", "");
                EnablePassword.IsVisible = true;
                DisablePassword.IsVisible = false;
                Variables._MainWindow.ShowMessageAsync("提示", "密码已清除");
            }
            else
            {
                Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
            }
        
        
    }

}