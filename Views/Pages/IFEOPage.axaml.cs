using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MultiOperationExecutioner;

public partial class IFEOPage : UserControl
{
    public static bool IsValidatedPassword = false;
    private Button bt2p;

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
        bt2p = new Button { IsEnabled = false, Width = 120, Content = "保存", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center }; ;
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
        var sp = new StackPanel { Margin = new Thickness(10), Orientation = Avalonia.Layout.Orientation.Horizontal, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        var bt = new Button { Width = 120, Content = "放弃操作", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        bt.Click += async(s,e) =>
        {
            
                if(await Variables._MainWindow.ShowMessageAsync("警告","确认放弃当前所有改动吗?该操作不可逆"))
                {
                    Variables._MainWindow.RootFrame.Navigate(typeof(IFEOConfig));
                }
            
        };
        sp.Children.Add(bt);
       
        bt2p.Click += SaveButton_Click;
        
        bt2p.IsEnabled = false;
        sp.Children.Add(bt2p);
        RootStackPanel.Children.Add(sp);
        Tip.IsVisible = false;
        await Task.Delay(50);
        bt2p.IsEnabled = false;
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        if(RegHelper.ValidatePassword())
        {
            for(int i = 0;i<RootStackPanel.Children.Count-1;i++)
            {
                if (RootStackPanel.Children[i] is StackPanel sp)
                {
                    if(sp.Tag is DelConfig sptag0)
                    {

                        if (RegHelper.RegKeyExists(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{sptag0.ChangeConfig.TargetProgram}"))
                        {

                            RegHelper.DeleteRegeditKey(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{sptag0.ChangeConfig.TargetProgram}");
                                
                        }
                        
                    }
                    if(sp.Tag is TagConfig sptag)
                    {
                        
                        
                        if (sptag.OriginalConfig.ExecuteProgramArgs != sptag.ChangeConfig.ExecuteProgramArgs || sptag.OriginalConfig.TargetProgram != sptag.ChangeConfig.TargetProgram)
                        {
                            
                            if (!string.IsNullOrEmpty(sptag.ChangeConfig.TargetProgram))
                            {
                                RegHelper.WriteRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{sptag.ChangeConfig.TargetProgram}");
                                if (sptag.ChangeConfig.ExecuteProgramArgs != null)
                                {



                                    RegHelper.WriteRegeditString(Registry.LocalMachine, @$"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{sptag.ChangeConfig.TargetProgram}", "Debugger", $"{sptag.ChangeConfig.ExecuteProgramArgs}");

                                }


                            }
                            else
                            {
                                Variables._MainWindow.ShowMessageAsync("提示", "必填项不能为空");
                            }
                        }
                    }
                }
            }
            Variables._MainWindow.RootFrame.Navigate(typeof(SettingsPage));
            Variables._MainWindow.RootFrame.Navigate(typeof(IFEOPage));
        }
        else
        {
            Variables._MainWindow.ShowMessageAsync("提示", "验证失败,操作已取消");
        }
    }

    private TextBox AddStackPanel(string TargetProgramArgs, string ExecuteProgramwithArguments, bool IsCreate = false)
    {
        var newifeoc = new IFEOConfig();
        var oriifeoc = new IFEOConfig();
        var sp = new StackPanel { Margin = new Thickness(5), Orientation = Avalonia.Layout.Orientation.Horizontal, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(new TextBlock { Text = "IFEO 目标应用(必填):", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
        var tb = new TextBox { Margin = new Thickness(5), MinWidth = 150, Text = TargetProgramArgs, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(tb);
        var bt = new Button { Width = 120, Content = "浏览...", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(bt);
        sp.Children.Add(new TextBlock { Text = "IFEO 转移应用及参数(可选):", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
        var tb0 = new TextBox { Margin = new Thickness(5), MinWidth = 150, Text = ExecuteProgramwithArguments, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(tb0);
        var bt1 = new Button { Width = 120, Content = "浏览...", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        sp.Children.Add(bt1);
        var btx = new Button { Width = 120, Content = "加入管控", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        var btx2 = new Button { Width = 120, Content = "解除管控", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        var bt3 = new Button { Width = 120, Content = "删除", Margin = new Thickness(5), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,Foreground=new SolidColorBrush(Colors.Red) };
        sp.Children.Add(bt3);
        sp.Tag = null;
        oriifeoc.ExecuteProgramArgs = ExecuteProgramwithArguments;
        oriifeoc.TargetProgram = TargetProgramArgs;

        if (ExecuteProgramwithArguments.Contains($"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe ExecutedMessage"))
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


        tb.TextChanged += (s, e) =>
        {
            newifeoc.TargetProgram = tb.Text;
            sp.Tag = new TagConfig { ChangeKind = TagKind.Change, ChangeConfig = newifeoc,OriginalConfig=oriifeoc };
            bt2p.IsEnabled = true;
            
        };
        tb0.TextChanged += (s, e) =>
        {
            newifeoc.ExecuteProgramArgs = tb0.Text;
            sp.Tag = new TagConfig { ChangeKind = TagKind.Change, ChangeConfig = newifeoc, OriginalConfig = oriifeoc };
            bt2p.IsEnabled = true;
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
                tb0.Text = $"{dlg.FileName}";
            }
        };

        btx.Click += (s, e) =>
        {

            tb0.Text = $"{Environment.CurrentDirectory}\\{Process.GetCurrentProcess().ProcessName}.exe ExecutedMessage";
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

        
        bt3.Click += async(s, e) =>
        {
            if(RegHelper.ValidatePassword())
            {
                if(await Variables._MainWindow.ShowMessageAsync("警告","确认删除该项吗?该操作不可逆"))
                {
                    IsValidatedPassword = true;
                    sp.Tag = new DelConfig
                    {
                        ChangeKind = TagKind.Delete,
                        ChangeConfig = newifeoc
                    };
                    bt2p.IsEnabled = true;
                    sp.IsVisible = false;
                }
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
    public string TargetProgram { get; set; }
    public string? ExecuteProgramArgs { get; set; } = null;
}

public class TagConfig
{
    public required string ChangeKind { get; set; }

    public required IFEOConfig OriginalConfig { get; set; }
    public required IFEOConfig ChangeConfig { get; set; }
}

public class DelConfig
{
    public required string ChangeKind { get; set; }

    public required IFEOConfig ChangeConfig { get; set; }
}

public class TagKind
{
    public const string Delete = "Delete";
    public const string Add = "Add";
    public const string Change = "Change";
}