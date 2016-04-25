using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM
{
    class Utilities
    {
        public string CheckNewCustomer(string firstName, string surname, string socialSecurityNbr, string employeeId)
        {
            string message = null;
            if (socialSecurityNbr.Length == 10)
            {
                try
                {
                    double ssn = double.Parse(socialSecurityNbr);
                }
                catch
                {
                    FormatException e;
                    message = "Incorrect input: Social security number. ";
                }
            }

            if (firstName.Length < 2)
            {
                message = "Incorrect input: First name. ";
            }

            if (surname.Length < 2)
            {
                message = "Incorrect input: surname. ";
            }
            if (employeeId.Length > 0)
            {
                try
                {
                    int empId = Int32.Parse(employeeId);
                }
                catch
                {
                    FormatException e;
                    message = "Incorrect input: Sales agent ID. ";
                }
            }

            return message;
        }
    }
}
