using System.Data.SqlTypes;
using RX.Svng.Web.Data.Exceptions;
using RX.Svng.Web.Service.Models;
using Xunit;

namespace RX.Svng.Web.Service.Test.Models
{
    public class PharmacyModelTest
    {
        [Fact]
        public void NameIsEmptyWhenAddressIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyModel
            {
                Name = "",
                Address = "Address1"
            });
        }

        [Fact]
        public void NameIsNullWhenAddressIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyModel
            {
                Name = null,
                Address = "Address1"
            });
        }

        [Fact]
        public void NameIsOutOfRangeWhenAddressIsSet()
        {
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyModel
            {
                Name = @"Morethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan
256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersname
Morethan256charactersnameMorethan256charactersname",
                Address = "Address1"
            });
        }


        [Fact]
        public void AddressIsEmptyWhenNameIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyModel
            {
                Name = "Name1",
                Address = ""
            });
        }

        [Fact]
        public void AddressIsNullWhenNameIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyModel
            {
                Name = "Name1",
                Address = null
            });
        }

        [Fact]
        public void AddressIsOutOfRangeWhenNameIsSet()
        {
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyModel
            {
                Name = "Name1",
                Address = @"Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress"
            });
        }

        [Fact]
        public void NameAndAddresOutOfRange()
        {
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyModel
            {
                Name = @"Morethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan
256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersnameMorethan256charactersname
Morethan256charactersnameMorethan256charactersname",
                Address = @"Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress
Morethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddressMorethan500charactersAddress"
            });
        }

        [Fact]
        public void NameAndAddresValid()
        {
            Assert.IsType<PharmacyModel>(new PharmacyModel
            {
                Name = "Name",
                Address = "Address1"
            });
        }
    }
}
