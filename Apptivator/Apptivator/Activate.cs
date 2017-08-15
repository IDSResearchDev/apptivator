using Apptivator.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Apptivator
{
    public static class Activate
    {
        public static void Run(StartupEventArgs e)
        {
            if (e.Args.Length >= 3)
            {
                ActivationView view = new ActivationView();

                view.vm.ActivationFile = e.Args[0].Replace(e.Args[2], " ");
                view.vm.ApplicationPath = e.Args[1].Replace(e.Args[2], " ");
                if (e.Args.Length >= 4)
                { view.vm.ActivationUrl = e.Args[3]; }
                view.Show();
            }
            else
            {
                App.Current.Shutdown();
            }
        }
    }
}
