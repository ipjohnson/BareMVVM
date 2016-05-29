using BareMVVM.Example.DataController;
using BareMVVM.Example.ViewModel;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using Xunit;

namespace BareMVVM.Example.Tests
{
    public class BasicAppTests
    {
        [Theory]
        [ApplicationData("BareMVVM.Example.exe")]
        public void BasicExample_CheckTextBlock(Application application)
        {
            var window = new WindowFixture(application.GetWindow("MainWindow"));

            window.GetText("TextBlock").Should().Be(DataService.BlahText);
        }

        [Theory]
        [ApplicationData("BareMVVM.Example.exe")]
        public void BasicExample_ClickButton_CheckText(Application application)
        {
            var window = new WindowFixture(application.GetWindow("MainWindow"));

            window.Click("ClickButton");

            window.GetText("ClickTextBlock").Should().Be(MainWindowViewModel.ClickString);
        }
    }
}
