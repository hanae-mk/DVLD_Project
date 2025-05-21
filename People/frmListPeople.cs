using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DVLD_BusinessLayer;

namespace DVLD_Project.People
{
    public partial class frmListPeople: Form
    {
        
        private static DataTable _dtAllPeople;       
        private DataTable _dtPeople;      

        public frmListPeople()
        {
            InitializeComponent();
        }
     
        private void frmListPeople_Load(object sender, EventArgs e)
        {
           
            //This is the 1st table that contains all columns
            _dtAllPeople = clsPerson.GetPeopleList();

            //Because we want less columns to show in the DGV, we built this 2nd table that
            //select just the columns we want to show in the DGV
            //true means that the returned DataTable returns only distinct
            //rows for all its columns
            //                  we change the table View
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                               "FirstName", "SecondName",
                                                               "ThirdName", "LastName",
                                                               "Gender", "DateOfBirth",
                                                               "Phone", "Nationality", "Email");

            dgvPeople.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0; //None

            lblRowsCount.Text = dgvPeople.Rows.Count.ToString();

            if(dgvPeople.Rows.Count > 0)
            { 
                 dgvPeople.Columns[0].HeaderText = "Person ID";
                 dgvPeople.Columns[0].Width = 80;
                 
                 dgvPeople.Columns[1].HeaderText = "National No";
                 dgvPeople.Columns[1].Width = 100;
                 
                 dgvPeople.Columns[2].HeaderText = "First Name";
                 dgvPeople.Columns[2].Width = 100;
                 
                 dgvPeople.Columns[3].HeaderText = "Second Name";
                 dgvPeople.Columns[3].Width = 100;
                 
                 dgvPeople.Columns[4].HeaderText = "Third Name";
                 dgvPeople.Columns[4].Width = 100;
                 
                 dgvPeople.Columns[5].HeaderText = "Last Name";
                 dgvPeople.Columns[5].Width = 100;
                 
                 dgvPeople.Columns[6].HeaderText = "Gender";
                 dgvPeople.Columns[6].Width = 80;
                 
                 dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                 dgvPeople.Columns[7].Width = 110;
                 
                 dgvPeople.Columns[8].HeaderText = "Phone";
                 dgvPeople.Columns[8].Width = 110;
                 
                 dgvPeople.Columns[9].HeaderText = "Nationality";
                 dgvPeople.Columns[9].Width = 100;
                 
                 dgvPeople.Columns[10].HeaderText = "Email";
                 dgvPeople.Columns[10].Width = 149;
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "Person ID": //Columns that are in DataGridView (Headers)
                    FilterColumn = "PersonID"; //Actual Columns that are in DataBase
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Gender":
                    FilterColumn = "Gender";
                    break;

                case "DateOfBirth":
                    FilterColumn = "Date Of Birth";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Nationality":
                    FilterColumn = "Nationality";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = ""; //No Filter Applied Here
                lblRowsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")//                       = bcs integer
                _dtPeople.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("{0} LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRowsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                //Here the method txtFilterValue_TextChanged() will be called!
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form Form1 = new frmAddUpdatePerson();
            Form1.ShowDialog();

            frmListPeople_Load(null, null);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Cells[0] means the first item in Row (PersonID)
            //we can simply put (int)dgv... in the constructor but it's just for
            //simple reading code, of course this is NOT the best way, it's better to 
            //write directly frmShowPersonInfo((int)dgvPeople...) to not consume resources
            Form frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListPeople_Load(null, null);
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            frmListPeople_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListPeople_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (clsPerson.IsPersonExist((int)dgvPeople.CurrentRow.Cells[0].Value))
            {
                //                                                            no need to cast to 'int'
                if (MessageBox.Show($"Are you sure you want to delete person {dgvPeople.CurrentRow.Cells[0].Value}", 
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo, 
                                     MessageBoxIcon.Question,
                                     MessageBoxDefaultButton.Button2) 
                                  == DialogResult.Yes)
                {
                    //We delete contact from the class not from object because it's FASTER
                    //and if you delete contact from object it's will be deleted just from database
                    //BUT it's will still in memory
                    if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                    { 
                        MessageBox.Show("Person Deleted Successfully", 
                                        "Successful", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Information);

                        frmListPeople_Load(null, null);
                    }
                    else
                        MessageBox.Show("There is data linked to this Person!", 
                                        "Error", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show($"Contact with ID = {dgvPeople.CurrentRow.Cells[0].Value} is NOT found!", 
                                 "Error", 
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);            
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature will be available soon!", "New Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PhoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature will be available soon!", "New Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListPeople_Load(null, null);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtFilterValue.Text == "Person ID")
                //check input character by character to prevent user input characters
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }      
    }
}
