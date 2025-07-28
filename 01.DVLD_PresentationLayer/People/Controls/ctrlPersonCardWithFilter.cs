using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using DVLD_BusinessLayer;

namespace DVLD_Project.People.Controls
{
    public partial class ctrlPersonCardWithFilter: UserControl
    {
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> Handler = OnPersonSelected;

            if(Handler != null)
            {
                Handler(PersonID);
            }
        }

        //we can set Visible = true or false;
        private bool _ShowAddPerson = true;
        private bool _FilterEnabled = true;

        public bool ShowAddPerson
        {
            set
            {
                _ShowAddPerson = value; //value here means true or false
                btnAddPerson.Visible = _ShowAddPerson;
                //if true : btnAddPerson.Visible = true;
                //if false : btnAddPerson.Visible = false;
                //so we can show or hide a control using property set and get 
            }
            get
            {
                return _ShowAddPerson;
            }
        }    

        public bool FilterEnabled
        {
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
                //In some cases we can enable or disable the gbFilters (all the GroupBox)
            }
            get
            {
                return _FilterEnabled;
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        //this member is public inside the class and private outside the class
        //remeber that the control ctrlPerson..Filter is a class that inherits UserControl
        private int _PersonID = -1;

        public int PersonID
        {
            get
            {
                return ctrlPersonCard1.PersonID; //READ ONLY PROPERTY
            }
            //we get the PersonID from ctrlPersonCard and put it in ctrlPersonCardWithFilter
        }

        public clsPerson SelectedPersonInfo //SelectedPersonInfo is an object
        {
            get
            {
                return ctrlPersonCard1.SelectedPersonInfo; //READ ONLY PROPERTY
            }
            //same thing in SelectedPersonInfo we Expose the Person Infos from here 
            //to ctrlPersonCard to Outside
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 1; //here 1 means the 2nd item in combo box
            txtFilterValue.Focus();
        }

        //this method we didn't use it in this control!
        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Text = PersonID.ToString();
            _FindNow();
        }

        private void _FindNow()
        {
            switch(cbFilterBy.Text)
            {
                //txtFilterValue.Text takes the value and displays all data
                //in ctrlPersonCard

                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    //we chose PersonID so we should convert txtFilterValue.Text to int 
                    //there are 2 methods int.Parse or Convert.ToInt32
                    break;

                case "National No":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                break;
            }

            //You can set your conditions as you want
            //OnPersonSelected != null means is this control used in a form
            //so we can use the event
            if (OnPersonSelected != null && FilterEnabled) // = is gbEnabled
            {
                //OnPersonSelected(ctrlPersonCard.PersonID); DEBUG BOTH
                PersonSelected(ctrlPersonCard1.PersonID);
                //Firing Event
            }
            //OnPersonSelected Event will appear in Events when we drag drop this control
            //inside a form!
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();            
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //This Event is for every method that ends with _Validating()
            if (!this.ValidateChildren()) 
            {
                MessageBox.Show("This filed is not valid!, put the mouse over the red icon to see the error", 
                                "Validation Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);                
                return;
            }

            _FindNow();
        }      

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            //we register to delegation that is in frmAddUpdatePerson before opening it
            frm.DataBack += _DataBackEvent;
            frm.ShowDialog();
        }

        private void _DataBackEvent(object sender, int PersonID)
        {
            cbFilterBy.SelectedIndex = 1; //This is the index of AddPerson in ComboBox
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            //here we will return the PersonID in Form AddUpdatePerson
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //First, we check if the "Enter" Key is pressed ((char)13 = Enter)
            if (e.KeyChar == (char)13)
                btnFind.PerformClick();

            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            //                                         what are control characters??
        }
    }
}
