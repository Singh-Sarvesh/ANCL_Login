using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using AdproLogin.Models;

namespace AdproLogin.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            try
            {
                Session["ErrorMsg"] = null;
                if (Session["UserId"] != null)
                    Response.Redirect(System.Web.Configuration.WebConfigurationManager.AppSettings["RootPath"] + "home/welcome", true);
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :Login::", ex);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.LoginViewModel user, FormCollection form)
        {
            try
            {
                Session["ErrorMsg"] = null;
                if (ModelState.IsValid)
                {
                    user.MachineIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (user.MachineIP == "" || user.MachineIP == null)
                        user.MachineIP = Request.ServerVariables["REMOTE_ADDR"];
                    user.MachineName = System.Environment.GetEnvironmentVariable("COMPUTERNAME"); ;
                    if (user.IsValid())
                    {
                        Session["ErrorMsg"] = null;
                        Session["Center"] = user.Center;
                        Session["CenterId"] = user.CenterId;
                        Session["Name"] = user.Name;
                        Session["UserId"] = user.UserId;
                        return RedirectToAction("Welcome", "Home");
                        // Response.Redirect("/home/welcome");
                    }
                    else
                        Session["ErrorMsg"] = user.ErrorMsg;
                }
                else
                    Session["ErrorMsg"] = "Please fill Username!";
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :Login::", ex);
            }
            return View(user);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetCenter(string xmlData)
        {
            List<ControlData> lst = new List<ControlData>();
            AccountDAL objAccount = new AccountDAL();
            try
            {
                DataTable dt = objAccount.getCenter(xmlData).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ControlData p = new ControlData();
                        p.value = Convert.ToString(dr[0]);
                        p.key = Convert.ToString(dr[1]);
                        lst.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :GetCenter :: ", ex, xmlData);
            }
            return Json(lst);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CheckOldPassword(string username, string oldpassword)
        {
            List<ControlData> lst = new List<ControlData>();
            oldpassword = EncryptNew_Password(oldpassword);
            AccountDAL objAccount = new AccountDAL();
            string xmlData = "<parameter><filtername>getcheckoldpassword</filtername><username>" + username + "</username>"
                           + "<oldpassword>" + oldpassword + "</oldpassword></parameter>";
            try
            {
                DataTable dt = objAccount.getChangePassword(xmlData).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ControlData p = new ControlData();
                        p.value = Convert.ToString(dr[0]);
                        p.key = Convert.ToString(dr[1]);
                        lst.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :GetChangePassword :: ", ex, xmlData);
            }
            return Json(lst);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangePassword(string username, string oldpassword, string newpassword)
        {
            List<ControlData> lst = new List<ControlData>();
            oldpassword = EncryptNew_Password(oldpassword);
            newpassword = EncryptNew_Password(newpassword);
            AccountDAL objAccount = new AccountDAL();
            string xmlData = "<parameter><filtername>getchangepassword</filtername><username>" + username + "</username>"
                           + "<oldpassword>" + oldpassword + "</oldpassword>"
                           + "<newpassword>" + newpassword + "</newpassword></parameter>";
            try
            {
                DataTable dt = objAccount.getChangePassword(xmlData).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ControlData p = new ControlData();
                        p.value = Convert.ToString(dr[0]);
                        p.key = Convert.ToString(dr[1]);
                        lst.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :GetChangePassword :: ", ex, xmlData);
            }
            return Json(lst);
        }

        public string EncryptNew_Password(string StrPass)
        {
            string Result = "";
            int FConstant1 = 52845, Fconstant2 = 22719, key = 19937;
            Byte b = 0;
            int len = (int)StrPass.Length;
            if (len == 0) return Result;
            char[] ChrValue = StrPass.ToCharArray();
            foreach (char letter in ChrValue)
            {
                b = (Byte)(Convert.ToByte(letter) ^ (key >> 8));
                key = (b + key) * FConstant1 + Fconstant2;
                Result += String.Format("{0,2:X}", b);
            }
            return Result.Replace(' ', '0');
        }

        [HttpPost]
        public JsonResult Logout()
        {
            try
            {
                UserLogOut();
                Session.Abandon();
                Session.Clear();
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :GetCenter :: ", ex);
            }
            Response.Redirect("Login", true);
            return Json('1');
        }        
        public ActionResult LogoutUser()
        {
            try
            {
                UserLogOut();
                Session.Abandon();
                Session.Clear();
            }
            catch (Exception ex)
            {
                Utility.ReportError("AccountController :GetCenter :: ", ex);
            }
            Response.Redirect("Login", true);
            return View("login");
        }
        public void UserLogOut()
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    string ClintIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (ClintIP == "" || ClintIP == null)
                    {
                        ClintIP = Request.ServerVariables["REMOTE_ADDR"];
                    }
                    AccountDAL objAccount = new AccountDAL();
                    objAccount.getLogout(Convert.ToInt32(Session["UserId"]), ClintIP, "", 0, Convert.ToInt32(Session["CenterId"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetSessionValue(string xmlParameter)
        {
            List<string> result = new List<string>();
            result.Add(Session[xmlParameter] == null ? "" : Session[xmlParameter].ToString());
            return Json(result);
        }

        [HttpPost]
        public JsonResult ReadBuildDetailFile()
        {
            string text = "";
            string BuildDetailPath = System.Web.HttpContext.Current.Server.MapPath("~/BuildDetail.txt");
            text = System.IO.File.ReadAllText(BuildDetailPath);
            return Json(text);
        }

    }

}
