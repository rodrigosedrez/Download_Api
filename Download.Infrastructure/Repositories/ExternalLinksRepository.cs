using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class ExternalLinksRepository(DownloadsDBContext downloadsDBContext) : IExternalLinksRepository
    {
        private readonly DownloadsDBContext _Context = downloadsDBContext;
       
        public async Task<ExternalLinksModel> AddExternalLinkAsync(ExternalLinksModel externalLinksModel)
        {
            _Context.ExternalLinksModels.Add(externalLinksModel);
            await _Context.SaveChangesAsync();
            return externalLinksModel;
        }
        public async Task<ExternalLinksModel> UpdateExternalLinkAsync(ExternalLinksModel externalLinksModel)
        {
             _Context.ExternalLinksModels.Update(externalLinksModel);
             await _Context.SaveChangesAsync();
            return externalLinksModel;
        }
         
            
        
        public async Task<ExternalLinksModel> DeleteExternalLinkAsync(int id)
        {
            var externalLink = await _Context.ExternalLinksModels.FindAsync(id);
            if (externalLink == null)
                return null!;
            _Context.ExternalLinksModels.Remove(externalLink);
            await _Context.SaveChangesAsync();
            return externalLink;
        }
        public async Task<ExternalLinksModel?> GetExternalLinkByIdAsync(int id)
        {
            var externalLink = await _Context.ExternalLinksModels.FindAsync(id);
            return externalLink;
        }
        public async Task<IEnumerable<ExternalLinksModel>> GetAllExternalLinksAsync()
        {
            var result = await _Context.ExternalLinksModels.ToListAsync();
            return result;
        }
    }
}
