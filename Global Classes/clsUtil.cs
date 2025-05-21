using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Global_Classes
{
    //Utility Class
    class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid NewGuid = Guid.NewGuid();
            return NewGuid.ToString();
        }
        
        public static string ReplaceImagePathWithGUID(string SourceImagePath)
        {
            FileInfo ImageInfo = new FileInfo(SourceImagePath);
            string Extension = ImageInfo.Extension;

            return GenerateGUID() + Extension;
        }
        
        public static bool IsFolderExists(string FolderPath)
        {
            if (Directory.Exists(FolderPath))
                return true;
            
            try
            {
                //In case the folder does NOT exist, we create one!
                Directory.CreateDirectory(FolderPath);
                return true;
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Create Folder Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            }

            return false;
        }
              
        public static bool CopyImageToProjectImagesFolder(ref string SourceImagePath)
        {
            string FolderPath = @"C:\DVLD People Images\";

            if(!IsFolderExists(FolderPath))
                return false;

            string DestinationImagePath = FolderPath + ReplaceImagePathWithGUID(SourceImagePath);

            try
            {
                File.Copy(SourceImagePath, DestinationImagePath, true);
            }
            catch(IOException Ex)
            {
                MessageBox.Show(Ex.Message, "Copy Image Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourceImagePath = DestinationImagePath;
            return true;

        }
    }   
}
