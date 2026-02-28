using Avalonia.Interactivity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiOperationExecutioner.Utils
{
    public static class RegHelper
    {
        public static void Initialize()
        {
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "IsStartUpCheckUpdate", bool.TrueString);
            RegHelper.WriteRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password", "");
        }

        

        public static void WriteRegeditString(RegistryKey Root, string KeyPath)
        {
            try
            {

                RegistryKey key = Root.CreateSubKey(KeyPath);
                    
                


            }
            catch (Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("写入注册表失败", $"{ex}");
            }
        }
        public static void WriteRegeditString(RegistryKey Root, string KeyPath, string Key, string Value)
        {
            try
            {

                using (RegistryKey key = Root.CreateSubKey(KeyPath))
                {
                    if (key != null)
                    {
                        key.SetValue($"{Key}", $"{Value}");
                    }
                }


            }
            catch (Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("写入注册表失败", $"{ex}");
            }
        }

        public static string? ReadRegeditString(RegistryKey Root, string KeyPath, string Key)
        {
            try
            {
                using (RegistryKey key = Root.OpenSubKey(KeyPath, false))
                {
                    if (key != null)
                    {
                        return key.GetValue(Key)?.ToString();

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("读取注册表失败", $"{ex}");
                return null;
            }
        }

        public static void DeleteRegeditKey(RegistryKey Root, string KeyPath, string Key)
        {
            try
            {
                using (RegistryKey key = Root.OpenSubKey(KeyPath, true))
                {
                    key?.DeleteValue(Key, false);
                }
            }
            catch (Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("读取注册表失败", $"{ex}");
            }
        }

        public static void DeleteRegeditKey(RegistryKey Root, string KeyPath)
        {
            try
            {
                
                
                    Root.DeleteSubKeyTree(KeyPath);
                
            }
            catch (Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("读取注册表失败", $"{ex}");
            }
        }



        public static bool RegKeyExists(RegistryKey Key,string subKeyPath)
        {
            using (var key = Key.OpenSubKey(subKeyPath))
            {
                return key != null;
            }
        }

        public static bool ValidatePassword()
        {
            if(!string.IsNullOrEmpty(RegHelper.ReadRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password")))
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
                if (dlg.ShowDialog())
                {
                    var res = dlg.Credentials.Password;
                    if (dlg.Credentials.Password == RegHelper.ReadRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "Password"))
                    {
                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static string[]? GetSubRegistryKeyNames(RegistryKey Key, string subKeyPath)
        {
            try
            {
                using(var key = Key.OpenSubKey(subKeyPath))
                {
                    return key?.GetSubKeyNames();
                }
            }
            catch(Exception ex)
            {
                Variables._MainWindow.ShowMessageAsync("读取注册表失败", $"{ex}");
                return null;
            }
        }
    }
}
