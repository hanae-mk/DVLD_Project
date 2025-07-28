namespace DVLD_Project.People.Controls
{
    partial class ctrlPersonCardWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.gbFilters = new System.Windows.Forms.GroupBox();
            this.ctrlPersonCard1 = new DVLD_Project.ctrlPersonCard();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.gbFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Filter :";
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.BackColor = System.Drawing.SystemColors.Window;
            this.btnAddPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPerson.Image = global::DVLD_Project.Properties.Resources.AddPerson_32;
            this.btnAddPerson.Location = new System.Drawing.Point(546, 21);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(58, 33);
            this.btnAddPerson.TabIndex = 12;
            this.btnAddPerson.UseVisualStyleBackColor = false;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Find By:";
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.Window;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Image = global::DVLD_Project.Properties.Resources.SearchPerson;
            this.btnFind.Location = new System.Drawing.Point(471, 21);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(58, 33);
            this.btnFind.TabIndex = 11;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Items.AddRange(new object[] {
            "Person ID",
            "National No"});
            this.cbFilterBy.Location = new System.Drawing.Point(106, 30);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(175, 24);
            this.cbFilterBy.TabIndex = 9;
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cbFilterBy_SelectedIndexChanged);
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilterValue.Location = new System.Drawing.Point(296, 32);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(158, 22);
            this.txtFilterValue.TabIndex = 10;
            this.txtFilterValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterValue_KeyPress);
            this.txtFilterValue.Validating += new System.ComponentModel.CancelEventHandler(this.txtFilterValue_Validating);
            // 
            // gbFilters
            // 
            this.gbFilters.Controls.Add(this.label1);
            this.gbFilters.Controls.Add(this.btnAddPerson);
            this.gbFilters.Controls.Add(this.label2);
            this.gbFilters.Controls.Add(this.btnFind);
            this.gbFilters.Controls.Add(this.cbFilterBy);
            this.gbFilters.Controls.Add(this.txtFilterValue);
            this.gbFilters.Location = new System.Drawing.Point(13, 10);
            this.gbFilters.Name = "gbFilters";
            this.gbFilters.Size = new System.Drawing.Size(638, 73);
            this.gbFilters.TabIndex = 13;
            this.gbFilters.TabStop = false;
            // 
            // ctrlPersonCard1
            // 
            this.ctrlPersonCard1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ctrlPersonCard1.BackColor = System.Drawing.SystemColors.Window;
            this.ctrlPersonCard1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlPersonCard1.Location = new System.Drawing.Point(13, 89);
            this.ctrlPersonCard1.Name = "ctrlPersonCard1";
            this.ctrlPersonCard1.Size = new System.Drawing.Size(884, 312);
            this.ctrlPersonCard1.TabIndex = 14;
            // 
            // ctrlPersonCardWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.ctrlPersonCard1);
            this.Controls.Add(this.gbFilters);
            this.Name = "ctrlPersonCardWithFilter";
            this.Size = new System.Drawing.Size(908, 359);
            this.Load += new System.EventHandler(this.ctrlPersonCardWithFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.gbFilters.ResumeLayout(false);
            this.gbFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox gbFilters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private ctrlPersonCard ctrlPersonCard1;
    }
}
