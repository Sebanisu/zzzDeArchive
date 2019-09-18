namespace zzzDeArchive_WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpExtract = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZZZextractInBrowse = new System.Windows.Forms.Button();
            this.btnMainExtractIN = new System.Windows.Forms.Button();
            this.txtZZZ_in = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOtherExtactIN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZZZextractOUTbrowse = new System.Windows.Forms.Button();
            this.txtZZZ_out = new System.Windows.Forms.TextBox();
            this.btnExtractExecute = new System.Windows.Forms.Button();
            this.tpWrite = new System.Windows.Forms.TabPage();
            this.tpMerge = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tpExtract.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpExtract);
            this.tabControl.Controls.Add(this.tpWrite);
            this.tabControl.Controls.Add(this.tpMerge);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl.TabIndex = 0;
            // 
            // tpExtract
            // 
            this.tpExtract.Controls.Add(this.tableLayoutPanel1);
            this.tpExtract.Location = new System.Drawing.Point(4, 22);
            this.tpExtract.Name = "tpExtract";
            this.tpExtract.Padding = new System.Windows.Forms.Padding(3);
            this.tpExtract.Size = new System.Drawing.Size(792, 424);
            this.tpExtract.TabIndex = 0;
            this.tpExtract.Text = "Extract";
            this.tpExtract.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnExtractExecute, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(786, 418);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 167);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose ZZZ to extract";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btnZZZextractInBrowse, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMainExtractIN, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtZZZ_in, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnOtherExtactIN, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(199, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 161);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnZZZextractInBrowse
            // 
            this.btnZZZextractInBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZZZextractInBrowse.Location = new System.Drawing.Point(441, 3);
            this.btnZZZextractInBrowse.Name = "btnZZZextractInBrowse";
            this.btnZZZextractInBrowse.Size = new System.Drawing.Size(140, 47);
            this.btnZZZextractInBrowse.TabIndex = 0;
            this.btnZZZextractInBrowse.Text = "&Browse";
            this.btnZZZextractInBrowse.UseVisualStyleBackColor = true;
            this.btnZZZextractInBrowse.Click += new System.EventHandler(this.btnZZZextractInBrowse_Click);
            // 
            // btnMainExtractIN
            // 
            this.btnMainExtractIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMainExtractIN.Location = new System.Drawing.Point(441, 56);
            this.btnMainExtractIN.Name = "btnMainExtractIN";
            this.btnMainExtractIN.Size = new System.Drawing.Size(140, 47);
            this.btnMainExtractIN.TabIndex = 1;
            this.btnMainExtractIN.Text = "&Set";
            this.btnMainExtractIN.UseVisualStyleBackColor = true;
            this.btnMainExtractIN.Click += new System.EventHandler(this.btnMain_Click);
            // 
            // txtZZZ_in
            // 
            this.txtZZZ_in.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZZZ_in.Location = new System.Drawing.Point(3, 3);
            this.txtZZZ_in.Multiline = true;
            this.txtZZZ_in.Name = "txtZZZ_in";
            this.txtZZZ_in.Size = new System.Drawing.Size(432, 47);
            this.txtZZZ_in.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(432, 53);
            this.label2.TabIndex = 3;
            this.label2.Text = "main.zzz";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(432, 55);
            this.label3.TabIndex = 4;
            this.label3.Text = "other.zzz";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOtherExtactIN
            // 
            this.btnOtherExtactIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOtherExtactIN.Location = new System.Drawing.Point(441, 109);
            this.btnOtherExtactIN.Name = "btnOtherExtactIN";
            this.btnOtherExtactIN.Size = new System.Drawing.Size(140, 49);
            this.btnOtherExtactIN.TabIndex = 5;
            this.btnOtherExtactIN.Text = "Se&t";
            this.btnOtherExtactIN.UseVisualStyleBackColor = true;
            this.btnOtherExtactIN.Click += new System.EventHandler(this.btnOtherExtactIN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 167);
            this.label4.TabIndex = 2;
            this.label4.Text = "Choose a Folder to where you want the files to go";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.btnZZZextractOUTbrowse, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtZZZ_out, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(199, 170);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(584, 161);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // btnZZZextractOUTbrowse
            // 
            this.btnZZZextractOUTbrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZZZextractOUTbrowse.Location = new System.Drawing.Point(441, 3);
            this.btnZZZextractOUTbrowse.Name = "btnZZZextractOUTbrowse";
            this.btnZZZextractOUTbrowse.Size = new System.Drawing.Size(140, 155);
            this.btnZZZextractOUTbrowse.TabIndex = 0;
            this.btnZZZextractOUTbrowse.Text = "B&rowse";
            this.btnZZZextractOUTbrowse.UseVisualStyleBackColor = true;
            // 
            // txtZZZ_out
            // 
            this.txtZZZ_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZZZ_out.Location = new System.Drawing.Point(3, 3);
            this.txtZZZ_out.Multiline = true;
            this.txtZZZ_out.Name = "txtZZZ_out";
            this.txtZZZ_out.Size = new System.Drawing.Size(432, 155);
            this.txtZZZ_out.TabIndex = 1;
            // 
            // btnExtractExecute
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnExtractExecute, 2);
            this.btnExtractExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtractExecute.Location = new System.Drawing.Point(3, 337);
            this.btnExtractExecute.Name = "btnExtractExecute";
            this.btnExtractExecute.Size = new System.Drawing.Size(780, 78);
            this.btnExtractExecute.TabIndex = 4;
            this.btnExtractExecute.Text = "E&xecute";
            this.btnExtractExecute.UseVisualStyleBackColor = true;
            // 
            // tpWrite
            // 
            this.tpWrite.Location = new System.Drawing.Point(4, 22);
            this.tpWrite.Name = "tpWrite";
            this.tpWrite.Padding = new System.Windows.Forms.Padding(3);
            this.tpWrite.Size = new System.Drawing.Size(792, 424);
            this.tpWrite.TabIndex = 1;
            this.tpWrite.Text = "Write";
            this.tpWrite.UseVisualStyleBackColor = true;
            // 
            // tpMerge
            // 
            this.tpMerge.Location = new System.Drawing.Point(4, 22);
            this.tpMerge.Name = "tpMerge";
            this.tpMerge.Padding = new System.Windows.Forms.Padding(3);
            this.tpMerge.Size = new System.Drawing.Size(792, 424);
            this.tpMerge.TabIndex = 2;
            this.tpMerge.Text = "Merge";
            this.tpMerge.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "zzzDeArchive";
            this.tabControl.ResumeLayout(false);
            this.tpExtract.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpExtract;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpWrite;
        private System.Windows.Forms.TabPage tpMerge;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnZZZextractInBrowse;
        private System.Windows.Forms.Button btnMainExtractIN;
        private System.Windows.Forms.TextBox txtZZZ_in;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOtherExtactIN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnZZZextractOUTbrowse;
        private System.Windows.Forms.TextBox txtZZZ_out;
        private System.Windows.Forms.Button btnExtractExecute;
    }
}

