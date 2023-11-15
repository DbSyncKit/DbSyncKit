using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace Sync.Test.SampleContract.DataContract
{
    public class Employee : DataContractUtility<Employee>
    {
        #region Decleration
        [Key]
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public int ReportsTo { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        #endregion

        #region Constructor

        public Employee(DataRow employeeData)
        {
            if (employeeData == null)
                throw new ArgumentNullException(nameof(employeeData));

            EmployeeId = employeeData.GetValue<int>("EmployeeId");

            if (employeeData.IsNull("LastName"))
                throw new ArgumentNullException(nameof(LastName), "LastName cannot be null.");

            LastName = employeeData.GetValue<string>("LastName");

            if (employeeData.IsNull("FirstName"))
                throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be null.");

            FirstName = employeeData.GetValue<string>("FirstName");

            Title = employeeData.GetValue<string>("Title");
            ReportsTo = employeeData.GetValue<int>("ReportsTo");
            BirthDate = employeeData.GetValue<DateTime>("BirthDate");
            HireDate = employeeData.GetValue<DateTime>("HireDate");
            Address = employeeData.GetValue<string>("Address");
            City = employeeData.GetValue<string>("City");
            State = employeeData.GetValue<string>("State");
            Country = employeeData.GetValue<string>("Country");
            PostalCode = employeeData.GetValue<string>("PostalCode");
            Phone = employeeData.GetValue<string>("Phone");
            Fax = employeeData.GetValue<string>("Fax");
        }

        #endregion
    }
}
