using AutoMapper;
using Download.Application.DTOs;
using Download.Domain.Entities;

namespace Download.Application.Mappings
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<GameModel, GameModelDTO>().ReverseMap();
            CreateMap<GameCategoryModel, GameCategoryModelDTO>().ReverseMap();
            CreateMap<EmulatorsModel, EmulatorsModelDTO>().ReverseMap();
            CreateMap<ExternalLinksModelDTO, ExternalLinksModel>().ReverseMap();
            CreateMap<MagazineModel, MagazineModelDTO>().ReverseMap();
            CreateMap<PlataformModels, PlataformModelsDTO>().ReverseMap();
            CreateMap<PostModel, PostModelDTO>().ReverseMap();
            CreateMap<Publicity, PublicityDTO>().ReverseMap();
            CreateMap<MisturebaModel, MisturebaModelDTO>().ReverseMap();
            CreateMap<GameModelDTOCreate, GameModel>().ReverseMap();
            CreateMap<GameTagModelDTO, GameTagModel>().ReverseMap();
        }
    }
}
