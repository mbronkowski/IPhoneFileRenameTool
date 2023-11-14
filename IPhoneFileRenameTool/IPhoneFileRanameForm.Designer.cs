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
            btnRename = new Button();
            btnSelectFolder = new Button();
            chkConvert = new CheckBox();
            label2 = new Label();
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
            // btnRename
            // 
            btnRename.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRename.Location = new Point(480, 158);
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
            chkConvert.Location = new Point(169, 112);
            chkConvert.Name = "chkConvert";
            chkConvert.Size = new Size(18, 17);
            chkConvert.TabIndex = 10;
            chkConvert.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 112);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 11;
            label2.Text = "Convert HICF to JPG";
            // 
            // IPhoneFileRanameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(594, 199);
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
            Name = "IPhoneFileRanameForm";
            Text = "IPhone file rename tool";
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
        private Button btnRename;
        private Button btnSelectFolder;
        private CheckBox chkConvert;
        private Label label2;
    }
}