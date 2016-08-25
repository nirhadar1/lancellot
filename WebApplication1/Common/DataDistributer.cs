using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebApplication1.Common
{
    public class DrawRequestRespons
    {
        private string _id;
        private string _countryFlag;
        private string _drawLogo;
        private string _state;
        private string _amount;
        private string _startDate;
        private string _endDate;
        private string _createdOn;
        private string _bannerImage;
        private string _bannerImageType;
        private string _prizeAmount;
        private string _status;
        private string _url;

        public string Id {          
            get { return this._id; }
            set { this._id = value; }
        }

        public string CountryFlag
        {
            get { return this._countryFlag; }
            set { this._countryFlag = value; }
        }

        public string DrawLogo
        {
            get { return this._drawLogo; }
            set { this._drawLogo = value; }
        }

        public string State
        {
            get { return this._state; }
            set { this._state = value; }
        }

        public string Amount
        {
            get { return this._amount; }
            set { this._amount = value; }
        }

        public string StartDate
        {
            get { return this._startDate; }
            set { this._startDate = value; }
        }

        public string EndDate
        {
            get { return this._endDate; }
            set { this._endDate = value; }
        }

        public string CreatedOn
        {
            get { return this._createdOn; }
            set { this._createdOn = value; }
        }

        public string BannerImage
        {
            get { return this._bannerImage; }
            set { this._bannerImage = value; }
        }

        public string BannerImageType
        {
            get { return this._bannerImageType; }
            set { this._bannerImageType = value; }
        }

        public string PrizeAmount
        {
            get { return this._prizeAmount; }
            set { this._prizeAmount = value; }
        }

        public string Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        public string Url
        {
            get { return this._url; }
            set { this._url = value; }
        }

        public  string print()
        {            
                string str;
                str = "ID: " + this.Id + "\n";
                str += "CountryFlag: " + this.CountryFlag + "\n";
                str += "DrawLogo: " + this.DrawLogo + "\n";
                str += "State: " + this.State + "\n";
                str += "Amount: " + this.Amount + "\n";
                str += "StartDate: " + this.StartDate + "\n";
                str += "EndDate: " + this.EndDate + "\n";
                str += "CreatedOn: " + this.CreatedOn + "\n";
                str += "BannerImage: " + this.BannerImage + "\n";
                str += "BannerImageType: " + this.BannerImageType + "\n";
                str += "PrizeAmount: " + this.PrizeAmount + "\n";
                str += "Status: " + this.Status + "\n";
                str += "Url: " + this.Url;
                return str;            
        }
    }
}
