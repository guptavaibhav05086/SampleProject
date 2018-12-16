using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using XoRisk_SelfService.DataModels;

namespace XoRisk_SelfService.Controllers
{
    public class FilesController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/FilesController/GetFiles")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetFiles(string fileName )
        {
            try
            {
                //Check if the file exists. If the file doesn't exist, throw a file not found exception
                //var downloadFilePath = HttpContext.Current.Server.MapPath("~/Video/" + fileName);
                var downloadFilePath = HttpContext.Current.Server.MapPath("~/" + fileName);
                string file = fileName.Split('/')[1];
                if (!System.IO.File.Exists(downloadFilePath))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                //Copy the source file stream to MemoryStream and close the file stream
                MemoryStream responseStream = new MemoryStream();
                Stream fileStream = System.IO.File.Open(downloadFilePath, FileMode.Open);

                fileStream.CopyTo(responseStream);
                fileStream.Close();
                responseStream.Position = 0;

                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;

                //Write the memory stream to HttpResponseMessage content
                response.Content = new StreamContent(responseStream);
                string contentDisposition = string.Concat("attachment; filename=", file);
                response.Content.Headers.ContentDisposition =
                              ContentDispositionHeaderValue.Parse(contentDisposition);
                return response;
            }
            catch(Exception ex)
            {
              return  Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                //throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [EnableCors("*", "*", "*")]
        [Route("api/FilesController/GetScriptNames")]
        [HttpGet]
        [AllowAnonymous]
        public List<string> GetScriptNames(string ProjectCode)
        {
            List<string> objResult = new List<string>();
            string pCode = ProjectCode.Trim();
            using (XoriskManagementBackendEntities db = new XoriskManagementBackendEntities())
            {
                try
                {
                    objResult = (from a in db.ProjectScripts
                                  join b in db.Projects
                                  on a.ProjectId equals b.Id
                                  join c in db.Scripts on a.ScriptId equals c.Id
                                  where b.Code == pCode
                                  select c.StoreLocation).ToList<string>();
                    

                }
                catch (Exception ex)
                {

                    throw;
                }
                return objResult;
            }
        }
        [EnableCors("*", "*", "*")]
        [Route("api/FilesController/GetScriptFileNames")]
        [HttpGet]
        [AllowAnonymous]
        public List<string> GetScriptFileNames(string ProjectCode)
        {
            List<string> objResult = new List<string>();
            string pCode = ProjectCode.Trim();
            using (XoriskManagementBackendEntities db = new XoriskManagementBackendEntities())
            {
                try
                {
                    objResult = (from a in db.ProjectScripts
                                 join b in db.Projects
                                 on a.ProjectId equals b.Id
                                 join c in db.Scripts on a.ScriptId equals c.Id
                                 where b.Code == pCode
                                 select c.Name).ToList<string>();


                }
                catch (Exception ex)
                {

                    throw;
                }
                return objResult;
            }
        }

        [EnableCors("*", "*", "*")]

        [Route("api/FilesController/UpdateScanReportStatus")]

        [HttpGet]

        [AllowAnonymous]

        public HttpResponseMessage UpdateScanReportStatus(string projectCode, string systemIP, string startTime, string endTime, string status, string scriptName,string hostName)

        {
            using (XoriskManagementBackendEntities db = new XoriskManagementBackendEntities())
            {
                ScanReport newReport = new ScanReport();
                newReport.CreatedAt = DateTime.Now;
                newReport.UpdatedAt = DateTime.Now;
                newReport.ProjectCode = projectCode;
                newReport.SystemIP = systemIP;
                DateTime s;
                DateTime.TryParse(startTime, out s);
                DateTime e;
                DateTime.TryParse(endTime, out e);
                //DateTime startDate = Helper.GetDate(startTime);
                newReport.StartTime =s;
                newReport.EndTime = e;
                //newReport.StartTime = Helper.GetDate(startTime);
                //newReport.EndTime = Helper.GetDate(endTime);
                //newReport.StartTime = startTime;
                //newReport.EndTime = endTime;
                newReport.Status = status;
                newReport.ScriptName = scriptName;
                newReport.SystemMAC = hostName;
                db.ScanReports.Add(newReport);
                db.SaveChanges();

                //newReport.Status = status;
            }
            HttpResponseMessage response = new HttpResponseMessage();

            response.StatusCode = HttpStatusCode.OK;

            return response;

            // return new string[] { "Video/Script1.vbe", "Video/Script2.vbe", "Video/Script3.vbe", "Video/Script4.vbe" };

        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/FilesController/GetProjectCodeFile")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetProjectCodeFile(string projectCode)
        {
            string file = projectCode.Trim() + ".txt";
            var downloadFilePath = HttpContext.Current.Server.MapPath("~/Video/" + file);

            if (!File.Exists(downloadFilePath))
            {
                using (StreamWriter sw = File.CreateText(downloadFilePath))
                {

                    sw.WriteLine("ProjectCode : " + projectCode.Trim());
                    //sw.WriteLine(projectCode);
                }
            }
           

            MemoryStream responseStream = new MemoryStream();
            Stream fileStream = System.IO.File.Open(downloadFilePath, FileMode.Open);

            fileStream.CopyTo(responseStream);
            fileStream.Close();
            responseStream.Position = 0;

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            //Write the memory stream to HttpResponseMessage content
            response.Content = new StreamContent(responseStream);
            string contentDisposition = string.Concat("attachment; filename=", file);
            response.Content.Headers.ContentDisposition =
                          ContentDispositionHeaderValue.Parse(contentDisposition);
            return response;

            // Create a new file 
            
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/FilesController/GetExecutableFile")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetExecutableFile()
        {
            try
            {
                string file = "Xorisk_SelfService.exe";
                var downloadFilePath = HttpContext.Current.Server.MapPath("~/Video/" + file);
                //Copy the source file stream to MemoryStream and close the file stream
                MemoryStream responseStream = new MemoryStream();
                Stream fileStream = System.IO.File.Open(downloadFilePath, FileMode.Open);

                fileStream.CopyTo(responseStream);
                fileStream.Close();
                responseStream.Position = 0;

                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;

                //Write the memory stream to HttpResponseMessage content
                response.Content = new StreamContent(responseStream);
                string contentDisposition = string.Concat("attachment; filename=", file);
                response.Content.Headers.ContentDisposition =
                              ContentDispositionHeaderValue.Parse(contentDisposition);
                return response;
            }
            catch (Exception ex)
            {
               
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                //HttpResponseMessage response = new HttpResponseMessage();
                //response.StatusCode = HttpStatusCode.InternalServerError;
                //return response;
            }
           
        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[Route("api/FilesController/GetExcelScanReport")]
        //[HttpGet]
        //[AllowAnonymous]
        //public HttpResponseMessage GetExcelScanReport()
        //{
        //    try
        //    {
        //        string filePath = HttpContext.Current.Server.MapPath("~/Video/" + "ScanReport");
        //        Helper.CreateExcel(filePath,"Scan");
        //        MemoryStream responseStream = new MemoryStream();
        //        Stream fileStream = System.IO.File.Open(filePath + ".xlsx", FileMode.Open);

        //        fileStream.CopyTo(responseStream);
        //        fileStream.Close();
        //        responseStream.Position = 0;

        //        HttpResponseMessage response = new HttpResponseMessage();
        //        response.StatusCode = HttpStatusCode.OK;

        //        //Write the memory stream to HttpResponseMessage content
        //        response.Content = new StreamContent(responseStream);
        //        string contentDisposition = string.Concat("attachment; filename=", "ScanReport.xlsx");
        //        response.Content.Headers.ContentDisposition =
        //                      ContentDispositionHeaderValue.Parse(contentDisposition);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
           
        //}
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[Route("api/FilesController/GetExcelProjectReport")]
        //[HttpGet]
        //[AllowAnonymous]
        //public HttpResponseMessage GetExcelProjectReport()
        //{
        //    try
        //    {
        //        string filePath = HttpContext.Current.Server.MapPath("~/Video/" + "ProjectReport");
        //        Helper.CreateExcel(filePath,"Project");
        //        MemoryStream responseStream = new MemoryStream();
        //        Stream fileStream = System.IO.File.Open(filePath + ".xlsx", FileMode.Open);

        //        fileStream.CopyTo(responseStream);
        //        fileStream.Close();
        //        responseStream.Position = 0;

        //        HttpResponseMessage response = new HttpResponseMessage();
        //        response.StatusCode = HttpStatusCode.OK;

        //        //Write the memory stream to HttpResponseMessage content
        //        response.Content = new StreamContent(responseStream);
        //        string contentDisposition = string.Concat("attachment; filename=", "ScanReport.xlsx");
        //        response.Content.Headers.ContentDisposition =
        //                      ContentDispositionHeaderValue.Parse(contentDisposition);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/FilesController/DeleteScript")]
        [HttpPost]
        
        public HttpResponseMessage DeleteScripts(List<Scripts> deleteScripts)
        {
            try
            {
                using (XoriskManagementBackendEntities db=new XoriskManagementBackendEntities())
                {
                    foreach (var item in deleteScripts)
                    {
                        var a = (from b in db.Scripts where b.Id == item.Id select b).FirstOrDefault();
                        string pathScript = HttpContext.Current.Server.MapPath("~/" + a.StoreLocation);
                        string imagePath = HttpContext.Current.Server.MapPath("~/" + a.Image);
                        File.Delete(pathScript);
                        File.Delete(imagePath);
                        db.Scripts.Remove(a);
                }
                    db.SaveChanges();
                }
                
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
