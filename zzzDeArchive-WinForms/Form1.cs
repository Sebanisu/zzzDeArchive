using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZzzArchive
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public partial class Form1 : Form
    {
        #region Fields

        private const string DefaultSteamPath = @"C:\Program Files(x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered";
        private const string SteamGamePathTag = @"InstallLocation";
        private const string SteamRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1026680";
        private readonly FolderBrowserDialog _folderBrowserDialogBrowse;
        private readonly OpenFileDialog _openFileDialogBrowse;
        private readonly Zzz _zzz;

        #endregion Fields

        #region Constructors

        public Form1()
        {
            InitializeComponent();
            GameLocation = GetGameLocation();
            btnOtherExtractIN_Click(this, null);
            btnMain_Click(this, null);
            btnOtherWriteOut_Click(this, null);
            btnMainWriteOut_Click(this, null);
            btnSrcOther_Click(this, null);
            btnSrcMain_Click(this, null);
            _openFileDialogBrowse = new OpenFileDialog();
            if (GameLocation != null)
                _openFileDialogBrowse.InitialDirectory = GameLocation;
            ButClick(btnMainExtractIN, txtReadDataZZZ, "main.zzz");
            _openFileDialogBrowse.Filter = "FF8R ZZZ archives(*.zzz)|*.zzz";
            _openFileDialogBrowse.Title = "Choose";
            _openFileDialogBrowse.CheckFileExists = true;
            _openFileDialogBrowse.Multiselect = false;
            btnExecuteExtract.Enabled = false;
            btnExecuteWrite.Enabled = false;
            _folderBrowserDialogBrowse = new FolderBrowserDialog();
            _zzz = new Zzz();

            var columnHeader1 = new ColumnHeader
            {
                Text = "Filenames"
            };
            lvMergeInput.Columns.AddRange(new[] { columnHeader1 });
            Application.ApplicationExit += Application_ApplicationExit;
        }

        #endregion Constructors

        #region Properties

        public static string GameLocation { get; private set; }

        #endregion Properties

        #region Methods

        private static void Application_ApplicationExit(object sender, EventArgs e) => Logger.DisposeChildren();

        private static void ButClick(Control but, Control txt, string file)
        {
            if (GameLocation != null)
            {
                var path = Path.Combine(GameLocation, file);
                if (File.Exists(path))
                {
                    txt.Text = path;
                    return;
                }
            }
            if (but != null)
                but.Enabled = false;
        }

        /// <summary>
        /// Detect the location of FF8R
        /// </summary>
        /// <returns>string path</returns>
        /// <see cref="https://github.com/MaKiPL/OpenVIII/blob/master/Core/Sources/Environment/Game/Location/WindowsGameLocationProvider.cs"/>
        private static string GetGameLocation()
        {
            foreach (var registryView in new[] { RegistryView.Registry32, RegistryView.Registry64 })
            {
                using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
                using (var registryKey = localMachine.OpenSubKey(SteamRegistryPath))
                {
                    if (registryKey != null)
                    {
                        var installLocation = (string)registryKey.GetValue(SteamGamePathTag);
                        var dataPath = installLocation;
                        if (Directory.Exists(dataPath))
                            return dataPath;
                    }
                }
            }
            if (Directory.Exists(DefaultSteamPath))
                return DefaultSteamPath;
            return null;
        }

        private static void MultiTxtDragDrop(Control txt, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                txt.Text = files[0];
            }
        }

        private static void OpenFolder(string folder, bool select = false)
        {
            try
            {
                folder = Path.GetFullPath(folder);
                if (Directory.Exists(folder) && !select)
                {
                    Process.Start(folder);
                }
                else if ((Directory.Exists(folder) && select) || File.Exists(folder))
                {
                    var args = $"/e, /select, \"{folder}\"";
                    Process.Start("explorer", args);
                }
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(folder);
            }
        }

        private void btnBrowseFolderWrite_IN_Click(object sender, EventArgs e)
        {
            if (_folderBrowserDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtBrowseFolderWrite_IN.Text = _folderBrowserDialogBrowse.SelectedPath;
            }
        }

        private void btnBrowseZZZWrite_OUT_Click(object sender, EventArgs e)
        {
            if (_openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtBrowseZZZWrite_OUT.Text = _openFileDialogBrowse.FileName;
            }
        }

        private void btnExecuteExtract_Click(object sender, EventArgs e)
        {
            btnExecuteExtract.Enabled = false;
            var temp = btnExecuteExtract.Text;
            btnExecuteExtract.Text = "Extracting...";
            Application.DoEvents();
            try
            {
                _zzz.In = new List<string> { Path.GetFullPath(txtZZZ_in.Text.Trim()) };
                _zzz.Path = lblZZZ_out.Text;
                OpenFolder(_zzz.Extract());
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(txtZZZ_in.Text.Trim());
                MessageBox.Show(err.Message, "Exception");
            }
            btnExecuteExtract.Text = temp;
            btnExecuteExtract.Enabled = true;
        }

        private void btnExecuteMerge_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtMergeSource.Text)) return;
            var @in = new HashSet<string>();
            foreach (ListViewItem item in lvMergeInput.Items)
            {
                if (File.Exists((string)item.Tag))
                    @in.Add((string)item.Tag);
            }

            if (@in.Count <= 0) return;
            btnMergeExecute.Enabled = false;
            var temp = btnMergeExecute.Text;
            btnMergeExecute.Text = "Merging...";
            Application.DoEvents();
            _zzz.In = @in.ToList();
            _zzz.Path = txtMergeSource.Text;
            _zzz.Out = Path.GetTempFileName();
            try
            {
                _zzz.Merge();
                try
                {
                    btnMergeExecute.Text = "Coping...";
                    Application.DoEvents();
                    Logger.WriteLine($"Coping {_zzz.Out} to {_zzz.Path}");
                    File.Copy(_zzz.Out, lblBrowseZZZWrite_OUT.Text, true);
                    OpenFolder(_zzz.Path);
                    Logger.WriteLine($"Deleting {_zzz.Out}");
                    File.Delete(_zzz.Out);
                }
                catch (Exception err)
                {
                    OpenFolder(_zzz.Out);
                    OpenFolder(Path.GetDirectoryName(_zzz.Path));
                    Logger.WriteLine("Coping Failed...");
                    MessageBox.Show($"File is named:\n{Path.GetFileName(_zzz.Out)}\n{err.Message}", "File Copy Failed");
                }
            }
            catch (PathTooLongException err0)
            {
                MessageBox.Show(err0.Message, "Path Too Long Exception");
            }
            catch (InvalidDataException err0)
            {
                MessageBox.Show(err0.Message, "Invalid Data Exception");
            }
            catch (ArgumentException err0)
            {
                MessageBox.Show(err0.Message, "Argument Exception");
            }
            btnMergeExecute.Text = temp;
            btnMergeExecute.Enabled = true;
        }

        private void btnExecuteWrite_Click(object sender, EventArgs e)
        {
            btnExecuteWrite.Enabled = false;
            var temp = btnExecuteWrite.Text;
            btnExecuteWrite.Text = "Writing...";
            Application.DoEvents();
            _zzz.Path = lblBrowseFolderWrite_IN.Text;
            _zzz.Out = Path.GetTempFileName();
            try
            {
                _zzz.Write();
                try
                {
                    btnExecuteWrite.Text = "Coping...";
                    Logger.WriteLine($"Coping {_zzz.Out} to {lblBrowseZZZWrite_OUT.Text}");
                    Application.DoEvents();
                    File.Copy(_zzz.Out, lblBrowseZZZWrite_OUT.Text, true);
                    OpenFolder(lblBrowseZZZWrite_OUT.Text);
                    Logger.WriteLine($"Deleting {_zzz.Out}");
                    File.Delete(_zzz.Out);
                }
                catch (Exception err)
                {
                    OpenFolder(_zzz.Out);
                    OpenFolder(Path.GetDirectoryName(lblBrowseZZZWrite_OUT.Text));
                    Logger.WriteLine("Coping Failed...");
                    MessageBox.Show($"File is named:\n{Path.GetFileName(_zzz.Out)}\n{err.Message}", "File Copy Failed");
                }
            }
            catch (PathTooLongException err0)
            {
                MessageBox.Show(err0.Message, "Path Too Long Exception");
            }
            catch (InvalidDataException err0)
            {
                MessageBox.Show(err0.Message, "Invalid Data Exception");
            }
            btnExecuteWrite.Text = temp;
            btnExecuteWrite.Enabled = true;
        }

        private void btnMain_Click(object sender, EventArgs e) => ButClick(btnMainExtractIN, txtZZZ_in, "main.zzz");

        private void btnMainWriteOut_Click(object sender, EventArgs e) => ButClick(btnMainWriteOut, txtBrowseZZZWrite_OUT, "main.zzz");

        private void btnMergeInputBrowse_Click(object sender, EventArgs e)
        {
            _openFileDialogBrowse.Multiselect = true;
            var str = _openFileDialogBrowse.InitialDirectory;
            _openFileDialogBrowse.InitialDirectory = Directory.GetCurrentDirectory();
            if (_openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in _openFileDialogBrowse.FileNames)
                {
                    var item = new ListViewItem(Path.GetFileNameWithoutExtension(file))
                    {
                        Tag = file
                    };
                    lvMergeInput.Items.Add(item);
                }
            }
            _openFileDialogBrowse.InitialDirectory = str;
            _openFileDialogBrowse.Multiselect = false;
            CheckMergeExeEnable();
        }

        private void btnMergeInputRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvMergeInput.SelectedItems)
            {
                lvMergeInput.Items.Remove(item);
            }
            CheckMergeExeEnable();
        }

        private void btnMergeSrcBrowse_Click(object sender, EventArgs e)
        {
            if (_openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtMergeSource.Text = _openFileDialogBrowse.FileName;
            }
        }

        private void btnOtherExtractIN_Click(object sender, EventArgs e) => ButClick(btnOtherExtactIN, txtZZZ_in, "other.zzz");

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void btnOtherWriteOut_Click(object _, EventArgs e) => ButClick(btnOtherWriteOut, txtBrowseZZZWrite_OUT, "other.zzz");

        private void btnSrcMain_Click(object sender, EventArgs e) => ButClick(btnSrcMain, txtMergeSource, "main.zzz");

        private void btnSrcOther_Click(object sender, EventArgs e) => ButClick(btnSrcOther, txtMergeSource, "other.zzz");

        private void btnZZZExtractInBrowse_Click(object sender, EventArgs e)
        {
            if (_openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_in.Text = _openFileDialogBrowse.FileName;
            }
        }

        private void btnZZZExtractOUTBrowse_Click(object sender, EventArgs e)
        {
            if (_folderBrowserDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_out.Text = _folderBrowserDialogBrowse.SelectedPath;
            }
        }

        private void CheckMergeExeEnable()
        {
            if (lvMergeInput.Items.Count > 0 && File.Exists(txtMergeSource.Text))
            {
                btnExcuteMerge.Enabled = true;
            }
            else btnExcuteMerge.Enabled = false;
        }

        private void Item_DragEnter(object sender, DragEventArgs e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;

        private void lvMergeInput_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    var item = new ListViewItem(Path.GetFileNameWithoutExtension(file))
                    {
                        Tag = file
                    };
                    lvMergeInput.Items.Add(item);
                }
            }
            CheckMergeExeEnable();
        }

        private void lvMergeInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keys.Delete == e.KeyCode)
                btnMergeInputRemove_Click(sender, e);
        }

        private void txtBrowseFolderWrite_IN_DragDrop(object sender, DragEventArgs e) => MultiTxtDragDrop(txtBrowseFolderWrite_IN, e);

        private void txtBrowseFolderWrite_IN_TextChanged(object sender, EventArgs e)
        {
            var en = true;
            try
            {
                lblBrowseFolderWrite_IN.Text = Path.GetFullPath(txtBrowseFolderWrite_IN.Text.Trim());
                if (!Directory.Exists(lblBrowseFolderWrite_IN.Text))
                {
                    en = false;
                }
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(txtBrowseFolderWrite_IN.Text.Trim());
                lblBrowseFolderWrite_IN.Text = "";
                en = false;
            }
            try
            {
                var path = Path.GetFullPath(txtBrowseZZZWrite_OUT.Text.Trim());
                lblBrowseZZZWrite_OUT.Text = Path.Combine(Path.GetDirectoryName(path) ?? throw new InvalidOperationException(), $"{Path.GetFileNameWithoutExtension(path)}.zzz");
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(txtBrowseZZZWrite_OUT.Text.Trim());
                lblBrowseZZZWrite_OUT.Text = "";
                en = false;
            }
            btnExecuteWrite.Enabled = en;
        }

        private void txtBrowseZZZWrite_OUT_DragDrop(object sender, DragEventArgs e) => MultiTxtDragDrop(txtBrowseZZZWrite_OUT, e);

        private void txtMergeSource_DragDrop(object sender, DragEventArgs e) => MultiTxtDragDrop(txtMergeSource, e);

        private void txtMergeSource_TextChanged(object sender, EventArgs e) => CheckMergeExeEnable();

        private void txtZZZ_in_DragOver(object sender, DragEventArgs e) => MultiTxtDragDrop(txtZZZ_in, e);

        private void txtZZZ_in_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.GetFullPath(txtZZZ_in.Text.Trim())))
                {
                    try
                    {
                        lblZZZ_out.Text = Path.GetFullPath(txtZZZ_out.Text.Trim());
                        btnExecuteExtract.Enabled = true;
                        return;
                    }
                    catch (Exception err)
                    {
                        Logger.WriteLine(err.Message);
                        Logger.WriteLine(txtZZZ_out.Text.Trim());
                        lblZZZ_out.Text = "";
                    }
                }
            }
            catch
            {
                // ignored
            }

            btnExecuteExtract.Enabled = false;
        }

        private void txtZZZ_out_DragDrop(object sender, DragEventArgs e) => MultiTxtDragDrop(txtZZZ_out, e);

        #endregion Methods

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void butReadDataBrowse_Click(object sender, EventArgs e)
        {
            if (_openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                txtReadDataZZZ.Text = _openFileDialogBrowse.FileName;
            }
        }

        private void butReadDataRead_Click(object sender, EventArgs e)
        {


            butReadDataBrowse.Enabled = false;
            butReadDataRead.Enabled = false;
            Application.DoEvents();
            //try
            //{
                _zzz.In = new List<string> { Path.GetFullPath(txtReadDataZZZ.Text.Trim()) };

                var head = _zzz.ReadHeader();
                dataGridView1.Rows.Clear();
                foreach (var dat in head.Data)
                {
                    dataGridView1.Rows.Add(dat.Offset, dat.Size, dat.Filename);
                    Application.DoEvents();
                }
            //}
            //catch (Exception err)
            //{
            //    Logger.WriteLine(err.Message);
            //    Logger.WriteLine(txtReadDataZZZ.Text.Trim());
            //    MessageBox.Show(err.Message, "Exception");
            //}
            butReadDataBrowse.Enabled = true;
            butReadDataRead.Enabled = true;
        }
    }
}