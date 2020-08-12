using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AdproLogin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Welcome()
        {
            try
            {
                string appRoot = ConfigurationManager.AppSettings["RootPath"];
                if (Session["UserId"] == "" || Session["UserId"] == null)
                    Response.Redirect(appRoot + "/account/login");
            }
            catch (Exception ex)
            {
                Utility.ReportError("HomeController :Welcome :: ", ex);
            } return View();
        }
        public ActionResult Filter()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetRights(string xmlData)
        {
            List<List<string>> listarray = new List<List<string>>();
            AccountDAL objAccount = new AccountDAL();
            try
            {
                DataTable dt = objAccount.getRight(xmlData).Tables[0];


                List<String> columnlist = (from dc in dt.Columns.Cast<DataColumn>()
                                           select dc.ColumnName).ToList();
                listarray.Add(columnlist);
                foreach (string clmnName in columnlist)
                {
                    List<String> itm = new List<String>();
                    itm.Add(clmnName);
                    itm.Add(dt.Rows[0][clmnName].ToString());
                    listarray.Add(itm);
                }

                //List<String> columnlist = (from dc in dt.Columns.Cast<DataColumn>()
                //                           select dc.ColumnName).ToList();
                //listarray.Add(columnlist);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    List<String> jst = dr.ItemArray.Select(o => o.ToString()).ToList();
                //    listarray.Add(jst);
                //}
            }
            catch (Exception ex)
            {
                Utility.ReportError("HomeController :GetRights :: ", ex, xmlData);
            }
            return Json(listarray);
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
