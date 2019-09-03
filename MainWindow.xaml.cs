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
using System.Windows.Forms;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string GameFolder = @"D:\Dev\Unreal\UnrealWWY\Saved\StagedBuilds\WindowsNoEditor\";


        private List<int> _width = new List<int>()
        {
            800, 1024, 1280, 1366, 1400, 1440, 1600, 1680, 1920
        };

        private List<int> _height = new List<int>()
        {
            600, 768, 720, 768, 1050, 900, 900, 1050, 1080
        };

        private string _projectName = "";


        private List<string> _resolutions = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < _height.Count; i++)
            {
                _resolutions.Add(_width[i] + " x " + _height[i]);
            }

            MapComboBox.ItemsSource = GetMapNames();
            MapComboBox.SelectedIndex = 0;

            var resX = Screen.PrimaryScreen.Bounds.Width;
            var resY = Screen.PrimaryScreen.Bounds.Height;
            ResolutionComboBox.ItemsSource = _resolutions;
            var index = _width.IndexOf(resX);
            int nativeResIndex = 0;
            if (_height[index] == resY)
            {
                nativeResIndex = index;
            }

            ResolutionComboBox.SelectedIndex = nativeResIndex;
        }

        private List<string> GetMapNames()
        {
            string[] fileEntries = Directory.GetDirectories(GameFolder);
            foreach (string fileName in fileEntries)
            {
                string[] subFileEntries = Directory.GetFiles(fileName);
                foreach (string subFileName in subFileEntries)
                {
                    if (subFileName.Contains(@".uproject"))
                    {
                        _projectName = System.IO.Path.GetFileNameWithoutExtension(subFileName);
                    }
                }
            }

            string[] mapFiles = Directory.GetFiles(GameFolder + _projectName + @"\Content\Maps\");

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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int parameterCount = 5;
            int resolutionIndex = ResolutionComboBox.SelectedIndex;
            int resHeight = _height[resolutionIndex];
            int resWidth = _width[resolutionIndex];
            bool? vsync = VSyncCheckBox.IsChecked;
            bool? fullscreen = FullscreenCheckBox.IsChecked;
            string mapName = MapComboBox.Text.Trim();

            List<string> parametersList = new List<string>();
            parametersList.Add(mapName);
            parametersList.Add("ResX=" + resWidth);
            parametersList.Add("ResY=" + resHeight);
            parametersList.Add((bool) vsync ? "-VSync" : "-NoVSync");
            parametersList.Add((bool) fullscreen ? "-FULLSCREEN" : "-WINDOWED");
            

            string parameters = "";
            for (int i = 0; i < parameterCount; i++)
            {
                parameters = String.Join(" ", parametersList);
            }

            string gameEXE = GameFolder + _projectName + @".exe";
            Debug.WriteLine(parameters);
            Process.Start(gameEXE, parameters);
        }
    }
}