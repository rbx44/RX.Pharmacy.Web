using System.Data.SqlTypes;
using RX.Svng.Web.Data.Exceptions;

namespace RX.Svng.Web.Service.Models
{
    public class PharmacyModel
    {
        private string _name;
        private string _address;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
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

        protected bool Equals(PharmacyModel other)
        {
            return string.Equals(_name, other._name) && string.Equals(_address, other._address) && Distance == other.Distance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PharmacyModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_address != null ? _address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Distance.GetHashCode();
                return hashCode;
            }
        }
    }
}
