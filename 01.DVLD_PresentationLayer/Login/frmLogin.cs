using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
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
    public partial class frmLogin: Form
    {
             
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsGlobal.IsRememberedCredentials(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindByUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if(User != null)
            {
                if (chkRememberMe.Checked)
                    //store username and password
                    clsGlobal.RememberUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                else
                    //store empty username and password
                    clsGlobal.RememberUserNameAndPassword("", "");

                if(!User.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your Account is Not Active, Please contact your Admin!",
                                    "Account Not Active",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide(); //jarreb this.Close();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid UserName/Password",
                                "Wrong Credientials",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }       
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Application.MessageLoop)
               Application.Exit();
        }
    }
}
