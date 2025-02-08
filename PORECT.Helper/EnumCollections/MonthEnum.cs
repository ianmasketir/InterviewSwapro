using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORECT.Helper
{
    public enum MonthEnum
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public static class MonthEnum_Extensions
    {
        public static MonthEnum ToMonthEnum(this int value)
        {
            switch (value)
            {
                case 1:
                    return MonthEnum.January;
                case 2:
                    return MonthEnum.February;
                case 3:
                    return MonthEnum.March;
                case 4:
                    return MonthEnum.April;
                case 5:
                    return MonthEnum.May;
                case 6:
                    return MonthEnum.June;
                case 7:
                    return MonthEnum.July;
                case 8:
                    return MonthEnum.August;
                case 9:
                    return MonthEnum.September;
                case 10:
                    return MonthEnum.October;
                case 11:
                    return MonthEnum.November;
                case 12:
                    return MonthEnum.December;
                default:
                    throw new Exception("Month enum not available");
            }
        }
        public static MonthEnum FromIDNMonth(this string value)
        {
            switch (value.ToLower())
            {
                case "januari":
                    return MonthEnum.January;
                case "februari":
                    return MonthEnum.February;
                case "maret":
                    return MonthEnum.March;
                case "april":
                    return MonthEnum.April;
                case "mei":
                    return MonthEnum.May;
                case "juni":
                    return MonthEnum.June;
                case "juli":
                    return MonthEnum.July;
                case "agustus":
                    return MonthEnum.August;
                case "september":
                    return MonthEnum.September;
                case "oktober":
                    return MonthEnum.October;
                case "november":
                    return MonthEnum.November;
                case "desember":
                    return MonthEnum.December;
                default:
                    throw new Exception("Month enum not available");
            }
        }
        public static string ToIDNMonth(this string value)
        {
            switch (value.ToLower())
            {
                case "january":
                    return "Januari";
                case "february":
                    return "Februari";
                case "march":
                    return "Maret";
                case "april":
                    return "April";
                case "may":
                    return "Mei";
                case "june":
                    return "Juni";
                case "july":
                    return "Juli";
                case "august":
                    return "Agustus";
                case "september":
                    return "September";
                case "october":
                    return "Oktober";
                case "november":
                    return "November";
                case "december":
                    return "Desember";
                default:
                    throw new Exception("Indonesian Month not available");
            }
        }
    }
}
