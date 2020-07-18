using System;

namespace TheRiceMill.Common.Util
{
    public class DateConverter
    {
        public string ConvertToDateTimeIso(DateTime dateTime)
        {
            return dateTime.ToString("O");
        }
        
        public string ConvertToDateTime(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("G");
            }

            return "";
        }

        public string ConvertToDate(DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
        public string ConvertToDateTime(DateTime dateTime)
        {
            return dateTime.ToString("g");
        }
    }
}