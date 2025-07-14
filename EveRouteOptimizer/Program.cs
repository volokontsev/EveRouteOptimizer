using System;
using System.Windows.Forms;

namespace EveRouteOptimizer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!DotNetRuntimeChecker.IsNet8Installed())
            {
                DialogResult result = MessageBox.Show(
                    ".NET 8 Runtime не установлен.\n\nНажмите OK, чтобы перейти на сайт загрузки.",
                    ".NET 8 не найден",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error);

                if (result == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime",
                        UseShellExecute = true
                    });
                }

                return; // завершить приложение
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}