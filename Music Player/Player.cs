using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Net;

namespace Music_Player
{
    public partial class Player : Form
    {
        public Player()
        {
            InitializeComponent();
            initForm2();
            this.Shown += (object sender, EventArgs e) =>
            {
                this.Hide();
            };
        }
        private void initForm2()
        {
            PlayerSelection form2 = new PlayerSelection();
            form2.Show();
            form2.comboBox1.TextChanged += (object sender, EventArgs e) =>
            {
                if (form2.comboBox1.Text == "Suno")
                {
                    setPlayer("https://suno.com/");
                }
                else if (form2.comboBox1.Text == "MonsterCat")
                {
                    setPlayer("https://player.monstercat.app/");
                }
                else
                {
                    throw new Exception("ComboBox not selected properly");
                }
                form2.Dispose();
            };
            form2.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                Application.Exit();
            };
        }
        private async Task setPlayer(string paramURL)
        {
            this.Show();
            webView21.Width = this.ClientSize.Width;
            webView21.Height = this.ClientSize.Height;
            webView21.Location = new Point(0, 0);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\skitamiPlayer";
            CoreWebView2Environment webView2Environment = await CoreWebView2Environment.CreateAsync(null, folderPath);
            await webView21.EnsureCoreWebView2Async(webView2Environment);
            webView21.Source = new Uri(paramURL);
            webView21.CoreWebView2.DocumentTitleChanged += (object sender, Object arg) =>
            {
                if (this.Text == "Player")
                {
                    this.Text = webView21.CoreWebView2.DocumentTitle;
                }
            };
            webView21.CoreWebView2.FaviconChanged += (object sender, Object arg) =>
            {
                if (sender is Microsoft.Web.WebView2.Core.CoreWebView2)
                {
                    Microsoft.Web.WebView2.Core.CoreWebView2 coreWebView = (CoreWebView2)sender;
                    using (var client = new WebClient())
                    {
                        string faviconPath = folderPath + "\\favicon.icon";
                        client.DownloadFile(coreWebView.FaviconUri, faviconPath);
                        this.Icon = new Icon(faviconPath);
                    }
                }
            };
        }
    }
}
