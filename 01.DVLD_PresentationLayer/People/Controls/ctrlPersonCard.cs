using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.People;
using DVLD_Project.Properties;
using System.IO;

namespace DVLD_Project
{
    public partial class ctrlPersonCard: UserControl
    {
        //There is no need to set an enum because this form is ONLY to show person info

        private int _PersonID = -1;
        clsPerson _Person;
        
        public int PersonID
        {
            get 
            { 
                return _PersonID; //READ ONLY PROPERTY
            }
        }

        public clsPerson SelectedPersonInfo
        {
            get
            {
                return _Person; //READ ONLY PROPERTY
            }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.FindPersonByID(PersonID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Person with ID = " + PersonID + " is Not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.FindPersonByNationalNo(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Person with National No = " + NationalNo + " is Not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pbGender.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Male_512;
        }

        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            lblGender.Text = (_Person.Gender == 0 ? "Male" : "Female");
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString(); // Ex : 17/04/2000
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = _Person.CountryInfo.CountryName;
            lblEditPersonInfoLink.Visible = true;

            if (_Person.Gender == 0)
            {
                pbGender.Image = Resources.Man_32;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            { 
                pbGender.Image = Resources.Woman_32;
                pbPersonImage.Image = Resources.Female_512;
            }

            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {              
            if (_Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                    //here we Load _Person.ImagePath in PictureBox
                    //Do NOT use.Load() method that is in PictureBox
                    //pbPersonImage.Load(_Person.ImagePath);
                    //because it's loads image and still have log to the file
                    //when you want to load another image and delete the old one
                    //it's throws an error that the file is used by another process
                    //so it's better to Load image by this property .ImageLocation
                    pbPersonImage.ImageLocation = _Person.ImagePath;                 
                else
                    MessageBox.Show("Image Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);              
            }                 
        }

        private void lblEditPersonInfoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            //Refresh Data
            LoadPersonInfo(_PersonID);
        }

        private void ctrlPersonCard_Load(object sender, EventArgs e)
        {

        }
    }
}
