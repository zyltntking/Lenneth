using AutoMapper;
using Lenneth.WebApi.Models;
using Lenneth.WebApi.Models.Mapper;

namespace Lenneth.WebApi
{
    /// <summary>
    /// entity映射
    /// </summary>
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            UserInfoMapper();
        }

        /// <summary>
        /// 用户信息映射
        /// </summary>
        private void UserInfoMapper()
        {
            CreateMap<BIBOUser, UserInfo>()
                .ForMember(dest => dest.Uiid, opt => opt.MapFrom(src => src.UIID))
                .ForMember(dest => dest.Uid, opt => opt.MapFrom(src => src.UID))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NAME))
                .ForMember(dest => dest.IdNo, opt => opt.MapFrom(src => src.IDNO))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MOBILENO))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TYPE))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GENDER))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.BIRTHDAY))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDR))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.PROVINCE))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CITY))
                .ForMember(dest => dest.Spell, opt => opt.MapFrom(src => src.SPELL))
                .ForMember(dest => dest.BankCard, opt => opt.MapFrom(src => src.BC))
                .ForMember(dest => dest.IdType, opt => opt.MapFrom(src => src.IDTYPE))
                .ForMember(dest => dest.Money, opt => opt.MapFrom(src => src.Money));
        }
    }
}