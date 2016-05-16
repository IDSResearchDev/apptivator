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
using System.Net;
using System.ComponentModel;

namespace Apptivator.ViewModel
{
    public class ActivationViewModel : BindableBase, IDataErrorInfo
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


        private string _organizationName;

        public string OrganizationName
        {
            get { return _organizationName; }
            set { SetProperty(ref _organizationName, value); }
        }

        #region ICommands

        public ICommand ActivateCode
        {
            get
            {
                return new DelegateCommand( async () =>
                {
                    // Validation
                    // web request here

                    CanValidate = true;
                    if (String.IsNullOrEmpty(ActivationCode) || String.IsNullOrEmpty(ActivationCode)) return;
                    if (String.IsNullOrEmpty(OrganizationName) || String.IsNullOrEmpty(OrganizationName)) return;

                    var util = new Rnd.Common.Utilities();
                    Activator.MacAddress = util.GetPhysicalAddress();

                    var weburi = $"http://webservice.intdesignservices.com/codeservice.php?code={ActivationCode}&mac={Activator.MacAddress}&name=OrganizationName";
                    var requeststate = new RequestState();

                    var response = await requeststate.Response(weburi);

                    if (response.ToLower().Equals("verified"))
                    {
                        util.SerializeBinFile(ActivationFile, Activator);
                        Process.Start(ApplicationPath);
                        App.Current.Shutdown();
                    }
                    else
                    {
                        MessageBox.Show("Activation code is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public new ICommand Close
        {
            get
            {
                return new DelegateCommand(() =>
               {
                   App.Current.Shutdown();
               });
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (CanValidate)
                {
                    if (columnName == "OrganizationName")
                    {
                        if (string.IsNullOrEmpty(OrganizationName))
                        {
                            return "Organization name is required.";
                        }
                    }

                    if (columnName == "ActivationCode")
                    {
                        if (string.IsNullOrEmpty(ActivationCode))
                        {
                            return "Activation code is required.";
                        }
                    }
                }

                return string.Empty;
            }
        }

        private bool _canValidate;
        private bool CanValidate
        {
            get { return _canValidate; }
            set
            {
                _canValidate = value;
                OnPropertyChanged("CanValidate");
                OnPropertyChanged("OrganizationName");
                OnPropertyChanged("ActivationCode");
            }
        }

        #endregion



    }

}
