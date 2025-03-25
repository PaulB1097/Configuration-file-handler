using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileFinder
{
    class XMLHandler
    {
        public static List<SystemVariable> SVariables = new List<SystemVariable>();
        public static List<WCUVariable> WCUVariables = new List<WCUVariable>();
        public XMLHandler()
        {
            SVariables = new List<SystemVariable>();
            WCUVariables = new List<WCUVariable>();
        }

        public static void ReadSystem()

        {
            XmlDocument doc = new XmlDocument();
            doc.Load("SystemTemplate.xml");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string Name = node["name"].InnerText;
                int Min_val = int.Parse(node["minvalue"].InnerText);
                int Default_val = int.Parse(node["defaultvalue"].InnerText);
                int Max_val = int.Parse(node["maxvalue"].InnerText);
                string Unit = node["unit"].InnerText;
                SystemVariable ob = new SystemVariable(Name, Min_val, Default_val, Max_val, Unit);
                SVariables.Add(ob);
            }
        }
        public static void ReadWCU()

        {
            XmlDocument doc = new XmlDocument();
            doc.Load("WCUTemplate.xml");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string Name = node["name"].InnerText;
                int Min_val = int.Parse(node["minvalue"].InnerText);
                int Default_val = int.Parse(node["defaultvalue"].InnerText);
                int Max_val = int.Parse(node["maxvalue"].InnerText);
                string Unit = node["unit"].InnerText;
                WCUVariable ob = new WCUVariable(Name, Min_val, Default_val, Max_val, Unit);
                WCUVariables.Add(ob);
            }
        }
    }
}
