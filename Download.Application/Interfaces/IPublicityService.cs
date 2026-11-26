

using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IPublicityService
    {
        Task<PublicityDTO> AddPublicityDTOAsync(PublicityDTO publicity);
        Task<PublicityDTO> UpdatePublicityDTOAsync(PublicityDTO publicity);
        Task<PublicityDTO> DeletePublicityDTOAsync(int id);
        Task<PublicityDTO?> GetPublicityDTOByIdAsync(int id);
        Task<IEnumerable<PublicityDTO>> GetAllPublicityDTOAsync();
    }
}
