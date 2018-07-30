using AppCore.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);

        IEnumerable<T> ListAll();

        T Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
