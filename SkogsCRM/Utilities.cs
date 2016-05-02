using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SkogsCRM
{
    public static class Utilities
    {
        static string socialSecurityNbrPattern = "^[0-9]{10}$";
        static string employeeIdPattern = "^[0-9]{1,5}$";
        static string namePattern = "^[a-zA-ZåäöÅÄÖ]{2,30}$";
        static string telephonePattern = "^[0-9]{10}$";
        static string coordinatesPattern = "^[0-9.,]{100,255}$";

        static Regex socialSecurityNbrRegex = new Regex(socialSecurityNbrPattern);
        static Regex employeeIdRegex = new Regex(employeeIdPattern);
        static Regex nameRegex = new Regex(namePattern);
        static Regex telephoneRegex = new Regex(telephonePattern);
        static Regex coordinatesRegex = new Regex(coordinatesPattern);

        static string coordinates = null;

        public static string Coordinates
        {
            get
            {
                return coordinates;
            }

            set
            {
                coordinates = value;
            }
        }

        public static string CheckCustomerFieldsFormatting(string socialSecurityNbr, string firstName, string surname, string employeeId)
        {
            string message = null;
            Match socialSecurityNbrMatch = socialSecurityNbrRegex.Match(socialSecurityNbr);
            Match firstNameMatch = nameRegex.Match(firstName);
            Match surnameMatch = nameRegex.Match(surname);
            Match employeeIdMatch = employeeIdRegex.Match(employeeId);

            if (!socialSecurityNbrMatch.Success){
                message = "Incorrect SSN format.";
            }
            if (!firstNameMatch.Success)
            {
                message = "Incorrect first name format.";
            }
            if (!surnameMatch.Success)
            {
                message = "Incorrect surname format.";
            }
            if (!employeeIdMatch.Success)
            {
                message = "Incorrect sales agent ID format.";
            }

            return message;
        }

        public static string CheckMySqlException(DbUpdateException e)
        {
            string message = null;
            MySql.Data.MySqlClient.MySqlException sqlError = e.InnerException.InnerException as MySql.Data.MySqlClient.MySqlException;
            if (sqlError != null)
            {
                switch (sqlError.Number)
                {
                    //PK duplicate violation
                    case 1062:
                        message = "Primary Key violation. Duplicate entries are not allowed.";
                        break;
                    //PK Not Null violation
                    case 1171:
                        message = "Primary Key cannot be null.";
                        break;
                    //FK violation
                    case 1216:
                        message = "Cannot add or update a child row: a foreign key constraint fails.";
                        break;
                    default:
                        message = "Something went terribly wrong. MySQL Error code: " + sqlError.Number.ToString();
                        break;
                }
            }
            else
            {
                message = "An unkown database error has occurred.";
            }
            return message;
        }

        public static string CheckSalesAgentFieldsFormatting(string firstName, string surname, string employeeId, string telephoneNbr)
        {
            string message = null;
            Match firstNameMatch = nameRegex.Match(firstName);
            Match surnameMatch = nameRegex.Match(surname);
            Match employeeIdMatch = employeeIdRegex.Match(employeeId);
            Match telephoneNbrMatch = telephoneRegex.Match(telephoneNbr);

            if (!firstNameMatch.Success)
            {
                message = "Incorrect first name format.";
            }
            if (!surnameMatch.Success)
            {
                message = "Incorrect surname format.";
            }
            if (!employeeIdMatch.Success)
            {
                message = "Incorrect sales agent ID format.";
            }
            if (!telephoneNbrMatch.Success)
            {
                message = "Incorrect telephone number format.";
            }

            return message;
        }
        public static string CheckForestEstateFieldsFormatting(string coordinates, string socialSecurityNbr)
        {
            string message = null;

            if (coordinates == null)
            {
                message = "Please draw an area on the map.";
            }
            else
            {
                Match coordinatesMatch = coordinatesRegex.Match(coordinates);
                if (!coordinatesMatch.Success)
                {
                    message = "The shape drawn is either incomplete or contains too many points.";
                }
            }

            if (socialSecurityNbr == "")
            {
                message = "Please choose a customer from the table above.";
            }
            else
            {
                Match socialSecurityNbrMatch = socialSecurityNbrRegex.Match(socialSecurityNbr);
                if (!socialSecurityNbrMatch.Success)
                {
                    message = "Incorrect SSN format.";
                }
            }

            return message;
        }
    }
}
