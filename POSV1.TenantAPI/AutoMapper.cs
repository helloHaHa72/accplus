using AutoMapper;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Models;

public class AutoProfile : Profile
{
    public AutoProfile()
    {
        CreateMap<VMCustomer, cus01customers>();
            //.ForMember(dest => dest.cus01deleted, opt => opt.Ignore()) 
            //.ForMember(dest => dest.CreatedName, opt => opt.Ignore())
            //.ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            //.ForMember(dest => dest.UpdatedName, opt => opt.Ignore())
            //.ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            //.ForMember(dest => dest.DeletedName, opt => opt.Ignore())
            //.ForMember(dest => dest.cus01create_by, opt => opt.Ignore())
            //.ForMember(dest => dest.cus01create_date, opt => opt.Ignore())
            //.ForMember(dest => dest.cus01update_by, opt => opt.Ignore())
            //.ForMember(dest => dest.cus01name_nep, opt => opt.Ignore());
    }
}
