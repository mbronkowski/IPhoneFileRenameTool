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
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IPhoneFileRenameTool
{
    public partial class IPhoneFileRanameForm : Form
    {
        private CancellationTokenSource? _cts;

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

        private async void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.Directory.Exists(txtFolder.Text))
                    throw new Exception($"Directory \"{txtFolder.Text}\" doesn't exist!");
                SaveSettingsInRegistry();
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(txtFolder.Text, "log.txt"))
                    .CreateLogger();

                btnRename.Enabled = false;
                btnCancel.Enabled = true;
                progressBar.Value = 0;
                _cts = new CancellationTokenSource();

                var progress = new Progress<(int current, int total)>(r =>
                {
                    if (r.total > 0)
                        progressBar.Value = (int)((double)r.current / r.total * 100);
                });

                var converter = FileConverter.Create(
                    txtFolder.Text, txtPostfix.Text, txtFormat.Text,
                    chkConvert.Checked, Convert.ToDouble(numHourOffset.Value),
                    chkRecursive.Checked);

                var result = await converter.RenameAndConvertAsync(progress, _cts.Token);
                if (!result.IsCancelled) progressBar.Value = 100;
                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while processing files: " + ex.Message);
            }
            finally
            {
                btnRename.Enabled = true;
                btnCancel.Enabled = false;
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
            btnCancel.Enabled = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            base.OnFormClosing(e);
        }

        private void SaveSettingsInRegistry()
        {
            SaveToRegistry("folder", txtFolder.Text);
            SaveToRegistry("format", txtFormat.Text);
            SaveToRegistry("postfix", txtPostfix.Text);
            SaveToRegistry("converHicf", chkConvert.Checked.ToString());
            SaveToRegistry("dateHourOffset", numHourOffset.Value.ToString());
            SaveToRegistry("recursive", chkRecursive.Checked.ToString());
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
            var strRecursive = LoadFromRegistry("recursive");
            if (bool.TryParse(strRecursive, out bool recursive))
                chkRecursive.Checked = recursive;
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
