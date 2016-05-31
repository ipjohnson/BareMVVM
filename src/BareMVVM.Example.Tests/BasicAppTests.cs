using BareMVVM.Example.DataController;
using BareMVVM.Example.ViewModel;
using FluentAssertions;
using SimpleWhiteFixture;
using SimpleWhiteFixture.xUnit;
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
        [UITheory]
        [Application("BareMVVM.Example.exe")]
        public void BasicExample_CheckTextBlock(IWindowFixture i)
        {
            i.Get.Text.From("TextBlock").Should().Be(DataService.BlahText);
        }

        [UITheory]
        [Application("BareMVVM.Example.exe")]
        public void BasicExample_ClickButton_CheckText(IWindowFixture i)
        {
            i.Click("ClickButton");

            i.Get.Text.From("ClickTextBlock").Should().Be(MainWindowViewModel.ClickString);
        }

        [UITheory]
        [Application("BareMVVM.Example.exe")]
        public void BasicExample_MediatorSendMessage_Receive(IWindowFixture i)
        {
            var newWindow = i.Click("SecondWindowButton").NewWindow("SecondWindow");

            var testString = "Some Test Value";

            newWindow.Fill("LeftTextBox").With(testString);

            newWindow.Click("SecondButton");
            
            i.Get.Text.From("ClickTextBlock").Should().Be(testString);
        }
    }
}
