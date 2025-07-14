using EveRouteOptimizer.Services;
using System;
using System.Windows.Forms;

namespace EveRouteOptimizer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {


            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}