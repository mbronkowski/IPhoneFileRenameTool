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
                    .WriteTo.File(Path.Combine(txtFolder.Text,"log.txt")) // Set the path to your log file
                    .CreateLogger();
                FileConverter converter = FileConverter.Create(txtFolder.Text, txtPostfix.Text, txtFormat.Text, chkConvert.Checked);
                MessageBox.Show(converter.RenameAndConvert().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while processing files: " + ex.Message);
            }
        }

        private void SaveSettingsInRegistry()
        {
            SaveToRegistry("folder",txtFolder.Text);
            SaveToRegistry("format", txtFormat.Text);
            SaveToRegistry("postfix", txtPostfix.Text);
            SaveToRegistry("converHicf", chkConvert.Checked.ToString());
        }

        private void LoadSettingsInRegistry()
        {
            txtFolder.Text = LoadFromRegistry("folder") ?? txtFolder.Text;
            txtFormat.Text = LoadFromRegistry("format") ?? txtFormat.Text;
            txtPostfix.Text = LoadFromRegistry("postfix") ?? txtPostfix.Text;
            var strConvertHicf = LoadFromRegistry("converHicf");
            if(bool.TryParse(strConvertHicf, out bool convertHicf))
                chkConvert.Checked = convertHicf;
        }

        private const string RegistryKeyPath = @"SOFTWARE\EdsSoftware\IphoneFileRanameTool";

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
    }
}