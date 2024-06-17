using CRUDTEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTEST.Services.Interfaces
{
    public interface IContactService
    {
        void AddContact();
        void RemoveContact();

        void ShowContacts();

        List<PhoneContact> GetContacts();

    }
}
