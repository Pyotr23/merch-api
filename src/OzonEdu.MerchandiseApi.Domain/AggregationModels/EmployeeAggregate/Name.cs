using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Exceptions;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Name : ValueObject
    {
        public string Value { get; }
        
        public Name(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                throw new EmptyStringException("Name is empty or whitespace");
            Value = name;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}