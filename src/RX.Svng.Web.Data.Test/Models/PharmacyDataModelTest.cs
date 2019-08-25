using System.Data.SqlTypes;
using RX.Svng.Web.Data.Exceptions;
using RX.Svng.Web.Data.Models;
using Xunit;

namespace RX.Svng.Web.Data.Test.Models
{
    public class PharmacyDataModelTest
    {
        [Fact]
        public void NameIsEmptyWhenAddressIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyDataModel
            {
                Name = "",
                Address = "Address1"
            });
        }

        [Fact]
        public void NameIsNullWhenAddressIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyDataModel
            {
                Name = null,
                Address = "Address1"
            });
        }

        [Fact]
        public void NameIsOutOfRangeWhenAddressIsSet()
        {
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyDataModel
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
            Assert.Throws<SqlNullValueException>(() => new PharmacyDataModel
            {
                Name = "Name1",
                Address = ""
            });
        }

        [Fact]
        public void AddressIsNullWhenNameIsSet()
        {
            Assert.Throws<SqlNullValueException>(() => new PharmacyDataModel
            {
                Name = "Name1",
                Address = null
            });
        }

        [Fact]
        public void AddressIsOutOfRangeWhenNameIsSet()
        {
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyDataModel
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
            Assert.Throws<SqlValueOutOfRangeException>(() => new PharmacyDataModel
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
            Assert.IsType<PharmacyDataModel>(new PharmacyDataModel
            {
                Name = "Name",
                Address = "Address1"
            });
        }
    }
}
