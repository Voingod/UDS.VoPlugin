using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using UDS.VoPlugin.Repository;

namespace UDS.VoPlugin
{
    public class ServiceClass
    {
        private IOrganizationService _service;
        public ServiceClass(IOrganizationService service)
        {
            _service = service;
        }

        public void MainMethod()
        {
            ContactRepository contactRepository = new ContactRepository(_service);
            EntityCollection contacts = contactRepository.GetContactsWithPhone(new Guid("09C2DEE9-84B8-E711-80FB-00155D05FA01"));
            if (contacts != null)
            {
                Console.WriteLine(contacts.Entities.Count);
            }
            else
            {
                Console.WriteLine("No contacts");
            }
            Console.ReadLine();
        }
    }
}
