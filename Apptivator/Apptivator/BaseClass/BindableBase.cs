using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Apptivator.BaseClass
{
    public class BindableBase : INotifyPropertyChanged
    {
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;
            member = val;
            OnPropertyChanged(propertyName);
        }


        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void Close()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }

        public System.Windows.Window GetCurrentWindow()
        {
            System.Windows.Window currentWindow = null;
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    currentWindow = window;
                }
            }
            return currentWindow;
        }
    }
}
