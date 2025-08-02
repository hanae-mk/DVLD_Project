using DVLD_Project.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class frmFindLicenseByLicenseIDPerson: Form
    {
        public delegate void SendDataBack(object sender, int PersonID);
        public event SendDataBack DataBack;

        public frmFindLicenseByLicenseIDPerson()
        {
            InitializeComponent();
        }

        private void frmFindLicenseByLicenseIDPerson_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this, ctrlPersonCardWithFilter1.PersonID);
            //PersonID is a public property
        }
    }
}
