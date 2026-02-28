using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;
using System;
using System.Threading.Tasks;

namespace MultiOperationExecutioner
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                RegisterGlobalExceptionHandlers();
                desktop.MainWindow = new MainWindow();
                if(!RegHelper.RegKeyExists(Registry.CurrentUser, @"Software\RocketGuard"))
                {
                    RegHelper.Initialize();
                }
                if(RegHelper.ReadRegeditString(Registry.CurrentUser, @"Software\RocketGuard", "IsStartUpCheckUpdate") == bool.TrueString)
                {
                    Update.CheckUpdate();
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static void RegisterGlobalExceptionHandlers()
        {
            // 捕获 UI 线程未处理的异常
            Avalonia.Threading.Dispatcher.UIThread.UnhandledException += (s, e) =>
            {


                WindowHelper.ShowExceptionDialog(e.Exception);


            };

            // 捕获非 UI 线程未处理的异常
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {

                if (e.ExceptionObject is Exception ec) WindowHelper.ShowExceptionDialog(ec);


            };

            // 捕获 Task 线程未处理的异常
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {

                WindowHelper.ShowExceptionDialog(e.Exception);

            };
        }
    }
}