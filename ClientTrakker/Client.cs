using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTrakker
{
    public class Client
    {
        // private data
        string clientFirstName;
        string clientLastName;
        string clientPhone;
        string clientEmail;
        string clientAddress;
        string clientCity;
        string clientProvince;
        string clientCountry;

        public string ClientFirstName
        {
            get { return this.clientFirstName; }
            set { this.clientFirstName = value; }
        }

        public string ClientLastName
        {
            get { return this.clientLastName; }
            set { this.clientLastName = value; }
        }

        public string ClientPhone
        {
            get { return this.clientPhone; }
            set { this.clientPhone = value; }
        }

        public string ClientEmail
        {
            get { return this.clientEmail; }
            set { this.clientEmail = value; }
        }

        public string ClientAddress
        {
            get { return this.clientAddress; }
            set { this.clientAddress = value; }
        }

        public string ClientCity
        {
            get { return this.clientCity; }
            set { this.clientCity = value; }
        }

        public string ClientProvince
        {
            get { return this.clientProvince; }
            set { this.clientProvince = value; }
        }

        public string ClientCountry
        {
            get { return this.clientCountry; }
            set { this.clientCountry = value; }
        }

        public Client(string clientFirstName, string clientLastName, string clientPhone, string clientEmail, string clientAddress, string clientCity, string clientProvince, string clientCountry)
        {
            this.clientFirstName = clientFirstName;
            this.clientLastName = clientLastName;
            this.clientPhone = clientPhone;
            this.clientEmail = clientEmail;
            this.clientAddress = clientAddress;
            this.clientCity = clientCity;
            this.clientProvince = clientProvince;
            this.clientCountry = clientCountry;
        }

        public override string ToString()
        {
            return clientFirstName + "\t" + clientLastName + "\t" + clientPhone + "\t" + clientEmail + "\t" + clientAddress + "\t" + clientCity + "\t" + clientProvince + "\t" + clientCountry;
        }
    }
}
