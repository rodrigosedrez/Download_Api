using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class PlataformService(IPlataformRepository plataformRepository, IMapper mapper) : IPlataformService
    {
        private readonly IPlataformRepository _plataformRepository = plataformRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PlataformModelsDTO> AddPlataformAsync(PlataformModelsDTO plataformModelDTO)
        {
            var plataformModel = _mapper.Map<PlataformModels>(plataformModelDTO);
            var addplataform = await _plataformRepository.AddPlataformAsync(plataformModel);
            return _mapper.Map<PlataformModelsDTO>(addplataform);
        }
        public async Task<PlataformModelsDTO> UpdatePlataformAsync(PlataformModelsDTO plataformModelDTO)
        {
            var plataformModel = _mapper.Map<PlataformModels>(plataformModelDTO);
            var result = await _plataformRepository.UpdatePlataformAsync(plataformModel);
            return _mapper.Map<PlataformModelsDTO>(result);
        }
        public async Task<PlataformModelsDTO> DeletePlataformAsync(int id)
        {
            var result = await _plataformRepository.DeletePlataformAsync(id);
            return _mapper.Map<PlataformModelsDTO>(result);
        }


        public async Task<PlataformModelsDTO?> GetPlataformByIdAsync(int id)
        {
            var result = await _plataformRepository.GetPlataformByIdAsync(id);
            return _mapper.Map<PlataformModelsDTO>(result);
        }


        public async Task<IEnumerable<PlataformModelsDTO>> GetAllPlataformsAsync()
        {
            var result = await _plataformRepository.GetAllPlataformsAsync();
            return _mapper.Map<IEnumerable<PlataformModelsDTO>>(result);
        }
        public async Task<PlataformModelsDTO?> IncrementDownloadCountPlataformAsync(int id)
        {
            var newCount = await _plataformRepository.IncrementDownloadCountPlataform(id);
            var plat = await _plataformRepository.GetPlataformByIdAsync(id);
            if(plat == null)
            {
                return null!;
            }
            return _mapper.Map<PlataformModelsDTO?>(plat);
        }
    }
}
