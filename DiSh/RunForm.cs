using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiSh
{
    public partial class RunForm : Form
    {
        private static List<(string, string)> _apps = new List<(string, string)>();

        public static void Populate()
        {
            foreach (var file in Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "*", SearchOption.AllDirectories))
            {
                _apps.Add((Path.GetFileNameWithoutExtension(file), file));
            }
        }

        public RunForm()
        {
            InitializeComponent();
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.AllSystemSources;
        }

        private void RunForm_Shown(object sender, EventArgs e)
        {
            Win32.SetForegroundWindow(Handle);
            textBox1.Focus();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var text = textBox1.Text;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Close();
                    try
                    {
                        var p = new ProcessStartInfo
                        {
                            FileName = text,
                            UseShellExecute = true,
                        };
                        Process.Start(p);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Oh no!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }
    }
}

