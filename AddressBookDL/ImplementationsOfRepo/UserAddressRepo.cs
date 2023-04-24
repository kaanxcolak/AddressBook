using AddressBookDL.InterfacesOfRepo;
using AddressBookEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDL.ImplementationsOfRepo
{
    public class UserAddressRepo:Repository<UserAddress,int>,IUserAddressRepo
    {
        public UserAddressRepo(MyContext c):base(c) //context yerine c de yazılabilir farketmez. Gstermek için yapıldı!
        {
            
        }
    }
}
