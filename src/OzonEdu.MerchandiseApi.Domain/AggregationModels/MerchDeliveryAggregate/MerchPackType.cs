using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;
using Enums = CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchPackType : MerchTypeEnumeration
    {
        public static MerchPackType WelcomePack 
            = new(Enums.MerchType.WelcomePack, new [] { MerchType.Pen });

        public static MerchPackType ProbationPeriodEndingPack 
            = new(Enums.MerchType.ProbationPeriodEndingPack, new [] { MerchType.Socks });
        
        public static MerchPackType ConferenceListenerPack 
            = new(Enums.MerchType.ConferenceListenerPack, new [] { MerchType.Notepad });
        
        public static MerchPackType ConferenceSpeakerPack 
            = new(Enums.MerchType.ConferenceSpeakerPack, new [] { MerchType.TShirt });
        
        public static MerchPackType VeteranPack 
            = new(Enums.MerchType.VeteranPack, new []{ MerchType.Bag, MerchType.Sweatshirt });
        
        public MerchPackType(Enums.MerchType packType, IEnumerable<MerchType> merchTypes) 
            : base((int)packType, packType.ToString(), merchTypes)
        { }
        
        public MerchPackType(int id, string name, IEnumerable<MerchType> merchTypes) 
            : base(id, name, merchTypes)
        { }
        
        public MerchPackType(int id, string name) 
            : base(id, name, Array.Empty<MerchType>())
        { }
    }
}