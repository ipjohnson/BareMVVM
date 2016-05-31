using BareMVVM.Example.Messages;
using BareMVVM.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.Example.ViewModel
{
    public class SecondWindowViewModel
    {
        private IDispatchedMessenger _dispatchedMessenger;

        public SecondWindowViewModel(IDispatchedMessenger dispatchedMessenger)
        {
            _dispatchedMessenger = dispatchedMessenger;
        }

        public string DataString { get; set; }

        public void SendMessage()
        {
            _dispatchedMessenger.Send(new TestDataMessage { Data = DataString });
        }
    }
}
