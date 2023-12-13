using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("InvoiceLine")]
    public class InvoiceLine : DataContractUtility<InvoiceLine>
    {
        #region Declerations
        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int InvoiceLineId { get; set; }
        public int InvoiceId { get; set; }
        public int TrackId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }


        #endregion

        #region Constructor

        public InvoiceLine(DataRow invoiceLineData)
        {
            if (invoiceLineData == null)
                throw new ArgumentNullException(nameof(invoiceLineData));

            InvoiceLineId = invoiceLineData.GetValue<int>("InvoiceLineId");
            InvoiceId = invoiceLineData.GetValue<int>("InvoiceId");
            TrackId = invoiceLineData.GetValue<int>("TrackId");

            if (invoiceLineData.IsNull("UnitPrice"))
                throw new ArgumentNullException(nameof(UnitPrice), "UnitPrice cannot be null.");

            UnitPrice = invoiceLineData.GetValue<decimal>("UnitPrice");

            if (invoiceLineData.IsNull("Quantity"))
                throw new ArgumentNullException(nameof(Quantity), "Quantity cannot be null.");

            Quantity = invoiceLineData.GetValue<int>("Quantity");
        }

        #endregion
    }
}
