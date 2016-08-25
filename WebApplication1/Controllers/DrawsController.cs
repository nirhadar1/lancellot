using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WebApplication1.Controllers
{
    public class DrawsController : ApiController
    {
        //static string configFilePath = @"C:\LotteryServer\LotteryServer\config\LotteryServer.txt";

        static string configFilePath = @"C:\temp\LotteryServer.txt";

        public IEnumerable<Common.DrawRequestRespons> GetAllDraws()
        {
            //Common.IniFile MyIni = new Common.IniFile(configFilePath);
            DBConnector.DBConnector.Instance.init(configFilePath);
            List<Common.DrawRequestRespons> draws = new List<Common.DrawRequestRespons>();
            bool resualt = DBConnector.DBConnector.Instance.getDB().getDraws(draws);
            return draws;
        }

        public IHttpActionResult GetDraw(int id)
        {
            Common.DrawRequestRespons drawResponse = new Common.DrawRequestRespons();
            bool resualt = DBConnector.DBConnector.Instance.getDB().getDrawsRequestResualt("x", "y", "z", "m", "n", "c", "v", "b", id, drawResponse);

            if (drawResponse == null)
            {
                return NotFound();
            }
            return Ok(drawResponse);
        }

    }
}
