using System.Collections.Generic;
using System.Linq;

namespace OzonEdu.MerchandiseApi.Domain.Models
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            return ReferenceEquals(left, null) ^ ReferenceEquals(right, null)
                ? false
                : ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x == null 
                    ? 0 
                    : x.GetHashCode())
                .Aggregate((x, y) => x ^ y);
        }

        public ValueObject? GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}