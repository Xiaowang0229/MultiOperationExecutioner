using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MultiOperationExecutioner;

public partial class IFEOPage : UserControl
{
    public static bool IsValidatedPassword = false;
    public IFEOPage()
    {
        InitializeComponent();
    }

    private void CreateNewItem_Click(object sender,RoutedEventArgs e)
    {
        
        
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择可执行文件",
                Filter = "可执行文件(*.exe)|*.exe",
                Multiselect = false
            };
            if (dlg.ShowDialog() == true)
            {
                AddStackPanel($"{Path.GetFileName(dlg.FileName)}", "", true).Focus();
            }
            
        
    }

    

    private async void Page_Loaded(object sender,RoutedEventArgs e)
    {
        IsValidatedPassword = false;
        RootStackPanel.Children.Clear();
        Tip.IsVisible = true;
        await Task.Delay(10);
        var s = RegHelper.GetSubRegistryKeyNames(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options");
        for(int i = 0; i<s.Length; i++)
        {
            if (s[i].Contains(".exe"))
            {
                if (string.IsNullOrEmpty(RegHelper.ReadRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{s[i]}", "Debugger")))
                { AddStackPanel($"{s[i]}", ""); }
                else
                { AddStackPanel($"{s[i]}", $"{RegHelper.ReadRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{s[i]}", "Debugger")}"); }
            }
        }
        Tip.IsVisible = false;
    }

    private TextBox AddStackPanel(string TargetProgramArgs,string ExecuteProgramwithArguments,bool IsCreate = false)
    {
        var newifeoc = new IFEOConfig();
        var sp = new StackPanel { Margin = new Thickness(5),Orientation=Avalonia.Layout.Orientation.Horizontal,VerticalAlignment=Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(new TextBlock { Text="IFEO 目标应用(必填):" ,Margin=new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
        var tb = new TextBox { Margin=new Thickness(5),MinWidth=150,Text=TargetProgramArgs, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(tb);
        var bt = new Button { Width=120,Content = "浏览...", Margin=new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(bt);
        sp.Children.Add(new TextBlock { Text = "IFEO 转移应用及参数(可选):", Margin=new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
        var tb0 = new TextBox { Margin=new Thickness(5), MinWidth = 150,Text=ExecuteProgramwithArguments, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(tb0);
        var bt1 = new Button { Width = 120, Content = "浏览...", Margin=new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(bt1);
        var btx = new Button { Width = 120, Content = "加入管控", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        var btx2 = new Button { Width = 120, Content = "解除管控", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        if(ExecuteProgramwithArguments == $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"ExecutedMessage\"")
        {
            sp.Children.Add(btx2);
            tb.IsEnabled = false;
            tb0.IsEnabled = false;
            bt.IsEnabled = false;
            bt1.IsEnabled = false;
        }
        else
        {
            sp.Children.Add(btx);
        }
        var bt2 = new Button {  Width=120,Content = "确定", Margin=new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };

        sp.Children.Add(bt2);
        var bt3 = new Button { Width = 120, Content = "删除", Margin=new Thickness(5),Foreground=new SolidColorBrush(Colors.Red), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(bt3);
        
        tb.TextChanged += (s, e) =>
        {
            newifeoc.TargetProgram = tb.Text;
        };
        tb0.TextChanged += (s, e) =>
        {
            newifeoc.ExecuteProgramArgs = tb0.Text;
        };

        bt.Click += (s, e) =>
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择可执行文件",
                Filter = "可执行文件(*.exe)|*.exe",
                Multiselect = false
            };
            if (dlg.ShowDialog() == true)
            {
                tb.Text = Path.GetFileName(dlg.FileName);
            }
        };

        bt1.Click += (s, e) =>
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择可执行文件",
                Filter = "可执行文件(*.exe)|*.exe",
                Multiselect = false
            };
            if (dlg.ShowDialog() == true)
            {
                tb0.Text = $"\"{dlg.FileName}\"";
            }
        };

        btx.Click += (s, e) =>
        {
            
            tb0.Text = $"\"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe\" \"ExecutedMessage\"";
            tb.IsEnabled = false;
            bt.IsEnabled = false;
            bt1.IsEnabled = false;
            tb0.IsEnabled = false;
            sp.Children.Remove(btx);
            sp.Children.Add(btx2);
        };
        btx2.Click += (s, e) =>
        {
            if (RegHelper.ValidatePassword())
            {
                tb0.Text = "";
                bt.IsEnabled = true;
                bt1.IsEnabled = true;
                tb.IsEnabled = true;
                tb0.IsEnabled = true;
                tb0.Focus();
                sp.Children.Remove(btx2);
                sp.Children.Add(btx);
            }
        };
        
        bt2.Click += (s, e) =>
        {
            
            if (RegHelper.ValidatePassword())
            {
                if (!string.IsNullOrEmpty(newifeoc.TargetProgram))
                {
                    RegHelper.WriteRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{newifeoc.TargetProgram}");
                    if (newifeoc.ExecuteProgramArgs != null)
                    {



                        RegHelper.WriteRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{newifeoc.TargetProgram}", "Debugger", $"\"{newifeoc.ExecuteProgramArgs}\"");

                    }


                }
                else
                {
                    Variables._MainWindow.ShowMessageAsync("提示", "必填项不能为空");
                }
                IsValidatedPassword = true;
            }
            else
            {
                Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
            }
        }
        ;
        bt3.Click += (s, e) =>
        {
            if(RegHelper.ValidatePassword())
            {
                IsValidatedPassword = true;
                if(RegHelper.RegKeyExists(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{newifeoc.TargetProgram}"))
                {
                    RegHelper.DeleteRegeditKey(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{newifeoc.TargetProgram}");
                }
                RootStackPanel.Children.Remove(sp);
            }
            else
            {
                Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
            }
        };
        
        if(IsCreate)
        {
            RootStackPanel.Children.Insert(0, (sp));
        }
        else
        {
            RootStackPanel.Children.Add(sp);
        }
        return tb;
    }

}

public class IFEOConfig
{
    public string? TargetProgram { get; set; }
    public string? ExecuteProgramArgs { get; set; } = null;
}