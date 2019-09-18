using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ZzzFile;
using _Logger;

namespace zzzDeArchive_WinForms
{
    public partial class Form1 : Form
    {
        private const string SteamRegistyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1026680";
        private const string SteamGamePathTag = @"InstallLocation";
        private const string DefaultSteamPath = @"C:\Program Files(x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered";
        
        private OpenFileDialog ofdBrowse;
        private FolderBrowserDialog fbdBrowse;
        private Zzz zzz;
        public static string GameLocation { get; private set; }

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

        public Form1()
        {
            InitializeComponent();
            GameLocation = GetGameLocation();
            btnOtherExtactIN_Click(this, null);
            btnMain_Click(this, null);
            btnOtherWriteOut_Click(this, null);
            btnMainWriteOut_Click(this, null);
            ofdBrowse = new OpenFileDialog();
            if (GameLocation != null)
                ofdBrowse.InitialDirectory = GameLocation;
            ofdBrowse.Filter = "FF8R ZZZ archives(*.zzz)|*.zzz";
            ofdBrowse.Title = "Choose";
            ofdBrowse.CheckFileExists = true;
            ofdBrowse.Multiselect = false;
            btnExtractExecute.Enabled = false;
            btnExcuteWrite.Enabled = false;
            fbdBrowse = new FolderBrowserDialog();
            zzz = new Zzz();
        }

        private void btnMain_Click(object sender, EventArgs e) => ButClick(btnMainExtractIN, txtZZZ_in, "main.zzz");

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

        private void btnOtherExtactIN_Click(object sender, EventArgs e) => ButClick(btnOtherExtactIN, txtZZZ_in, "other.zzz");

        private void btnZZZextractInBrowse_Click(object sender, EventArgs e)
        {
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_in.Text = ofdBrowse.FileName;
            }
        }

        private void txtZZZ_in_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.GetFullPath(txtZZZ_in.Text.Trim())))
                {
                    try
                    {
                        lblZZZ_out.Text = Path.GetFullPath(txtZZZ_out.Text.Trim());
                        btnExtractExecute.Enabled = true;
                        return;
                    }
                    catch
                    {
                        lblZZZ_out.Text = "";
                    }
                }
            }
            catch
            {
            }
            btnExtractExecute.Enabled = false;
        }

        private void btnZZZextractOUTbrowse_Click(object sender, EventArgs e)
        {
            if (fbdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_out.Text = fbdBrowse.SelectedPath;
            }
        }

        private void btnExtractExecute_Click(object sender, EventArgs e)
        {
            btnExtractExecute.Enabled = false;
            var strtmp = btnExtractExecute.Text;
            btnExtractExecute.Text = "Extracting...";
            Application.DoEvents();
            zzz.In = new List<string> { Path.GetFullPath(txtZZZ_in.Text.Trim()) };
            zzz.Path_ = lblZZZ_out.Text;
            openfolder(zzz.Extract());
            btnExtractExecute.Text = strtmp;
            btnExtractExecute.Enabled = true;
        }

        private void openfolder(string folder)
        {
            try
            {
                folder = Path.GetFullPath(folder);
                if (Directory.Exists(folder))
                    Process.Start(folder);
            }
            catch
            {
            }
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

        private void btnMainWriteOut_Click(object sender, EventArgs e)
        {
            ButClick(btnMainWriteOut, txtBrowseZZZWrite_OUT, "main.zzz");
        }
        private void btnOtherWriteOut_Click(object sender, EventArgs e)
        {
            ButClick(btnOtherWriteOut, txtBrowseZZZWrite_OUT, "other.zzz");
        }

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
            catch
            {
                lblBrowseFolderWrite_IN.Text = "";
                en = false;
            }
            try
            {
                string path = Path.GetFullPath(txtBrowseZZZWrite_OUT.Text.Trim());
                lblBrowseZZZWrite_OUT.Text = Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.zzz");
            }
            catch
            {
                txtBrowseZZZWrite_OUT.Text = "";
                en = false;
            }
            btnExcuteWrite.Enabled = en;
        }

        private void btnExcuteWrite_Click(object sender, EventArgs e)
        {
            btnExcuteWrite.Enabled = false;
            var strtmp = btnExcuteWrite.Text;
            btnExcuteWrite.Text = "Writing...";
            Application.DoEvents();
            zzz.Path_ = lblBrowseFolderWrite_IN.Text;
            zzz.Out = Path.GetTempFileName();
            zzz.Write();
            try
            {
                btnExcuteWrite.Text = "Coping...";
                Logger.WriteLine($"Coping {zzz.Out} to {lblBrowseZZZWrite_OUT.Text}");
                Application.DoEvents();
                File.Copy(zzz.Out, lblBrowseZZZWrite_OUT.Text, true);
                openfolder(Path.GetDirectoryName(lblBrowseZZZWrite_OUT.Text));
                Logger.WriteLine($"Deleting {zzz.Out}");
                File.Delete(zzz.Out);
            }
            catch
            {
                openfolder(Path.GetDirectoryName(zzz.Out));
                openfolder(Path.GetDirectoryName(lblBrowseZZZWrite_OUT.Text));
                Logger.WriteLine($"Coping Failed...");
                MessageBox.Show($"File is named:\n{Path.GetFileName(zzz.Out)}", "File Copy Failed");                
            }           
            btnExcuteWrite.Text = strtmp;
            btnExcuteWrite.Enabled = true;
        }

        private void btnMergeSrcBrowse_Click(object sender, EventArgs e)
        {

        }
    }
}