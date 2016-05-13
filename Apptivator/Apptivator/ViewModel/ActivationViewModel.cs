using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Apptivator.BaseClass;


namespace Apptivator.ViewModel
{
    class ActivationViewModel : BindableBase
    {

        private string _macAddress;

        public string MacAddress
        {
            get { return _macAddress; }
            set { SetProperty(ref _macAddress, value); }

        }

        

        #region ICommands

        public ICommand ActivateCode
        {
            get
            {
                return new DelegateCommand(() => 
                {
                    MacAddress = GetMac();
                });
            }
        }

        private string GetMac()
        {

            var networks = NetworkInterface.GetAllNetworkInterfaces();
            var activeNetworks = networks.Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
            var nonVirtual = activeNetworks.Where(s => !s.Description.Contains("Virtual") && !s.Name.Contains("vEthernet"));
            return nonVirtual.First().GetPhysicalAddress().ToString();
                //string.Join(":",nonVirtual.First().GetPhysicalAddress().GetAddressBytes().Select(s => s.ToString("X2").ToArray())).ToString();
        }


        #endregion
    }
    
}
