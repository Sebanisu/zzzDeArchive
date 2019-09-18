using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zzzDeArchive_WinForms
{
    public partial class Form1 : Form
    {
        private const string SteamRegistyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1026680";
        private const string SteamGamePathTag = @"InstallLocation";
        private const string DefaultSteamPath = @"C:\Program Files(x86)\Steam\steamapps\common\FINAL FANTASY VIII Remastered";

        private OpenFileDialog ofdBrowse;
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
            ofdBrowse = new OpenFileDialog();
            if(GameLocation!=null)
                ofdBrowse.InitialDirectory = GameLocation;
            ofdBrowse.Filter="FF8R ZZZ archives(*.zzz)|*.zzz";
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            ButClick(btnMainExtractIN, txtZZZ_in, "main.zzz");
        }

        private void ButClick(Button but,TextBox txt,string file)
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

        private void btnOtherExtactIN_Click(object sender, EventArgs e)
        {
            ButClick(btnOtherExtactIN, txtZZZ_in, "other.zzz");
        }

        private void btnZZZextractInBrowse_Click(object sender, EventArgs e)
        {
            if (ofdBrowse.ShowDialog() == DialogResult.OK)
            {
                txtZZZ_in.Text = ofdBrowse.FileName;
            }
        }
    }
}
