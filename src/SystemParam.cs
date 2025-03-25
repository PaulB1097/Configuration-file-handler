using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class SystemParam
    {
        public string name { get; set; }
        public int value { get; set; }
        public string unit { get; set; }
        public SystemParam(string Name, int Value, string Unit)
        {
            name = Name;
            value = Value;
            unit = Unit;
        }
        public SystemParam(string Name, int Value)
        {
            name = Name;
            value = Value;
        }

    }
}
