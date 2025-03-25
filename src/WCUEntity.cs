using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
        class WCUEntity
        {
            public string nameEntity { get; set; }
            public StringBuilder textEntity { get; set; }
            public WCUEntity(string name, StringBuilder continut)
            {
                nameEntity = name;
                textEntity = continut;
            }
            public override string ToString()
            {
                return nameEntity;
            }
        }
    
}
