using FileFinder.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace FileFinder
{
	public class Settings : Form
	{

		private Label label1;

		private Button browseButton;

		private CheckBox owCBox;

		private TextBox pathBox;
        private Button button1;
        private Button saveButton;

		public Settings()
		{
			this.InitializeComponent();
			this.pathBox.Text = finderForm.resultsPath;
			
			if (finderForm.overwrite)
			{
				this.owCBox.Checked = true;
				return;
			}
			this.owCBox.Checked = false;
		}

		private void browse1_Click(object sender, EventArgs e)
		{
            try
            {
                using (var fbd = new Ionic.Utils.FolderBrowserDialogEx())
                {
                    fbd.ShowNewFolderButton = true;
                    fbd.ShowEditBox = true;
                    fbd.SelectedPath = this.pathBox.Text;
                    fbd.ShowFullPathInEditBox = true;
                    fbd.RootFolder = global::System.Environment.SpecialFolder.MyComputer;
                    if (fbd.ShowDialog() == global::System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Directory.GetFiles(fbd.SelectedPath);
                        string selectedPath = fbd.SelectedPath;
                        char chr = Convert.ToChar(92);
                        finderForm.resultsPath = string.Concat(selectedPath, chr.ToString());
                        this.pathBox.Text = fbd.SelectedPath;
                    }
                }
                
			}
			catch (Exception)
            {
				MessageBox.Show("Error opening folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		
		private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.owCBox = new System.Windows.Forms.CheckBox();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(8, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Save files to..";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(433, 69);
            this.browseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(99, 35);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browse1_Click);
            // 
            // owCBox
            // 
            this.owCBox.AutoSize = true;
            this.owCBox.BackColor = System.Drawing.Color.Transparent;
            this.owCBox.Location = new System.Drawing.Point(12, 219);
            this.owCBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.owCBox.Name = "owCBox";
            this.owCBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.owCBox.Size = new System.Drawing.Size(141, 24);
            this.owCBox.TabIndex = 4;
            this.owCBox.Text = "? Copy old files";
            this.owCBox.UseVisualStyleBackColor = false;
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(127, 69);
            this.pathBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(277, 26);
            this.pathBox.TabIndex = 5;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(433, 192);
            this.saveButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(120, 51);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "Delete old files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(565, 256);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.owCBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		/*private void owCBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.owCBox.Checked)
			{
				TXTHandler.overwrite = true;
			}
		}*/

		private void saveButton_Click(object sender, EventArgs e)
		{
			FileFinder.Properties.Settings.Default.resultsPath = this.pathBox.Text;
            finderForm.resultsPath = this.pathBox.Text; 
            FileFinder.Properties.Settings.Default.overwrite = this.owCBox.Checked;
            finderForm.overwrite= this.owCBox.Checked;
            FileFinder.Properties.Settings.Default.Save();
			base.Close();
		}
    	private void xmlBrowse_Click(object sender, EventArgs e)
		{
            try
            {
				using (FolderBrowserDialog fbd = new FolderBrowserDialog())
				{
					if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					{
						Directory.GetFiles(fbd.SelectedPath);
						string selectedPath = fbd.SelectedPath;
						char chr = Convert.ToChar(92);
						//XMLHandler.xmlPath = string.Concat(selectedPath, chr.ToString());
						
					}
				}
			}
			catch (Exception)
            {
				MessageBox.Show("Error opening folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

        private void Button1_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(finderForm.filePath, string.Concat("*.", finderForm.fileExt), SearchOption.AllDirectories);
            for(int i=0; i<files.Length;i++)
            {
                if (files[i].ToUpper().Contains("_OLD"))
                {
                    try
                    {
                        File.Delete(files[i]);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error delete files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
        }
    }
}