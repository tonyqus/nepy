namespace Nepy.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DateTimeExtensions;
    using NUnit.Framework;
    using Nepy.Parsers;
    using System.Globalization;
    using Nepy.Core;

    [TestFixture]
    public class DateTimeEnglishParserTests
    {
        private readonly DateTime timeBase = new DateTime(2008, 2, 13);

        [Test]
        public void Tomorrow()
        {
            this.VerifyParse("tomorrow", this.timeBase.AddDays(1));
        }

        [Test]
        public void Yesterday()
        {
            this.VerifyParse("yesterday", this.timeBase.AddDays(-1));
        }

        [Test]
        public void ThisFriday()
        {
            this.VerifyParse("friday", this.timeBase.Next(DayOfWeek.Friday));
        }

        [Test]
        public void SameDayOfWeek()
        {
            this.VerifyParse(this.timeBase.DayOfWeek.ToString(), this.timeBase.Next(this.timeBase.DayOfWeek));
        }

        [Test]
        public void Today()
        {
            this.VerifyParse("today", this.timeBase);
        }

        [Test]
        public void DayAfterTomorrow()
        {
            this.VerifyParse("day after tomorrow", this.timeBase.AddDays(2));
        }

        [Test]
        public void TheDayAfterTomorrow()
        {
            this.VerifyParse("the day after tomorrow", this.timeBase.AddDays(2));
        }

        [Test]
        public void NextSunday()
        {
            this.VerifyParse("next sunday", this.timeBase.Next(DayOfWeek.Sunday).Next(DayOfWeek.Sunday));
        }

        [Test]
        public void NextMonday()
        {
            this.VerifyParse("Next Monday", this.timeBase.Next(DayOfWeek.Monday).Next(DayOfWeek.Monday));
        }

        [Test]
        public void TwoWeeksFromTuesday()
        {
            this.VerifyParse("Two weeks from Tuesday", this.timeBase.Next(DayOfWeek.Tuesday).AddDays(14));
        }

        [Test]
        public void ThreeWeeksFromTuesday()
        {
            this.VerifyParse("Three weeks from Tuesday", this.timeBase.AddDays(21).Next(DayOfWeek.Tuesday));
        }

        [Test]
        public void TwoWeeksFromToday()
        {
            this.VerifyParse("two weeks from now", this.timeBase.AddDays(14));
        }

        [Test]
        public void InTwoWeeks()
        {
            this.VerifyParse("in two weeks", this.timeBase.AddDays(14));
        }

        [Test]
        public void OneWeekAfterNow()
        {
            this.VerifyParse("one week after now", this.timeBase.AddDays(7));
        }

        [Test]
        public void OneWeekFromNow()
        {
            this.VerifyParse("one week from now", this.timeBase.AddDays(7));
        }

        [Test]
        public void TwoDaysFromNow()
        {
            this.VerifyParse("in two days", this.timeBase.AddDays(2));
        }

        [Test]
        public void TwoDaysFromTuesday()
        {
            this.VerifyParse("two days from tuesday", this.timeBase.Next(DayOfWeek.Tuesday).AddDays(2));
        }

        [Test]
        public void In20Days()
        {
            this.VerifyParse("in 20 days", this.timeBase.AddDays(20));
        }

        [Test]
        public void InTwoWeeksAndTwoDays()
        {
            this.VerifyParse("in 2 weeks and 2 days", this.timeBase.AddDays(16));
        }

        [Test]
        public void FridayAfterNext()
        {
            this.VerifyParse("friday after next", this.timeBase.Next(DayOfWeek.Friday).AddDays(14));
        }

        [Test]
        public void LastFriday()
        {
            this.VerifyParse("last friday", this.timeBase.Next(DayOfWeek.Friday).AddDays(-7));
        }

        [Test]
        public void OneWeekAgo()
        {
            this.VerifyParse("one week ago", this.timeBase.AddDays(-7));
        }

        [Test]
        public void OneWeekAgoFriday()
        {
            this.VerifyParse("one week ago Friday", this.timeBase.Next(DayOfWeek.Friday).AddDays(-7));
        }

        [Test]
        public void TwoWeekBeforeMonday()
        {
            this.VerifyParse("two weeks before Monday", this.timeBase.Next(DayOfWeek.Monday).AddDays(-14));
        }

        [Test]
        public void TwoWeeksBeforeLastMonday()
        {
            this.VerifyParse("two weeks before last Monday", this.timeBase.Next(DayOfWeek.Monday).AddDays(-21));
        }

        [Test]
        public void ThreeDaysAgo()
        {
            this.VerifyParse("three days ago", this.timeBase.AddDays(-3));
        }

        [Test]
        public void TwoDaysBeforeLastFriday()
        {
            this.VerifyParse("two days before last friday", this.timeBase.Next(DayOfWeek.Friday).AddDays(-9));
        }

        [Test]
        public void ShortDayName()
        {
            this.VerifyParse("fri", this.timeBase.Next(DayOfWeek.Friday));
        }

        [Test]
        public void ShortestDayName()
        {
            this.VerifyParse("fr", this.timeBase.Next(DayOfWeek.Friday));
        }

        [Test]
        public void OneMonthFromToday()
        {
            this.VerifyParse("one month from today", this.timeBase.Last().AddDays(this.timeBase.Day));
        }

        [Test]
        public void TwoMonthsFromToday()
        {
            this.VerifyParse("two months from today", this.timeBase.Last().AddDays(1).Last().AddDays(this.timeBase.Day));
        }

        [Test]
        public void OneMonthAgo()
        {
            this.VerifyParse("one month ago",
                             this.timeBase.First().AddDays(-1).First().AddDays(-1).AddDays(this.timeBase.Day));
        }

        [Test]
        public void ThreeMonthsAgo()
        {
            this.VerifyParse("three months ago", this.timeBase.AddDays(-90).First().AddDays(this.timeBase.Day - 1));
        }

        [Test]
        public void NextMonth()
        {
            this.VerifyParse("next month", this.timeBase.Last().AddDays(1));
        }

        [Test]
        public void FirstMondayNextMonth()
        {
            this.VerifyParse("first monday in next month", this.timeBase.Last().AddDays(1).Next(DayOfWeek.Monday));
        }

        [Test]
        public void TwoMondaysFromNow()
        {
            this.VerifyParse("two mondays from now", this.timeBase.Next(DayOfWeek.Monday).Next(DayOfWeek.Monday));
        }

        [Test]
        public void ThreeFriFromNow()
        {
            this.VerifyParse("three fri from now",
                             this.timeBase.Next(DayOfWeek.Friday).Next(DayOfWeek.Friday).Next(DayOfWeek.Friday));
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tu")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tu")]
        [Test]
        public void TwoTuAgo()
        {
            this.VerifyParse("two tu ago", this.timeBase.Next(DayOfWeek.Tuesday).AddDays(-14));
        }

        [Test]
        public void SecondTuesdayNextMonth()
        {
            this.VerifyParse("second tuesday of next month",
                             this.timeBase.Last().AddDays(1).Next(DayOfWeek.Tuesday).Next(DayOfWeek.Tuesday));
        }

        [Test]
        public void April()
        {
            this.VerifyParse("April", new DateTime(2008, 4, 1));
        }

        [Test]
        public void ThirdFridayOfMarch()
        {
            this.VerifyParse("third friday of March", new DateTime(2008, 3, 21));
        }

        [Test]
        public void JanBeforeNow()
        {
            this.VerifyParse("Jan", new DateTime(2009, 1, 1));
        }

        [Test]
        public void LastMonth()
        {
            this.VerifyParse("last month", new DateTime(2008, 1, 1));
        }

        [Test]
        public void NextJune()
        {
            this.VerifyParse("next june", new DateTime(2009, 6, 1));
        }

        [Test]
        public void SecondAsString()
        {
            this.VerifyParse("2nd monday in may", new DateTime(2008, 5, 12));
        }

        [Test]
        public void FirstAsString()
        {
            this.VerifyParse("1st monday in may", new DateTime(2008, 5, 5));
        }

        [Test]
        public void ThirdAsString()
        {
            this.VerifyParse("3rd monday in may", new DateTime(2008, 5, 19));
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "th")]
        [Test]
        public void The18th()
        {
            this.VerifyParse("the 18th", new DateTime(2008, 2, 18));
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "thIs")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "th")]
        [Test]
        public void The12thIsNextMonth()
        {
            this.VerifyParse("the 12th", new DateTime(2008, 3, 12));
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "th")]
        [Test]
        public void June20th()
        {
            this.VerifyParse("june 20th", new DateTime(2008, 6, 20));
        }

        [Test]
        public void TwentiethDayOfJune()
        {
            this.VerifyParse("20th day of june", new DateTime(2008, 6, 20));
        }

        [Test]
        public void TwentiethOfJune()
        {
            this.VerifyParse("20th of june", new DateTime(2008, 6, 20));
        }

        [Test]
        public void NumericDateWithYear()
        {
            this.VerifyParse("3/12/2008", new DateTime(2008, 3, 12));
        }

        [Test]
        public void NumericDateWithoutYear()
        {
            this.VerifyParse("3/12", new DateTime(2008, 3, 12));
        }
        [Ignore("this format has different meanings even for human")]
        [Test]
        public void NumericDateWithShortYear()
        {
            this.VerifyParse("3/12/08", new DateTime(2008, 3, 12));
        }

        [Test]
        public void FourthWeek()
        {
            this.VerifyParse("fourth friday in april", new DateTime(2008, 4, 25));
        }

        [Test]
        public void ThisMonth()
        {
            this.VerifyParse("second tuesday of this month", new DateTime(2008, 2, 12));
        }

        [Test]
        public void ThisJune()
        {
            this.VerifyParse("this june", new DateTime(2008, 6, 1));
        }

        [Test]
        public void ThisWeek()
        {
            this.VerifyParse("this week", CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek== DayOfWeek.Sunday? new DateTime(2008, 2, 10):new DateTime(2008,2,11));
        }

        [Test]
        public void ThisMonday()
        {
            this.VerifyParse("this monday", this.timeBase.AddWeeks(-1).Next(DayOfWeek.Monday));
        }

        [Test]
        public void ThisJanuary()
        {
            this.VerifyParse("this jan", new DateTime(2008, 1, 1));
        }

        //[Test]
        //public void FriBeforeLast()
        //{
        //    VerifyParse("fri before last fri",new DateTime(2008,2,1));
        //}
        //wed before last
        //years
        //next week
        //last week

        private void VerifyParse(string dateString, DateTime expectedDateTime)
        {
            ParserContext context = new ParserContext();
            EnglishDateTimeParser parser=new EnglishDateTimeParser(context);
            Assert.AreEqual(expectedDateTime, parser.ParseRelative(this.timeBase, dateString));
        }
    }
}