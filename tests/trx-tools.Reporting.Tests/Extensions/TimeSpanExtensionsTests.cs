using FluentAssertions;
using NUnit.Framework;
using trx_tools.HtmlReporting.Extensions;

namespace trx_tools.HtmlReporting.Tests.Extensions;

[TestFixture]
public class TimeSpanExtensionsTests
{
    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void ToHumanReadableTimeSpan_WhenCalledWithTimeSpan_ShouldReturnHumanReadableTimeSpan(TimeSpanTestCase testCase)
    {
        // Act
        var result = testCase.TimeSpan.ToHumanReadableTimeSpan();
        
        // Assert
        result.Should().Be(testCase.Expected);
    }

    private static IEnumerable<TimeSpanTestCase> TestCases()
    {
        yield return new TimeSpanTestCase(new TimeSpan(1, 2, 3, 4, 5), "1d 2h 3m 4s 5ms");
        yield return new TimeSpanTestCase(TimeSpan.FromMilliseconds(123), "123ms");
        yield return new TimeSpanTestCase(TimeSpan.FromSeconds(123), "2m 3s");
        yield return new TimeSpanTestCase(TimeSpan.FromMinutes(123.12345), "2h 3m 7s 407ms");
        yield return new TimeSpanTestCase(TimeSpan.FromHours(123.12345), "5d 3h 7m 24s 420ms");
    }
    
    public record TimeSpanTestCase(TimeSpan TimeSpan, string Expected);
}