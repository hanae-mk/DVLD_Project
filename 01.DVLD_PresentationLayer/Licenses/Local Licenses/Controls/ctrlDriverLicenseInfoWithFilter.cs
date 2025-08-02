using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Local_Licenses
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;

        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int LicenseID)
        {
            Action<int> Handler = OnLicenseSelected;

            if(Handler != null)
            {
                Handler(LicenseID); //Raise the event with the parameter
            }
        }

        private bool _IsFilterEnabled = true;

        public bool IsFilterEnabled
        {
            set 
            { 
                _IsFilterEnabled = value;
                gbFilters.Enabled = _IsFilterEnabled;
            }

            get { return _IsFilterEnabled; }
        }

        private int _LicenseID = -1;

        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }
        
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadDriverInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;

            if(OnLicenseSelected != null && IsFilterEnabled)
            {
                OnLicenseSelected(_LicenseID); //Raise the event with a parameter
            }
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            { 
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This Field Is Required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            //Also We Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
                btnFindLicenseByLicenseIDLicense.PerformClick(); //That means btnFindLicenseByLicenseIDLicense Perform that Enter Click!
        }

        private void btnFindLicenseByLicenseIDLicense_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!, Put The Mouse Over The Red Icon(s) To See The Error",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _LicenseID = int.Parse(txtLicenseID.Text);
            LoadLicenseInfo(_LicenseID);
        }
        
        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }
    }
}
