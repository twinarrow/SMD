using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using miniSmartCloudService;
using System.Data.SqlClient;

namespace TestminiSmartService
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ScoreFrame VehichleScore = new ScoreFrame();
            VehichleScore.Name = "Vasanth";
            VehichleScore.Score = 25.3M;
            VehichleScore.VINNumber = "MA1YL2HJUB6A60004U";
            VehichleScore.Rank = "0.0";


            SqlConnection sqlconn = new SqlConnection("Server=tcp:mminismartadmin.database.windows.net,1433;Initial Catalog=miniSMART;Persist Security Info=False;User ID=admin123;Password=Admin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            string str = "Select * from tab_m_VIN_SCORE";
            SqlCommand myCommand = new SqlCommand(str, sqlconn);
            Service1 myservice = new Service1();
            var response = myservice.SubmitScoreData(VehichleScore);
            Assert.AreEqual(response.IsSuccessful, true);
        }
    }
}
