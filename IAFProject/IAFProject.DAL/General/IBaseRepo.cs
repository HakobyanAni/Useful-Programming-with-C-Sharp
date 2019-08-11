using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IAFProject.DAL.General
{
    public interface IBaseRepo<T> where T : class, IBaseEntity
    {
        Task<int> Add(T entity);
        Task<int> AddRange(List<T> entities);
    }
}