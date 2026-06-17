using Moveo_backend.Rental.Domain.Model.ValueObjects;
using Moveo_backend.UserManagement.Domain.Model.ValueObjects;
using Xunit;

namespace Moveo_backend.Tests;

public class EmailAddressTests
{
    [Fact]
    public void IsValid_WithProperEmail_ReturnsTrue()
    {
        var email = new EmailAddress("user@moveo.com");
        Assert.True(email.IsValid());
    }

    [Fact]
    public void IsValid_WithoutAtSymbol_ReturnsFalse()
    {
        var email = new EmailAddress("user.moveo.com");
        Assert.False(email.IsValid());
    }

    [Fact]
    public void IsValid_WithEmptyAddress_ReturnsFalse()
    {
        var email = new EmailAddress(string.Empty);
        Assert.False(email.IsValid());
    }
}

public class PasswordTests
{
    [Fact]
    public void IsSecure_WithSixOrMoreCharacters_ReturnsTrue()
    {
        var password = new Password("secret123");
        Assert.True(password.IsSecure());
    }

    [Fact]
    public void IsSecure_WithFewerThanSixCharacters_ReturnsFalse()
    {
        var password = new Password("abc");
        Assert.False(password.IsSecure());
    }
}

public class PersonNameTests
{
    [Fact]
    public void FullName_CombinesFirstAndLastName()
    {
        var name = new PersonName("Andrew", "Soto");
        Assert.Equal("Andrew Soto", name.FullName);
    }

    [Fact]
    public void FullName_WithOnlyFirstName_TrimsTrailingSpace()
    {
        var name = new PersonName("Andrew");
        Assert.Equal("Andrew", name.FullName.Trim());
    }
}

public class DateRangeTests
{
    [Fact]
    public void Constructor_WithValidRange_SetsDates()
    {
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 1, 5);

        var range = new DateRange(start, end);

        Assert.Equal(start, range.StartDate);
        Assert.Equal(end, range.EndDate);
    }

    [Fact]
    public void Constructor_WhenEndBeforeStart_ThrowsArgumentException()
    {
        var start = new DateTime(2026, 1, 5);
        var end = new DateTime(2026, 1, 1);

        Assert.Throws<ArgumentException>(() => new DateRange(start, end));
    }

    [Fact]
    public void Constructor_WhenEndEqualsStart_ThrowsArgumentException()
    {
        var date = new DateTime(2026, 1, 1);

        Assert.Throws<ArgumentException>(() => new DateRange(date, date));
    }
}
