using System;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class EmailAddressTests
    {
        [Fact]
        public void Constructor_NotCorrectEmail_ArgumentException()
        {
            const string badEmail = "ya@ya";
            
            Assert.Throws<ArgumentException>(() => new EmailAddress(badEmail));
        }
        
        [Fact]
        public void Constructor_CorrectEmail_Success()
        {
            const string email = "ya@ya.ru";
            
            var emailAddress = new EmailAddress(email);
            
            Assert.Equal(email, emailAddress.Value);
        }
    }
}