using Avalonia.Controls;
using Avalonia.Interactivity;
using MultiOperationExecutioner.Utils;
using Button = Avalonia.Controls.Button;

namespace MultiOperationExecutioner;

public partial class AboutPage : UserControl
{
    public AboutPage()
    {
        InitializeComponent();
        Updatelog.Markdown = Variables.VersionLog;
        //EULA.Markdown = Variables.EULAString;
        //EULA.Width = EULAExpander.Width;
        EULABox.Text = "";
    }

    private void RepoUrl_Click(object sender,RoutedEventArgs e)
    {
        Update.OpenBrowser(RepoLink.Content.ToString());
    }
    private void IssueUrl_Click(object sender, RoutedEventArgs e)
    {
        Update.OpenBrowser(IssueLink.Content.ToString());
    }
    private void Url_Click(object sender, RoutedEventArgs e)
    {
        HyperlinkButton s = (HyperlinkButton)sender;
        Update.OpenBrowser(s.Tag.ToString());
    }

    private async void ChkUpd_Click(object sender,RoutedEventArgs e)
    {
        if(sender is Button btn)
        {
            btn.IsEnabled = false;
            await Update.CheckUpdate(true);
            btn.IsEnabled = true;
        }
        
    }
}