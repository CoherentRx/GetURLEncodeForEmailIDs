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
        private static string tokenVector = System.Configuration.ConfigurationManager.AppSettings["TokenVector"];
        private static string tokenKey = System.Configuration.ConfigurationManager.AppSettings["TokenKey"];


        static void Main(string[] args)
        {
            WriteToLog LogObj = new WriteToLog();
            Console.WriteLine("GetURLEncodeForEmailIDs STARTED");
            LogObj.WriteToLogFile("Start --------------------------------");

            var emailIDList = getEmailIDs();


            foreach (int emailID in emailIDList)
            {
                CRXModels.Utilities.Util.EncryptInfo(emailID.ToString());
                LogObj.WriteToLogFile("EmailID: " + emailID + "  | URLEncode Value : " + System.Web.HttpUtility.UrlEncode(CRXModels.Utilities.Util.EncryptInfo(emailID.ToString())));
            }

            LogObj.WriteToLogFile("END --------------------------------");
            Console.WriteLine("GetURLEncodeForEmailIDs ENDED");
            Console.Read();
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
