using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Name Name { get; }
        
        public ClothingSize? ClothingSize { get; private set; }
        
        public EmailAddress? EmailAddress { get; private set; }

        public EmailAddress? ManagerEmailAddress { get; private set; }

        public List<MerchDelivery> MerchDeliveries { get; } = new();

        public Employee(Name name, EmailAddress? email)
        {
            Name = name;
            SetEmailAddress(email);
        }
        
        public Employee(int id, Name name, EmailAddress? email, EmailAddress? managerEmail, ClothingSize? clothingSize)
        : this(name, email, managerEmail, clothingSize)
        {
            Id = id;
        }

        public Employee(Name name, EmailAddress? email, EmailAddress? managerEmail, ClothingSize? clothingSize)
        {
            Name = name;
            SetEmailAddress(email);
            SetManagerEmailAddress(managerEmail);
            SetClothingSize(clothingSize);
        }

        public void SetEmailAddress(EmailAddress? email)
        {
            if (email is null)
                return;
            EmailAddress = email;
        }
        
        public void SetManagerEmailAddress(EmailAddress? email)
        {
            if (email is null)
                return;
            ManagerEmailAddress = email;
        }

        public void SetClothingSize(ClothingSize? size)
        {
            if (size is null)
                return;
            ClothingSize = size;
        }

        public void AddMerchDelivery(MerchDelivery merchDelivery)
        {
            MerchDeliveries.Add(merchDelivery);
        }
    }
}