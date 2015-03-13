namespace Nepy.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using DateTimeExtensions;
    using Nepy.Core;

    [ParserDefaultOrder(40)]
    public class EnglishDateTimeParser:IParser
    {
        #region  Fields

        private static readonly List<string> allDayNames = new List<string>();
        private static readonly List<string> allMonthNames = new List<string>();
        private static readonly Regex inferredDaysRegex = new Regex(@"^(\d+) of");
        private static readonly string[] pastIndicators = new[] {"ago", "before"};
        private static readonly Dictionary<string, int> wordsToNumbers = new Dictionary<string, int>();
        private static Regex nextLastUnit;
        private static Regex thisWithUnits;
        private static Regex unitsFromMatch;

        #endregion

        #region  Constructor

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static EnglishDateTimeParser()
        {
            CreateNameLists();
            CreateRegexes();
            CreateWordToNumberList();
        }

        private static void CreateWordToNumberList()
        {
            // PLEASE, if you know of one, replace this with a proper library!
            wordsToNumbers.Add("one", 1);
            wordsToNumbers.Add("two", 2);
            wordsToNumbers.Add("second", 2);
            wordsToNumbers.Add("three", 3);
            wordsToNumbers.Add("third", 3);
            wordsToNumbers.Add("fourth", 4);
            wordsToNumbers.Add("four", 4);
            wordsToNumbers.Add("five", 5);
            wordsToNumbers.Add("fifth", 4);
        }

        private static void CreateRegexes()
        {
            var unitList = new List<string>(new[] {"week", "day", "month"});
            unitList.AddRange(allDayNames);
            unitList.AddRange(allMonthNames);
            string units = String.Join("|", unitList.ToArray());
            unitsFromMatch = new Regex(
                string.Format(CultureInfo.CurrentCulture, @"(\d+) ({0})(?:s|) (\w+) ", units),
                RegexOptions.IgnoreCase);
            nextLastUnit = new Regex(string.Format(CultureInfo.CurrentCulture, @"^(next|last) ({0})", units),
                                     RegexOptions.IgnoreCase);
            thisWithUnits = new Regex(string.Format(CultureInfo.CurrentCulture, @"this ({0})", units),
                                      RegexOptions.IgnoreCase);
        }

        private static void CreateNameLists()
        {
            allDayNames.AddRange(dateTimeFormat.DayNames);
            allDayNames.AddRange(dateTimeFormat.AbbreviatedDayNames);
            allDayNames.AddRange(dateTimeFormat.ShortestDayNames);
            allMonthNames.AddRange(dateTimeFormat.MonthNames);
            allMonthNames.AddRange(dateTimeFormat.AbbreviatedMonthNames);
            allMonthNames.RemoveAll(String.IsNullOrEmpty);
        }

        #endregion

        private static DateTimeFormatInfo dateTimeFormat
        {
            get { return CultureInfo.GetCultureInfo("en-us").DateTimeFormat; }
        }


        internal DateTime ParseRelative(DateTime baseTime, string dateToParse)
        {
            DateTime dateBase = baseTime.Midnight();
            dateToParse = PreProcessString(dateToParse, dateBase);

            return ParseString(dateToParse, dateBase);
        }

        #region PreProcessing

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private static string PreProcessString(string dateString, DateTime dateBase)
        {
            dateString = dateString.ToLowerInvariant();
            dateString = dateString.Replace("first ", "");
            dateString = StripOrdinalCharacters(dateString);
            dateString = ConvertWordsToNumbers(dateString);
            dateString = ConvertPrecedingIn(dateString);
            dateString = ClarifyTrailingNext(dateString);
            dateString = ClarifyTrailingAgo(dateString);
            dateString = StripPrecedingThe(dateString);
            dateString = AddImplicitDays(dateString);
            dateString = StripOrConvertThis(dateString, dateBase);
            return dateString;
        }

        /// <summary>
        /// 15th of june => 15 of june
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string StripOrdinalCharacters(string dateString)
        {
            var stripOrdinalRegex = new Regex(@"(\s|^)(\d+)(?:st|nd|rd|th)(\s|$)");
            return stripOrdinalRegex.Replace(dateString, "$1$2$3");
        }

        /// <summary>
        /// third week in jan => 3 week in jan
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string ConvertWordsToNumbers(string dateString)
        {
            foreach (var pair in wordsToNumbers)
            {
                dateString = dateString.Replace(pair.Key, pair.Value.ToString(CultureInfo.CurrentCulture));
            }
            return dateString;
        }

        /// <summary>
        /// in two weeks => two weeks from now
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string ConvertPrecedingIn(string dateString)
        {
            if (dateString.StartsWith("in ", StringComparison.CurrentCulture))
            {
                dateString += " from now";
            }
            return dateString;
        }

        /// <summary>
        /// week after next => week after next week
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string ClarifyTrailingNext(string dateString)
        {
            if (dateString.EndsWith("next", StringComparison.CurrentCulture))
            {
                dateString += " " + dateString.GetWord(1);
            }
            return dateString;
        }

        /// <summary>
        /// 1 month ago => 1 month ago today
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string ClarifyTrailingAgo(string dateString)
        {
            if (dateString.EndsWith("ago", StringComparison.CurrentCulture))
            {
                dateString += " today";
            }
            return dateString;
        }

        /// <summary>
        /// the 2 monday of april => 2 monday of april
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string StripPrecedingThe(string dateString)
        {
            return Regex.Replace(dateString, @"^the ", "");
        }

        /// <summary>
        /// 25 of june => 25 days of june
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static string AddImplicitDays(string dateString)
        {
            return inferredDaysRegex.Replace(dateString, "$1 days of");
        }

        /// <summary>
        /// this monday => 7/12/2008
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private static string StripOrConvertThis(string dateString, DateTime dateBase)
        {
            Match match = thisWithUnits.Match(dateString);
            if (match.Success)
            {
                string unit = match.Groups[1].Value;
                switch (unit)
                {
                    case "month":
                        return thisWithUnits.Replace(dateString, dateBase.ToString("MMMM", dateTimeFormat));
                    case "week":
                        return thisWithUnits.Replace(dateString, dateBase.This(DateUnit.Week).ToString());
                }
                if (allDayNames.Contains(unit, StringComparer.CurrentCultureIgnoreCase))
                {
                    return thisWithUnits.Replace(dateString,
                                                 dateBase.Last(DateUnit.Week).Next(StringToDayOfWeek(unit)).ToString());
                }
                if (allMonthNames.Contains(unit, StringComparer.CurrentCultureIgnoreCase))
                {
                    DateTime parsedResult=DateTime.ParseExact(unit,
                                                                     new[] {"MMMM", "MMM"},
                                                                     dateTimeFormat,
                                                                     DateTimeStyles.NoCurrentDateDefault);
                    DateTime result= new DateTime(dateBase.Year,parsedResult.Month, parsedResult.Day);
                    return thisWithUnits.Replace(dateString,result.ToString());
                }
            }
            return dateString.Replace("this", "");
        }

        #endregion

        #region Parsing

        private DateTime ParseString(string dateString, DateTime dateBase)
        {
            return 
                   CheckNextLastIndicator(dateString, dateBase) ??
                   CheckUnitsFromMatch(dateString, dateBase) ??
                   CheckEntireStringIsDay(dateString, dateBase) ??
                   CheckFirstWordIsDay(dateString, dateBase) ??
                   CheckEntireStringIsMonth(dateString, dateBase) ??
                   CheckFirstWordIsMonth(dateString, dateBase) ??
                   CheckSpecificDayNames(dateString, dateBase) ??
                   CheckDayNumeric(dateString, dateBase) ??
                   CheckForActualDateTime(dateString, dateBase) ??
                   dateBase;
        }

        /// <summary>
        /// 4/15/2008
        /// 4/15
        /// 03/05/08
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private DateTime? CheckForActualDateTime(string dateString, DateTime dateBase)
        {
            DateTime directParse;
            if (DateTime.TryParse(dateString, out directParse))
            {
                return new DateTime( dateBase.Year,directParse.Month, directParse.Day, directParse.Hour, directParse.Minute, directParse.Second);
            }
            return null;
        }

        /// <summary>
        /// next week
        /// last month
        /// next june
        /// last monday
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckNextLastIndicator(string dateString, DateTime dateBase)
        {
            Match match = nextLastUnit.Match(dateString);
            if (match.Success)
            {
                bool isNext = match.Groups[1].Value.Equals("next");
                string unit = match.Groups[2].Value;
                if (StringIsDayOfTheWeek(unit))
                {
                    return ParseRelative(dateBase, dateString.Substring(5)).AddWeeks(isNext ? 1 : -1);
                }
                if (unit.Equals("month"))
                {
                    return isNext ? dateBase.Next(DateUnit.Month) : dateBase.Last(DateUnit.Month);
                }
                if (allMonthNames.Contains(unit, StringComparer.CurrentCultureIgnoreCase))
                {
                    return ParseRelative(dateBase, unit).AddYears(1);
                }
            }
            return null;
        }

        /// <summary>
        /// ## (days,weeks,months, mondays, etc) ago/after/etc.
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckUnitsFromMatch(string dateString, DateTime dateBase)
        {
            Match unitsMatch = unitsFromMatch.Match(dateString);
            if (unitsMatch.Success)
            {
                int multiplier = Int32.Parse(unitsMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                dateBase = ParseRelative(dateBase, unitsFromMatch.Replace(dateString, "", 1));
                return
                    dateBase.AddDays(
                        DaysFromUnits(unitsMatch.Groups[2].Value, multiplier, unitsMatch.Groups[3].Value, dateBase));
            }
            return null;
        }

        /// <summary>
        /// friday
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckEntireStringIsDay(string dateString, DateTime dateBase)
        {
            if (StringIsDayOfTheWeek(dateString))
            {
                return dateBase.Next(StringToDayOfWeek(dateString));
            }
            return null;
        }

        /// <summary>
        /// friday after next friday
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckFirstWordIsDay(string dateString, DateTime dateBase)
        {
            if (StringIsDayOfTheWeek(dateString.GetWord(1)))
            {
                return
                    ParseRelative(dateBase, dateString.GetStringAfterWord(2)).Next(
                        StringToDayOfWeek(dateString.GetWord(1)));
            }
            return null;
        }

        /// <summary>
        /// february
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckEntireStringIsMonth(string dateString, DateTime dateBase)
        {
            DateTime monthDate;
            if (DateTime.TryParseExact(dateString,
                                       new[] {"MMMM", "MMM"},
                                       dateTimeFormat,
                                       DateTimeStyles.AllowWhiteSpaces,
                                       out monthDate))
            {
                return new DateTime(dateBase.Year + (monthDate.Month < dateBase.Month ? 1 : 0), monthDate.Month, 1);
            }
            return null;
        }

        /// <summary>
        /// june 20
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckFirstWordIsMonth(string dateString, DateTime dateBase)
        {
            if (allMonthNames.Contains(dateString.GetWord(1), StringComparer.CurrentCultureIgnoreCase))
            {
                return ParseRelative(CheckEntireStringIsMonth(dateString.GetWord(1), dateBase).Value,
                                     dateString.Substring(dateString.GetWord(1).Length + 1));
            }
            return null;
        }

        private DateTime? CheckSpecificDayNames(string dateString, DateTime dateBase)
        {
            switch (dateString)
            {
                case "tomorrow":
                    return dateBase.AddDays(1);
                case "yesterday":
                    return dateBase.AddDays(-1);
                case "today":
                    return dateBase;
                case "day after tomorrow":
                    return dateBase.AddDays(2);
            }
            return null;
        }

        /// <summary>
        /// 20 (in the base month)
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateBase"></param>
        /// <returns></returns>
        private DateTime? CheckDayNumeric(string dateString, DateTime dateBase)
        {
            int date;
            if (Int32.TryParse(dateString, out date))
            {
                return new DateTime(dateBase.Year, dateBase.Month + (date > dateBase.Day ? 0 : 1), date);
            }
            return null;
        }

        #endregion

        #region  Utility

        private static bool StringIsDayOfTheWeek(string dateString)
        {
            return allDayNames.Contains(dateString, StringComparer.CurrentCultureIgnoreCase);
        }

        private static DayOfWeek StringToDayOfWeek(IEquatable<string> dateString)
        {
            return GetDayOfWeek(dateString, allDayNames) ??
                   DateTime.Now.DayOfWeek;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private static DayOfWeek? GetDayOfWeek(IEquatable<string> dateString, IList<string> dayNames)
        {
            for (int i = 0; i < dayNames.Count; i++)
            {
                if (dateString.Equals(dayNames[i].ToLowerInvariant()))
                {
                    return (DayOfWeek) (i % 7);
                }
            }
            return null;
        }

        private static double DaysFromUnits(string unit, int multiplier, string direction, DateTime timeBase)
        {
            int directionMultipler = pastIndicators.Contains(direction) ? -1 : 1;
            if (unit.Equals("month"))
            {
                return GetDaysFromMonth(timeBase, directionMultipler == 1, multiplier);
            }
            if (StringIsDayOfTheWeek(unit))
            {
                return GetDaysOfWeekFromNow(timeBase, directionMultipler, multiplier, unit);
            }
            int dayUnits = unit == "day" ? 1 : 7;
            return dayUnits * multiplier * directionMultipler - (direction == "of" ? 1 : 0);
        }

        private static double GetDaysOfWeekFromNow(DateTime timeBase,
                                                   int directionMultiplier,
                                                   int multiplier,
                                                   string unit)
        {
            DateTime timeResult = timeBase;
            if (directionMultiplier == 1)
            {
                multiplier--;
            }
            timeResult = timeResult.Next(StringToDayOfWeek(unit)).AddWeeks(multiplier * directionMultiplier);
            return (timeResult - timeBase).TotalDays;
        }

        private static double GetDaysFromMonth(DateTime timeBase, bool future, int multiplier)
        {
            DateTime timeResult = timeBase;
            for (int i = 1; i <= multiplier; i++)
            {
                timeResult = future ? timeResult.Next(DateUnit.Month) : timeResult.Last(DateUnit.Month);
            }
            timeResult = timeResult.AddDays(timeBase.Day - 1);
            return (timeResult - timeBase).TotalDays;
        }

        #endregion

        ParserContext context;
        public EnglishDateTimeParser(ParserContext context)
        {
            this.context = context;
        }
        public ParserContext Context
        {
            get { return this.context; }
        }

        public ParseResultCollection Parse(int startIndex)
        {
            throw new NotImplementedException();
        }
    }
}