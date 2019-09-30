using ZzzArchive;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZzzArchive
{
    public partial class Form1 : Form
    {
        #region Fields

        private const string DefaultSteamPath = @"C:\Program Files(x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered";
        private const string SteamGamePathTag = @"InstallLocation";
        private const string SteamRegistyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1026680";
        private FolderBrowserDialog fbdBrowse;
        private OpenFileDialog ofdBrowse;
        private Zzz zzz;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Detect the location of FF8R
        /// </summary>
        /// <returns>string path</returns>
        /// <see cref="https://github.com/MaKiPL/OpenVIII/blob/master/Core/Sources/Environment/Game/Location/WindowsGameLocationProvider.cs"/>
        private static string GetGameLocation()
        {
            foreach (RegistryView registryView in new RegistryView[] { RegistryView.Registry32, RegistryView.Registry64 })
            {
                using (RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
                using (RegistryKey registryKey = localMachine.OpenSubKey(SteamRegistyPath))
                {
                    if (registryKey != null)
                    {
                        string installLocation = (string)registryKey.GetValue(SteamGamePathTag);
                        string dataPath = installLocation;
                        if (Directory.Exists(dataPath))
                            return dataPath;
                    }
                }
            }
            if (Directory.Exists(DefaultSteamPath))
                return DefaultSteamPath;
            return null;
        }

        private void btnBrowseFolderWrite_IN_Click(object sender, EventArgs e)
        {
            if (fbdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtBrowseFolderWrite_IN.Text = fbdBrowse.SelectedPath;
            }
        }

        private void btnBrowseZZZWrite_OUT_Click(object sender, EventArgs e)
        {
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtBrowseZZZWrite_OUT.Text = ofdBrowse.FileName;
            }
        }

        private void btnExcuteMerge_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtMergeSource.Text))
            {
                HashSet<string> @in = new HashSet<string>();
                foreach (ListViewItem item in lvMergeInput.Items)
                {
                    if (File.Exists((string)item.Tag))
                        @in.Add((string)item.Tag);
                }
                if (@in.Count > 0)
                {
                    btnMergeExecute.Enabled = false;
                    string strtmp = btnMergeExecute.Text;
                    btnMergeExecute.Text = "Merging...";
                    Application.DoEvents();
                    zzz.In = @in.ToList();
                    zzz.Path_ = txtMergeSource.Text;
                    zzz.Out = Path.GetTempFileName();
                    try
                    {
                        zzz.Merge();
                        try
                        {
                            btnMergeExecute.Text = "Coping...";
                            Application.DoEvents();
                            Logger.WriteLine($"Coping {zzz.Out} to {zzz.Path_}");
                            File.Copy(zzz.Out, lblBrowseZZZWrite_OUT.Text, true);
                            openfolder(zzz.Path_);
                            Logger.WriteLine($"Deleting {zzz.Out}");
                            File.Delete(zzz.Out);
                        }
                        catch (Exception err)
                        {
                            openfolder(zzz.Out);
                            openfolder(Path.GetDirectoryName(zzz.Path_));
                            Logger.WriteLine($"Coping Failed...");
                            MessageBox.Show($"File is named:\n{Path.GetFileName(zzz.Out)}\n{err.Message}", "File Copy Failed");
                        }
                    }
                    catch(PathTooLongException err0)
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
                    btnMergeExecute.Text = strtmp;
                    btnMergeExecute.Enabled = true;
                }
            }
        }

        private void btnExecuteWrite_Click(object sender, EventArgs e)
        {
            btnExecuteWrite.Enabled = false;
            string strtmp = btnExecuteWrite.Text;
            btnExecuteWrite.Text = "Writing...";
            Application.DoEvents();
            zzz.Path_ = lblBrowseFolderWrite_IN.Text;
            zzz.Out = Path.GetTempFileName();
            try
            {
                zzz.Write();
                try
                {
                    btnExecuteWrite.Text = "Coping...";
                    Logger.WriteLine($"Coping {zzz.Out} to {lblBrowseZZZWrite_OUT.Text}");
                    Application.DoEvents();
                    File.Copy(zzz.Out, lblBrowseZZZWrite_OUT.Text, true);
                    openfolder(lblBrowseZZZWrite_OUT.Text);
                    Logger.WriteLine($"Deleting {zzz.Out}");
                    File.Delete(zzz.Out);
                }
                catch (Exception err)
                {
                    openfolder(zzz.Out);
                    openfolder(Path.GetDirectoryName(lblBrowseZZZWrite_OUT.Text));
                    Logger.WriteLine($"Coping Failed...");
                    MessageBox.Show($"File is named:\n{Path.GetFileName(zzz.Out)}\n{err.Message}", "File Copy Failed");
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
            btnExecuteWrite.Text = strtmp;
            btnExecuteWrite.Enabled = true;
        }

        private void btnExecuteExtract_Click(object sender, EventArgs e)
        {
            btnExecuteExtract.Enabled = false;
            string strtmp = btnExecuteExtract.Text;
            btnExecuteExtract.Text = "Extracting...";
            Application.DoEvents();
            try
            {
                zzz.In = new List<string> { Path.GetFullPath(txtZZZ_in.Text.Trim()) };
                zzz.Path_ = lblZZZ_out.Text;
                openfolder(zzz.Extract());
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(txtZZZ_in.Text.Trim());
                MessageBox.Show(err.Message, "Exception");
            }
            btnExecuteExtract.Text = strtmp;
            btnExecuteExtract.Enabled = true;
        }

        private void btnMain_Click(object sender, EventArgs e) => ButClick(btnMainExtractIN, txtZZZ_in, "main.zzz");

        private void btnMainWriteOut_Click(object sender, EventArgs e) => ButClick(btnMainWriteOut, txtBrowseZZZWrite_OUT, "main.zzz");

        private void btnMergeInputBrowse_Click(object sender, EventArgs e)
        {
            ofdBrowse.Multiselect = true;
            string str = ofdBrowse.InitialDirectory;
            ofdBrowse.InitialDirectory = Directory.GetCurrentDirectory();
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofdBrowse.FileNames)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(file))
                    {
                        Tag = file
                    };
                    lvMergeInput.Items.Add(item);
                }
            }
            ofdBrowse.InitialDirectory = str;
            ofdBrowse.Multiselect = false;
            Checkmergeexeenable();
        }

        private void btnMergeInputRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvMergeInput.SelectedItems)
            {
                lvMergeInput.Items.Remove(item);
            }
            Checkmergeexeenable();
        }

        private void btnMergeSrcBrowse_Click(object sender, EventArgs e)
        {
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtMergeSource.Text = ofdBrowse.FileName;
            }
        }

        private void btnOtherExtactIN_Click(object sender, EventArgs e) => ButClick(btnOtherExtactIN, txtZZZ_in, "other.zzz");

        private void btnOtherWriteOut_Click(object sender, EventArgs e) => ButClick(btnOtherWriteOut, txtBrowseZZZWrite_OUT, "other.zzz");

        private void btnSrcMain_Click(object sender, EventArgs e) => ButClick(btnSrcMain, txtMergeSource, "main.zzz");

        private void btnSrcOther_Click(object sender, EventArgs e) => ButClick(btnSrcOther, txtMergeSource, "other.zzz");

        private void btnZZZextractInBrowse_Click(object sender, EventArgs e)
        {
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_in.Text = ofdBrowse.FileName;
            }
        }

        private void btnZZZextractOUTbrowse_Click(object sender, EventArgs e)
        {
            if (fbdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_out.Text = fbdBrowse.SelectedPath;
            }
        }

        private void ButClick(Button but, TextBox txt, string file)
        {
            if (GameLocation != null)
            {
                string path = Path.Combine(GameLocation, file);
                if (File.Exists(path))
                {
                    txt.Text = path;
                    return;
                }
            }
            if (but != null)
                but.Enabled = false;
        }

        private void Checkmergeexeenable()
        {
            if (lvMergeInput.Items.Count > 0 && File.Exists(txtMergeSource.Text))
            {
                btnExcuteMerge.Enabled = true;
            }
            else btnExcuteMerge.Enabled = false;
        }

        private void Item_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lvMergeInput_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(file))
                    {
                        Tag = file
                    };
                    lvMergeInput.Items.Add(item);
                }
            }
            Checkmergeexeenable();
        }

        private void lvMergeInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keys.Delete == e.KeyCode)
                btnMergeInputRemove_Click(sender, e);
        }

        private void multitxtDragDrop(TextBox txt, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                txt.Text = files[0];
            }
        }

        private void openfolder(string folder,bool select = false)
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
                    string args = string.Format("/e, /select, \"{0}\"", folder);
                    Process.Start("explorer", args);

                }
            }
            catch (Exception err)
            {
                Logger.WriteLine(err.Message);
                Logger.WriteLine(folder);
            }
        }

        private void txtBrowseFolderWrite_IN_DragDrop(object sender, DragEventArgs e) => multitxtDragDrop(txtBrowseFolderWrite_IN, e);

        private void txtBrowseFolderWrite_IN_TextChanged(object sender, EventArgs e)
        {
            bool en = true;
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
                string path = Path.GetFullPath(txtBrowseZZZWrite_OUT.Text.Trim());
                lblBrowseZZZWrite_OUT.Text = Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.zzz");
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

        private void txtBrowseZZZWrite_OUT_DragDrop(object sender, DragEventArgs e) => multitxtDragDrop(txtBrowseZZZWrite_OUT, e);

        private void txtMergeSource_DragDrop(object sender, DragEventArgs e) => multitxtDragDrop(txtMergeSource, e);

        private void txtMergeSource_TextChanged(object sender, EventArgs e) => Checkmergeexeenable();

        private void txtZZZ_in_DragOver(object sender, DragEventArgs e) => multitxtDragDrop(txtZZZ_in, e);

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
            }
            btnExecuteExtract.Enabled = false;
        }

        private void txtZZZ_out_DragDrop(object sender, DragEventArgs e) => multitxtDragDrop(txtZZZ_out, e);

        #endregion Methods

        #region Constructors

        public Form1()
        {
            InitializeComponent();
            GameLocation = GetGameLocation();
            btnOtherExtactIN_Click(this, null);
            btnMain_Click(this, null);
            btnOtherWriteOut_Click(this, null);
            btnMainWriteOut_Click(this, null);
            btnSrcOther_Click(this, null);
            btnSrcMain_Click(this, null);
            ofdBrowse = new OpenFileDialog();
            if (GameLocation != null)
                ofdBrowse.InitialDirectory = GameLocation;
            ofdBrowse.Filter = "FF8R ZZZ archives(*.zzz)|*.zzz";
            ofdBrowse.Title = "Choose";
            ofdBrowse.CheckFileExists = true;
            ofdBrowse.Multiselect = false;
            btnExecuteExtract.Enabled = false;
            btnExecuteWrite.Enabled = false;
            fbdBrowse = new FolderBrowserDialog();
            zzz = new Zzz();

            ColumnHeader columnHeader1 = new ColumnHeader
            {
                Text = "Filenames"
            };
            lvMergeInput.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_ApplicationExit(object sender, EventArgs e) => Logger.DisposeChildren();

        #endregion Constructors

        #region Properties

        public static string GameLocation { get; private set; }

        #endregion Properties
    }
}