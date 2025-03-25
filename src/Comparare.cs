using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    static class Comparare
    {
        internal static bool checkValues { get; set; }

        public static void EliminareDuplicateSystem()
        {
            char tab = Convert.ToChar(9);
            using (StreamWriter file = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
            {
                
                for (int i = 0; i < TXTHandler.SParams.Count; i++)
                {
                    bool ok = false;
                    int j = i + 1;
                    int count = 1;
                    while (j < TXTHandler.SParams.Count)
                    {
                        
                        if (TXTHandler.SParams[i].name == TXTHandler.SParams[j].name)
                        {
                            file.WriteLine("{0}Duplicat {1}: {2} = {3} // {4} ",tab, count, TXTHandler.SParams[j].name, TXTHandler.SParams[j].value, TXTHandler.SParams[j].unit);
                            TXTHandler.SParams.RemoveAt(j);
                            j--;
                            count++;
                            ok = true;
                        }
                        j++;
 
                    }
                    if (ok) file.WriteLine();
                }
                
            }
        }
        public static void ReordonareSystem()
        {
            for (int j = 0; j < XMLHandler.SVariables.Count; j++)//Parcurg listele cu acelasi indice si verific ordinea
            {
                if (j >= TXTHandler.SParams.Count)//Daca nr parametrilor din lista fisier(cu parametri din fisier) este depasita  => lista template contine parametri noi si ii inserez in lista fisier
                {
                    TXTHandler.SParams.Insert(j, new SystemParam(XMLHandler.SVariables[j].name, XMLHandler.SVariables[j].default_val, XMLHandler.SVariables[j].unit));
                }
                else
                {
                    if (TXTHandler.SParams[j].name != XMLHandler.SVariables[j].name)//Daca parametri nu corespund => *
                    {
                        bool exista = false;
                        int pozitie = 0;
                        int i = j + 1;
                        while (i < TXTHandler.SParams.Count) // * =>caut in continoare listei fisier daca exista parametrul
                        {
                            if (XMLHandler.SVariables[j].name.Equals(TXTHandler.SParams[i].name))// daca exista retin pozitia
                            {
                                exista = true;
                                pozitie = i;
                                break;
                            }
                            i++;
                        }
                        if (exista) // daca exista interschimb valorile
                        {
                            string nameaux = TXTHandler.SParams[j].name;
                            int valueaux = TXTHandler.SParams[j].value;
                            string unitaux = TXTHandler.SParams[j].unit;
                            TXTHandler.SParams[j].name = TXTHandler.SParams[pozitie].name;
                            TXTHandler.SParams[j].value = TXTHandler.SParams[pozitie].value;
                            TXTHandler.SParams[j].unit = TXTHandler.SParams[pozitie].unit;
                            TXTHandler.SParams[pozitie].name = nameaux;
                            TXTHandler.SParams[pozitie].value = valueaux;
                            TXTHandler.SParams[pozitie].unit = unitaux;

                        }
                        else // daca nu exista il inserez in lista fisier pe cel din template
                        {
                            TXTHandler.SParams.Insert(j, new SystemParam(XMLHandler.SVariables[j].name, XMLHandler.SVariables[j].default_val, XMLHandler.SVariables[j].unit));
                        }


                    }
                }

            }
        }
        public static void CheckValuesSystem()
        {
            bool ok = false;
            for (int j = 0; j < XMLHandler.SVariables.Count; j++)//numarul parametrilor din lista acum sunt egali cu cei din template
            {
                if ((TXTHandler.SParams[j].value < XMLHandler.SVariables[j].min_val) || (TXTHandler.SParams[j].value > XMLHandler.SVariables[j].max_val))
                {
                    ok = true;
                    break;
                }
            }
            checkValues = ok;

        }
        public static void rectificationValuesSystem()
        {
            char tab = Convert.ToChar(9);
            using (StreamWriter file = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
            {
                for (int j = 0; j < XMLHandler.SVariables.Count; j++)//numarul parametrilor din lista acum sunt egali cu cei din template
                {
                    if ((TXTHandler.SParams[j].value < XMLHandler.SVariables[j].min_val) || (TXTHandler.SParams[j].value > XMLHandler.SVariables[j].max_val))
                    {
                        file.WriteLine("{0}Name parameter: {1}, line: {2}",tab, TXTHandler.SParams[j].name, j + 4);//linia am calculat-o ca fiind ordinea in lista + primele 4 randuri
                        file.WriteLine("{0}[ Min_Value: {1}    Current_Value:{2}    Max_Value: {3} ]", tab,XMLHandler.SVariables[j].min_val, TXTHandler.SParams[j].value, XMLHandler.SVariables[j].max_val);
                        file.WriteLine();

                    }
                }
            }
        }
        public static void EliminareDuplicateWCU()
        {
            char tab = Convert.ToChar(9);
            using (StreamWriter file = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
            {
                for (int i = 0; i < TXTHandler.WCUParams.Count; i++)
                {
                    bool ok = false;
                    int j = i + 1;
                    int count = 1;
                    while (j < TXTHandler.WCUParams.Count)
                    {

                        if (TXTHandler.WCUParams[i].name == TXTHandler.WCUParams[j].name)
                        {
                            file.WriteLine("{0}Duplicat {1}: {2} = {3} // {4} ",tab, count, TXTHandler.WCUParams[j].name, TXTHandler.WCUParams[j].value, TXTHandler.WCUParams[j].unit);
                            TXTHandler.WCUParams.RemoveAt(j);
                            j--;
                            count++;
                            ok = true;
                        }
                        j++;

                    }
                    if (ok) file.WriteLine();
                }

            }
        }
        
        public static void ReordonareWCU()
        {
            for (int j = 0; j < XMLHandler.WCUVariables.Count; j++)//Parcurg listele cu acelasi indice si verific ordinea
            {
                if (j >= TXTHandler.WCUParams.Count)//Daca nr parametrilor din lista fisier(cu parametri din fisier) este depasita  => lista template contine parametri noi si ii inserez in lista fisier
                {
                    TXTHandler.WCUParams.Insert(j, new WCUParam(XMLHandler.WCUVariables[j].name, XMLHandler.WCUVariables[j].default_val, XMLHandler.WCUVariables[j].unit));
                }
                else
                {
                    if (TXTHandler.WCUParams[j].name != XMLHandler.WCUVariables[j].name)//Daca parametri nu corespund => *
                    {
                        bool exista = false;
                        int pozitie = 0;
                        int i = j + 1;
                        while (i < TXTHandler.WCUParams.Count) // * =>caut in continoare listei fisier daca exista parametrul
                        {
                            if (XMLHandler.WCUVariables[j].name.Equals(TXTHandler.WCUParams[i].name))// daca exista retin pozitia
                            {
                                exista = true;
                                pozitie = i;
                                break;
                            }
                            i++;
                        }
                        if (exista) // daca exista interschimb valorile
                        {
                            string nameaux = TXTHandler.WCUParams[j].name;
                            int valueaux = TXTHandler.WCUParams[j].value;
                            string unitaux = TXTHandler.WCUParams[j].unit;
                            TXTHandler.WCUParams[j].name = TXTHandler.WCUParams[pozitie].name;
                            TXTHandler.WCUParams[j].value = TXTHandler.WCUParams[pozitie].value;
                            TXTHandler.WCUParams[j].unit = TXTHandler.WCUParams[pozitie].unit;
                            TXTHandler.WCUParams[pozitie].name = nameaux;
                            TXTHandler.WCUParams[pozitie].value = valueaux;
                            TXTHandler.WCUParams[pozitie].unit = unitaux;

                        }
                        else // daca nu exista il inserez in lista fisier pe cel din template
                        {
                            TXTHandler.WCUParams.Insert(j, new WCUParam(XMLHandler.WCUVariables[j].name, XMLHandler.WCUVariables[j].default_val, XMLHandler.WCUVariables[j].unit));
                        }


                    }
                }

            }
        }

        public static void CheckValuesWCU()
        {
            bool ok = false;
            for (int j = 0; j < XMLHandler.WCUVariables.Count; j++)//numarul parametrilor din lista acum sunt egali cu cei din template
            {
                if ((TXTHandler.WCUParams[j].value < XMLHandler.WCUVariables[j].min_val) || (TXTHandler.WCUParams[j].value > XMLHandler.WCUVariables[j].max_val))
                {
                    ok = true;
                    break;
                }
            }
            checkValues = ok;

        }

        public static void rectificationValuesWCU()
        {
            char tab = Convert.ToChar(9);
            using (StreamWriter file = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
            {
                for (int j = 0; j < XMLHandler.WCUVariables.Count; j++)//numarul parametrilor din lista acum sunt egali cu cei din template
                {
                    if ((TXTHandler.WCUParams[j].value < XMLHandler.WCUVariables[j].min_val) || (TXTHandler.WCUParams[j].value > XMLHandler.WCUVariables[j].max_val))
                    {
                        file.WriteLine("{0}Name parameter: {1}, line: {2}",tab, TXTHandler.WCUParams[j].name, j + 4);//linia am calculat-o ca fiind ordinea in lista + primele 4 randuri
                        file.WriteLine("{0}[ Min_Value: {1}    Current_Value:{2}    Max_Value: {3} ]",tab, XMLHandler.WCUVariables[j].min_val, TXTHandler.WCUParams[j].value, XMLHandler.WCUVariables[j].max_val);
                        file.WriteLine();

                    }
                }
            }
        }
       
    }
}