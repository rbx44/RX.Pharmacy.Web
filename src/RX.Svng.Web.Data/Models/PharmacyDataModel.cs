using System.Data.SqlTypes;
using RX.Svng.Web.Data.Exceptions;

namespace RX.Svng.Web.Data.Models
{
    public class PharmacyDataModel
    {
        private string _name;
        private string _address;
        public string Name
        {
            get => _name;
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new SqlNullValueException("Pharmacy name cannot be null");

                if (value.Length > 256)
                    throw new SqlValueOutOfRangeException("Pharmacy name cannot be more than 256 characters");

                _name = value;
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new SqlNullValueException("Pharmacy address cannot be null");

                if (value.Length > 500)
                    throw new SqlValueOutOfRangeException("Pharmacy address cannot be more than 500 characters");

                _address = value;
            }
        }
        public decimal Distance { get; set; }
    }
}
