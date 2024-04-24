using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCProjectdbContext _dbcontext;

        public IDepartmentRepository DepartmentRepository { get ; set; }
        public IEmployeeRepository EmployeeRepository { get; set ; }

        public UnitOfWork(MVCProjectdbContext dbcontext) 
        {
            DepartmentRepository = new DepartmentRepository(dbcontext);
            EmployeeRepository = new EmployeeRepository(dbcontext);
            _dbcontext = dbcontext;
        }
        public async Task<int> Complete()
        {
            return await _dbcontext.SaveChangesAsync();
        }
        public void Dispose() // will be called by CLR
        {
            _dbcontext.Dispose();
        }
    }
}
