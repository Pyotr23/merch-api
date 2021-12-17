using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; }
        
        public EmailAddress(string email)
        {
            if (!IsValidMail(email))
                throw new ArgumentException("Not valid email", nameof(email));
            Value = email;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        
        private static bool IsValidMail(string emailAddress)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");  
            var match = regex.Match(emailAddress);
            return match.Success;
        }
    }
}