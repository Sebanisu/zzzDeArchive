namespace ZzzArchive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.lblZZZ_out = new System.Windows.Forms.Label();
            this.btnExecuteExtract = new System.Windows.Forms.Button();
            this.tpWrite = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBrowseZZZWrite_OUT = new System.Windows.Forms.Button();
            this.btnMainWriteOut = new System.Windows.Forms.Button();
            this.txtBrowseZZZWrite_OUT = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnOtherWriteOut = new System.Windows.Forms.Button();
            this.lblBrowseZZZWrite_OUT = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBrowseFolderWrite_IN = new System.Windows.Forms.Button();
            this.txtBrowseFolderWrite_IN = new System.Windows.Forms.TextBox();
            this.lblBrowseFolderWrite_IN = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnExecuteWrite = new System.Windows.Forms.Button();
            this.tpMerge = new System.Windows.Forms.TabPage();
            this.btnMergeExecute = new System.Windows.Forms.TableLayoutPanel();
            this.btnExcuteMerge = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSrcOther = new System.Windows.Forms.Button();
            this.btnSrcMain = new System.Windows.Forms.Button();
            this.txtMergeSource = new System.Windows.Forms.TextBox();
            this.btnMergeSrcBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.lvMergeInput = new System.Windows.Forms.ListView();
            this.btnMergeInputBrowse = new System.Windows.Forms.Button();
            this.btnMergeInputRemove = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tpExtract.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tpWrite.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tpMerge.SuspendLayout();
            this.btnMergeExecute.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.btnExecuteExtract, 0, 2);
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
            this.btnZZZextractInBrowse.Click += new System.EventHandler(this.btnZZZExtractInBrowse_Click);
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
            this.txtZZZ_in.AllowDrop = true;
            this.txtZZZ_in.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZZZ_in.Location = new System.Drawing.Point(3, 3);
            this.txtZZZ_in.Multiline = true;
            this.txtZZZ_in.Name = "txtZZZ_in";
            this.txtZZZ_in.Size = new System.Drawing.Size(432, 47);
            this.txtZZZ_in.TabIndex = 2;
            this.txtZZZ_in.TextChanged += new System.EventHandler(this.txtZZZ_in_TextChanged);
            this.txtZZZ_in.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            this.txtZZZ_in.DragOver += new System.Windows.Forms.DragEventHandler(this.txtZZZ_in_DragOver);
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
            this.btnOtherExtactIN.Click += new System.EventHandler(this.btnOtherExtractIN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 167);
            this.label4.TabIndex = 2;
            this.label4.Text = "Choose a Folder to where you want the files to go.\r\n\r\nThis will overwrite existin" +
    "g files.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.btnZZZextractOUTbrowse, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtZZZ_out, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblZZZ_out, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(199, 170);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(584, 161);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // btnZZZextractOUTbrowse
            // 
            this.btnZZZextractOUTbrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZZZextractOUTbrowse.Location = new System.Drawing.Point(441, 3);
            this.btnZZZextractOUTbrowse.Name = "btnZZZextractOUTbrowse";
            this.tableLayoutPanel3.SetRowSpan(this.btnZZZextractOUTbrowse, 2);
            this.btnZZZextractOUTbrowse.Size = new System.Drawing.Size(140, 155);
            this.btnZZZextractOUTbrowse.TabIndex = 0;
            this.btnZZZextractOUTbrowse.Text = "B&rowse";
            this.btnZZZextractOUTbrowse.UseVisualStyleBackColor = true;
            this.btnZZZextractOUTbrowse.Click += new System.EventHandler(this.btnZZZExtractOUTBrowse_Click);
            // 
            // txtZZZ_out
            // 
            this.txtZZZ_out.AllowDrop = true;
            this.txtZZZ_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZZZ_out.Location = new System.Drawing.Point(3, 3);
            this.txtZZZ_out.Multiline = true;
            this.txtZZZ_out.Name = "txtZZZ_out";
            this.txtZZZ_out.Size = new System.Drawing.Size(432, 122);
            this.txtZZZ_out.TabIndex = 1;
            this.txtZZZ_out.TextChanged += new System.EventHandler(this.txtZZZ_in_TextChanged);
            this.txtZZZ_out.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtZZZ_out_DragDrop);
            this.txtZZZ_out.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            // 
            // lblZZZ_out
            // 
            this.lblZZZ_out.AutoSize = true;
            this.lblZZZ_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblZZZ_out.Location = new System.Drawing.Point(3, 128);
            this.lblZZZ_out.Name = "lblZZZ_out";
            this.lblZZZ_out.Size = new System.Drawing.Size(432, 33);
            this.lblZZZ_out.TabIndex = 2;
            // 
            // btnExtractExecute
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnExecuteExtract, 2);
            this.btnExecuteExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExecuteExtract.Enabled = false;
            this.btnExecuteExtract.Location = new System.Drawing.Point(3, 337);
            this.btnExecuteExtract.Name = "btnExtractExecute";
            this.btnExecuteExtract.Size = new System.Drawing.Size(780, 78);
            this.btnExecuteExtract.TabIndex = 4;
            this.btnExecuteExtract.Text = "E&xecute";
            this.btnExecuteExtract.UseVisualStyleBackColor = true;
            this.btnExecuteExtract.Click += new System.EventHandler(this.btnExecuteExtract_Click);
            // 
            // tpWrite
            // 
            this.tpWrite.Controls.Add(this.tableLayoutPanel4);
            this.tpWrite.Location = new System.Drawing.Point(4, 22);
            this.tpWrite.Name = "tpWrite";
            this.tpWrite.Padding = new System.Windows.Forms.Padding(3);
            this.tpWrite.Size = new System.Drawing.Size(792, 424);
            this.tpWrite.TabIndex = 1;
            this.tpWrite.Text = "Write";
            this.tpWrite.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnExecuteWrite, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(786, 418);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Controls.Add(this.btnBrowseZZZWrite_OUT, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnMainWriteOut, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.txtBrowseZZZWrite_OUT, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.btnOtherWriteOut, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.lblBrowseZZZWrite_OUT, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(199, 128);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(584, 203);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // btnBrowseZZZWrite_OUT
            // 
            this.btnBrowseZZZWrite_OUT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseZZZWrite_OUT.Location = new System.Drawing.Point(441, 3);
            this.btnBrowseZZZWrite_OUT.Name = "btnBrowseZZZWrite_OUT";
            this.btnBrowseZZZWrite_OUT.Size = new System.Drawing.Size(140, 75);
            this.btnBrowseZZZWrite_OUT.TabIndex = 0;
            this.btnBrowseZZZWrite_OUT.Text = "&Browse";
            this.btnBrowseZZZWrite_OUT.UseVisualStyleBackColor = true;
            this.btnBrowseZZZWrite_OUT.Click += new System.EventHandler(this.btnBrowseZZZWrite_OUT_Click);
            // 
            // btnMainWriteOut
            // 
            this.btnMainWriteOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMainWriteOut.Location = new System.Drawing.Point(441, 84);
            this.btnMainWriteOut.Name = "btnMainWriteOut";
            this.tableLayoutPanel6.SetRowSpan(this.btnMainWriteOut, 2);
            this.btnMainWriteOut.Size = new System.Drawing.Size(140, 74);
            this.btnMainWriteOut.TabIndex = 1;
            this.btnMainWriteOut.Text = "&Set";
            this.btnMainWriteOut.UseVisualStyleBackColor = true;
            this.btnMainWriteOut.Click += new System.EventHandler(this.btnMainWriteOut_Click);
            // 
            // txtBrowseZZZWrite_OUT
            // 
            this.txtBrowseZZZWrite_OUT.AllowDrop = true;
            this.txtBrowseZZZWrite_OUT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBrowseZZZWrite_OUT.Location = new System.Drawing.Point(3, 3);
            this.txtBrowseZZZWrite_OUT.Multiline = true;
            this.txtBrowseZZZWrite_OUT.Name = "txtBrowseZZZWrite_OUT";
            this.txtBrowseZZZWrite_OUT.Size = new System.Drawing.Size(432, 75);
            this.txtBrowseZZZWrite_OUT.TabIndex = 2;
            this.txtBrowseZZZWrite_OUT.TextChanged += new System.EventHandler(this.txtBrowseFolderWrite_IN_TextChanged);
            this.txtBrowseZZZWrite_OUT.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBrowseZZZWrite_OUT_DragDrop);
            this.txtBrowseZZZWrite_OUT.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(432, 40);
            this.label7.TabIndex = 3;
            this.label7.Text = "main.zzz";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(432, 42);
            this.label9.TabIndex = 4;
            this.label9.Text = "other.zzz";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOtherWriteOut
            // 
            this.btnOtherWriteOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOtherWriteOut.Location = new System.Drawing.Point(441, 164);
            this.btnOtherWriteOut.Name = "btnOtherWriteOut";
            this.btnOtherWriteOut.Size = new System.Drawing.Size(140, 36);
            this.btnOtherWriteOut.TabIndex = 5;
            this.btnOtherWriteOut.Text = "Se&t";
            this.btnOtherWriteOut.UseVisualStyleBackColor = true;
            // 
            // lblBrowseZZZWrite_OUT
            // 
            this.lblBrowseZZZWrite_OUT.AutoSize = true;
            this.lblBrowseZZZWrite_OUT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrowseZZZWrite_OUT.Location = new System.Drawing.Point(3, 81);
            this.lblBrowseZZZWrite_OUT.Name = "lblBrowseZZZWrite_OUT";
            this.lblBrowseZZZWrite_OUT.Size = new System.Drawing.Size(432, 40);
            this.lblBrowseZZZWrite_OUT.TabIndex = 6;
            this.lblBrowseZZZWrite_OUT.Text = "label10";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.btnBrowseFolderWrite_IN, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtBrowseFolderWrite_IN, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblBrowseFolderWrite_IN, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(199, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(584, 119);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // btnBrowseFolderWrite_IN
            // 
            this.btnBrowseFolderWrite_IN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseFolderWrite_IN.Location = new System.Drawing.Point(441, 3);
            this.btnBrowseFolderWrite_IN.Name = "btnBrowseFolderWrite_IN";
            this.tableLayoutPanel5.SetRowSpan(this.btnBrowseFolderWrite_IN, 2);
            this.btnBrowseFolderWrite_IN.Size = new System.Drawing.Size(140, 113);
            this.btnBrowseFolderWrite_IN.TabIndex = 0;
            this.btnBrowseFolderWrite_IN.Text = "B&rowse";
            this.btnBrowseFolderWrite_IN.UseVisualStyleBackColor = true;
            this.btnBrowseFolderWrite_IN.Click += new System.EventHandler(this.btnBrowseFolderWrite_IN_Click);
            // 
            // txtBrowseFolderWrite_IN
            // 
            this.txtBrowseFolderWrite_IN.AllowDrop = true;
            this.txtBrowseFolderWrite_IN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBrowseFolderWrite_IN.Location = new System.Drawing.Point(3, 3);
            this.txtBrowseFolderWrite_IN.Multiline = true;
            this.txtBrowseFolderWrite_IN.Name = "txtBrowseFolderWrite_IN";
            this.txtBrowseFolderWrite_IN.Size = new System.Drawing.Size(432, 80);
            this.txtBrowseFolderWrite_IN.TabIndex = 1;
            this.txtBrowseFolderWrite_IN.TextChanged += new System.EventHandler(this.txtBrowseFolderWrite_IN_TextChanged);
            this.txtBrowseFolderWrite_IN.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBrowseFolderWrite_IN_DragDrop);
            this.txtBrowseFolderWrite_IN.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            // 
            // lblBrowseFolderWrite_IN
            // 
            this.lblBrowseFolderWrite_IN.AutoSize = true;
            this.lblBrowseFolderWrite_IN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrowseFolderWrite_IN.Location = new System.Drawing.Point(3, 86);
            this.lblBrowseFolderWrite_IN.Name = "lblBrowseFolderWrite_IN";
            this.lblBrowseFolderWrite_IN.Size = new System.Drawing.Size(432, 33);
            this.lblBrowseFolderWrite_IN.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 125);
            this.label5.TabIndex = 0;
            this.label5.Text = "Choose folder to create a ZZZ file from";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(190, 209);
            this.label8.TabIndex = 2;
            this.label8.Text = resources.GetString("label8.Text");
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnExcuteWrite
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.btnExecuteWrite, 2);
            this.btnExecuteWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExecuteWrite.Enabled = false;
            this.btnExecuteWrite.Location = new System.Drawing.Point(3, 337);
            this.btnExecuteWrite.Name = "btnExcuteWrite";
            this.btnExecuteWrite.Size = new System.Drawing.Size(780, 78);
            this.btnExecuteWrite.TabIndex = 4;
            this.btnExecuteWrite.Text = "E&xecute";
            this.btnExecuteWrite.UseVisualStyleBackColor = true;
            this.btnExecuteWrite.Click += new System.EventHandler(this.btnExecuteWrite_Click);
            // 
            // tpMerge
            // 
            this.tpMerge.Controls.Add(this.btnMergeExecute);
            this.tpMerge.Location = new System.Drawing.Point(4, 22);
            this.tpMerge.Name = "tpMerge";
            this.tpMerge.Padding = new System.Windows.Forms.Padding(3);
            this.tpMerge.Size = new System.Drawing.Size(792, 424);
            this.tpMerge.TabIndex = 2;
            this.tpMerge.Text = "Merge";
            this.tpMerge.UseVisualStyleBackColor = true;
            // 
            // btnMergeExecute
            // 
            this.btnMergeExecute.ColumnCount = 1;
            this.btnMergeExecute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.btnMergeExecute.Controls.Add(this.btnExcuteMerge, 0, 1);
            this.btnMergeExecute.Controls.Add(this.tabControl1, 0, 0);
            this.btnMergeExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMergeExecute.Location = new System.Drawing.Point(3, 3);
            this.btnMergeExecute.Name = "btnMergeExecute";
            this.btnMergeExecute.RowCount = 2;
            this.btnMergeExecute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.btnMergeExecute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.btnMergeExecute.Size = new System.Drawing.Size(786, 418);
            this.btnMergeExecute.TabIndex = 0;
            // 
            // btnExcuteMerge
            // 
            this.btnMergeExecute.SetColumnSpan(this.btnExcuteMerge, 2);
            this.btnExcuteMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExcuteMerge.Enabled = false;
            this.btnExcuteMerge.Location = new System.Drawing.Point(3, 316);
            this.btnExcuteMerge.Name = "btnExcuteMerge";
            this.btnExcuteMerge.Size = new System.Drawing.Size(780, 99);
            this.btnExcuteMerge.TabIndex = 5;
            this.btnExcuteMerge.Text = "E&xecute";
            this.btnExcuteMerge.UseVisualStyleBackColor = true;
            this.btnExcuteMerge.Click += new System.EventHandler(this.btnExecuteMerge_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(780, 307);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(772, 281);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source ZZZ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel7.Controls.Add(this.btnSrcOther, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.btnSrcMain, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.txtMergeSource, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnMergeSrcBrowse, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(766, 275);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // btnSrcOther
            // 
            this.btnSrcOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSrcOther.Location = new System.Drawing.Point(507, 209);
            this.btnSrcOther.Name = "btnSrcOther";
            this.btnSrcOther.Size = new System.Drawing.Size(256, 63);
            this.btnSrcOther.TabIndex = 3;
            this.btnSrcOther.Text = "Other";
            this.btnSrcOther.UseVisualStyleBackColor = true;
            this.btnSrcOther.Click += new System.EventHandler(this.btnSrcOther_Click);
            // 
            // btnSrcMain
            // 
            this.btnSrcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSrcMain.Location = new System.Drawing.Point(255, 209);
            this.btnSrcMain.Name = "btnSrcMain";
            this.btnSrcMain.Size = new System.Drawing.Size(246, 63);
            this.btnSrcMain.TabIndex = 2;
            this.btnSrcMain.Text = "Main";
            this.btnSrcMain.UseVisualStyleBackColor = true;
            this.btnSrcMain.Click += new System.EventHandler(this.btnSrcMain_Click);
            // 
            // txtMergeSource
            // 
            this.txtMergeSource.AllowDrop = true;
            this.tableLayoutPanel7.SetColumnSpan(this.txtMergeSource, 2);
            this.txtMergeSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMergeSource.Location = new System.Drawing.Point(255, 3);
            this.txtMergeSource.Multiline = true;
            this.txtMergeSource.Name = "txtMergeSource";
            this.txtMergeSource.Size = new System.Drawing.Size(508, 200);
            this.txtMergeSource.TabIndex = 0;
            this.txtMergeSource.TextChanged += new System.EventHandler(this.txtMergeSource_TextChanged);
            this.txtMergeSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMergeSource_DragDrop);
            this.txtMergeSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            // 
            // btnMergeSrcBrowse
            // 
            this.btnMergeSrcBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMergeSrcBrowse.Location = new System.Drawing.Point(3, 209);
            this.btnMergeSrcBrowse.Name = "btnMergeSrcBrowse";
            this.btnMergeSrcBrowse.Size = new System.Drawing.Size(246, 63);
            this.btnMergeSrcBrowse.TabIndex = 1;
            this.btnMergeSrcBrowse.Text = "Browse";
            this.btnMergeSrcBrowse.UseVisualStyleBackColor = true;
            this.btnMergeSrcBrowse.Click += new System.EventHandler(this.btnMergeSrcBrowse_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(246, 206);
            this.label6.TabIndex = 4;
            this.label6.Text = "Select the source zzz file you want to merge other zzz files into.\r\n\r\nThis will o" +
    "verwrite the original.";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(772, 281);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Input ZZZ Files";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel8.Controls.Add(this.lvMergeInput, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnMergeInputBrowse, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.btnMergeInputRemove, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(766, 275);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // lvMergeInput
            // 
            this.lvMergeInput.AllowDrop = true;
            this.tableLayoutPanel8.SetColumnSpan(this.lvMergeInput, 2);
            this.lvMergeInput.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvMergeInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMergeInput.HideSelection = false;
            this.lvMergeInput.Location = new System.Drawing.Point(255, 3);
            this.lvMergeInput.Name = "lvMergeInput";
            this.lvMergeInput.Size = new System.Drawing.Size(508, 214);
            this.lvMergeInput.TabIndex = 3;
            this.lvMergeInput.UseCompatibleStateImageBehavior = false;
            this.lvMergeInput.View = System.Windows.Forms.View.SmallIcon;
            this.lvMergeInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvMergeInput_DragDrop);
            this.lvMergeInput.DragEnter += new System.Windows.Forms.DragEventHandler(this.Item_DragEnter);
            this.lvMergeInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvMergeInput_KeyUp);
            // 
            // btnMergeInputBrowse
            // 
            this.btnMergeInputBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMergeInputBrowse.Location = new System.Drawing.Point(255, 223);
            this.btnMergeInputBrowse.Name = "btnMergeInputBrowse";
            this.btnMergeInputBrowse.Size = new System.Drawing.Size(246, 49);
            this.btnMergeInputBrowse.TabIndex = 4;
            this.btnMergeInputBrowse.Text = "Browse";
            this.btnMergeInputBrowse.UseVisualStyleBackColor = true;
            this.btnMergeInputBrowse.Click += new System.EventHandler(this.btnMergeInputBrowse_Click);
            // 
            // btnMergeInputRemove
            // 
            this.btnMergeInputRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMergeInputRemove.Location = new System.Drawing.Point(507, 223);
            this.btnMergeInputRemove.Name = "btnMergeInputRemove";
            this.btnMergeInputRemove.Size = new System.Drawing.Size(256, 49);
            this.btnMergeInputRemove.TabIndex = 5;
            this.btnMergeInputRemove.Text = "Remove";
            this.btnMergeInputRemove.UseVisualStyleBackColor = true;
            this.btnMergeInputRemove.Click += new System.EventHandler(this.btnMergeInputRemove_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.tableLayoutPanel8.SetRowSpan(this.label10, 2);
            this.label10.Size = new System.Drawing.Size(246, 275);
            this.label10.TabIndex = 6;
            this.label10.Text = "Mod zzz files to be merged into the source file.";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.Text = "zzzDeArchive 0.1.7.5";
            this.tabControl.ResumeLayout(false);
            this.tpExtract.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tpWrite.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tpMerge.ResumeLayout(false);
            this.btnMergeExecute.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
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
        private System.Windows.Forms.Button btnExecuteExtract;
        private System.Windows.Forms.Label lblZZZ_out;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnBrowseZZZWrite_OUT;
        private System.Windows.Forms.Button btnMainWriteOut;
        private System.Windows.Forms.TextBox txtBrowseZZZWrite_OUT;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnOtherWriteOut;
        private System.Windows.Forms.Label lblBrowseZZZWrite_OUT;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnBrowseFolderWrite_IN;
        private System.Windows.Forms.TextBox txtBrowseFolderWrite_IN;
        private System.Windows.Forms.Label lblBrowseFolderWrite_IN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExecuteWrite;
        private System.Windows.Forms.TableLayoutPanel btnMergeExecute;
        private System.Windows.Forms.Button btnExcuteMerge;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button btnSrcOther;
        private System.Windows.Forms.Button btnSrcMain;
        private System.Windows.Forms.TextBox txtMergeSource;
        private System.Windows.Forms.Button btnMergeSrcBrowse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.ListView lvMergeInput;
        private System.Windows.Forms.Button btnMergeInputBrowse;
        private System.Windows.Forms.Button btnMergeInputRemove;
        private System.Windows.Forms.Label label10;
    }
}

