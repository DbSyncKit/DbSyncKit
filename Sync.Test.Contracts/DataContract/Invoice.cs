using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("Invoice"), TableSchema("dbo")]

    public class Invoice : DataContractUtility<Invoice>
    {
        #region Declerations
        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingPostalCode { get; set; }
        public Decimal Total { get; set; }

        #endregion

        #region Constructor

        public Invoice(DataRow invoiceData)
        {
            if (invoiceData == null)
                throw new ArgumentNullException(nameof(invoiceData));

            InvoiceId = invoiceData.GetValue<int>("InvoiceId");
            CustomerId = invoiceData.GetValue<int>("CustomerId");
            InvoiceDate = invoiceData.GetValue<DateTime>("InvoiceDate");

            BillingAddress = invoiceData.GetValue<string>("BillingAddress");
            BillingCity = invoiceData.GetValue<string>("BillingCity");
            BillingState = invoiceData.GetValue<string>("BillingState");
            BillingCountry = invoiceData.GetValue<string>("BillingCountry");
            BillingPostalCode = invoiceData.GetValue<string>("BillingPostalCode");

            if (invoiceData.IsNull("Total"))
                throw new ArgumentNullException(nameof(Total), "Total cannot be null.");

            Total = invoiceData.GetValue<Decimal>("Total");
        }

        #endregion
    }
}
