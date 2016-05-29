using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace BareMVVM.Example.Tests
{
    public class WindowFixture
    {
        private Window _window;

        public WindowFixture(Window window)
        {
            _window = window;
        }

        public WindowFixture Click(string id)
        {
            var item2 = _window.Get<Button>(id);

            try
            {
                var item = _window.Get<UIItem>(id);

                item.Click();
            }
            catch(Exception exp)
            {
                exp.ToString();
            }

            _window.WaitWhileBusy();

            return this;
        }    

        public string GetText(string id)
        {
            var item = _window.Get<UIItem>(id);

            var element = _window.GetElement(SearchCriteria.ByAutomationId(id));

            return item.Name;
        }
    }
}
