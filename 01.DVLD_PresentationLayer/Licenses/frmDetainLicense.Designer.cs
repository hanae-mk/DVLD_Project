namespace DVLD_Project.Licenses
{
    partial class frmDetainLicense
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
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gbDetainInfo = new System.Windows.Forms.GroupBox();
            this.txtFineFees = new System.Windows.Forms.TextBox();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblLicenseID = new System.Windows.Forms.Label();
            this.lblDetainDate = new System.Windows.Forms.Label();
            this.lblDetainID = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LinklblShowLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.LinklblShowLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.btnDetainLicense = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlDriverLicenseInfoWithFilter1 = new DVLD_Project.Licenses.Local_Licenses.ctrlDriverLicenseInfoWithFilter();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbDetainInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblTitle.Location = new System.Drawing.Point(81, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(776, 43);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Detain License";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbDetainInfo
            // 
            this.gbDetainInfo.Controls.Add(this.txtFineFees);
            this.gbDetainInfo.Controls.Add(this.lblCreatedBy);
            this.gbDetainInfo.Controls.Add(this.lblLicenseID);
            this.gbDetainInfo.Controls.Add(this.lblDetainDate);
            this.gbDetainInfo.Controls.Add(this.lblDetainID);
            this.gbDetainInfo.Controls.Add(this.pictureBox5);
            this.gbDetainInfo.Controls.Add(this.pictureBox4);
            this.gbDetainInfo.Controls.Add(this.pictureBox3);
            this.gbDetainInfo.Controls.Add(this.pictureBox2);
            this.gbDetainInfo.Controls.Add(this.pictureBox1);
            this.gbDetainInfo.Controls.Add(this.label6);
            this.gbDetainInfo.Controls.Add(this.label5);
            this.gbDetainInfo.Controls.Add(this.label4);
            this.gbDetainInfo.Controls.Add(this.label3);
            this.gbDetainInfo.Controls.Add(this.label1);
            this.gbDetainInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDetainInfo.Location = new System.Drawing.Point(17, 481);
            this.gbDetainInfo.Name = "gbDetainInfo";
            this.gbDetainInfo.Size = new System.Drawing.Size(893, 115);
            this.gbDetainInfo.TabIndex = 15;
            this.gbDetainInfo.TabStop = false;
            this.gbDetainInfo.Text = "Detain Info";
            // 
            // txtFineFees
            // 
            this.txtFineFees.Location = new System.Drawing.Point(149, 82);
            this.txtFineFees.Name = "txtFineFees";
            this.txtFineFees.Size = new System.Drawing.Size(88, 22);
            this.txtFineFees.TabIndex = 15;
            this.txtFineFees.Validating += new System.ComponentModel.CancelEventHandler(this.txtFineFees_Validating);
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.Location = new System.Drawing.Point(495, 59);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(49, 16);
            this.lblCreatedBy.TabIndex = 14;
            this.lblCreatedBy.Text = "[????]";
            // 
            // lblLicenseID
            // 
            this.lblLicenseID.AutoSize = true;
            this.lblLicenseID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicenseID.Location = new System.Drawing.Point(495, 30);
            this.lblLicenseID.Name = "lblLicenseID";
            this.lblLicenseID.Size = new System.Drawing.Size(41, 16);
            this.lblLicenseID.TabIndex = 13;
            this.lblLicenseID.Text = "[???]";
            // 
            // lblDetainDate
            // 
            this.lblDetainDate.AutoSize = true;
            this.lblDetainDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetainDate.Location = new System.Drawing.Point(146, 56);
            this.lblDetainDate.Name = "lblDetainDate";
            this.lblDetainDate.Size = new System.Drawing.Size(91, 16);
            this.lblDetainDate.TabIndex = 11;
            this.lblDetainDate.Text = "[??/??/????]";
            // 
            // lblDetainID
            // 
            this.lblDetainID.AutoSize = true;
            this.lblDetainID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetainID.Location = new System.Drawing.Point(146, 30);
            this.lblDetainID.Name = "lblDetainID";
            this.lblDetainID.Size = new System.Drawing.Size(41, 16);
            this.lblDetainID.TabIndex = 10;
            this.lblDetainID.Text = "[???]";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::DVLD_Project.Properties.Resources.User_32__2;
            this.pictureBox5.Location = new System.Drawing.Point(448, 52);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(28, 24);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 9;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::DVLD_Project.Properties.Resources.LocalDriving_License;
            this.pictureBox4.Location = new System.Drawing.Point(449, 22);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(28, 24);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 8;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::DVLD_Project.Properties.Resources.money_32;
            this.pictureBox3.Location = new System.Drawing.Point(102, 80);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(28, 24);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DVLD_Project.Properties.Resources.Calendar_32;
            this.pictureBox2.Location = new System.Drawing.Point(102, 51);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(28, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD_Project.Properties.Resources.Number_32;
            this.pictureBox1.Location = new System.Drawing.Point(102, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(359, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Created By:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(355, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "License ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Fine Fees:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Detain Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Detain ID:";
            // 
            // LinklblShowLicenseHistory
            // 
            this.LinklblShowLicenseHistory.AutoSize = true;
            this.LinklblShowLicenseHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinklblShowLicenseHistory.Location = new System.Drawing.Point(14, 612);
            this.LinklblShowLicenseHistory.Name = "LinklblShowLicenseHistory";
            this.LinklblShowLicenseHistory.Size = new System.Drawing.Size(135, 16);
            this.LinklblShowLicenseHistory.TabIndex = 16;
            this.LinklblShowLicenseHistory.TabStop = true;
            this.LinklblShowLicenseHistory.Text = "Show License History";
            this.LinklblShowLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinklblShowLicenseHistory_LinkClicked);
            // 
            // LinklblShowLicenseInfo
            // 
            this.LinklblShowLicenseInfo.AutoSize = true;
            this.LinklblShowLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinklblShowLicenseInfo.Location = new System.Drawing.Point(163, 612);
            this.LinklblShowLicenseInfo.Name = "LinklblShowLicenseInfo";
            this.LinklblShowLicenseInfo.Size = new System.Drawing.Size(114, 16);
            this.LinklblShowLicenseInfo.TabIndex = 17;
            this.LinklblShowLicenseInfo.TabStop = true;
            this.LinklblShowLicenseInfo.Text = "Show License Info";
            this.LinklblShowLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinklblShowLicenseInfo_LinkClicked);
            // 
            // btnDetainLicense
            // 
            this.btnDetainLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetainLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetainLicense.Image = global::DVLD_Project.Properties.Resources.International_32;
            this.btnDetainLicense.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDetainLicense.Location = new System.Drawing.Point(761, 606);
            this.btnDetainLicense.Name = "btnDetainLicense";
            this.btnDetainLicense.Size = new System.Drawing.Size(149, 29);
            this.btnDetainLicense.TabIndex = 18;
            this.btnDetainLicense.Text = "         Detain License";
            this.btnDetainLicense.UseVisualStyleBackColor = true;
            this.btnDetainLicense.Click += new System.EventHandler(this.btnDetainLicense_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::DVLD_Project.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(658, 606);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 29);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "       Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlDriverLicenseInfoWithFilter1
            // 
            this.ctrlDriverLicenseInfoWithFilter1.IsFilterEnabled = true;
            this.ctrlDriverLicenseInfoWithFilter1.Location = new System.Drawing.Point(12, 55);
            this.ctrlDriverLicenseInfoWithFilter1.Name = "ctrlDriverLicenseInfoWithFilter1";
            this.ctrlDriverLicenseInfoWithFilter1.Size = new System.Drawing.Size(914, 426);
            this.ctrlDriverLicenseInfoWithFilter1.TabIndex = 20;
            this.ctrlDriverLicenseInfoWithFilter1.OnLicenseSelected += new System.Action<int>(this.ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmDetainLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(938, 644);
            this.Controls.Add(this.ctrlDriverLicenseInfoWithFilter1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDetainLicense);
            this.Controls.Add(this.LinklblShowLicenseInfo);
            this.Controls.Add(this.LinklblShowLicenseHistory);
            this.Controls.Add(this.gbDetainInfo);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDetainLicense";
            this.Text = "DetainedLicenses";
            this.Activated += new System.EventHandler(this.frmDetainedLicenses_Activated);
            this.Load += new System.EventHandler(this.frmDetainLicense_Load);
            this.gbDetainInfo.ResumeLayout(false);
            this.gbDetainInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox gbDetainInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDetainDate;
        private System.Windows.Forms.Label lblDetainID;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtFineFees;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblLicenseID;
        private System.Windows.Forms.LinkLabel LinklblShowLicenseHistory;
        private System.Windows.Forms.LinkLabel LinklblShowLicenseInfo;
        private System.Windows.Forms.Button btnDetainLicense;
        private System.Windows.Forms.Button btnClose;
        private Local_Licenses.ctrlDriverLicenseInfoWithFilter ctrlDriverLicenseInfoWithFilter1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}