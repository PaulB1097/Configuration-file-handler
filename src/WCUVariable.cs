using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class WCUVariable
    {
        public string name { get; private set; }
        public int min_val { get; private set; }
        public int default_val { get; private set; }
        public int max_val { get; private set; }
        public string unit { get; private set; }

        public WCUVariable(string Name, int Min_val, int Default_val, int Max_val, string Unit)
        {
            name = Name;
            min_val = Min_val;
            default_val = Default_val;
            max_val = Max_val;
            unit = Unit;
        }
    }
}
