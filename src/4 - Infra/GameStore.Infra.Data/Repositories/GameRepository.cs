using GameStore.Domain.Entities;
using GameStore.Domain.Interfaces.Repositories;
using GameStore.Infra.Data.Context;
using GameStore.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GameStore.Infra.Data.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private GameStoreContext _db;
        public GameRepository(GameStoreContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Game> SearchByName(string search)
        {
            return _db.Games.Where(p => p.Name.Contains(search));
        }

        public async Task<IEnumerable<dynamic>> GetAllGamesWithDevelopersAsync()
        {
            var query = from game in _db.Games
                        select new { game };

            return await query.ToListAsync();
        }

        public override async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _db.Games
                      .Include(_ => _.GameDevelopers)
                      .ThenInclude(_ => _.Developer)
                      .Include(_ => _.GameGenres)
                      .ThenInclude(_ => _.Genre)
                      .Include(_ => _.GamePlataforms)
                      .ThenInclude(_ => _.Plataform)
                      .Include(_ => _.GamePublishers)
                      .ThenInclude(_ => _.Publisher)
                      .ToListAsync();
        }

        public override async Task<Game> GetByIdAsync(Guid id)
        {
            return await _db.Games
                      .Include(_ => _.GameDevelopers)
                      .ThenInclude(_ => _.Developer)
                      .Include(_ => _.GameGenres)
                      .ThenInclude(_ => _.Genre)
                      .Include(_ => _.GamePlataforms)
                      .ThenInclude(_ => _.Plataform)
                      .Include(_ => _.GamePublishers)
                      .ThenInclude(_ => _.Publisher)
                      .Where(_ => _.Id == id)
                      .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Game>> GetBestRatedGamesAsync()
        {
            var query = (from games in _db.Games
                            orderby games.Score descending
                            select games).Take(5);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetBestSellerGamesAsync()
        {
            var query = from games in _db.Games
                        where (
                            from cartItems in _db.CartItems
                            join carts in _db.ShoppingCarts on cartItems.ShoppingCart.Id equals carts.Id
                            join orders in _db.Orders on carts.OrderId equals orders.Id
                            where orders.Active == false
                            select cartItems.Id
                        ).Take(5).Contains(games.Id)
                        select games;   

            return await query.ToListAsync();
        }
    }
}