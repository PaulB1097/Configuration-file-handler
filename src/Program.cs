using System;
using System.Windows.Forms;

namespace FileFinder
{
	internal class Program : TXTHandler
	{
		public Program()
		{
		}

		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new finderForm());
		}
        
    }
}