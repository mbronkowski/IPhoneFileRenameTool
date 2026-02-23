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

namespace IPhoneFileRenameTool
{
    public partial class IPhoneFileRanameForm : Form
    {
        private CancellationTokenSource? _cts;

        public IPhoneFileRanameForm()
        {
            InitializeComponent();
            LoadSettingsInRegistry();
            UpdateFormatPreview();
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
                lblCurrentFile.Text = string.Empty;
                _cts = new CancellationTokenSource();

                var progress = new Progress<(int current, int total, string fileName)>(r =>
                {
                    if (r.total > 0)
                        progressBar.Value = (int)((double)r.current / r.total * 100);
                    lblCurrentFile.Text = r.fileName;
                });

                var converter = FileConverter.Create(
                    txtFolder.Text, txtPostfix.Text, txtFormat.Text,
                    chkConvert.Checked, Convert.ToDouble(numHourOffset.Value),
                    chkRecursive.Checked, chkDeleteAAE.Checked);

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
                lblCurrentFile.Text = string.Empty;
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

        private void txtFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateFormatPreview();
        }

        private void UpdateFormatPreview()
        {
            try
            {
                lblFormatPreview.Text = "np. " + DateTime.Now.ToString(txtFormat.Text) + txtPostfix.Text + ".jpg";
                lblFormatPreview.ForeColor = SystemColors.GrayText;
            }
            catch
            {
                lblFormatPreview.Text = "(niepoprawny format daty)";
                lblFormatPreview.ForeColor = Color.Red;
            }
        }

        private void IPhoneFileRanameForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void IPhoneFileRanameForm_DragDrop(object sender, DragEventArgs e)
        {
            var paths = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            if (paths.Length > 0 && System.IO.Directory.Exists(paths[0]))
                txtFolder.Text = paths[0];
        }

        private void SaveSettingsInRegistry()
        {
            SaveToRegistry("folder", txtFolder.Text);
            SaveToRegistry("format", txtFormat.Text);
            SaveToRegistry("postfix", txtPostfix.Text);
            SaveToRegistry("converHicf", chkConvert.Checked.ToString());
            SaveToRegistry("dateHourOffset", numHourOffset.Value.ToString());
            SaveToRegistry("recursive", chkRecursive.Checked.ToString());
            SaveToRegistry("deleteAAE", chkDeleteAAE.Checked.ToString());
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
            var strDeleteAAE = LoadFromRegistry("deleteAAE");
            if (bool.TryParse(strDeleteAAE, out bool deleteAAE))
                chkDeleteAAE.Checked = deleteAAE;
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

        private void RegisterFileExplorerAction()
        {
            string actionName = "Rename IPhone media files";
            string appPath = Assembly.GetEntryAssembly().Location;
            appPath = appPath.Remove(appPath.Length - 3) + "exe";
            using (RegistryKey folderShellKey = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\" + actionName))
            {
                if (folderShellKey != null)
                {
                    using (RegistryKey commandKey = folderShellKey.CreateSubKey("command"))
                    {
                        if (commandKey != null)
                        {
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
            Console.ReadLine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterFileExplorerAction();
        }
    }
}
