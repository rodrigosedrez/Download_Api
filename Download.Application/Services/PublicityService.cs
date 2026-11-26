using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class PublicityService(IPublicityRepository publicityRepository, IMapper mapper) : IPublicityService
    {
        private readonly IPublicityRepository _publicityRepository = publicityRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PublicityDTO> AddPublicityDTOAsync(PublicityDTO publicityDTO)
        {
            var publicity = _mapper.Map<Publicity>(publicityDTO);
            var addedPublicity = await _publicityRepository.AddPublicityAsync(publicity);
            return _mapper.Map<PublicityDTO>(addedPublicity);
        }

        public async Task<PublicityDTO> UpdatePublicityDTOAsync(PublicityDTO publicityDTO)
        {
            var publicity = _mapper.Map<Publicity>(publicityDTO);
            var updatedPublicity = await _publicityRepository.UpdatePublicityAsync(publicity);
            return _mapper.Map<PublicityDTO>(updatedPublicity);
        }

        public async Task<PublicityDTO> DeletePublicityDTOAsync(int id)
        {
            var deletedPublicity = await _publicityRepository.DeletePublicityAsync(id);
            return _mapper.Map<PublicityDTO>(deletedPublicity);
        }

        public async Task<PublicityDTO?> GetPublicityDTOByIdAsync(int id)
        {
            var publicity = await _publicityRepository.GetPublicityByIdAsync(id);
            return _mapper.Map<PublicityDTO>(publicity);
        }

        public async Task<IEnumerable<PublicityDTO>> GetAllPublicityDTOAsync()
        {
            var publicitys = await _publicityRepository.GetAllPublicityAsync();
            return _mapper.Map<IEnumerable<PublicityDTO>>(publicitys);
        }
    }
}