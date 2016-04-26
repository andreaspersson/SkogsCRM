using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkogsCRM
{
    class Utilities
    {
        static string socialSecurityNbrPattern = "^[0-9]{10}$";
        static string employeeIdPattern = "^[0-9]{1,5}$";
        static string namePattern = "^[a-zA-ZåäöÅÄÖ]{2,30}$";

        Regex socialSecurityNbrRegex = new Regex(socialSecurityNbrPattern);
        Regex employeeIdRegex = new Regex(employeeIdPattern);
        Regex nameRegex = new Regex(namePattern);
        public string CheckCustomerFieldsFormatting(string firstName, string surname, string socialSecurityNbr, string employeeId)
        {
            string message = null;
            Match socialSecurityNbrMatch = socialSecurityNbrRegex.Match(socialSecurityNbr);
            Match firstNameMatch = nameRegex.Match(firstName);
            Match surnameMatch = nameRegex.Match(surname);
            Match employeeIdMatch = employeeIdRegex.Match(employeeId);

            if (!socialSecurityNbrMatch.Success){
                message = "Incorrect SSN format";
            }
            if (!firstNameMatch.Success)
            {
                message = "Incorrect first name format";
            }
            if (!surnameMatch.Success)
            {
                message = "Incorrect surname format";
            }
            if (!employeeIdMatch.Success)
            {
                message = "Incorrect sales agent ID format";
            }

            return message;
        }
    }
}
