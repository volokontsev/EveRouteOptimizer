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
                    ".NET 8 Runtime �� ����������.\n\n������� OK, ����� ������� �� ���� ��������.",
                    ".NET 8 �� ������",
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

                return; // ��������� ����������
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}