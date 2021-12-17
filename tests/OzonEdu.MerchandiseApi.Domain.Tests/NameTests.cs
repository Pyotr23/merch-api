using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class NameTests
    {
        [Fact]
        public void Constructor_CorrectName_Success()
        {
            const string name = "test name";

            var result = new Name(name);
            
            Assert.Equal(name, result.Value);
        }
        
        [Fact]
        public void Constructor_EmptyName_EmptyStringException()
        {
            Assert.Throws<EmptyStringException>(() => new Name(string.Empty));
        }
        
        [Fact]
        public void Constructor_WhiteSpaceName_EmptyStringException()
        {
            Assert.Throws<EmptyStringException>(() => new Name(" "));
        }
    }
}