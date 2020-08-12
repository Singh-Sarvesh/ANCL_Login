using System.Configuration;
using System.Data;
using DBInterface;

namespace AdproLogin
{
    public class AccountDAL
    {
        string ConnectionString
        {
            get
            {
                string conn = ConfigurationSettings.AppSettings["DefaultConnection"].ToString();
                return conn;
            }
        }

        //Old code
        //public DataTable getLogin(string UserName, string Password, string MachineIP, int allowAdproLoginUser, int CenterId)
        //{
        //    using (DBManager db = new DBManager())
        //    {
        //        db.Open();
        //        db.CreateParameters(6);
        //        db.AddParameters(0, "@UserName", UserName);
        //        db.AddParameters(1, "@Password", Password);
        //        db.AddParameters(2, "@MachineIP", MachineIP);
        //        db.AddParameters(3, "@msg", "", ParameterDirection.Output, 1000);
        //        db.AddParameters(4, "@allowAdproLoginUser", allowAdproLoginUser);
        //        db.AddParameters(5, "@CenterId", CenterId);
        //        DataTable dt = new DataTable();
        //        dt = db.ExecuteDataSet(CommandType.StoredProcedure, "usp_EBK_GetLogin").Tables[0];
        //        dt.Columns.Add("msg", typeof(System.String));
        //        dt.Rows[0]["msg"] = db.Parameters[3].Value.ToString();
        //        return dt;
        //    }
        //}

        // New Code
        public DataTable getLogin(string UserName, string Password, string MachineIP, int allowAdproLoginUser, int CenterId)
        {
            string xmlParameter = "<parameter><filtername>getlogin</filtername>"
                                + "<username>" + UserName + "</username>"
                                + "<password>" + Password + "</password>"
                                + "<machineip>" + MachineIP + "</machineip>"
                                + "<centerid>" + CenterId + "</centerid>"
                                + "</parameter>";
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                db.AddParameters(1, "@msg", "", ParameterDirection.Output, 1000);
                DataTable dt = new DataTable();
                dt = db.ExecuteDataSet(CommandType.StoredProcedure, "EBK_SP_FillLoginControl").Tables[0];
                return dt;
            }
        }
        public DataTable getMachineDetail(int UserId, int CenterId, string MachineIP)
        {
            string xmlParameter = "<parameter><filtername>machinedetail</filtername>"
                                + "<userid>" + UserId + "</userid>"
                                + "<centerid>" + CenterId + "</centerid>"
                                + "<machinename>" + MachineIP + "</machinename>"
                                + "</parameter>";
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                DataTable dt = new DataTable();
                dt = db.ExecuteDataSet(CommandType.StoredProcedure, "EBK_SP_FillLoginControl").Tables[0];
                return dt;
            }
        }

        public DataSet getCenter(string xmlParameter)
        {
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                return db.ExecuteDataSet(CommandType.StoredProcedure, "[EBK_SP_FillLoginControl]");
            }
        }
        public DataSet getRight(string xmlParameter)
        {
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                return db.ExecuteDataSet(CommandType.StoredProcedure, "[EBK_SP_FillLoginControl]");
            }
        }
        // Old Code
        //public void getLogout(int userId, string MachineIP, string msg, int allowAdproLoginUser, int CenterId)
        //{
        //    using (DBManager db = new DBManager())
        //    {
        //        db.Open();
        //        db.CreateParameters(5);
        //        db.AddParameters(0, "@UserId", userId);
        //        db.AddParameters(1, "@MachineIP", MachineIP);
        //        db.AddParameters(2, "@msg", msg);
        //        db.AddParameters(3, "@allowAdproLoginUser", allowAdproLoginUser);
        //        db.AddParameters(4, "@BookingCentreID", CenterId);
        //        db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_EBK_GetLogout");
        //    }
        //}

        // New Code

        public DataSet getChangePassword(string xmlParameter)
        {
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                return db.ExecuteDataSet(CommandType.StoredProcedure, "[EBK_SP_FillLoginControl]");
            }
        }

        public void getLogout(int userId, string MachineIP, string msg, int allowAdproLoginUser, int CenterId)
        {
            string xmlParameter = "<parameter><filtername>getlogout</filtername>"
                                + "<userid>" + userId + "</userid>"
                                + "<machineip>" + MachineIP + "</machineip>"
                                + "<centerid>" + CenterId + "</centerid>"
                                + "</parameter>";
            using (DBManager db = new DBManager())
            {
                db.Open();
                db.CreateParameters(1);
                db.AddParameters(0, "@xmlParameter", xmlParameter);
                db.ExecuteNonQuery(CommandType.StoredProcedure, "EBK_SP_FillLoginControl");
            }
        }

    }
}