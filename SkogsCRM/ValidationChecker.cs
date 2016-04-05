using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM
{
    class ValidationChecker
    {
        public bool CheckNewCustomer (string socNbr, string firstName, string surname, int employeeId)
        {
            bool success;
            if (socNbr.Length == 10 && firstName.Length > 0 && surname.Length > 0 && employeeId > 0)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }
        public bool CheckNewSalesAgent (string firstName, string surname, int employeeId)
        {
            bool success;
            if (firstName.Length > 0 && surname.Length > 0 && employeeId > 0)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }
        public bool CheckNewForestEstate (string coordinates, string socNbr)
        {
            bool success;
            if (coordinates.Length > 20 && socNbr.Length == 10)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }


    }
}
