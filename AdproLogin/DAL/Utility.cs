using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.IO;
/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
    public static void ReportError(string message, Exception exceptionMessage,string xml="")
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLogGenerate"] == "1")
            {
                FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs1);
                writer.WriteLine("----" + message + "------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "-----------------\n" + exceptionMessage.Message +"\n"+ xml);
                writer.Close();
            }
        }
        catch (Exception ex) { }
    }
    public static string SaveToLog(string message)
    {
        string path = "";
        try
        {
            path = HttpContext.Current.Server.MapPath("~/log.txt");
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLogGenerate"] == "1")
            {
                FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs1);
                writer.WriteLine("----------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "-----------------\n" + message);
                writer.Close();
            }
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
