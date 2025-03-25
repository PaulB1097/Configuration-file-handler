using FileFinder.Properties;        //Librarie creata undeva pentru proprietati initiale ale locului unde se saleveaza etc..
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Threading;
namespace FileFinder
{
    public class finderForm : Form  //Clasa de baza
    {
        //proprietati Clasa

        public static string filePath;

        public static string fileExt;

        public static long results;

        public static string resultsPath;

        public static bool othertableResults;

        public static bool overwrite;

       

       
        public Button submitButton;

        private Label pathLabel;

        private Label extLabel;

        private TextBox pathBox;

        private TextBox extBox;

        private Label resultsLabel;

        private TextBox resultsBox;

        private Button cancelButton;

        private Button browseButton;

        private Button logButton;

        private Button commitButton;

        private Button pushButton;

        private Button filterButton;

        private Button settingsButton;

        private Button readButton;
        private CheckBox ConfData_WCU;
        private CheckBox ConfData_System;
        private Label label1;
        private Button ResultsButton;
        private Button button1;
        private Button btnEditWCUTemplate;
        private Button Open;
        private PictureBox pictureBox1;
        private TextBox containsBox;

        public finderForm()             //constructor
        {
            //Initializare variabile pentru a nu putea apasa butoanele pana cand nu se introduce calea si fisierul cautat
            this.InitializeComponent();
           
            this.pathBox.Text = FileFinder.Properties.Settings.Default.savedPath;
            this.logButton.Enabled = false;
            this.filterButton.Enabled = false;
            this.ResultsButton.Enabled = false;
            this.submitButton.Enabled = false;
            if (string.IsNullOrEmpty(FileFinder.Properties.Settings.Default.resultsPath))
            {
                string implicitpathdirectory = System.IO.Directory.GetCurrentDirectory();
                FileFinder.Properties.Settings.Default.resultsPath = implicitpathdirectory;
                finderForm.resultsPath = FileFinder.Properties.Settings.Default.resultsPath;
                finderForm.overwrite = FileFinder.Properties.Settings.Default.overwrite;
                finderForm.fileExt = this.extBox.Text;
                finderForm.filePath = this.pathBox.Text;

            }
            else
            {
                finderForm.resultsPath = FileFinder.Properties.Settings.Default.resultsPath;
                finderForm.overwrite = FileFinder.Properties.Settings.Default.overwrite;
            }
           
        }
  
        //Salvez in baza de date daca modific path-ul
        private void pathBox_TextChanged(object sender, EventArgs e)
        {
            finderForm.filePath = this.pathBox.Text;
            FileFinder.Properties.Settings.Default.savedPath = this.pathBox.Text;
            FileFinder.Properties.Settings.Default.Save();
            
           
        }
        private void extBox_TextChanged(object sender, EventArgs e)
        {
            finderForm.fileExt = this.extBox.Text;
        }
        private void browseButton_Click(object sender, EventArgs e)
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
                        this.pathBox.Text = fbd.SelectedPath;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }



        }
        //Buton inchidere fereastra
        private void cancelButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        
        private void readButton_Click(object sender, EventArgs e) 
        {
            finderForm.fileExt = this.extBox.Text;
            finderForm.filePath = this.pathBox.Text;
            if (!this.containsBox.Enabled)  //Dezactivez butonul submit pentru fisierele cautate dupa nume(pot exista fisiere care nu sunt de tip WCU sau System
            {
                this.submitButton.Enabled = true;
            }
            this.ResultsButton.Enabled = true;
            this.logButton.Enabled = true;
            this.filterButton.Enabled = true;
            finderForm.results = 0;

            if (!Directory.Exists(this.pathBox.Text))//Verific daca exista directorul in care sa caut fisierele
            {
                MessageBox.Show("Given path doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                try
                {
                    //vector cu numele fisierelor din folderul ales prin path si cu extensia aleasa
                    string[] files = Directory.GetFiles(this.pathBox.Text, string.Concat("*.", this.extBox.Text), SearchOption.AllDirectories);
                    string str = finderForm.resultsPath;//Path-ul directorului in care vor fi scrise fisierele "results" si "log"

                    using (StreamWriter file = new StreamWriter(Path.Combine(str, "results.txt")))//creez fisierul in care o sa salvez toate path-urile(fisierelor) pe care doresc sa le modific
                    {
                   
                        for (int i = 0; i < (int)files.Length; i++)
                        {
                            string line = files[i];
                            if (this.containsBox.Enabled == false)//pentru fiecare fisiere verific cum a fost cautat(dupa nume sau dupa checkbox), pentru checkbox caut exclud fisierele care contin numele ANI sau OLD
                            {
                                if ((this.ConfData_System.Checked == true) && (this.ConfData_WCU.Checked == true))//Caut WCU si System si le scriu in fisierul results 
                                {
                                    if (((line.ToUpper().Contains("CONFDATA_SYSTEM")) || (line.ToUpper().Contains("CONFDATA_WCU"))) && !(line.ToUpper().Contains("ANI")))
                                    {
                                        if (!line.ToUpper().Contains("_OLD"))
                                        {
                                            file.WriteLine(line);
                                            finderForm.results++;
                                        }

                                    }
                                }
                                else if ((this.ConfData_System.Checked == false) && (this.ConfData_WCU.Checked == true))//Caut doar WCU 
                                {
                                    if ((line.ToUpper().Contains("CONFDATA_WCU")) && (!(line.ToUpper().Contains("ANI"))))
                                    {
                                        if (!line.ToUpper().Contains("_OLD"))
                                        {
                                            file.WriteLine(line);
                                            finderForm.results++;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((this.ConfData_WCU.Checked == false) && (this.ConfData_System.Checked == true))//Caut doar System
                                    {
                                        if ((line.ToUpper().Contains("CONFDATA_SYSTEM")) && (!(line.ToUpper().Contains("ANI"))))
                                        {
                                            if (!line.ToUpper().Contains("_OLD"))
                                            {
                                                file.WriteLine(line);
                                                finderForm.results++;
                                            }
                                        }
                                    }
                                }
                            }
                            else//Daca fisierele sunt cautate dupa nume, caut toate fisierele
                            {
                                if (this.containsBox.Text == null)
                                {
                                    finderForm.results++;
                                    file.WriteLine(line);
                                }
                                else if (line.ToUpper().Contains(this.containsBox.Text.ToUpper()))
                                {
                                    finderForm.results++;
                                    file.WriteLine(line);
                                }

                            }

                        }

                        if (finderForm.results == 0)
                        {
                            MessageBox.Show("No results were found.", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Access was denied to the given folder or one of its subfolders.", "Error: no access", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            this.resultsBox.Text = finderForm.results.ToString();

        }
        private void FilterButton_Click(object sender, EventArgs e)
        {


            if (this.containsBox.Enabled == false)//Pentru cautarea dupa nume fac un filtru personalizat cu toate fisierele, iar pentru cautarea dupa checkbox un filtru care repartizeaza fisierele pe cele doua categorii WCU si System
            {
                othertableResults = false;
            }
            else
            {
                othertableResults = true;
            }
            (new Results(this)).ShowDialog();
            this.resultsBox.Text = finderForm.results.ToString();
        }

        //cand parasesc butonul filter activez butoanele pentru modificare

		
        //la apasarea tastei enter este apasat butonul submit
		private void finderForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.submitButton.PerformClick();
			}
		}


		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(finderForm));
            this.submitButton = new System.Windows.Forms.Button();
            this.pathLabel = new System.Windows.Forms.Label();
            this.extLabel = new System.Windows.Forms.Label();
            this.extBox = new System.Windows.Forms.TextBox();
            this.resultsLabel = new System.Windows.Forms.Label();
            this.resultsBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.logButton = new System.Windows.Forms.Button();
            this.commitButton = new System.Windows.Forms.Button();
            this.pushButton = new System.Windows.Forms.Button();
            this.filterButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.readButton = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.ConfData_WCU = new System.Windows.Forms.CheckBox();
            this.ConfData_System = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.containsBox = new System.Windows.Forms.TextBox();
            this.ResultsButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnEditWCUTemplate = new System.Windows.Forms.Button();
            this.Open = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Location = new System.Drawing.Point(505, 305);
            this.submitButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(107, 38);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            this.submitButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.submitButton_KeyDown);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.BackColor = System.Drawing.Color.Transparent;
            this.pathLabel.Location = new System.Drawing.Point(19, 26);
            this.pathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(29, 13);
            this.pathLabel.TabIndex = 1;
            this.pathLabel.Text = "Path";
            // 
            // extLabel
            // 
            this.extLabel.AutoSize = true;
            this.extLabel.BackColor = System.Drawing.Color.Transparent;
            this.extLabel.Location = new System.Drawing.Point(17, 135);
            this.extLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.extLabel.Name = "extLabel";
            this.extLabel.Size = new System.Drawing.Size(53, 13);
            this.extLabel.TabIndex = 2;
            this.extLabel.Text = "Extension";
            // 
            // extBox
            // 
            this.extBox.Location = new System.Drawing.Point(116, 133);
            this.extBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extBox.Name = "extBox";
            this.extBox.Size = new System.Drawing.Size(53, 20);
            this.extBox.TabIndex = 4;
            this.extBox.Text = "txt";
            this.extBox.TextChanged += new System.EventHandler(this.extBox_TextChanged);
            // 
            // resultsLabel
            // 
            this.resultsLabel.AutoSize = true;
            this.resultsLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultsLabel.Location = new System.Drawing.Point(19, 171);
            this.resultsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Size = new System.Drawing.Size(42, 13);
            this.resultsLabel.TabIndex = 5;
            this.resultsLabel.Text = "Results";
            // 
            // resultsBox
            // 
            this.resultsBox.Enabled = false;
            this.resultsBox.Location = new System.Drawing.Point(116, 167);
            this.resultsBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.resultsBox.Name = "resultsBox";
            this.resultsBox.Size = new System.Drawing.Size(53, 20);
            this.resultsBox.TabIndex = 6;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(385, 305);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(107, 38);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(370, 24);
            this.browseButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(56, 23);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // logButton
            // 
            this.logButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logButton.BackColor = System.Drawing.Color.Transparent;
            this.logButton.Location = new System.Drawing.Point(539, 63);
            this.logButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(59, 25);
            this.logButton.TabIndex = 9;
            this.logButton.Text = "Log";
            this.logButton.UseVisualStyleBackColor = false;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // commitButton
            // 
            this.commitButton.Location = new System.Drawing.Point(0, 0);
            this.commitButton.Name = "commitButton";
            this.commitButton.Size = new System.Drawing.Size(75, 23);
            this.commitButton.TabIndex = 0;
            // 
            // pushButton
            // 
            this.pushButton.Location = new System.Drawing.Point(0, 0);
            this.pushButton.Name = "pushButton";
            this.pushButton.Size = new System.Drawing.Size(75, 23);
            this.pushButton.TabIndex = 0;
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(179, 164);
            this.filterButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(56, 23);
            this.filterButton.TabIndex = 19;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.settingsButton.Location = new System.Drawing.Point(539, 21);
            this.settingsButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(59, 27);
            this.settingsButton.TabIndex = 23;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // readButton
            // 
            this.readButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.readButton.Location = new System.Drawing.Point(505, 253);
            this.readButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(107, 38);
            this.readButton.TabIndex = 24;
            this.readButton.Text = "Search";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // pathBox
            // 
            this.pathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathBox.Location = new System.Drawing.Point(116, 27);
            this.pathBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(251, 20);
            this.pathBox.TabIndex = 3;
            this.pathBox.TextChanged += new System.EventHandler(this.pathBox_TextChanged);
            // 
            // ConfData_WCU
            // 
            this.ConfData_WCU.AutoSize = true;
            this.ConfData_WCU.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ConfData_WCU.Location = new System.Drawing.Point(247, 68);
            this.ConfData_WCU.Name = "ConfData_WCU";
            this.ConfData_WCU.Size = new System.Drawing.Size(103, 17);
            this.ConfData_WCU.TabIndex = 26;
            this.ConfData_WCU.Text = "ConfData_WCU";
            this.ConfData_WCU.UseVisualStyleBackColor = false;
            this.ConfData_WCU.CheckedChanged += new System.EventHandler(this.ConfData_WCU_CheckedChanged);
            // 
            // ConfData_System
            // 
            this.ConfData_System.AutoSize = true;
            this.ConfData_System.Location = new System.Drawing.Point(116, 68);
            this.ConfData_System.Name = "ConfData_System";
            this.ConfData_System.Size = new System.Drawing.Size(111, 17);
            this.ConfData_System.TabIndex = 27;
            this.ConfData_System.Text = "ConfData_System";
            this.ConfData_System.UseVisualStyleBackColor = true;
            this.ConfData_System.CheckedChanged += new System.EventHandler(this.ConfData_System_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "File Type";
            // 
            // containsBox
            // 
            this.containsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.containsBox.Location = new System.Drawing.Point(116, 89);
            this.containsBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.containsBox.Name = "containsBox";
            this.containsBox.Size = new System.Drawing.Size(87, 20);
            this.containsBox.TabIndex = 29;
            this.containsBox.EnabledChanged += new System.EventHandler(this.ContainsBox_EnabledChanged);
            // 
            // ResultsButton
            // 
            this.ResultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsButton.Location = new System.Drawing.Point(539, 101);
            this.ResultsButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ResultsButton.Name = "ResultsButton";
            this.ResultsButton.Size = new System.Drawing.Size(59, 26);
            this.ResultsButton.TabIndex = 30;
            this.ResultsButton.Text = "Results";
            this.ResultsButton.UseVisualStyleBackColor = true;
            this.ResultsButton.Click += new System.EventHandler(this.ResultsButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(8, 298);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 47);
            this.button1.TabIndex = 31;
            this.button1.Text = "EditSystemTemplate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnEditWCUTemplate
            // 
            this.btnEditWCUTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditWCUTemplate.Location = new System.Drawing.Point(135, 298);
            this.btnEditWCUTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEditWCUTemplate.Name = "btnEditWCUTemplate";
            this.btnEditWCUTemplate.Size = new System.Drawing.Size(113, 47);
            this.btnEditWCUTemplate.TabIndex = 32;
            this.btnEditWCUTemplate.Text = "EditWCUTemplate";
            this.btnEditWCUTemplate.UseVisualStyleBackColor = true;
            this.btnEditWCUTemplate.Click += new System.EventHandler(this.BtnEditWCUTemplate_Click);
            // 
            // Open
            // 
            this.Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Open.Location = new System.Drawing.Point(430, 24);
            this.Open.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(53, 23);
            this.Open.TabIndex = 33;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.BtnCurrentFolder_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(395, 52);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // finderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(621, 353);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.btnEditWCUTemplate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ResultsButton);
            this.Controls.Add(this.containsBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConfData_System);
            this.Controls.Add(this.ConfData_WCU);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.filterButton);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.resultsBox);
            this.Controls.Add(this.resultsLabel);
            this.Controls.Add(this.extBox);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.extLabel);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.submitButton);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinimumSize = new System.Drawing.Size(574, 361);
            this.Name = "finderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find file(s)";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.submitButton_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void logButton_Click(object sender, EventArgs e)//lansez in executie notepad.exe
        {
            try
            {
				Process.Start("notepad.exe", Path.Combine(finderForm.resultsPath, "log.txt"));
			}
			catch (Exception)
            {
				MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}


        protected override bool ProcessDialogKey(Keys keyData)//Inchid aplicatia la apasarea tastei ESC si lansez in executie butonul submit pe tasta ENTER
		{
			if (Control.ModifierKeys == Keys.None && keyData == Keys.Escape)
			{
				if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo) != global::System.Windows.Forms.DialogResult.No)
				{
					base.Close();
				}
				return true;
			}
			if (Control.ModifierKeys != Keys.None || keyData != Keys.Return)
			{
				return base.ProcessDialogKey(keyData);
			}
			this.submitButton.PerformClick();
			return true;
		}

		

		private void settingsButton_Click(object sender, EventArgs e)
		{
			(new FileFinder.Settings()).Show();
		}
        
        public void submitButton_Click(object sender, EventArgs e)
        {
            XMLHandler.SVariables.Clear();//Sterg continutul listelor pentru un nou continut pentru datele provenite din Template
            XMLHandler.WCUVariables.Clear();
            XMLHandler.ReadSystem();
            XMLHandler.ReadWCU();
            File.WriteAllText(Path.Combine(finderForm.resultsPath, "log.txt"), string.Empty);
            
            string str = finderForm.resultsPath;////Path-ul directorului in care vor fi scrise fisierele "results" si "log"
            using (StreamReader file = new StreamReader(Path.Combine(finderForm.resultsPath, "results.txt")))//deschid fisierul results pentru citire (fisierul cu toate path-urile fisierelor ce trebuie reordonate si modificate)
            {
                while (!file.EndOfStream)
                {
                    string currentPath = file.ReadLine();
                    if (finderForm.overwrite)//Daca checkbox din settings este bifat crez un fiser clona in care salvez continutul fisierului inainte sa il modific
                    {
                        string alltext=File.ReadAllText(currentPath);
                        string oldPath = currentPath.Insert(currentPath.Length - 4, "_Old");
                        using (StreamWriter file1 = new StreamWriter(oldPath, true))
                        {
                            DateTime d = DateTime.Now;//afisez data
                            string data = d.ToString();
                            file1.WriteLine("Copied to {0} ================================================================================================================",data);
                            file1.WriteLine(alltext);
                        }
                    }
                    if (currentPath.ToUpper().Contains("CONFDATA_SYSTEM"))
                    {
                        TXTHandler.SParams.Clear();//Golesc lista fisiere pentru a citi parametri din fisierul curent                   
                        TXTHandler.ReadSystem(currentPath);//Citesc parametrii din fisierul curent in lista fisiere(lista cu parametri din fisier), tot in aceasta functie verific daca sunt dulicate
                        if (TXTHandler.DuplicateParameters)//Daca exista duplicate afisez in log de inceput path-ul fisierului si mesajul erorii
                        {
                            using (StreamWriter file1 = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
                            {
                                file1.WriteLine("Path: {0}", currentPath);
                                file1.WriteLine("Duplicate Parameters!");
                                
                            }
                            Comparare.EliminareDuplicateSystem();//Afisez toate duplicatele ce exista in fisierul curent si le elimin din lista
                        }

                        Comparare.ReordonareSystem();//Reordonez lista fisiere(lista cu parametri din fisier) dupa lista template(lista cu parametri din template)
                        Comparare.CheckValuesSystem();//Verific daca exista cel putin un parametru neincadrat in domeniul stabilit in template
                        bool ok = Comparare.checkValues;//Booleana salvata din functia Comparare.CheckValuesSystem()

                        if (ok)
                        {
                            using (StreamWriter file1 = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
                            {
                                if (TXTHandler.DuplicateParameters == false)//Daca fisierul a continut duplicate a fost afisat path-ul la inceput
                                {
                                    file1.WriteLine("Path: {0}", currentPath);
                                }
                                file1.WriteLine("Value out of range!");
                               
                                
                            }
                            Comparare.rectificationValuesSystem();//Afisez in log toti parametrii care nu se incadreaza
                        }
                        TXTHandler.WriteSystem(currentPath);//Rescriu fisierul cu lista fisier reordonata si modificata
                    }
                    else
                    {
                        TXTHandler.WCUParams.Clear();//Aceleasi lucruri ca si pentru System, aici fact pentru WCU                   
                        TXTHandler.WCUEntitys.Clear();
                        TXTHandler.ReadWCU(currentPath);
                        if (TXTHandler.DuplicateParameters)
                        {
                            using (StreamWriter file1 = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
                            {
                                file1.WriteLine("Path: {0}", currentPath);
                                file1.WriteLine("Duplicate Parameters!");
                               
                            }
                            Comparare.EliminareDuplicateWCU();
                        }
                        Comparare.ReordonareWCU();//Reordonez lista fisiere(lista cu parametri din fisier) dupa lista template(lista cu parametri din template)
                        Comparare.CheckValuesWCU();
                        bool ok = Comparare.checkValues;
                        if (ok)
                        {
                            using (StreamWriter file1 = new StreamWriter(Path.Combine(finderForm.resultsPath, "log.txt"), true))
                            {
                                if (TXTHandler.DuplicateParameters == false)
                                {
                                    file1.WriteLine();
                                    file1.WriteLine("Path: {0}", currentPath);
                                }
                                file1.WriteLine("Value out of range!");

                            }
                            Comparare.rectificationValuesWCU();
                        }
                        TXTHandler.ReadWCUEntitys(currentPath);
                        (new EditEntity(currentPath)).ShowDialog();//Pentru WCU mai am de prelucrat si alti parametri pentru care interactionez cu fiecare fisier in parte
                        TXTHandler.WriteWCU(currentPath);//Rescriu fisierul cu lista fisier reordonata si modificata
                        
                    }
                }
            }
        }
       
        private void submitButton_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.submitButton.PerformClick();
			}
		}

        private void ConfData_System_CheckedChanged(object sender, EventArgs e)//Dezactivez cautarea dupa nume cand cel putin unul dintre check box este activat
        {

            if (this.ConfData_WCU.Checked == true)
            {
                this.containsBox.Enabled = false;
                this.containsBox.Clear();
            }
            else if ((this.containsBox.Enabled == true))
            {
                this.containsBox.Enabled = false;
                this.containsBox.Clear();
            }
            else
            {
                this.containsBox.Enabled = true;
               
            }
        }

        private void ConfData_WCU_CheckedChanged(object sender, EventArgs e)
        {

            if (this.ConfData_System.Checked == true)
            {
                this.containsBox.Enabled = false;
                this.containsBox.Clear();
            }
            else if (this.containsBox.Enabled == true)
            {
                this.containsBox.Enabled = false;
                this.containsBox.Clear();
            
            }
            else
            {
                this.containsBox.Enabled = true;
            }
        }

        private void ResultsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", Path.Combine(finderForm.resultsPath, "results.txt"));
            }
            catch (Exception)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            (new FileFinder.EditTemplateSystem("SystemTemplate.xml")).Show();//fereastra pentru modificarea Template-ului
        }

        private void BtnEditWCUTemplate_Click(object sender, EventArgs e)
        {
            (new FileFinder.EditTemplateSystem("WCUTemplate.xml")).Show();
        }

        private void ContainsBox_EnabledChanged(object sender, EventArgs e)
        {
            if (this.containsBox.Enabled==false)
                this.submitButton.Enabled = true;
            else
                this.submitButton.Enabled = false;
        }

        private void BtnCurrentFolder_Click(object sender, EventArgs e)//lansez folderul in care se cauta fisierele dorite
        {
            try
            {
                Process.Start(this.pathBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}