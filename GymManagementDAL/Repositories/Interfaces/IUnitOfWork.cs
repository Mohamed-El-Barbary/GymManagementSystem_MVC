using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        int SaveChanges();

    }
}
