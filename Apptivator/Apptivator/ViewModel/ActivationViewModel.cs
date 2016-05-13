using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Apptivator.BaseClass;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace Apptivator.ViewModel
{
    public class ActivationViewModel : BindableBase
    {
        public string ActivationFile { get; set; }
        public string ApplicationPath { get; set; }        

        public Rnd.Common.Models.Activator Activator { get; set; } = new Rnd.Common.Models.Activator();

        private string _activationCode;

        public string ActivationCode
        {
            get { return _activationCode; }
            set { SetProperty(ref _activationCode, value); }

        }

        #region ICommands

        public ICommand ActivateCode
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Validation
                    // web request here
                    try
                    {                       
                        var util = new Rnd.Common.Utilities();
                        Activator.MacAddress = util.GetPhysicalAddress();
                        //Activator.ActivationCode = ActivationCode;
                        util.SerializeBinFile(ActivationFile, Activator);

                        Process.Start(ApplicationPath);
                        App.Current.Shutdown();
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show(e.GetBaseException().ToString());
                    }



                });
            }
        }

        #endregion
    }

}
