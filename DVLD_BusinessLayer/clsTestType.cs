using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestType
    {
        public enum enMode { AddNew = 1, Update = 2};
        public enum enTestType { VisionTest = 0, WrittenTest = 1, StreetTest = 2};

        public enMode Mode;

        public clsTestType.enTestType TestTypeID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }
        public float TestTypeFees { set; get; }

        public clsTestType()
        {
            TestTypeID = clsTestType.enTestType.VisionTest; //0
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0.00f;

            Mode = enMode.AddNew;
        }

        public clsTestType(clsTestType.enTestType TestTypeID, string TestTypeTitle, 
                           string TestTypeDescription, float TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;

            Mode = enMode.Update;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        private bool _AddNewTestType()
        {
            //                CAST integer to enum
            this.TestTypeID = (clsTestType.enTestType)clsTestTypeData.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
            
            return (this.TestTypeTitle != ""); //??
        }

        private bool _UpdateTestType()
        {
            //                                    CAST enum to int
            return clsTestTypeData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            float TestTypeFees = 0.00f;

            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateTestType();

                default:
                    return false;
            }
        }
    }
}
