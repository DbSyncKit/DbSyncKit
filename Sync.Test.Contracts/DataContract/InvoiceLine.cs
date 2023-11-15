﻿using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    public class InvoiceLine : DataContractUtility<InvoiceLine>
    {
        #region Declerations
        [Key]
        public int InvoiceLineId { get; set; }
        public int CustomerId { get; set; }
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
            CustomerId = invoiceLineData.GetValue<int>("CustomerId");
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
