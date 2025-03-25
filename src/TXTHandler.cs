using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class TXTHandler
    {
        public static List<SystemParam> SParams = new List<SystemParam>(); //Lista cu parametri din fisier System
        public static List<WCUParam> WCUParams = new List<WCUParam>();//Lista cu parametri din fisier WCU
        public static List<WCUEntity> WCUEntitys = new List<WCUEntity>();
        public static bool DuplicateParameters;
        public TXTHandler()
        {
            SParams = new List<SystemParam>();
            WCUParams = new List<WCUParam>();
            WCUEntitys = new List<WCUEntity>();
        }
        public static void ReadSystem(string currentPath)
        {
            using (StreamReader file = new StreamReader(currentPath))
            {

                if (file.EndOfStream)
                    goto emptyfileSystem;
                
                string line;
                string withoutTab = "";
                string withoutNull;
                string finalline;
                                
                line = file.ReadLine();
                if(string.IsNullOrEmpty(line))
                {
                    while (string.IsNullOrEmpty(line))
                    {
                        line = file.ReadLine();
                        int caracter = file.Peek();
                        if (caracter != 13)//verific daca uramatorul caracter este diferit de spatiu fara sa cobor linia
                            break;//daca da ies din bucla
                        line = file.ReadLine();//trec peste spatiul liber dintre antet si parametri

                    }
                    goto readparametersSystem;
                }
                string comment = line.Substring(0, 2);
                if (comment.Equals("//"))
                {
                    while (comment.Equals("//"))
                    {
                        line = file.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;
                        comment = line.Substring(0, 2);
                        
                    }
                    if(string.IsNullOrEmpty(line))
                    {
                        while (string.IsNullOrEmpty(line))
                        {
                            int caracter = file.Peek();
                            if (caracter != 13)//verific daca uramatorul caracter este diferit de spatiu fara sa cobor linia
                                break;//daca da ies din bucla
                            line = file.ReadLine();//trec peste spatiul liber dintre antet si parametri
                        }
                           
                    }

                }
                readparametersSystem:
                
                
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();//Citesc urmatoarea linie(primul parametru)
                                           //if (line == "")//daca linia citita este iarasi goala am ajuns la sfarsitul parametrilor si ies din bucla
                                           //  break;
                    if (string.IsNullOrEmpty(line)) //Daca exista spatii intre paramaetrii sar peste citirea liniei
                    {
                        goto loop;
                    }
                    if (line.Contains("\t") || line.Contains("\0"))//Daca linia contine caracterul null sau caractere tab inlocuiesc cu spatiu pentru ca fac separarea intre cuvinte prin spatii
                    {
                        if ((line.Contains("\t")) && (line.Contains("\0")))
                        {
                            withoutNull = line.Replace("\0", " ");
                            withoutTab = withoutNull.Replace("\t", " ");
                        }
                        else
                        {
                            if (line.Contains("\t"))
                                withoutTab = line.Replace("\t", " ");

                            if (line.Contains("\0"))
                            {
                                withoutNull = line.Replace("\0", " ");//cand rescriu fisierul cu parametrii din lista doar la anumite pozitii, spatiul dintre cuvinte este considerat \0 
                                withoutTab = withoutNull;
                            }
                        }
                    }
                    else
                    {
                        withoutTab = line;
                    }
                    if (withoutTab.Contains("//")) //unele linii contine unitatea de masura de genul "//cm".Eroare, am nevoie de patiu intre cuvinte
                    {
                        finalline = withoutTab.Replace("//", "   //   ");
                    }
                    else
                    {
                        finalline = withoutTab;
                    }
                    string finishline = finalline.Replace("=", "  =    ");
                    char[] separator = { ' ' };
                    string[] words = finishline.Split(separator, StringSplitOptions.RemoveEmptyEntries);//Transform linia intr-un tablou de stringuri cu cuvintele din linie 

                    string Name = words[0];
                    int Value = int.Parse(words[2]);
                    if (words.Length <= 4)
                    {
                        var ob = new SystemParam(Name, Value);// construiesc noul parametru cu campurile name. value, unit
                        SParams.Add(ob);// il aduag listei

                    }
                    else
                    {
                        string Unit = words[4];
                        var ob = new SystemParam(Name, Value, Unit);// construiesc noul parametru cu campurile name. value, unit
                        SParams.Add(ob);// il aduag listei
                    }
                loop:;
                }
            }
            //verific daca exista duplicate
            DuplicateParameters = false;
            for (int i = 0; i < TXTHandler.SParams.Count; i++)
            {
                int j = i + 1;
                while(j < TXTHandler.SParams.Count)
                {
                    if (TXTHandler.SParams[i].name==TXTHandler.SParams[j].name)
                    {
                        DuplicateParameters = true;
                        break;
                    }
                    j++;
                }
            }
        emptyfileSystem:;
        }

        public static void ReadWCU(string currentPath)//Acelasi lucru pentru WCU
        {
            
            using (StreamReader file = new StreamReader(currentPath))
            {
                if (file.EndOfStream)
                    goto emptyfileWCU;

                string line;
                string withoutTab = "";
                string withoutNull;
                string finalline;

                line = file.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    while (string.IsNullOrEmpty(line))
                    {
                        line = file.ReadLine();
                        int caracter = file.Peek();
                        if (caracter != 13)//verific daca uramatorul caracter este diferit de spatiu fara sa cobor linia
                            break;//daca da ies din bucla
                        line = file.ReadLine();//trec peste spatiul liber dintre antet si parametri

                    }
                    goto readparametersWCU;
                }
                string comment = line.Substring(0, 2);
                if (comment.Equals("//"))
                {
                    while (comment.Equals("//"))
                    {
                        line = file.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;
                        comment = line.Substring(0, 2);

                    }
                    if (string.IsNullOrEmpty(line))
                    {
                        while (string.IsNullOrEmpty(line))
                        {
                            int caracter = file.Peek();
                            if (caracter != 13)//verific daca uramatorul caracter este diferit de spatiu fara sa cobor linia
                                break;//daca da ies din bucla
                            line = file.ReadLine();//trec peste spatiul liber dintre antet si parametri
                        }

                    }

                }
            readparametersWCU:

                line = file.ReadLine();
                do
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        goto loop;
                    }
                    if (line.Contains("\t") || line.Contains("\0"))//Sparii
                    {
                        if ((line.Contains("\t")) && (line.Contains("\0")))
                        {
                            withoutNull = line.Replace("\0", " ");
                            withoutTab = withoutNull.Replace("\t", " ");
                        }
                        else
                        {
                            if (line.Contains("\t"))
                                withoutTab = line.Replace("\t", " ");

                            if (line.Contains("\0"))
                            {
                                withoutNull = line.Replace("\0", " ");
                                withoutTab = withoutNull;
                            }
                        }
                    }
                    else
                    {
                        withoutTab = line;
                    }
                    if (withoutTab.Contains("//"))
                    {
                        finalline = withoutTab.Replace("//", "   //   ");
                    }
                    else
                    {
                        finalline = withoutTab;
                    }
                    string finishline = finalline.Replace("=", "  =    ");
                    char[] separator = { ' ' };
                    string[] words = finishline.Split(separator, StringSplitOptions.RemoveEmptyEntries);//Separ linia intr-un tablou de string-uri care contine fiecare cuvant intr-un element

                    string Name = words[0];
                    int Value = int.Parse(words[2]);

                    if (words.Length <= 4)
                    {
                        var ob = new WCUParam(Name, Value);// construiesc noul parametru cu campurile name. value, unit
                        WCUParams.Add(ob);// il aduag listei

                    }
                    else
                    {
                        string Unit = words[4];
                        var ob = new WCUParam(Name, Value, Unit);// construiesc noul parametru cu campurile name. value, unit
                        WCUParams.Add(ob);// il aduag listei
                    }
                loop:;
                    int caracter = file.Peek();
                    if (caracter == 91)
                        break;
                    line = file.ReadLine();
                } while ((!file.EndOfStream));
            }

            DuplicateParameters = false;
            for (int i = 0; i < TXTHandler.WCUParams.Count; i++)
            {
                int j = i + 1;
                while (j < TXTHandler.WCUParams.Count)
                {
                    if (TXTHandler.WCUParams[i].name == TXTHandler.WCUParams[j].name)
                    {
                        DuplicateParameters = true;
                        break;
                    }
                    j++;
                }
            }

          emptyfileWCU:;
        }
        public static void ReadWCUEntitys(string currentPath)
        {
            using (StreamReader file = new StreamReader(currentPath))
            {
                string name;
                string line = null;
                do
                {
                    int caracter = file.Peek();
                    if (caracter == 91)
                        break;
                    line = file.ReadLine();
                }
                while (!file.EndOfStream);

                while (!file.EndOfStream)
                {
                    StringBuilder textEntity = new StringBuilder();
                    line = file.ReadLine();
                    name = line.Substring(1, line.Length - 2);
                    while (91 != file.Peek())
                    {
                        line = file.ReadLine();
                        textEntity.AppendLine(line);
                        if (file.EndOfStream)
                            break;
                    }
                    WCUEntitys.Add(new WCUEntity(name, textEntity));
                }
            }
        }

        public static void WriteSystem(string currentPath)
        {


            using (FileStream file = File.Create(currentPath))//Deschid fisierul pentru scriere
            {
                file.Seek(0, SeekOrigin.Begin);//Pozitionez cursorul la inceputul primei linii
                string linia1 = "// confdata_system.txt";//afisez numele fisieruk
                byte[] blinia1 = new UTF8Encoding(true).GetBytes(linia1);
                file.Write(blinia1, 0, blinia1.Length);

                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);//linie noua
                file.Write(newline, 0, newline.Length);

                string linia2 = "// Last modified on ";//afisez mesajul
                byte[] blinia2 = new UTF8Encoding(true).GetBytes(linia2);
                file.Write(blinia2, 0, blinia2.Length);

                DateTime d = DateTime.Now;//afisez data
                string data = d.ToString();
                byte[] bdata = new UTF8Encoding(true).GetBytes(data);
                file.Write(bdata, 0, bdata.Length);

                file.Write(newline, 0, newline.Length);//Doua linii spatiu
                file.Write(newline, 0, newline.Length);

                foreach (SystemParam parameter in TXTHandler.SParams)
                {
                    file.Seek(0, SeekOrigin.Current); //afisez numele parametrului
                    char[] charName = parameter.name.ToCharArray();
                    byte[] byName = new byte[charName.Length];

                    Encoder e = Encoding.UTF8.GetEncoder();
                    e.GetBytes(charName, 0, charName.Length, byName, 0, true);
                    file.Write(byName, 0, byName.Length);

                    file.Seek(50 - byName.Length, SeekOrigin.Current);//afisez egal la pozitia 50
                    file.WriteByte(61);

                    file.Seek(2, SeekOrigin.Current);// afisez egal dupa dupa spatii
                    string value = parameter.value.ToString();
                    char[] charValue = value.ToCharArray();
                    byte[] byValue = new byte[charValue.Length];

                    e.GetBytes(charValue, 0, charValue.Length, byValue, 0, true);
                    file.Write(byValue, 0, byValue.Length);

                    if (parameter.unit != null)
                    {
                        file.Seek(15 - byValue.Length, SeekOrigin.Current);
                        file.WriteByte(47);
                        file.WriteByte(47);

                        file.Seek(2, SeekOrigin.Current);// afisez unitatea de masura cu doua pozitii mai departe de slash
                        char[] charUnit = parameter.unit.ToCharArray();
                        byte[] byUnit = new byte[charUnit.Length];

                        e.GetBytes(charUnit, 0, charUnit.Length, byUnit, 0, true);
                        file.Write(byUnit, 0, byUnit.Length);
                    }



                    file.Write(newline, 0, newline.Length);

                }



            }
           
        }
        public static void WriteWCU(string currentPath)
        {
            using (FileStream file = File.Create(currentPath))//Deschid fisierul pentru scriere
            {
                file.Seek(0, SeekOrigin.Begin);//Pozitionez cursorul la inceputul primei linii
                string linia1 = "// confdata_WCU.txt";//afisez numele
                byte[] blinia1 = new UTF8Encoding(true).GetBytes(linia1);
                file.Write(blinia1, 0, blinia1.Length);

                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);//linie noua
                file.Write(newline, 0, newline.Length);

                string linia2 = "// Last modified on ";
                byte[] blinia2 = new UTF8Encoding(true).GetBytes(linia2);
                file.Write(blinia2, 0, blinia2.Length);

                DateTime d = DateTime.Now;
                string data = d.ToString();
                byte[] bdata = new UTF8Encoding(true).GetBytes(data);
                file.Write(bdata, 0, bdata.Length);

                file.Write(newline, 0, newline.Length);
                file.Write(newline, 0, newline.Length);
                
                foreach (WCUParam parameter in TXTHandler.WCUParams)
                {
                    file.Seek(0, SeekOrigin.Current);
                    char[] charName = parameter.name.ToCharArray();
                    byte[] byName = new byte[charName.Length];

                    Encoder e = Encoding.UTF8.GetEncoder();
                    e.GetBytes(charName, 0, charName.Length, byName, 0, true);
                    file.Write(byName, 0, byName.Length);

                    file.Seek(50 - byName.Length, SeekOrigin.Current);
                    file.WriteByte(61);

                    file.Seek(2, SeekOrigin.Current);
                    string value = parameter.value.ToString();
                    char[] charValue = value.ToCharArray();
                    byte[] byValue = new byte[charValue.Length];

                    e.GetBytes(charValue, 0, charValue.Length, byValue, 0, true);
                    file.Write(byValue, 0, byValue.Length);

                   

                    if (parameter.unit != null)
                    {
                        file.Seek(15 - byValue.Length, SeekOrigin.Current);
                        file.WriteByte(47);
                        file.WriteByte(47);

                        file.Seek(2, SeekOrigin.Current);
                        char[] charUnit = parameter.unit.ToCharArray();
                        byte[] byUnit = new byte[charUnit.Length];

                        e.GetBytes(charUnit, 0, charUnit.Length, byUnit, 0, true);
                        file.Write(byUnit, 0, byUnit.Length);
                    }
                    
                    file.Write(newline, 0, newline.Length);

                }
                file.Write(newline, 0, newline.Length);
                file.Write(newline, 0, newline.Length);
                foreach (WCUEntity parameter in TXTHandler.WCUEntitys)
                {
                    byte[] pstanga = new UTF8Encoding(true).GetBytes("[");
                    file.Write(pstanga, 0, pstanga.Length);

                    string NameEntity = parameter.nameEntity;//afisez numele
                    byte[] bNameEntity = new UTF8Encoding(true).GetBytes(NameEntity);
                    file.Write(bNameEntity, 0, bNameEntity.Length);

                    byte[] pdreapta = new UTF8Encoding(true).GetBytes("]");
                    file.Write(pdreapta, 0, pstanga.Length);

                    file.Write(newline, 0, newline.Length);

                    string TextEntity = Convert.ToString(parameter.textEntity);//afisez numele
                    byte[] bTextEntity = new UTF8Encoding(true).GetBytes(TextEntity);
                    file.Write(bTextEntity, 0, bTextEntity.Length);

                    //file.Write(newline, 0, newline.Length);

                }

               
            }



        }
    }
}

