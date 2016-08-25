using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Data.SqlClient;


namespace WebApplication1.DBConnector
{
    using Common;
    public class Database
    {
        SqlConnection _connection;
        public void init(SqlConnection connection)
        {
            _connection = connection;
        }

        public bool getDraws(List<DrawRequestRespons> draws)
        {
            SqlDataReader rdr = null;

            string stm = "SELECT * FROM lancellotDB.dbo.draws";
            SqlCommand cmd = new SqlCommand(stm, _connection);

            using (rdr = cmd.ExecuteReader())
            {

                while (rdr.Read())
                {
                    DrawRequestRespons DrawRequestRespons = new DrawRequestRespons();
                    DrawRequestRespons.Id = rdr[0].ToString();
                    DrawRequestRespons.CountryFlag = rdr[1].ToString();
                    DrawRequestRespons.DrawLogo = rdr[2].ToString();
                    DrawRequestRespons.State = rdr[3].ToString();
                    DrawRequestRespons.Amount = rdr[4].ToString();
                    DrawRequestRespons.StartDate = rdr[5].ToString();
                    DrawRequestRespons.EndDate = rdr[6].ToString();
                    DrawRequestRespons.CreatedOn = rdr[7].ToString();
                    DrawRequestRespons.BannerImage = rdr[8].ToString();
                    DrawRequestRespons.BannerImageType = rdr[9].ToString();
                    DrawRequestRespons.PrizeAmount = rdr[10].ToString();
                    DrawRequestRespons.Status = rdr[11].ToString();
                    DrawRequestRespons.Url = rdr[12].ToString();

                    draws.Add(DrawRequestRespons);
                }
            }

            rdr.Close();
            return true;
       }

    public bool getDrawsRequestResualt(string request_type,
	 string from_record,
	 string to_record,
	 string sort_type,
	 string sort_dir,
	 string device_type,
	 string device_id,
	 string decode,
     int id,
     DrawRequestRespons DrawRequestRespons) {

            SqlDataReader rdr = null;

            string stm = String.Format("SELECT * FROM lancellotDB.dbo.draws where id = {0}", id);
            SqlCommand cmd = new SqlCommand(stm, _connection);
            rdr = cmd.ExecuteReader();

            int numOfFields = rdr.FieldCount;
            if ((numOfFields == 13) &&( rdr.Depth==0)) {
                while (rdr.Read())
                {
                    DrawRequestRespons.Id =             rdr[0].ToString();
                    DrawRequestRespons.CountryFlag =    rdr[1].ToString();
                    DrawRequestRespons.DrawLogo =       rdr[2].ToString();
                    DrawRequestRespons.State =          rdr[3].ToString();
                    DrawRequestRespons.Amount =         rdr[4].ToString();
                    DrawRequestRespons.StartDate =      rdr[5].ToString();
                    DrawRequestRespons.EndDate =        rdr[6].ToString();
                    DrawRequestRespons.CreatedOn =      rdr[7].ToString();
                    DrawRequestRespons.BannerImage =    rdr[8].ToString();
                    DrawRequestRespons.BannerImageType = rdr[9].ToString();
                    DrawRequestRespons.PrizeAmount =    rdr[10].ToString();
                    DrawRequestRespons.Status =         rdr[11].ToString();
                    DrawRequestRespons.Url =            rdr[12].ToString();
    }
            } else
            {
                return false;
            }

            //while (rdr.Read())
            //{
            //    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            //}
            rdr.Close();
            return true;
        }

    }
}
   

