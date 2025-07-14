using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using EveRouteOptimizer.ESI;

namespace EveRouteOptimizer
{
    public partial class FormOAuthLogin : Form
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        private readonly string _state;

        public FormOAuthLogin()
        {
            InitializeComponent();
            _state = Guid.NewGuid().ToString("N");
        }

        private async void FormOAuthLogin_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Подготовка к авторизации...";

            using var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:12345/callback/");
            try
            {
                listener.Start();
            }
            catch (HttpListenerException)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            string url = EsiAuthManager.BuildLoginUrl(_state);
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Не удалось открыть браузер. Перейдите по ссылке вручную:\n\n" + url, "Внимание");
            }

            HttpListenerContext context;
            try
            {
                context = await listener.GetContextAsync();
            }
            catch
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            try
            {
                string html = "<html><head><meta charset='UTF-8'></head><body><h2>Авторизация завершена. Можете закрыть окно.</h2></body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(html);
                context.Response.ContentType = "text/html; charset=utf-8";
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                await Task.Delay(250); // 🔧 пауза перед закрытием
                context.Response.OutputStream.Close();
            }
            catch { /* Ошибку можно игнорировать */ }

            listener.Stop();

            var query = context.Request.Url.Query;
            var parameters = HttpUtility.ParseQueryString(query);
            string receivedState = parameters["state"];
            string code = parameters["code"];

            if (receivedState != _state || string.IsNullOrEmpty(code))
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            try
            {
                lblStatus.Text = "Получение токенов...";
                var result = await EsiAuthManager.ExchangeCodeAsync(code);
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                DialogResult = DialogResult.OK;
            }
            catch
            {
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
