using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace FileFinder
{
	public class Results : Form
	{
		private List<Label> labels = new List<Label>();

		private List<CheckBox> checkBoxes = new List<CheckBox>();

		private finderForm findForm;

		private FlowLayoutPanel flowLayoutPanel1;

		private Button doneButton;

		private FlowLayoutPanel flowLayoutPanel2;

		private CheckBox selAll;
        private Label label1;
        private Label label2;
        private TableLayoutPanel tableConfigData;
        private Label label3;
        private TableLayoutPanel tableCustom;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button button1;
        private CheckBox desAll;

		public Results(finderForm findform)
		{
			this.findForm = findform;
			this.InitializeComponent();
			string str = finderForm.resultsPath;
			char chr = Convert.ToChar(92);
            if (finderForm.othertableResults == false)
            {
                tableConfigData.Visible = true;
                tableCustom.Visible = false;
            }
            else
            {
                tableConfigData.Visible = false;
                tableCustom.Visible =true;
            }
            foreach (string line in File.ReadLines(string.Concat(str, chr.ToString(), "results.txt")))
			{
				Label lbl = new Label()
				{
					AutoSize = true,
					Text = line
				};
				CheckBox chkBox = new CheckBox()
				{
					AutoSize = true,
					Checked = true
				};
                if (finderForm.othertableResults == false)
                {
                    if (lbl.Text.ToUpper().Contains("CONFDATA_WCU"))
                    {
                        this.flowLayoutPanel2.Controls.Add(lbl);
                        this.flowLayoutPanel2.Controls.Add(chkBox);
                    }
                    else if (lbl.Text.ToUpper().Contains("CONFDATA_SYSTEM"))
                    {
                        this.flowLayoutPanel1.Controls.Add(lbl);
                        this.flowLayoutPanel1.Controls.Add(chkBox);
                    }
                }
                else
                {
                    this.flowLayoutPanel3.Controls.Add(lbl);
                    this.flowLayoutPanel3.Controls.Add(chkBox);
                }
				this.labels.Add(lbl);
				this.checkBoxes.Add(chkBox);
                  
            }
		}

		private void desAll_CheckedChanged(object sender, EventArgs e)
		{
			if (this.desAll.Checked)
			{
				for (int i = 0; i < this.labels.Count<Label>(); i++)
				{
					this.checkBoxes[i].Checked = false;
				}
				this.selAll.Checked = false;
			}
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			finderForm.results = 0;
			string str = finderForm.resultsPath;
			char chr = Convert.ToChar(92);
			using (StreamWriter file = new StreamWriter(string.Concat(str, chr.ToString(), "results.txt")))
			{
				for (int i = 0; i < this.labels.Count<Label>(); i++)
				{
					if (this.checkBoxes[i].Checked)
					{
						file.WriteLine(this.labels[i].Text);
						finderForm.results++;
					}
				}
			}
			
			File.WriteAllText(string.Concat(finderForm.resultsPath, "log.txt"), string.Empty);
            base.Close();
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Results));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.doneButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.selAll = new System.Windows.Forms.CheckBox();
            this.desAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableConfigData = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableCustom = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.tableConfigData.SuspendLayout();
            this.tableCustom.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 29);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(510, 383);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // doneButton
            // 
            this.doneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.doneButton.Location = new System.Drawing.Point(1104, 396);
            this.doneButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(112, 45);
            this.doneButton.TabIndex = 1;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(519, 29);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(510, 383);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // selAll
            // 
            this.selAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selAll.AutoSize = true;
            this.selAll.BackColor = System.Drawing.Color.Transparent;
            this.selAll.Location = new System.Drawing.Point(1099, 27);
            this.selAll.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.selAll.Name = "selAll";
            this.selAll.Size = new System.Drawing.Size(99, 24);
            this.selAll.TabIndex = 2;
            this.selAll.Text = "Select all";
            this.selAll.UseVisualStyleBackColor = false;
            this.selAll.CheckedChanged += new System.EventHandler(this.selAll_CheckedChanged);
            // 
            // desAll
            // 
            this.desAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.desAll.AutoSize = true;
            this.desAll.BackColor = System.Drawing.Color.Transparent;
            this.desAll.Location = new System.Drawing.Point(1099, 61);
            this.desAll.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.desAll.Name = "desAll";
            this.desAll.Size = new System.Drawing.Size(117, 24);
            this.desAll.TabIndex = 3;
            this.desAll.Text = "Deselect all";
            this.desAll.UseVisualStyleBackColor = false;
            this.desAll.CheckedChanged += new System.EventHandler(this.desAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(510, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "ConfigData_System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(519, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(510, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "ConfigData_WCU";
            // 
            // tableConfigData
            // 
            this.tableConfigData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableConfigData.ColumnCount = 2;
            this.tableConfigData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableConfigData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableConfigData.Controls.Add(this.label2, 1, 0);
            this.tableConfigData.Controls.Add(this.label1, 0, 0);
            this.tableConfigData.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableConfigData.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableConfigData.Location = new System.Drawing.Point(12, 12);
            this.tableConfigData.MinimumSize = new System.Drawing.Size(500, 200);
            this.tableConfigData.Name = "tableConfigData";
            this.tableConfigData.RowCount = 2;
            this.tableConfigData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.956113F));
            this.tableConfigData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.04388F));
            this.tableConfigData.Size = new System.Drawing.Size(1032, 417);
            this.tableConfigData.TabIndex = 4;
            this.tableConfigData.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Custom";
            // 
            // tableCustom
            // 
            this.tableCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableCustom.ColumnCount = 1;
            this.tableCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCustom.Controls.Add(this.label3, 0, 0);
            this.tableCustom.Controls.Add(this.flowLayoutPanel3, 0, 1);
            this.tableCustom.Location = new System.Drawing.Point(12, 12);
            this.tableCustom.MinimumSize = new System.Drawing.Size(400, 200);
            this.tableCustom.Name = "tableCustom";
            this.tableCustom.RowCount = 2;
            this.tableCustom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.931932F));
            this.tableCustom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.06807F));
            this.tableCustom.Size = new System.Drawing.Size(961, 417);
            this.tableCustom.TabIndex = 5;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.AutoScroll = true;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 27);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(955, 387);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1083, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 54);
            this.button1.TabIndex = 6;
            this.button1.Text = "Open checked files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1228, 455);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableCustom);
            this.Controls.Add(this.tableConfigData);
            this.Controls.Add(this.desAll);
            this.Controls.Add(this.selAll);
            this.Controls.Add(this.doneButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "Results";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Results";
            this.tableConfigData.ResumeLayout(false);
            this.tableConfigData.PerformLayout();
            this.tableCustom.ResumeLayout(false);
            this.tableCustom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (Control.ModifierKeys == Keys.None && keyData == Keys.Escape)
			{
				base.Close();
				return true;
			}
			if (Control.ModifierKeys == Keys.None && keyData == Keys.Return)
			{
				this.doneButton.PerformClick();
				return true;
			}
			if (Control.ModifierKeys == Keys.None && keyData == Keys.S)
			{
				this.selAll.Checked = true;
				return true;
			}
			if (Control.ModifierKeys != Keys.None || keyData != Keys.D)
			{
				return false;
			}
			this.desAll.Checked = true;
			return true;
		}

		

		private void selAll_CheckedChanged(object sender, EventArgs e)
		{
			if (this.selAll.Checked)
			{
				for (int i = 0; i < this.labels.Count<Label>(); i++)
				{
					this.checkBoxes[i].Checked = true;
				}
				this.desAll.Checked = false;
			}
		}

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.labels.Count<Label>(); i++)
            {
                if(this.checkBoxes[i].Checked == true)
                {
                    try
                    {
                        Process.Start("notepad.exe", this.labels[i].Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
        }
    }
}