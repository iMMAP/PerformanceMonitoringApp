
namespace SRFROWCA.Common
{
    public class MonthNumber
    {
        internal static int GetMonthNumber(string monthName)
        {
            int monthNumber = 0;
            if (monthName == "January" || monthName == "Janvier")
            {
                monthNumber =  1;
            }
            else if (monthName == "Février" || monthName == "February")
            {
                monthNumber = 2;
            }
            else if (monthName == "Mars" || monthName == "March")
            {
                monthNumber = 3;
            }
            else if (monthName == "Avril" || monthName == "April")
            {
                monthNumber = 4;
            }
            else if (monthName == "Mai" || monthName == "May")
            {
                monthNumber = 5;
            }
            else if (monthName == "Juin" || monthName == "June")
            {
                monthNumber = 6;
            }
            else if (monthName == "Juillet" || monthName == "July")
            {
                monthNumber = 7;
            }
            else if (monthName == "Août" || monthName == "August")
            {
                monthNumber = 8;
            }
            else if (monthName == "Septembre" || monthName == "September")
            {
                monthNumber = 9;
            }
            else if (monthName == "Octobre" || monthName == "October")
            {
                monthNumber = 10;
            }
            else if (monthName == "novembre" || monthName == "November")
            {
                monthNumber = 11;
            }
            else if (monthName == "Décembre" || monthName == "December")
            {
                monthNumber = 12;
            }

            return monthNumber;
        }
    }
}