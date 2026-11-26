using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;
using System;

namespace Download.Application.Services
{
    public class MagazineService(IMagazineRepository magazineRepository, IMapper mapper) : IMagazineService
    {
        private readonly IMagazineRepository _magazineRepository = magazineRepository;
        private readonly IMapper _mapper = mapper;
        
        public async Task<MagazineModelDTO> AddMagazineAsync(MagazineModelDTO magazineModelDTO)
        {
            var magazineModel = _mapper.Map<MagazineModel>(magazineModelDTO);
            var addedMagazine = await _magazineRepository.AddMagazineAsync(magazineModel);
            return _mapper.Map<MagazineModelDTO>(addedMagazine);
        }

        public async Task<MagazineModelDTO> UpdateMagazineAsync(MagazineModelDTO magazineModelDTO)
        {
            var magazineModel2 = _mapper.Map<MagazineModel>(magazineModelDTO);
             await _magazineRepository.UpdateMagazineAsync(magazineModel2);
            return _mapper.Map<MagazineModelDTO>(magazineModel2);
        }
        public async Task<MagazineModelDTO> DeleteMagazineAsync(int id)
        {
            var deletedMagazine = await _magazineRepository.DeleteMagazineAsync(id);
            return _mapper.Map<MagazineModelDTO>(deletedMagazine);
        }

        public async Task<MagazineModelDTO?> GetMagazineByIdAsync(int id)
        {
            var magazineModel = await _magazineRepository.GetMagazineByIdAsync(id);
            return _mapper.Map<MagazineModelDTO>(magazineModel);
        }


        public async Task<IEnumerable<MagazineModelDTO>> GetAllMagazinesAsync()
        {
            var magazines = await _magazineRepository.GetAllMagazinesAsync();
            return _mapper.Map<IEnumerable<MagazineModelDTO>>(magazines);
        }
    }
}
