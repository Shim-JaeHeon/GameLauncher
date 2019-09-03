using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string GamePath = @"D:\Dev\Unreal\UnrealWWY\Saved\StagedBuilds\WindowsNoEditor\";

        public MainWindow()
        {
            InitializeComponent();

            MapComboBox.ItemsSource = GetMapNames();
            MapComboBox.SelectedIndex = 0;
            //System.Windows.SystemParameters.PrimaryScreenWidth;
            //System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        private List<string> GetMapNames()
        {
            string projectName = "";

            string[] fileEntries = Directory.GetDirectories(GamePath);
            foreach (string fileName in fileEntries)
            {
                string[] subFileEntries = Directory.GetFiles(fileName);
                foreach (string subFileName in subFileEntries)
                {
                    if (subFileName.Contains(@".uproject"))
                    {
                        projectName = System.IO.Path.GetFileNameWithoutExtension(subFileName);
                    }
                }
            }
            string[] mapFiles = Directory.GetFiles(GamePath + projectName + @"\Content\Maps\");

            List<string> mapNames = new List<string>();
            foreach (string mapFile in mapFiles)
            {
                if (mapFile.Contains(@".umap"))
                {
                    mapNames.Add(System.IO.Path.GetFileNameWithoutExtension(mapFile));
                }
            }
            foreach (string mapName in mapNames)
                Debug.WriteLine(mapName);
            return mapNames;
        }
    }
}
