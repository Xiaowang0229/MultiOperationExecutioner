using Avalonia;
using MultiOperationExecutioner.Utils;
using Ookii.Dialogs.Wpf;
using System;

namespace MultiOperationExecutioner
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            
            if(args.Length == 0)
            {
                BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
                Variables.Args = args;
                
            }
            else if(args.Length == 1)
            {
                if (args[0] == "ExecutedMessage")
                {
                    var mb = new Ookii.Dialogs.Wpf.TaskDialog
                    {
                        WindowTitle = "提示",
                        MainIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Warning,
                        MainInstruction = "程序已加入管控,暂无法开启",
                        ButtonStyle = TaskDialogButtonStyle.CommandLinks,


                    };
                    var mbb41 = new TaskDialogButton
                    {
                        Text = "退出程序",
                    };
                    mb.Buttons.Add(mbb41);
                    var mbb4 = new TaskDialogButton
                    {
                        ButtonType = ButtonType.Close
                    };
                    mb.Buttons.Add(mbb4);
                    mb.ShowDialog();
                   
                }
            }
            else if(args.Length == 2)
            {
                System.Windows.MessageBox.Show($"{args[0]}\r\n{args[1]}", "T");
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
