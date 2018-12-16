using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;

namespace XoRisk_SelfService.DataModels
{
    public static class Helper
    {
        public static  string siteUrl = "http://13.127.207.248:81/"; 
        public static string GenerateProjectCode(string companyName)
        {
            string projectCode = string.Empty;
            const string CodeInitial = "XORISK";
            try
            {
                if (companyName.Length >3)
                {
                    projectCode = CodeInitial + companyName.Substring(0, 3) + DateTime.Now.ToString("ddmyyyy");

                }
                else
                {
                    projectCode = CodeInitial + companyName + DateTime.Now.ToString("ddmyyyy");
                }

            }
            catch (Exception ex)
            {

                
            }
            return projectCode;
        }

        public static DateTime GetDate(string date)
        {
            DateTime myDate = new DateTime();
            try
            {
                if (date != null)
                {
                    //char[] monthdata = date.Split('/')[0].ToCharArray();
                    //string mon = string.Empty;
                    //for (int i = 0; i < monthdata.Length; i++)
                    //{
                    //    mon = mon + monthdata[i].ToString();
                    //}
                    string[] Splitdate = date.Split('/');
                    string monthString = Splitdate[0].ToString().Trim();
                    int month = Convert.ToInt32(Splitdate[0]);
                    // Int32.TryParse(mon, out month);
                    int day;
                    Int32.TryParse(Splitdate[1].ToString().Trim(), out day);
                    int year;
                    Int32.TryParse(Splitdate[2].ToString().Split(' ')[0].Trim(), out year);
                    myDate = new DateTime(year, month, day);



                }
                return myDate;
            }
            catch (Exception ex)
            {

                return myDate;
            }
        }

        public static bool createProjectScript(int projectID,int ScriptID,string projectCode)
        {
            bool result = false;
            try
            {
                using (XoriskManagementBackendEntities db = new XoriskManagementBackendEntities())
                {
                    ProjectScript obj = new ProjectScript();
                    obj.ProjectCode = projectCode;
                    obj.ProjectId = projectID;
                    obj.ScriptId = ScriptID;
                    obj.CreatedAt = DateTime.Now;
                    db.ProjectScripts.Add(obj);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public static bool SendFilesInMail(string subject, StringBuilder mailBody, string toMailId)

        {

            try

            {

                string sourceDir = AppDomain.CurrentDomain.BaseDirectory;

                //string[] reportList = Directory.GetFiles(sourceDir, "*.csv");

                MailMessage mail = new MailMessage();
                //string subject = "";
                //StringBuilder mailBody = new StringBuilder();
                //set the addresses 
                mail.From = new MailAddress("aucsalejobs@gmail.com"); //IMPORTANT: This must be same as your smtp authentication address.
                mail.Subject = subject;
                mail.Body = mailBody.ToString();
                mail.IsBodyHtml = true;
                //send the message 
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true;

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                string SMTPUser = "aucsalejobs@gmail.com", SMTPPassword = "Iamdon@1987";
                NetworkCredential Credentials = new NetworkCredential(SMTPUser, SMTPPassword);
                smtp.Credentials = Credentials;
                mail.To.Add(toMailId);
               
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                

                return true;

            }

            catch (Exception ex)

            {



                return false;

            }
        }
        
        public static System.Data.DataTable ExportToExcel(string rptName)
        {
            DataTable dt = null;
            try
            {
                using (XoriskManagementBackendEntities db= new XoriskManagementBackendEntities())
                {
                    if (rptName=="Scan")
                    {
                        List<ScanReport> dtTable = (from a in db.ScanReports select a).ToList<ScanReport>();
                         dt = ToDataTable(dtTable);
                    }
                    else if (rptName=="Project")
                    {
                        List<Project> dtTable = (from a in db.Projects select a).ToList<Project>();
                         dt = ToDataTable(dtTable);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return dt;
                
            }
            //System.Data.DataTable table = new System.Data.DataTable();
            //table.Columns.Add("ID", typeof(int));
            //table.Columns.Add("Name", typeof(string));
            //table.Columns.Add("Sex", typeof(string));
            //table.Columns.Add("Subject1", typeof(int));
            //table.Columns.Add("Subject2", typeof(int));
            //table.Columns.Add("Subject3", typeof(int));
            //table.Columns.Add("Subject4", typeof(int));
            //table.Columns.Add("Subject5", typeof(int));
            //table.Columns.Add("Subject6", typeof(int));
            //table.Rows.Add(1, "Amar", "M", 78, 59, 72, 95, 83, 77);
            //table.Rows.Add(2, "Mohit", "M", 76, 65, 85, 87, 72, 90);
            //table.Rows.Add(3, "Garima", "F", 77, 73, 83, 64, 86, 63);
            //table.Rows.Add(4, "jyoti", "F", 55, 77, 85, 69, 70, 86);
            //table.Rows.Add(5, "Avinash", "M", 87, 73, 69, 75, 67, 81);
            //table.Rows.Add(6, "Devesh", "M", 92, 87, 78, 73, 75, 72);
            //return table;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ@$#";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
    }
}