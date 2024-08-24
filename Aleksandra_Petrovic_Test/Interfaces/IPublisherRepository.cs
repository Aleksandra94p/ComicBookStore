using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Publisher = Aleksandra_Petrovic_Test.Models.Publisher;

namespace Aleksandra_Petrovic_Test.Interfaces
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        Publisher GetById(int id);  
        List<Publisher> GetByName(string name);

    }
}


