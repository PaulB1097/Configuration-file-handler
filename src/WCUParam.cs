using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class WCUParam
    {
        public string name { get; set; }
        public int value { get; set; }
        public string unit { get; set; }
        public WCUParam(string Name, int Value, string Unit)
        {
            name = Name;
            value = Value;
            unit = Unit;
        }
        public WCUParam(string Name, int Value)
        {
            name = Name;
            value = Value;
            
        }
    }
}
