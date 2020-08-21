using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GetURLEncodeForEmailIDs
{
    class Program
    {
        private static string sConn = "Data Source=10.1.203.33;Initial Catalog=CoherentRX_Production;User ID=CRX_ProductionDB;Password=2@c45m1t4;Connection Timeout=120;Column Encryption Setting=Enabled";

        static void Main(string[] args)
        {
            WriteToLog LogObj = new WriteToLog();
            LogObj.WriteToLogFile("Start --------------------------------");

            var emailIDList = getEmailIDs();


            foreach (int emailID in emailIDList)
            {
                var s = CRXModels.Utilities.Util.EncryptInfo(emailID.ToString());
                LogObj.WriteToLogFile("EmailID: " + emailID + "URLEncode Value : " + System.Web.HttpUtility.UrlEncode(s));
            }

            LogObj.WriteToLogFile("END --------------------------------");
        }

        static List<int> getEmailIDs()
        {
            var emailIDs = new List<int>();

            using (var connection = new SqlConnection(sConn))
            {
                try
                {
                    emailIDs = connection.Query<int>(sql: "EmailIDtoCreateEncryptedValue",
                                                  commandType: CommandType.StoredProcedure).AsList<int>();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }

            }
            return emailIDs;
        }
    }
}
