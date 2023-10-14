using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public class TierRepositories : ITierRepositories
    {
        private readonly AppDbContext _dbContext;
        public TierRepositories(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        AddTiersDTO ITierRepositories.AddTier(AddTiersDTO addTiersDTO)
        {
            var TierDomainModel = new Tiers
            {
                tiername = addTiersDTO.tiername,
            };
            _dbContext.Tiers.Add(TierDomainModel);
            _dbContext.SaveChanges();
            return addTiersDTO;

        }

        Tiers? ITierRepositories.DeleteTierById(int id)
        {
            var TierDomain = _dbContext.Tiers.FirstOrDefault(n => n.Id == id);
            if (TierDomain != null)
            {
                _dbContext.Tiers.Remove(TierDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }

        TiersListResult ITierRepositories.GetlAllTier(int pageNumber = 1, int pageSize = 5)
        {
            var skipResults = (pageNumber - 1) * pageSize;

            var query = _dbContext.Tiers.Select(Tiers => new TiersDTO
            {
                Id = Tiers.Id,
                tiername = Tiers.tiername,
                Users = Tiers.tier_user.Select(n => n.user.firstname + " " + n.user.lastname).ToList()
            }).ToList();
           
            var totalTiers = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalTiers / pageSize);

            var tiers = query
                .OrderBy(u => u.Id)
                .Skip(skipResults)
                .Take(pageSize)
                .ToList();

            var result = new TiersListResult
            {
                Tiers = tiers,
                total = totalTiers,
                TotalPages = totalPages,
            };

            return result;
        }

         TiersNoIdDTO ITierRepositories.GetTierById(int id)
        {
            var TierWithDomain = _dbContext.Tiers.Where(n => n.Id == id);
            var TierWithIdDTO = TierWithDomain.Select(Tier => new TiersNoIdDTO(){
                tiername = Tier.tiername,
                Users = Tier.tier_user.Select(n => n.user.firstname + " " + n.user.lastname).ToList()
            }).FirstOrDefault();
            return TierWithIdDTO;
        }

        AddTiersDTO ITierRepositories.UpdateTierById(int id, AddTiersDTO tiersNoIdDTO)
        {
            var TierDomain = _dbContext.Tiers.FirstOrDefault(n => n.Id == id);
            if (TierDomain != null)
            {
                TierDomain.tiername = tiersNoIdDTO.tiername;
                _dbContext.SaveChanges();
            }
            return tiersNoIdDTO;
        }
    }
}
