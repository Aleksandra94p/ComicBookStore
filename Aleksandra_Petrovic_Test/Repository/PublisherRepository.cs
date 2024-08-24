using Aleksandra_Petrovic_Test.Interfaces;
using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Publisher = Aleksandra_Petrovic_Test.Models.Publisher;

namespace Aleksandra_Petrovic_Test.Repository
{
    public class PublisherRepository : IPublisherRepository
    {
         private readonly AppDbContext _context;

        public PublisherRepository(AppDbContext context)
        {
            _context = context;

        }

        public List<Publisher> GetAll()
        {
            return _context.Publishers.ToList().OrderBy(x => x.Name).ToList();

        }

        public Publisher GetById(int id)
        {
            return _context.Publishers.FirstOrDefault(x => x.Id == id);
        }

        public List<Publisher> GetByName(string name)
        {
            return _context.Publishers.Where(x => x.Name.Contains(name)).OrderByDescending(x => x.Year).ThenBy(x => x.Name).ToList();
        }


    }
}
