using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class AccountObjectService : BaseService<AccountObject>, IAccountObjectService
    {
        IAccountObjectRepository _accountObjectRepository;
        public AccountObjectService(IAccountObjectRepository accountObjectRepository) : base(accountObjectRepository)
        {
            _accountObjectRepository = accountObjectRepository;
        }
    }
}
