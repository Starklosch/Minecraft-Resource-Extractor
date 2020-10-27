using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Resource_Extractor
{
    static class Program
    {
        public static string APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string MINECRAFT_PATH = APPDATA + @"\.minecraft";
        public static string ASSETS_PATH = MINECRAFT_PATH + @"\assets";
        public static string INDEXS_PATH = ASSETS_PATH + @"\indexes";
        public static string OBJECTS_PATH = ASSETS_PATH + @"\objects";

        public static string EXTRACT_PATH = "";

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine(Environment.CurrentDirectory);

            if (Directory.Exists(MINECRAFT_PATH))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show(".minecraft not found");
            }
        }
    }
}
