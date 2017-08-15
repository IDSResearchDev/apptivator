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
        public string ActivationUrl { get; set; }

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

                    try
                    {
                        var util = new Rnd.Common.Utilities();
                        Activator.MacAddress = util.GetPhysicalAddress();

                        // Connection Creator Activation URL link (for existing)
                        var weburi = $"http://webservice.intdesignservices.com/codeservice.php?code={ActivationCode}&mac={Activator.MacAddress}&name={OrganizationName}";

                        
                        if(!string.IsNullOrEmpty(ActivationUrl))
                        { weburi = $"{ActivationUrl}codeservice.php?code={ActivationCode}&mac={Activator.MacAddress}&name={OrganizationName}"; }

                        var requeststate = new RequestState();

                        var response = await requeststate.Response(weburi);


                        switch (response.ToLower())
                        {
                            case "alreadyactivated":
                            case "verified":
                                util.SerializeBinFile(ActivationFile, Activator);
                                Process.Start(ApplicationPath);
                                App.Current.Shutdown();
                                break;
                            case "failed":
                                MessageBox.Show(this.GetCurrentWindow(),"Activation code is invalid.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            case "allactivated":
                                MessageBox.Show(this.GetCurrentWindow(), "Number of license already consumed.", "All activated", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(this.GetCurrentWindow(), x.Message, "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
