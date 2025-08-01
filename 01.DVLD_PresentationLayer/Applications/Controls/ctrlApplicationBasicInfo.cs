﻿using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplication _Application;

        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.GetApplicationInfoByID(ApplicationID);

            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblApplicationStatus.Text = _Application.StatusText;
            lblApplicationType.Text = _Application.ApplicationTypeInfo.ApplicationTypeTitle;
            lblApplicationFees.Text = _Application.PaidFees.ToString();
            lblApplicantName.Text = _Application.PersonInfo.FullName;
            lblApplicationDate.Text = clsFormat.ShortDateTime(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.ShortDateTime(_Application.LastStatusDate);
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblApplicationStatus.Text = "[????]";
            lblApplicationType.Text = "[????]";
            lblApplicationFees.Text = "[????]";
            lblApplicantName.Text = "[????]";
            lblApplicationDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";
        }

        private void linklblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_Application.ApplicantPersonID);
            frm.ShowDialog();

            //Refresh
            LoadApplicationInfo(_ApplicationID);
        }
    }
}
