using BareMVVM.Example.DataController;
using BareMVVM.Example.Messages;
using BareMVVM.Example.Views;
using BareMVVM.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.Example.ViewModel
{
    public class MainWindowViewModel : Grace.Data.NotifyObject
    {
        private IDataService _dataService;

        public MainWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;
            AnotherString = "Hello World";
        }

        public string TestString { get { return _dataService.Blah; } }

        private string _anotherString;

        public string AnotherString
        {
            get { return _anotherString; }
            set { SetProperty(ref _anotherString, value); }
        }

        public const string ClickString = "I've been clicked";

        public void ClickHandler()
        {
            AnotherString = ClickString;
        }

        public void OpenNewSecondWindow()
        {
            var secondWindow = new SecondWindow();

            secondWindow.Show();
        }

        [MessageHandler]
        public void MessageHandler(TestDataMessage message)
        {
            AnotherString = message.Data;
        }
    }
}
