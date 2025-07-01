using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmListUsers: Form
    {
        private static DataTable _dtAllUsers;

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
                        
            _dtAllUsers = clsUser.GetUsersList();
            dgvUsers.DataSource = _dtAllUsers;

            cbFilterBy.SelectedIndex = 0;

            lblRowsCount.Text = dgvUsers.Rows.Count.ToString();

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 80;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 80;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].Width = 100;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 80;

            }
        }
     
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Prevent User to input characters, ONLY numbers are valid!
            if(cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
            {
                //check input character by character to prevent user input characters
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                //IsControl indicates if the unicode character is a control CHARACTER or not
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            string FilterValue = txtFilterValue.Text.Trim();

            switch (cbFilterBy.Text) 
            {
                case "User ID":              //column name in DGV
                    FilterColumn = "UserID"; //column name in DataBase
                    break;

                case "UserName":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            //We reset the filters in case nothing selected or filter value contains nothing.
            if (FilterColumn == "None" || FilterValue == "")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRowsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID" || FilterColumn == "UserID")
                _dtAllUsers.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, FilterValue);
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("{0} LIKE '{1}%'", FilterColumn, FilterValue);

            lblRowsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            { 
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
            }
            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }        
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text; //All / Yes / No

            switch (FilterValue)
            {
               case "All":
                   break;

               case "Yes":
                   FilterValue = "1";//we change the filter value from yes to 1
                   break;

               case "No":
                   FilterValue = "0";
                   break;

               default:
                   break;
            }

            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, FilterValue);
            
            lblRowsCount.Text = dgvUsers.Rows.Count.ToString();
                 
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm = new frmShowUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;

            if (clsUser.IsUserExist(UserID))
            {
                if(MessageBox.Show("Are you sure you want to delete this user?", 
                                   "Confirm Delete", 
                                   MessageBoxButtons.OK, 
                                   MessageBoxIcon.Question) 
                                == DialogResult.OK)
                
                if(clsUser.DeleteUser(UserID))
                {
                     MessageBox.Show("User with ID = " + UserID + " Deleted successfully!",
                                        "Delete Person",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                }
                else
                     MessageBox.Show(@"Cannot delete this user because User with ID = " + 
                                       UserID + " is linked with another data",
                                       "Delete Failed",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("User with ID = " + UserID + " was not found!",
                                "Delete Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            frmListUsers_Load(null, null);
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature will be available soon!", "New Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature will be available soon!", "New Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
