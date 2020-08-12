using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using System.Data;

namespace AdproLogin.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Center")]
        public string Center { get; set; }

        [Display(Name = "CenterId")]
        public int CenterId { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [DefaultValue("")]
        public string Password { get; set; }
        [DefaultValue("")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public string ErrorMsg { get; set; }
        public string MachineIP { get; set; }
        public string MachineName { get; set; }
        public int allowAdproLoginUser { get; set; }

        public bool IsValid()
        {
            AccountDAL objAccount = new AccountDAL();
            DataTable dt = objAccount.getLogin(UserName, Encrypt_Password(Password), MachineIP, allowAdproLoginUser, CenterId);
            if (dt.Rows[0]["Status"].ToString() != "1")
            {
                ErrorMsg = dt.Rows[0]["ErrorMessage"].ToString();
                return false;
            }
            else
            {
                Name = dt.Rows[0]["UserName"].ToString();
                UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                DataTable dt1 = objAccount.getMachineDetail(UserId, CenterId, MachineIP);
                if (dt1.Rows[0]["Status"].ToString() != "1")
                {
                    ErrorMsg = dt1.Rows[0]["ErrorMessage"].ToString();
                    return false;
                }
                else
                    return true;
            }

        }
        public string Encrypt_Password(string StrPass)
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
    }
}

