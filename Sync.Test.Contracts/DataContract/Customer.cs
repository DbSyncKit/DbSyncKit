using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("Customer"), TableSchema("dbo")]

    public class Customer : DataContractUtility<Customer>
    {
        #region Properties
        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int SupportRepId { get; set; }

        #endregion

        #region Constructor

        public Customer(DataRow customerData)
        {
            if (customerData == null)
                throw new ArgumentNullException(nameof(customerData));

            CustomerId = customerData.GetValue<int>("CustomerId");
            if (customerData.IsNull("FirstName"))
                throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be null.");

            FirstName = customerData.GetValue<string>("FirstName");

            if (customerData.IsNull("LastName"))
                throw new ArgumentNullException(nameof(LastName), "LastName cannot be null.");

            LastName = customerData.GetValue<string>("LastName");
            Company = customerData.GetValue<string>("Company");
            Address = customerData.GetValue<string>("Address");
            City = customerData.GetValue<string>("City");
            State = customerData.GetValue<string>("State");
            Country = customerData.GetValue<string>("Country");
            PostalCode = customerData.GetValue<string>("PostalCode");
            Phone = customerData.GetValue<string>("Phone");
            Fax = customerData.GetValue<string>("Fax");
            Email = customerData.GetValue<string>("Email");
            SupportRepId = customerData.GetValue<int>("SupportRepId");
        }

        #endregion
    }
}
