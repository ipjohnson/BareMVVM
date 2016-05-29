using BareMVVM.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.Example.DataController
{
    public interface IDataService
    {
        string Blah { get; }
    }

    public class DataService : IDataService
    {
        public const string BlahText = "Blah Blah hello 4";

        public string Blah
        {
            get
            {
                return BlahText;
            }
        }
    }

    [DesignTimeOnly]
    public class DesignTimeDataService : IDataService
    {
        public string Blah
        {
            get
            {
                return "Design time baby";
            }
        }
    }
}
