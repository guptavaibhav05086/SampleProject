using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using XoRisk_SelfService.DataModels;
using XoRisk_SelfService.Models;

namespace XoRisk_SelfService.Controllers
{
    [System.Web.Http.Authorize]
    public class DashBoardController : ApiController
    {
        XoriskManagementBackendEntities db = null;
        private ApplicationUserManager _userManager;

        public DashBoardController()
        {
            
        }
        public DashBoardController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/GetAllProjects")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        public List<ProjectDetails> GetAllProjects()
        {
            List<ProjectDetails> colAllProjects = new List<ProjectDetails>();
           

            try
            {
                using (db= new XoriskManagementBackendEntities())
                {
                    var result = (from b in db.Projects
                                  select new ProjectDetails
                                  {
                                      Id=b.Id,
                                      Company=b.Company
                                  });

                    colAllProjects = (from a in db.Projects select new ProjectDetails {
                        Id= a.Id,
                        Company=a.Company,
                        ProjectCode=a.Code,
                        StartDateTime = a.StartDate,
                        EndDateTime=a.EndDate,
                        Email = a.Email,
                        Status = a.Status,
                        POCName = a.POCName
                    }).ToList<ProjectDetails>();

                   

                    foreach (var item in colAllProjects)
                    {
                        List<Scripts> scriptData = new List<Scripts>();
                        scriptData = (from b in db.Scripts
                                      select new Scripts
                                      {
                                          Id = b.Id,
                                          MappedFolderPath = b.StoreLocation,
                                          Name = b.Name,
                                          Status = false,
                                          ImagePath = Helper.siteUrl + b.Image

                                      }).ToList<Scripts>();
                        List<Scripts> SelectedscriptData = new List<Scripts>();
                        SelectedscriptData = (from b in db.Scripts join c in db.ProjectScripts on b.Id equals c.ScriptId into col
                                      from c in col.DefaultIfEmpty() where c.ProjectId == item.Id
                                      select new Scripts
                                      {
                                          Id = b.Id,
                                          MappedFolderPath = b.StoreLocation,
                                          Name = b.Name,
                                         // Status = c.ProjectId ? false :true 

                                      }).ToList<Scripts>();
                        foreach (var script in SelectedscriptData)
                        {
                            var data = (from a in scriptData where a.Id == script.Id select a).FirstOrDefault();
                            if (data !=null)
                            {
                                data.Status = true;
                            }
                        }
                       
                        item.StartDate = item.StartDateTime.ToShortDateString();
                        item.EndDate = item.EndDateTime.ToShortDateString();
                        item.scripts = scriptData;
                    }

                    
                }
                
               
                //Dummy Projects Data need to replace with DB code

                
            }
            catch (Exception ex)
            {

                
            }
            return colAllProjects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/GetAllScripts")]
        [System.Web.Http.HttpGet]
        public List<Scripts> GetAllScripts()
        {
            List<Scripts> scriptData = new List<Scripts>();
            using (db = new XoriskManagementBackendEntities())
            {
                scriptData = (from b in db.Scripts

                              select new Scripts
                              {
                                  Id = b.Id,
                                  MappedFolderPath = b.StoreLocation,
                                  Name = b.Name,
                                  Status = false,
                                  ImagePath = Helper.siteUrl + b.Image,

                              }).ToList<Scripts>();
            }
            
            //try
            //{            
            //    //Dummy Projects Data need to replace with DB code

            //    for (int j = 1; j < 8; j++)
            //    {
            //        Script objScript = new Script();
            //        objScript.Id = j;
            //        objScript.Name = "System Scan Script " + j;
            //        objScript.MappedFolderPath = @"C:\Scripts";
            //        if (j % 2 == 0)
            //        {
            //            objScript.Status = true;

            //        }
            //        else
            //        {
            //            objScript.Status = false;
            //        }

            //        scriptData.Add(objScript);
            //    }

            //}
            //catch (Exception ex)
            //{


            //}
            return scriptData;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProject"></param>
        /// <returns></returns>

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/UpdateProject")]
        [System.Web.Http.HttpPost]
        

        public bool UpdateProject(ProjectDetails dataProject)
        {

            bool status = false;
            try
            {
                using (db = new XoriskManagementBackendEntities())
                {
                  Project project=  db.Projects.Find(dataProject.Id);
                    if (project!=null)
                    {
                        if (!String.IsNullOrEmpty(dataProject.StartDate))
                        {
                            project.StartDate = Helper.GetDate(dataProject.StartDate);
                            //project.StartDate = Convert.tos(dataProject.StartDate);
                        }
                        if (!String.IsNullOrEmpty(dataProject.EndDate))
                        {
                            project.EndDate = Helper.GetDate(dataProject.EndDate);
                            //project.EndDate = Convert.ToDateTime(dataProject.EndDate);
                        }
                        if (!String.IsNullOrEmpty(dataProject.Company))
                        {
                            project.Company = dataProject.Company;
                        }
                        if (!String.IsNullOrEmpty(dataProject.Email))
                        {
                            project.Email = dataProject.Email;
                        }
                        project.UpdatedAt = DateTime.Now;
                        project.POCName = dataProject.POCName;
                        //enable - disable project
                        project.Status = dataProject.Status;

                        foreach (var item in dataProject.scripts)
                        {
                            if (item.Status)
                            {
                                var projScript = (from a in db.ProjectScripts where a.ScriptId == item.Id && a.ProjectId ==dataProject.Id  select a).FirstOrDefault();
                                if (projScript ==null)
                                {
                                    Helper.createProjectScript(dataProject.Id, item.Id, dataProject.ProjectCode);
                                }
                            }
                            else
                            {
                                var projScript = (from a in db.ProjectScripts where a.ScriptId == item.Id && a.ProjectId == dataProject.Id select a).FirstOrDefault();
                                if (projScript != null)
                                {
                                    db.ProjectScripts.Remove(projScript);
                                }
                            }
                        }
                        db.SaveChanges();
                        status = true;

                    }

                }
            }
            catch (Exception ex)
            {

                
            }
            //

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProject"></param>
        /// <returns></returns>

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/CreateNewProject")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage CreateNewProject(ProjectDetails dataProject)
        {
         
            try
            {
                using (db=new XoriskManagementBackendEntities())
                {
                    Project project = new Project();
                    if (!String.IsNullOrEmpty(dataProject.StartDate))
                    {
                        //project.StartDate = Helper.GetDate(dataProject.StartDate);
                        project.StartDate = DateTime.Now;
                    }
                    if (!String.IsNullOrEmpty(dataProject.EndDate))
                    {
                        //project.EndDate = Helper.GetDate(dataProject.EndDate);
                        project.EndDate = DateTime.Now;
                    }
                    if (!String.IsNullOrEmpty(dataProject.Company))
                    {
                        project.Company = dataProject.Company;
                    }
                    if (!String.IsNullOrEmpty(dataProject.Email))
                    {
                        project.Email = dataProject.Email;
                    }
                    project.Code = Helper.GenerateProjectCode(project.Company);
                    project.Status = false;
                    project.POCName = dataProject.POCName;
                    project.UpdatedAt = DateTime.Now;
                    project.CreatedAt = DateTime.Now;
                    db.Projects.Add(project);
                    db.SaveChanges();


                    //customer registration

                    var password = Helper.CreateRandomPassword(5);
                    password = "Xorisk@1" + password;
                        
                    var user = new ApplicationUser() { UserName = project.Email, Email = project.Email };

                    IdentityResult result = UserManager.Create(user, password);

                    if (!result.Succeeded)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                            "Customer registration failed");
                    }
                   

                    //make active user
                    var customer = (from a in db.AspNetUsers where a.Email == project.Email select a).FirstOrDefault();
                    customer.EmailConfirmed = true;
                    //customer.Role = (int)RoleTypes.Customer;
                    db.SaveChanges();
                    


                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append("<p>Dear " + dataProject.POCName + ",<p>");

                    mailBody.Append("<p>Welcome to X-Link Portal of XORISK. This portal will be leveraged to collect inventory for machines that are not covered under our centralized scan for any reasons. All Inventory collected will be emailed to email ID <Insert Email ID> and optionally to XORISK if you choose to do so. All data collected is limited to inventory only and is collected/transmitted in a secure manner. You can read our policy at <Insert link> <p>");

                    mailBody.Append("<p>The Project Code assigned to your organization is " + project.Code +". You can access X-Link at this url - <Insert URL><p>");
                    mailBody.Append("<p>For any questions, please reach out to your assigned engagement representative at XORISK.<p>");
                    mailBody.Append("<p>Thank You<p>");
                    mailBody.Append("<p>Team XORISK<p>");
                    mailBody.Append("<p>info@xorisk.com<p>");
                    string subject = "XORISK- Code Generated for System Scan";
                    Helper.SendFilesInMail(subject,mailBody,project.Email);
                    foreach (var item in dataProject.scripts)
                    {
                        if (item.Status)
                        {
                            Helper.createProjectScript(project.Id, item.Id, project.Code);
                        }
                        
                    }
                    
                }
                
                return Request.CreateResponse(HttpStatusCode.Created, "Request Created");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            //return Request;
            
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("api/DashBoardController/PostScripts")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostScripts()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string filePath = string.Empty;
            string imageFilePath = string.Empty;
            Script objScript = new Script();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string MappedFileName = httpRequest.Form["MappedName"];

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        //int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (file=="script")
                        {
                            filePath = HttpContext.Current.Server.MapPath("~/Video/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            objScript.StoreLocation = "Video/" + postedFile.FileName;
                            //objScript.Image = imageFilePath;
                        }
                        else if (file=="image")
                        {
                            imageFilePath = HttpContext.Current.Server.MapPath("~/Video/" + postedFile.FileName);
                            postedFile.SaveAs(imageFilePath);
                            objScript.Image = "Video/" + postedFile.FileName;
                           // objScript.Image = imageFilePath;
                        }                     
                        
                    }

                    var message1 = string.Format("Script Uploaded Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                using (db = new XoriskManagementBackendEntities())
                {
                   
                    objScript.UpdatedAt = DateTime.Now;

                    
                    objScript.CreatedAt = DateTime.Now;
                    objScript.Name = MappedFileName;
                    db.Scripts.Add(objScript);
                    db.SaveChanges();
                }
                //var res = string.Format("Please Upload a image.");
                //dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.OK, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, dict);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/GetScanReport")]
        [System.Web.Http.HttpGet]
        public List<SystemScanReportModel> GetScanReport(string projectCode)
        {
            //TODO on logout from front end need to logout from server so call API from frond end
            //getLoggedIn user from session
            var customerEmail = User.Identity.Name;

            List<SystemScanReportModel> scriptData = new List<SystemScanReportModel>();

            try
            {
                using (db = new XoriskManagementBackendEntities())
                {
                    if (projectCode=="All")
                    {
                        scriptData = (from a in db.ScanReports
                                      orderby a.EndTime descending
                                      select new SystemScanReportModel
                                      {
                                          SystemIP = a.SystemIP,
                                          ProjectCode = a.ProjectCode,
                                          ScanStatus = a.Status,
                                          ScanStart = a.StartTime,
                                          ScanEnd = a.EndTime



                                      }).ToList<SystemScanReportModel>();
                    }else if (projectCode == "customer")
                    {
                        var customerProjectCodes = db.Projects.Where(x => x.Email == customerEmail).ToList();
                        foreach (var project in customerProjectCodes)
                        {
                            var data = (from a in db.ScanReports
                                          where a.ProjectCode == project.Code
                                          orderby a.EndTime descending
                                          select new SystemScanReportModel
                                          {
                                              SystemIP = a.SystemIP,
                                              ProjectCode = a.ProjectCode,
                                              ScanStatus = a.Status,
                                              ScanStart = a.StartTime,
                                              ScanEnd = a.EndTime
                                          }).ToList<SystemScanReportModel>();
                            scriptData.AddRange(data);

                        }
                    }
                    else
                    {
                        scriptData = (from a in db.ScanReports
                            where a.ProjectCode == projectCode
                            orderby a.EndTime descending
                            select new SystemScanReportModel
                            {
                                SystemIP = a.SystemIP,
                                ProjectCode = a.ProjectCode,
                                ScanStatus = a.Status,
                                ScanStart = a.StartTime,
                                ScanEnd = a.EndTime


                            }).ToList<SystemScanReportModel>();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return scriptData;
        }

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/EnableDisbaleProject")]
        [System.Web.Http.HttpGet]
        public void EnableDisbaleProject(string projectCode)
        {
            try
            {
                using (db=new XoriskManagementBackendEntities())
                {
                    var project = (from a in db.Projects where a.Code == projectCode select a).FirstOrDefault();
                    if (project !=null)
                    {
                        if (project.Status)
                        {
                            project.Status = false;
                        }
                        else
                        {
                            project.Status = true;

                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/CheckProjectCodeStatus")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        public bool CheckProjectCodeStatus(string projectCode)
        {
            bool status = false;
            try
            {
                using (db = new XoriskManagementBackendEntities())
                {
                    var project = (from a in db.Projects where a.Code == projectCode select a).FirstOrDefault();
                    if (project != null)
                    {
                        status = project.Status;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/GetAdminUserList")]
        [System.Web.Http.HttpGet]
        public List<AdminUser> GetAdminUserList(string userRole)
        {
            List<AdminUser> colUser = new List<AdminUser>();
            try
            {
                using (db = new XoriskManagementBackendEntities())
                {
                    colUser = (from a in db.AspNetUsers select new AdminUser {
                        Username = a.UserName,
                        Email = a.Email,
                        Active=a.EmailConfirmed
                    }).ToList<AdminUser>();
                }
            }
            catch (Exception ex)
            {

                
            }
            return colUser;
        }

        [EnableCors("*", "*", "*")]
        [System.Web.Http.Route("api/DashBoardController/ChangeUserStatus")]
        [System.Web.Http.HttpGet]
        public bool ChangeUserStatus(string userMail)
        {
            bool status = true;
            try
            {
                using (db = new XoriskManagementBackendEntities())
                {
                    var item = (from a in db.AspNetUsers where a.Email == userMail select a).FirstOrDefault();
                    if (item.EmailConfirmed)
                    {
                        item.EmailConfirmed = false;
                        //item.Role = (int)RoleTypes.Admin;
                    }
                    else
                    {
                        item.EmailConfirmed = true;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                status = false;
            }
            return status;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
