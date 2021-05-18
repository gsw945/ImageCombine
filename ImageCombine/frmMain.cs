using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ImageCombine
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// refer: https://stackoverflow.com/questions/50860392/how-to-combine-two-images/50886285#50886285
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPickImages_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show("操作提示", "请设置 保存文件名", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOutput.Focus();
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] filePathes = openFileDialog.FileNames;
                string outputPath = Utils.GetRealPath(txtOutput.Text);
                ImageHelper.Combin(outputPath, filePathes);
            }
        }

        private void btnOutputDir_Click(object sender, EventArgs e)
        {
            string outputPath = Utils.GetRealPath(txtOutput.Text);
            Utils.OpenExplorer(File.Exists(outputPath) ? outputPath : Utils.GetExecuteDir());
        }
    }
}
