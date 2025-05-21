using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.Global_Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUserNameAndPassword(string UserName, string Password)
        {
            try
            {
                //This will get the current project directory folder
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                //Define the path to the text file where you want to save the data
                string FilePath = CurrentDirectory + @"\Data.txt";

                //Incase username is empty, delete the file
                if(UserName == "" && File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }

                //Concatonate UserName & Password
                string LoginInfo = UserName + "#//#" + Password;

                //Create a StreamWriter to write to the file
                using(StreamWriter Writer = new StreamWriter(FilePath))
                {
                    //Write LoginInfo in the file
                    Writer.WriteLine(LoginInfo);
                    return true;
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show($"An Error Occured while saving Login Info in the file : {Ex.Message}");
                return false;
            }
        }

        public static bool IsRememberedCredentials(ref string UserName, ref string Password)
        {
            try
            {
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                string FilePath = CurrentDirectory + @"\Data.txt";

                //Check First if the file exist or not before reading lines

                if (File.Exists(FilePath))
                {
                    using (StreamReader Reader = new StreamReader(FilePath))
                    {
                        string Line = "";

                        while ((Line = Reader.ReadLine()) != null) //Reader.ReadLine()) != null
                        {
                            //Line = Reader.ReadLine();

                            //Output each line of data to the console
                            Console.WriteLine(Line);
                            string[] Result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            UserName = Result[0];
                            Password = Result[1];
                        }

                        return true;
                    }
                }
                else
                    return false;
            }
            catch(Exception Ex)
            {
                MessageBox.Show("An Error Occured : " + Ex.Message);
                return false;
            }
        }
    }
}
