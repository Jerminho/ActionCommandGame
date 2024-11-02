using ActionCommandGame.DTO.Filters;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Extensions.Filters
{
    internal static class PlayerItemFilterExtensions
    {
        public static IQueryable<PlayerItem> ApplyFilter(this IQueryable<PlayerItem> query,
            PlayerItemFilterDto? filter)
        {
            if (filter is null)
            {
                return query;
            }

            if (filter.PlayerId.HasValue)
            {
                query = query.Where(pi => pi.PlayerId == filter.PlayerId.Value);
            }

            return query;
        }
    }
}
