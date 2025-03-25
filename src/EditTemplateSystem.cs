using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
namespace FileFinder
{
    public partial class EditTemplateSystem : Form
    {
        public string mytemplate { get; set; }
        public int index { get; set; }
        public string sindex { get; set; }
        public int itemactivate { get; set; }
        public EditTemplateSystem(string MYTEMPLATE)//Aici am observat cu adevarat cat de importanta este programarea orientata pe obiect(Initial am creat fereastra doar pentru editarea TemplateSystem);!!!!!!!!
        {
            InitializeComponent();
            mytemplate = MYTEMPLATE;
            LoadParameters();
            this.Width = 1125;
            this.Height = 500;
        }
        private void LoadParameters()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(mytemplate);
            index = 0;
            foreach (XmlNode node in doc.DocumentElement)
            {

                string sindex = index.ToString();
                string name = node["name"].InnerText;
                string minvalue = node["minvalue"].InnerText;
                string defaultvalue = node["defaultvalue"].InnerText;
                string maxvalue = node["maxvalue"].InnerText;
                string unit = node["unit"].InnerText;
                sindex = index.ToString();
                String[] row = { sindex, name, minvalue, defaultvalue, maxvalue, unit };
                ListViewItem item = new ListViewItem(row);
                listV.Items.Add(item);
                index++;
            }


        }
        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "\t",
            NewLineOnAttributes = true
        };
        private void WriteXml()
        {

           
            XmlWriter xmlWriter = XmlWriter.Create( mytemplate, xmlWriterSettings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("root");
            for (int i = 0; i < listV.Items.Count; i++)
            {
                xmlWriter.WriteStartElement(listV.Items[i].SubItems[1].Text);

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(listV.Items[i].SubItems[1].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("minvalue");
                xmlWriter.WriteString(listV.Items[i].SubItems[2].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("defaultvalue");
                xmlWriter.WriteString(listV.Items[i].SubItems[3].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("maxvalue");
                xmlWriter.WriteString(listV.Items[i].SubItems[4].Text);
                xmlWriter.WriteEndElement();


                xmlWriter.WriteStartElement("unit");
                xmlWriter.WriteString(listV.Items[i].SubItems[5].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

        }

        private void add(string sindex, string name, string minvalue, string defaultvalue, string maxvalue, string unit)
        {

            String[] row = { sindex, name, minvalue, defaultvalue, maxvalue, unit };
            ListViewItem item = new ListViewItem(row);
            listV.Items.Add(item);
            txtName.Clear();
            txtDefaultValue.Clear();
            txtMaxValue.Clear();
            txtMinValue.Clear();
            txtUnit.Clear();
            txtName.Focus();


        }
        private void insert(string sindex, string name, string minvalue, string defaultvalue, string maxvalue, string unit)
        {

            String[] row = { sindex, name, minvalue, defaultvalue, maxvalue, unit };
            ListViewItem item = new ListViewItem(row);
            int x = int.Parse(sindex);
            listV.Items.Insert(x, item);
            for (int i = x; i < listV.Items.Count; i++)
            {

                listV.Items[i].SubItems[0].Text = i.ToString();
            }
            txtName.Clear();
            txtDefaultValue.Clear();
            txtMaxValue.Clear();
            txtMinValue.Clear();
            txtUnit.Clear();
            txtIndex.Clear();
            txtName.Focus();
        }

        private void cleanfields(string name, string minvalue, string defaultvalue, string maxvalue, string unit)
        {
          
            if (name.Contains(" "))
            {
                 txtName.Text=name.Replace(" ", "\0");
            }
            if (minvalue.Contains(" "))
            {
                txtMinValue.Text=minvalue.Replace(" ", "\0");
            }
            if (maxvalue.Contains(" "))
            {
                txtMaxValue.Text=maxvalue.Replace(" ", "\0");
            }
            if (defaultvalue.Contains(" "))
            {
                txtDefaultValue.Text = defaultvalue.Replace(" ", "\0");
            }
            if (unit.Contains(" "))
            {
                txtUnit.Text = unit.Replace(" ", "\0");
            }
           
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            index = listV.Items.Count;
            index = index++;
            sindex = index.ToString();
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDefaultValue.Text) || string.IsNullOrEmpty(txtUnit.Text) || string.IsNullOrEmpty(txtMinValue.Text) || string.IsNullOrEmpty(txtMaxValue.Text))
            {
                MessageBox.Show("Empty field", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int checkisnumber;
            if ((!int.TryParse(txtMaxValue.Text, out checkisnumber)) || !int.TryParse(txtMinValue.Text, out checkisnumber) || !int.TryParse(txtDefaultValue.Text, out checkisnumber))
            {
                MessageBox.Show("Values are not numbers", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            cleanfields(txtName.Text, txtMinValue.Text, txtDefaultValue.Text, txtMaxValue.Text, txtUnit.Text);
            add(sindex, txtName.Text, txtMinValue.Text, txtDefaultValue.Text, txtMaxValue.Text, txtUnit.Text);


        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDefaultValue.Text) || string.IsNullOrEmpty(txtUnit.Text) || string.IsNullOrEmpty(txtMinValue.Text) || string.IsNullOrEmpty(txtMaxValue.Text))
            {
                MessageBox.Show("Empty field", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtIndex.Text))
            {
                MessageBox.Show("The Index is not specified", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int checkisnumber;
            if ((!int.TryParse(txtIndex.Text, out checkisnumber)) || (!int.TryParse(txtMaxValue.Text, out checkisnumber)) || !int.TryParse(txtMinValue.Text, out checkisnumber) || !int.TryParse(txtDefaultValue.Text, out checkisnumber))
            {
                MessageBox.Show("Values are not numbers", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if((int.TryParse(txtIndex.Text, out checkisnumber)))
            {
                if(checkisnumber>listV.Items.Count)
                {
                    MessageBox.Show("Index out of range", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            cleanfields(txtName.Text, txtMinValue.Text, txtDefaultValue.Text, txtMaxValue.Text, txtUnit.Text);
            insert(txtIndex.Text, txtName.Text, txtMinValue.Text, txtDefaultValue.Text, txtMaxValue.Text, txtUnit.Text);

        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (listV.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you want to remove?", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    
                    for (int i = 0; i < listV.SelectedItems.Count; i++)
                    {
                        listV.Items.RemoveAt(listV.SelectedItems[i].Index);
                        i--;
                    }

                }
                for (int i = 0; i < listV.Items.Count; i++)
                {

                    listV.Items[i].SubItems[0].Text = i.ToString();
                }
            }
            else
            {
                MessageBox.Show("Item is not selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ListV_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listV.DoDragDrop(listV.SelectedItems, DragDropEffects.Move);
        }

        private void ListV_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ListV_DragDrop(object sender, DragEventArgs e)
        {

            if (listV.SelectedItems.Count == 0) { return; }

            Point Pt = listV.PointToClient(new Point(e.X, e.Y));
            ListViewItem ItemDrag = listV.GetItemAt(Pt.X, Pt.Y);

            if (ItemDrag == null) { return; }

            int ItemDragIndex = ItemDrag.Index;
            ListViewItem[] Sel = new ListViewItem[listV.SelectedItems.Count];

            for (int i = 0; i < listV.SelectedItems.Count; i++)
            {
                Sel[i] = listV.SelectedItems[i];
            }
            for (int i = 0; i < Sel.GetLength(0); i++)
            {
                ListViewItem Item = Sel[i];
                int ItemIndex = ItemDragIndex;

                if (ItemIndex == Item.Index) { return; }

                if (Item.Index < ItemIndex)
                {
                    ItemIndex++;
                }
                else
                {
                    ItemIndex = ItemDragIndex + i;
                }
                ListViewItem InsertItem = (ListViewItem)Item.Clone();
                listV.Items.Insert(ItemIndex, InsertItem);
                listV.Items.Remove(Item);

            }
            for (int i = 0; i < listV.Items.Count; i++)
            {

                listV.Items[i].SubItems[0].Text = i.ToString();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this template?", "SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                WriteXml();
                base.Close();
            }

        }

        private void ListV_ItemActivate(object sender, EventArgs e)
        {
            txtName.Text = listV.SelectedItems[0].SubItems[1].Text;
            txtMinValue.Text = listV.SelectedItems[0].SubItems[2].Text;
            txtDefaultValue.Text = listV.SelectedItems[0].SubItems[3].Text;
            txtMaxValue.Text = listV.SelectedItems[0].SubItems[4].Text;
            txtUnit.Text = listV.SelectedItems[0].SubItems[5].Text;
            itemactivate = listV.SelectedItems[0].Index;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            cleanfields(txtName.Text, txtMinValue.Text, txtDefaultValue.Text, txtMaxValue.Text, txtUnit.Text);
            if (listV.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listV.SelectedItems.Count; i++)
                {
                    if (!string.IsNullOrEmpty(txtMinValue.Text))
                        listV.Items[listV.SelectedItems[i].Index].SubItems[2].Text = txtMinValue.Text;
                    if (!string.IsNullOrEmpty(txtDefaultValue.Text))
                        listV.Items[listV.SelectedItems[i].Index].SubItems[3].Text = txtDefaultValue.Text;
                    if (!string.IsNullOrEmpty(txtMaxValue.Text))
                        listV.Items[listV.SelectedItems[i].Index].SubItems[4].Text = txtMaxValue.Text;
                    if (!string.IsNullOrEmpty(txtMaxValue.Text))
                        listV.Items[listV.SelectedItems[i].Index].SubItems[5].Text = txtUnit.Text;
                }

            }
            else
            {
                listV.Items[itemactivate].SubItems[1].Text = txtName.Text;
                listV.Items[itemactivate].SubItems[2].Text = txtMinValue.Text;
                listV.Items[itemactivate].SubItems[3].Text = txtDefaultValue.Text;
                listV.Items[itemactivate].SubItems[4].Text = txtMaxValue.Text;
                listV.Items[itemactivate].SubItems[5].Text = txtUnit.Text;
            }
                txtName.Clear();
                txtDefaultValue.Clear();
                txtMaxValue.Clear();
                txtMinValue.Clear();
                txtUnit.Clear();
                txtName.Focus();
            
            
        }

        private void ListV_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Control.ModifierKeys == Keys.Control)
            {

            }
            else
            {

                txtName.Clear();
                txtDefaultValue.Clear();
                txtMaxValue.Clear();
                txtMinValue.Clear();
                txtUnit.Clear();
                txtName.Focus();
            }
        }
        
       
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void BtnRemoveAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really remove all items??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                listV.Items.Clear();
            }
        }

       
    }
}