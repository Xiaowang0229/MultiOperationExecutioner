using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiOperationExecutioner.Utils
{
    public static class RegHelper
    {
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
                        return key.GetValue("Key")?.ToString();

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

        public static void DeleteRegeditString(RegistryKey Root, string KeyPath, string Key)
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
    }
}
