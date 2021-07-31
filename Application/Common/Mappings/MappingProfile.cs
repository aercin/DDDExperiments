using Application.Customer.Queries.Models;
using Application.StoredEvent.Queries.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Aggregates.Customer.Customer, CustomerDto>();
            CreateMap<Domain.Aggregates.StoredEvent.StoredEvent, CustomerHistoryDto>().ConvertUsing(new StoredEventConverter());
        }

        public class StoredEventConverter : ITypeConverter<Domain.Aggregates.StoredEvent.StoredEvent, CustomerHistoryDto>
        {  
            public CustomerHistoryDto Convert(Domain.Aggregates.StoredEvent.StoredEvent source, CustomerHistoryDto destination, ResolutionContext context)
            { 
                var template = new { Item = new { Id = "", Name = "", Email = "", BirthDate = "" }, MessageType = "", Timestamp = "" };
                
                var anonymousTemplateObj = JsonConvert.DeserializeAnonymousType(source.Data, template);

                return  new CustomerHistoryDto
                {
                    Action = anonymousTemplateObj.MessageType,
                    Id = anonymousTemplateObj.Item.Id,
                    Name = anonymousTemplateObj.Item.Name,
                    Email = anonymousTemplateObj.Item.Email,
                    BirthDate = anonymousTemplateObj.Item.BirthDate,
                    Timestamp = anonymousTemplateObj.Timestamp
                };
            }
        }

        //private CustomerHistoryDto CustomConversionOfStoreEvent(Domain.Aggregates.StoredEvent.StoredEvent prm1, CustomerHistoryDto prm2)
        //{
        //    var template = new { Item = new { Id = "", Name = "", Email = "", BirthDate = "" }, MessageType = "", Timestamp = "" };
        //    var anonymousTemplateObj = JsonConvert.DeserializeAnonymousType(prm1.Data, template);

        //    return new CustomerHistoryDto
        //    {
        //        Action = anonymousTemplateObj.MessageType,
        //        Id = anonymousTemplateObj.Item.Id,
        //        Name = anonymousTemplateObj.Item.Name,
        //        Email = anonymousTemplateObj.Item.Email,
        //        BirthDate = anonymousTemplateObj.Item.BirthDate,
        //        Timestamp = anonymousTemplateObj.Timestamp
        //    };
        //}
    }
}
