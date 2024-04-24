using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly MVCProjectdbContext _dbcontext;

        public EmployeeRepository(MVCProjectdbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbcontext.Employees.Where(E => E.Address == address);
        }

        public IQueryable<Employee> SearchEmployeesByName(string SearchValue)
        {
            return _dbcontext.Employees.Where(E => E.Name.ToLower().Trim().Contains(SearchValue.ToLower().Trim()));

        }
    }
}
