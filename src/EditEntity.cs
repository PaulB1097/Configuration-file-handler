using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace FileFinder
{
    public partial class EditEntity : Form
    {
        string currentPath;
        string textWcuSpecificData = "WcuAtpUnitId     	=	0\nChannelId        	=	0\n@\n";
        string textPSDTables = "// --- 1 ---\nTraintype        	=	0\nPlatformtype     	=	0\nStoppingPosition 	=	0\nPsdc            	=	0, 0, 0, 0, //psdc#1..4\n@\n";
        string textAdjacentWCU = "WcuAtpUnitId     	=	0\nChannelId        	=	0\n@\n";
        string textTtsListOfTrains = "TrainGroup =      0, 0\n@\n";
        public EditEntity(string currentPath)
        {
            InitializeComponent();
            this.currentPath = currentPath;
            loadEntity();
        }
        public void loadEntity()
        {
            lblcurrentPath.Text = currentPath;
            foreach (WCUEntity entity in TXTHandler.WCUEntitys)
            {
                string text =Convert.ToString(entity.textEntity);
                String[] row= { entity.nameEntity, text } ;
                ListViewItem item = new ListViewItem(row);
                listEntity.Items.Add(item);
            }
           
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            TXTHandler.WCUEntitys.Clear();
            for(int i=0; i<listEntity.Items.Count;i++)
            {
                StringBuilder text = new StringBuilder();
                text.Append(listEntity.Items[i].SubItems[1].Text);
                TXTHandler.WCUEntitys.Add(new WCUEntity(listEntity.Items[i].SubItems[0].Text, text));
            }

                base.Close();
        }

       

       

        private void ListEntity_MouseClick(object sender, MouseEventArgs e)
        {
            richTextEntity.Text = listEntity.SelectedItems[0].SubItems[1].Text;
        }
       
        private void ListEntity_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listEntity.DoDragDrop(listEntity.SelectedItems, DragDropEffects.Move);
        }

        private void ListEntity_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ListEntity_DragDrop(object sender, DragEventArgs e)
        {
            if (listEntity.SelectedItems.Count == 0) { return; }

            Point Pt = listEntity.PointToClient(new Point(e.X, e.Y));
            ListViewItem ItemDrag = listEntity.GetItemAt(Pt.X, Pt.Y);

            if (ItemDrag == null) { return; }

            int ItemDragIndex = ItemDrag.Index;
            ListViewItem[] Sel = new ListViewItem[listEntity.SelectedItems.Count];

            for (int i = 0; i < listEntity.SelectedItems.Count; i++)
            {
                Sel[i] = listEntity.SelectedItems[i];
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
                listEntity.Items.Insert(ItemIndex, InsertItem);
                listEntity.Items.Remove(Item);

            }
           
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(listEntity.SelectedItems.Count>0)
                listEntity.SelectedItems[0].SubItems[1].Text = richTextEntity.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (listEntity.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you want to remove?", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {

                    for (int i = 0; i < listEntity.SelectedItems.Count; i++)
                    {
                        listEntity.Items.RemoveAt(listEntity.SelectedItems[i].Index);
                        i--;
                    }

                }
               
            }
            else
            {
                MessageBox.Show("Item is not selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboEntity.Text))
            {
                MessageBox.Show("Empty field", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string text=null;
            if (comboEntity.Text == "WcuSpecificData")
                text = textWcuSpecificData;
            else if (comboEntity.Text == "PSD-Tables")
                text = textPSDTables;
            else if (comboEntity.Text == "AdjacentWCU")
                text = textAdjacentWCU;
            else
                text = textTtsListOfTrains;          
            String[] row = { comboEntity.Text, text };
            ListViewItem item = new ListViewItem(row);
            listEntity.Items.Add(item);
            richTextEntity.Text = text;
            richTextEntity.Focus();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        
    }
    
}
