using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListwareDesktop.Framework
{
    //This is a static class to help auto detect inputs. Add any more variations here.
    internal static class AutoDetectInputs
    {
        static string[] firstNameVariations = { "namefirst", "first", "firstname", "fname", "first name", "f name", "first_name", "fn", "firstnm" };

        static string[] lastNameVariations = { "namelast", "last", "lastname", "lname", "last name", "l name", "last_name", "ln", "lastnm" };

        static string[] fullNameVariations = { "namefull", "fullname", "name", "full name", "contact", "name1", "name 1" };

        static string[] addressVariations = { "a1", "addressline1", "address", "street", "addressline1", "address line1", "address 1", "address1", "street address", "address_1", "delivery address", "str1", "add1", "addr1", "business address", "add 1", "address line 1" };

        static string[] address2Variations = { "a2", "addressline2", "address2", "addressline2", "address line2", "street2", "street 2", "address 2", "address_2", "str2", "add2", "add 2", "addr2", "address line 2" };

        static string[] cityVariations = { "city", "town" };

        static string[] stateVariations = { "state", "st", "province", "prov" };

        static string[] zipVariations = { "postal", "zip", "zipcode", "postalcode", "postal_code", "postal code", "post_code", "postcode", "post code" };

        static string[] companyVariations = { "company", "business", "organization", "organization name" };

        static string[] suiteVariations = { "suite" };

        static string[] plus4Variations = { "+4", "plus4", "plus 4" };

        static string[] recordIDVariations = { "recordid", "recnum", "recno", "recid", "id", "rec" };

        static string[] phoneVariations = { "phone", "phonenumber", "fone", "fonenumber", "phonenum", "fonenum" };

        static string[] emailVariations = { "email", "emailaddress", "emailaddr", "e-mail" };

        //This relates input column names to the variation lists
        //Add input columns from services here in all lower case as the key, then the variation list as the value
        internal static Dictionary<string, string[]> variationDictionary = new Dictionary<string, string[]>()
        {
            {"namefirst", firstNameVariations},
            {"namelast", lastNameVariations},
            {"namefull", fullNameVariations},
            {"addressline1", addressVariations},
            {"addressline2", address2Variations},
            {"city", cityVariations},
            {"state", stateVariations},
            {"postalcode", zipVariations},
            {"company", companyVariations},
            {"suite", suiteVariations},
            {"plus4", plus4Variations},
            {"firstname", firstNameVariations},
            {"lastname", lastNameVariations},
            {"fullname", fullNameVariations},
            {"companyname", companyVariations},
            {"recordid", recordIDVariations},
            {"emailaddress", emailVariations},
            {"phonenumber", phoneVariations},
            {"a1", addressVariations},
            {"a2", address2Variations},
            {"postal", zipVariations}
        };
    }
}
