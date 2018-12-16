using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using XoRisk_SelfService.DataModels;

namespace XoRisk_SelfService.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ValuesController()
        {
            
        }
        public ValuesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [EnableCors("*", "*", "*")]
        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(string email)
        {

            //todo random logic to create password
            var newPassord = Helper.CreateRandomPassword(8);
            newPassord = "Xorisk@1" + newPassord;
            var user = UserManager.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                var removePasswordResult= await UserManager.RemovePasswordAsync(user.Id);
                if (removePasswordResult.Succeeded)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(user.Id, newPassord);
                    if (result.Succeeded)
                    {
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append("<p>Dear " + user.UserName + ",<p>");

                        mailBody.Append("<p>Your password has been reset. please use below password to login</p>");

                        mailBody.Append("<p> New Password - " + newPassord + " <p>");
                        mailBody.Append("<p>Thank You<p>");
                        mailBody.Append("<p>Team XORISK<p>");
                        mailBody.Append("<p>info@xorisk.com<p>");
                        string subject = "XORISK- Forgot Password";
                        var status = Helper.SendFilesInMail(subject, mailBody, email);

                        if (!status)
                        {
                            return GetErrorResult(result);
                        }
                    }
                    else
                    {
                        return GetErrorResult(result);
                    }
                }
                else
                {
                    return GetErrorResult(removePasswordResult);
                }

            }
            else
            {
                return GetErrorResult(new IdentityResult());
            }

            return Ok();
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
