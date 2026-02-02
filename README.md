 # 🚗 Driving License Management System

 ### 📋 **Overview**  

A comprehensive **Windows Forms Application** for managing driver licenses, applications, and related administrative tasks.

This system provides a robust solution for driving license authorities to manage licenses, applications, tests, and driver records efficiently.
***
## 🚀 **Features**

👤 **User Management**

- Secure user authentication and authorization 

- Role-based access control

- User profile management

👥 **Driver Management**

- Driver registration and profile management

- Driver history tracking

- License status monitoring

📄 **License Management**  

- Local driving license processing

- International license management

- License renewal and updates

- License Replacement For Damaged OR Lost

- License Detain and Release

- License class management

📄 **Application Processing**

- New license applications

- License renewal applications

- Application status tracking

- Fee management

🧪 **Test Management**

- Test scheduling and appointments

- Different test types (Vision, Written(Theory), Road(Practical))

- Test results tracking

- Test type management

📊 **Reporting**

- License status reports

- Application statistics

- Test results reports

- Licenses Driver history reports
***
## 🏗️ **Architecture**
The DVLD Project follows a three-tier architecture :
**1.Presentation Layer:** Windows Forms UI Components
**2.Business Layer:** Business Logic & Validations
**3.Data Access Layer:** Database Operations & Data Access
***
## 🛠️ **Technologies**
**Framework:** .NET Framework
**UI Framework:** Winforms
**Database:** Microsoft SQL Server
**Data Access Technology:** ADO.NET 
**IDE:** Visual Studio 2022
***
## 🚀 **Installation**

1. Clone The Repository:  
Use This Command Line :  
git clone https://github.com/hanae-mk/DVLD_Project.git

2. Create a new SQL Server Database and name it "DVLD Database"

3. Download the Database Script in the "00.DataBase BackUp File" Folder

4. Restore the Database by clicking on New Query then Paste this command Line :  
   RESTORE DATABASE DVLD FROM Disk = 'C:\DVLD.bak';

5. Open The Project Go to DVLD_DataAccessLayer scroll down and search "clsDataAccessSettings.cs" class
   You will find a ConnectionString
   Update DataBase, User ID and Password.
   
6. Build and Run the Project  
### **Login Details**   
   **UserName** : Hanae    
   **Password** : 0000
***
## 📽️ **Project Demo**

Please visit this link:  
https://lnkd.in/ekBaZMPM

***
### 👤 Contact  
If you have any question feel free to contact me via my Linkedin  
https://www.linkedin.com/in/hanae-mk/





