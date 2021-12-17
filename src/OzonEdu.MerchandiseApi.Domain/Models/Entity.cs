using System;
using System.Collections.Generic;
using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Models
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private readonly List<INotification> _domainEvents = new();

        public int Id { get; protected set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private bool IsTransient() => Id == default;

        public override bool Equals(object? obj)
        {
            if (obj is not Entity entity)
                return false;

            if (ReferenceEquals(this, entity))
                return true;

            if (GetType() != entity.GetType() || entity.IsTransient() || IsTransient())
                return false;

            return entity.Id == Id;
        }

        public override int GetHashCode()
        {
            if (IsTransient())
                return base.GetHashCode();
            
            _requestedHashCode ??= HashCode.Combine(Id, 31);

            return _requestedHashCode.Value;
        }
        public static bool operator ==(Entity? left, Entity? right)
        {
            return left?.Equals(right) 
                   ?? Equals(right, null);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }
}