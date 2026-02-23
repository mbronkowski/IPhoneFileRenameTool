using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace IPhoneFileRenameTool
{
    partial class IPhoneFileRanameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtFolder = new TextBox();
            txtPostfix = new TextBox();
            label3 = new Label();
            txtFormat = new TextBox();
            label4 = new Label();
            lblFormatPreview = new Label();
            btnRename = new Button();
            btnSelectFolder = new Button();
            chkConvert = new CheckBox();
            label2 = new Label();
            button1 = new Button();
            numHourOffset = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            chkRecursive = new CheckBox();
            labelDeleteAAE = new Label();
            chkDeleteAAE = new CheckBox();
            progressBar = new ProgressBar();
            lblCurrentFile = new Label();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numHourOffset).BeginInit();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(23, 15);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 0;
            label1.Text = "Folder";
            //
            // txtFolder
            //
            txtFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFolder.Location = new Point(169, 12);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(372, 27);
            txtFolder.TabIndex = 1;
            txtFolder.Text = "D:\\Zdjęcia\\2023\\Telefon Michała\\";
            //
            // txtPostfix
            //
            txtPostfix.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPostfix.Location = new Point(169, 45);
            txtPostfix.Name = "txtPostfix";
            txtPostfix.Size = new Size(405, 27);
            txtPostfix.TabIndex = 4;
            txtPostfix.Text = "_Michał";
            txtPostfix.TextChanged += txtFormat_TextChanged;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(23, 45);
            label3.Name = "label3";
            label3.Size = new Size(52, 20);
            label3.TabIndex = 5;
            label3.Text = "Postfix";
            //
            // txtFormat
            //
            txtFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFormat.Location = new Point(169, 75);
            txtFormat.Name = "txtFormat";
            txtFormat.Size = new Size(405, 27);
            txtFormat.TabIndex = 6;
            txtFormat.Text = "yyyyMMdd_HHmmss";
            txtFormat.TextChanged += txtFormat_TextChanged;
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Location = new Point(23, 78);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 7;
            label4.Text = "Format";
            //
            // lblFormatPreview
            //
            lblFormatPreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblFormatPreview.AutoSize = false;
            lblFormatPreview.ForeColor = System.Drawing.SystemColors.GrayText;
            lblFormatPreview.Location = new Point(169, 105);
            lblFormatPreview.Name = "lblFormatPreview";
            lblFormatPreview.Size = new Size(405, 20);
            lblFormatPreview.TabIndex = 19;
            lblFormatPreview.Text = "";
            //
            // btnRename
            //
            btnRename.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRename.Location = new Point(480, 297);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(94, 29);
            btnRename.TabIndex = 8;
            btnRename.Text = "Rename";
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += btnRename_Click;
            //
            // btnSelectFolder
            //
            btnSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectFolder.Location = new Point(547, 11);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(27, 29);
            btnSelectFolder.TabIndex = 9;
            btnSelectFolder.Text = "...";
            btnSelectFolder.UseVisualStyleBackColor = true;
            btnSelectFolder.Click += btnSelectFolder_Click;
            //
            // chkConvert
            //
            chkConvert.AutoSize = true;
            chkConvert.Location = new Point(169, 142);
            chkConvert.Name = "chkConvert";
            chkConvert.Size = new Size(18, 17);
            chkConvert.TabIndex = 10;
            chkConvert.UseVisualStyleBackColor = true;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(23, 142);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 11;
            label2.Text = "Convert HICF to JPG";
            //
            // button1
            //
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.Location = new Point(12, 297);
            button1.Name = "button1";
            button1.Size = new Size(207, 29);
            button1.TabIndex = 12;
            button1.Text = "Register file explorer action";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            //
            // numHourOffset
            //
            numHourOffset.DecimalPlaces = 1;
            numHourOffset.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numHourOffset.Location = new Point(480, 137);
            numHourOffset.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numHourOffset.Minimum = new decimal(new int[] { 99999, 0, 0, int.MinValue });
            numHourOffset.Name = "numHourOffset";
            numHourOffset.Size = new Size(94, 27);
            numHourOffset.TabIndex = 13;
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new Point(357, 142);
            label5.Name = "label5";
            label5.Size = new Size(117, 20);
            label5.TabIndex = 14;
            label5.Text = "Date hour offset";
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Location = new Point(23, 172);
            label6.Name = "label6";
            label6.Size = new Size(161, 20);
            label6.TabIndex = 15;
            label6.Text = "Uwzględnij podfoldery";
            //
            // chkRecursive
            //
            chkRecursive.AutoSize = true;
            chkRecursive.Location = new Point(190, 175);
            chkRecursive.Name = "chkRecursive";
            chkRecursive.Size = new Size(18, 17);
            chkRecursive.TabIndex = 16;
            chkRecursive.UseVisualStyleBackColor = true;
            //
            // labelDeleteAAE
            //
            labelDeleteAAE.AutoSize = true;
            labelDeleteAAE.Location = new Point(23, 202);
            labelDeleteAAE.Name = "labelDeleteAAE";
            labelDeleteAAE.Size = new Size(120, 20);
            labelDeleteAAE.TabIndex = 20;
            labelDeleteAAE.Text = "Usuń pliki .AAE";
            //
            // chkDeleteAAE
            //
            chkDeleteAAE.AutoSize = true;
            chkDeleteAAE.Location = new Point(190, 205);
            chkDeleteAAE.Name = "chkDeleteAAE";
            chkDeleteAAE.Size = new Size(18, 17);
            chkDeleteAAE.TabIndex = 21;
            chkDeleteAAE.UseVisualStyleBackColor = true;
            //
            // progressBar
            //
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 235);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(570, 23);
            progressBar.TabIndex = 17;
            //
            // lblCurrentFile
            //
            lblCurrentFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCurrentFile.AutoSize = false;
            lblCurrentFile.ForeColor = System.Drawing.SystemColors.GrayText;
            lblCurrentFile.Location = new Point(12, 261);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(570, 20);
            lblCurrentFile.TabIndex = 22;
            lblCurrentFile.Text = "";
            //
            // btnCancel
            //
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(374, 297);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 18;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            //
            // IPhoneFileRanameForm
            //
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(594, 338);
            Controls.Add(labelDeleteAAE);
            Controls.Add(chkDeleteAAE);
            Controls.Add(lblCurrentFile);
            Controls.Add(lblFormatPreview);
            Controls.Add(label6);
            Controls.Add(chkRecursive);
            Controls.Add(progressBar);
            Controls.Add(btnCancel);
            Controls.Add(label5);
            Controls.Add(numHourOffset);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(chkConvert);
            Controls.Add(btnSelectFolder);
            Controls.Add(btnRename);
            Controls.Add(label4);
            Controls.Add(txtFormat);
            Controls.Add(label3);
            Controls.Add(txtPostfix);
            Controls.Add(txtFolder);
            Controls.Add(label1);
            DragEnter += IPhoneFileRanameForm_DragEnter;
            DragDrop += IPhoneFileRanameForm_DragDrop;
            Name = "IPhoneFileRanameForm";
            Text = "IPhone file rename tool";
            ((System.ComponentModel.ISupportInitialize)numHourOffset).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtFolder;
        private TextBox txtPostfix;
        private Label label3;
        private TextBox txtFormat;
        private Label label4;
        private Label lblFormatPreview;
        private Button btnRename;
        private Button btnSelectFolder;
        private CheckBox chkConvert;
        private Label label2;
        private Button button1;
        private NumericUpDown numHourOffset;
        private Label label5;
        private Label label6;
        private CheckBox chkRecursive;
        private Label labelDeleteAAE;
        private CheckBox chkDeleteAAE;
        private ProgressBar progressBar;
        private Label lblCurrentFile;
        private Button btnCancel;
    }
}
