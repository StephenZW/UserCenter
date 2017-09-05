using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services
{
    public abstract class BaseService<T> where T : BaseEntity
    {
        protected abstract DbContext Db { get; set; }

        protected DbSet<T> Entities => Db.Set<T>();

        public async Task<T> GetByIdAsync(long Id, bool isTracking = false)
        {
            if (isTracking)
            {
                return await Entities.SingleOrDefaultAsync(p => p.Id == Id);
            }
            return await Entities.AsNoTracking().SingleOrDefaultAsync(p => p.Id == Id);
        }
    }
}
