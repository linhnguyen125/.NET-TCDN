using Dapper;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Infrastructure.Postgres.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Postgres.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {

    }
}
