using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Project.People.Controls;
using DVLD_BusinessLayer;

namespace DVLD_Project.Users.Controls
{
    public partial class ctrlUserCard: UserControl
    {
        private int _UserID = -1;
        private clsUser _User;

        public int UserID
        {
            get
            {
                return _UserID;
            }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }
        
        private void _ResetUserInfo()
        {
            ctrlUserCard1.ResetPersonInfo();
            lblUserID.Text = "[????]";
            lblUserName.Text = "[????]";
            lblIsActive.Text = "[????]";
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindByUserID(UserID);

            if(_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("User with ID = " + UserID + " is not found!",
                                 "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                //we cannot set this.Close() bcs this is a UserControl not a Form
                return;
            }

            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            ctrlUserCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName; //hnaya kifash 3melna dik f.n + m.n + t.n + l.n nsiiiit
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
        }

        private void ctrlLoginInfo_Load(object sender, EventArgs e)
        {
            

        }




















    }
}
