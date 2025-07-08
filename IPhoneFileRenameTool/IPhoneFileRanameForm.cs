using ImageMagick;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using Microsoft.Win32;
using Serilog;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IPhoneFileRenameTool
{
    public partial class IPhoneFileRanameForm : Form
    {
        public IPhoneFileRanameForm()
        {
            InitializeComponent();
            LoadSettingsInRegistry();
        }

        public IPhoneFileRanameForm(string folderPath) : this()
        {
            this.txtFolder.Text = folderPath;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtFolder.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.Directory.Exists(txtFolder.Text))
                    throw new Exception($"Directory \"{txtFolder.Text}\" dosen't exist!");
                SaveSettingsInRegistry();
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(txtFolder.Text, "log.txt")) // Set the path to your log file
                    .CreateLogger();
                FileConverter converter = FileConverter.Create(txtFolder.Text, txtPostfix.Text, txtFormat.Text, chkConvert.Checked, Convert.ToDouble(numHourOffset.Value));
                MessageBox.Show(converter.RenameAndConvert().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while processing files: " + ex.Message);
            }
        }

        private void SaveSettingsInRegistry()
        {
            SaveToRegistry("folder", txtFolder.Text);
            SaveToRegistry("format", txtFormat.Text);
            SaveToRegistry("postfix", txtPostfix.Text);
            SaveToRegistry("converHicf", chkConvert.Checked.ToString());
            SaveToRegistry("dateHourOffset", numHourOffset.Value.ToString());
        }

        private void LoadSettingsInRegistry()
        {
            txtFolder.Text = LoadFromRegistry("folder") ?? txtFolder.Text;
            txtFormat.Text = LoadFromRegistry("format") ?? txtFormat.Text;
            txtPostfix.Text = LoadFromRegistry("postfix") ?? txtPostfix.Text;
            var strConvertHicf = LoadFromRegistry("converHicf");
            if (bool.TryParse(strConvertHicf, out bool convertHicf))
                chkConvert.Checked = convertHicf;
            var strDateHourOffset = LoadFromRegistry("dateHourOffset");
            if (decimal.TryParse(strDateHourOffset, out decimal dateHourOffset))
                numHourOffset.Value = dateHourOffset;
        }

        private const string RegistryKeyPath = @"SOFTWARE\EdsSoftware\IphoneFileRanameTool";
        private string v;

        private void SaveToRegistry(string key, string value)
        {
            try
            {
                using (var regkey = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                {
                    regkey.SetValue(key, value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to registry: {ex.Message}");
            }
        }

        private string? LoadFromRegistry(string key)
        {
            try
            {
                using (var regkey = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    return regkey?.GetValue(key)?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading from registry: {ex.Message}");
                return string.Empty;
            }
        }

        private void RegisterFileExplorerAction()
        {
            // Specify the name of your custom action
            string actionName = "Rename IPhone media files";


            // Specify the path to your application executable
            string appPath = Assembly.GetEntryAssembly().Location; // @"C:\Path\To\Your\Application.exe";
            appPath = appPath.Remove(appPath.Length - 3) + "exe";
            // Create the key for the folder shell
            using (RegistryKey folderShellKey = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\" + actionName))
            {
                if (folderShellKey != null)
                {
                    // Create the key for the command
                    using (RegistryKey commandKey = folderShellKey.CreateSubKey("command"))
                    {
                        if (commandKey != null)
                        {
                            // Set the default value of the command key to your application path with "%1" as a parameter
                            commandKey.SetValue("", $"\"{appPath}\" \"%1\"");
                        }
                        else
                        {
                            Console.WriteLine("Error creating command key");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error creating folder shell key");
                }
            }

            Console.WriteLine("Registry entries added successfully. Restart Windows Explorer or refresh the context menu.");

            // Keep the console window open for debugging
            Console.ReadLine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterFileExplorerAction();
        }
    }
}