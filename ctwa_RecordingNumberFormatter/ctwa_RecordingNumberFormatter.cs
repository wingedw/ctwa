
using System;

namespace ctwa_Helper
{
    public class ctwa_RecordingNumberFormatter
    {
        public enum Operation
        {
            King,
            Pierce
        }

        private enum DateInterval
        {
            second,
            minute,
            hour,
            day,
            month,
            year
        }
        public string FormatRecordingNumber(string instrumentnumber,  DateTime recorddate, Operation operation)
        {
            string _return = string.Empty;
            switch (operation)
            {
                case Operation.King:
                    {
                        _return = King(instrumentnumber,  recorddate);
                        break;
                    }

                case Operation.Pierce:
                    {
                        _return = Pierce(instrumentnumber, recorddate);
                        break;
                    }
            }

            return _return;
        }
        private string King(string instrumentnumber, DateTime recorddate)
        {

            string recordingnumber =string.Empty;

            // Documents dated June 14th, 1999 to present
            // 14 digit Document Number
            // YYYYMMDD00#### 
            // Four digit year, two digit month, two digit day, two zeros, 4 digit id number- if the ID number is only 1-3 digits fill with leading zeros to make it a four digit number.
            // Example()
            // 06/18/2009 167  converts to 20090618000167
            // 01/06/2009 1237 converts to 20090106001237
            var _ddatecompare1 = DateTime.Parse("1999-06-14");
            if (dateDiff(DateInterval.day, _ddatecompare1, recorddate) >= 0L)
            {
                while (instrumentnumber.Length < 4)
                    instrumentnumber = instrumentnumber.Insert(0, "0");
                if (instrumentnumber.StartsWith("9") && instrumentnumber.Length == 4 | instrumentnumber.Length == 6)
                {
                    recordingnumber = recorddate.ToString("yyyyMMdd") + instrumentnumber;
                }
                else
                {
                    recordingnumber = recorddate.ToString("yyyyMMdd") + "00" + instrumentnumber;
                }
            }

            // Documents dated Jan. 4th 1971 to June 13th 1999
            // 10 Digit Document Number
            // YYMMDD####
            // 2 digit year, two digit month, two digit day, 4 digit id number- if the ID number is only 1-3 digits fill with leading zeros to make it a four digit number.
            // Example()
            // 01/04/1974 52 converts to 7401040052
            // 12/31/1989 2006 converts to 8912312006
            _ddatecompare1 = DateTime.Parse("1971-01-04");
            var dDateCompare2 = DateTime.Parse("1999-06-13");
            if (dateDiff(DateInterval.day, _ddatecompare1, recorddate) >= 0L & dateDiff(DateInterval.day, recorddate, dDateCompare2) >= 0L)
            {
                while (instrumentnumber.Length < 4)
                    instrumentnumber = instrumentnumber.Insert(0, "0");
                recordingnumber = recorddate.ToString("yyMMdd") + instrumentnumber;
            }

            // Documents recorded 1/1/1969 to Jan. 3rd 1971
            // 7:                      Digit(Numbers)
            // The date is not used in these recording numbers, the ID number is the whole number.
            // example()
            // 6/13/1969 6655729 converts to 6655729
            _ddatecompare1 = DateTime.Parse("1969-01-01");
            dDateCompare2 = DateTime.Parse("1971-01-03");
            if (dateDiff(DateInterval.day, _ddatecompare1, recorddate) >= 0L & dateDiff(DateInterval.day, recorddate, dDateCompare2) >= 0L)
            {
                recordingnumber = instrumentnumber;
            }

            // Documents recorded from Statehood to 1/1/1969 
            // These documents do not show up in Titlepoint and won't be in the original text file.  
            // The examiner will have to manually add these numbers to request them from the Doc Handler.
            // 1 to 7 Digit number without the date
            // 558892 converts to 558892
            // 1125697 converts to 1125697

            return recordingnumber;
        }

        private string Pierce(string instrumentnumber,  DateTime recorddate)
        {
            string recordingnumber = string.Empty;

            // Documents recorded after Jan 1 2000
            var dDateCompare1 = DateTime.Parse("2000-01-01");
            if (dateDiff(DateInterval.day, dDateCompare1, recorddate) >= 0L)
            {
                while (instrumentnumber.Length < 4)
                    instrumentnumber = instrumentnumber.Insert(0, "0");
                recordingnumber = recorddate.ToString("yyyyMMdd") + instrumentnumber;
            }

            // Documents between Aug 1 1980 and Dec 31 1999
            dDateCompare1 = DateTime.Parse("1980-08-01");
            var dDateCompare2 = DateTime.Parse("1999-12-31");
            if (dateDiff(DateInterval.day, dDateCompare1, recorddate) >= 0L & dateDiff(DateInterval.day, recorddate, dDateCompare2) >= 0L)
            {
                while (instrumentnumber.Length < 4)
                    instrumentnumber = instrumentnumber.Insert(0, "0");
                recordingnumber = recorddate.ToString("yyMMdd") + instrumentnumber;
            }

            // Documents recorded from Statehood to 8/1/1980 
            dDateCompare1 = DateTime.Parse("1980-08-01");
            if (dateDiff(DateInterval.day, recorddate, dDateCompare1) >= 0L)
            {
                recordingnumber = instrumentnumber;
            }

            return recordingnumber;
        }

        /// <summary>
        /// Return the instrument number from a given string that could be the instrument number, recording number, 
        /// if an instrument number can not be discerned from the given data and empty string is returned.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetInstrumentNumber(string data)
        {
            string _return = string.Empty;
            int _charcount;
            _charcount = data.TrimStart('0').Length;

            if (int.TryParse(data.TrimStart('0'), out _))
            {
                switch (_charcount)
                {
                    case var case0 when 1 <= case0 && case0 <= 4:
                        {
                            _return = data.TrimStart('0');
                            break;
                        }

                    case var case1 when 5 <= case1 && case1 <= 9:
                        {
                            _return = string.Empty;
                            break;
                        }

                    case var case2 when 10 <= case2 && case2 <= 12:
                        {
                            _return = data.Substring(data.Length - 4).TrimStart('0');
                            break;
                        }

                    default:
                        {
                            _return = string.Empty;
                            break;
                        }
                }
            }

            return _return;
        }

        private int dateDiff(DateInterval interval, DateTime beginDate, DateTime endDate)
        {
            int _return = 0;
            TimeSpan ts = endDate - beginDate;

            switch (interval)
            {
                case DateInterval.second:
                    _return = Fix(ts.TotalSeconds);
                    break;
                case DateInterval.minute:
                    _return = Fix(ts.TotalMinutes);
                    break;
                case DateInterval.day:
                    _return = Fix(ts.TotalDays);
                    break;
                case DateInterval.month:
                    _return = (endDate.Month - beginDate.Month) + (12 * (endDate.Year - beginDate.Year));
                    break;
                case DateInterval.year:
                    _return = endDate.Year - beginDate.Year;
                    break;
            }

            return _return;
        }
        private static int Fix(double Number)
        {
            if (Number >= 0)
            {
                return (int)Math.Floor(Number);
            }
            return (int)Math.Ceiling(Number);
        }
    }
}
