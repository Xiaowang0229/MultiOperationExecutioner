using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;
using MultiOperationExecutioner.Utils;

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
    }
}