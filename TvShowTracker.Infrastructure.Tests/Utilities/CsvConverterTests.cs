using System;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using TvShowTracker.Infrastructure.Utilities;

namespace TvShowTracker.Infrastructure.Tests.Utilities
{
    [TestFixture]
    public class CsvConverterTests
    {

        [Test]
        public void GetCsvBytes_Returns_Csv_File_Data()
        {
            var data = new List<MockModel>()
            {
                new MockModel()
                {
                    PropertyString = "MyStr",
                    PropertyNumber = 1,
                    PropertyDate = new DateTime(2022,5,12),
                    PropertyBool = false
                },
                new MockModel()
                {
                    PropertyBool = true,
                    PropertyNumber = 40,
                    PropertyDate = new DateTime(2021,5,12),
                    PropertyString = "str"
                }
            };

            var bytes = CsvConverter.GetCsvBytes(data);

            //convert to string to be human readable
            var str = System.Text.Encoding.UTF8.GetString(bytes);
            // TODO: ideally this should be compared to an existing file csv
            str.Should().BeEquivalentTo(@"PropertyBool, PropertyDate, PropertyNumber, PropertyString
False, 5/12/2022 12:00:00 AM, 1, MyStr
True, 5/12/2021 12:00:00 AM, 40, str
");
        }

        public class MockModel
        {
            public string PropertyString { get; set; }

            public bool? PropertyBool { get; set; }

            public int? PropertyNumber { get; set; }

            public DateTime? PropertyDate { get; set; }
        }
    }
}
