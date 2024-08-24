using Aleksandra_Petrovic_Test.Interfaces;
using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleksandra_Petrovic_Test.Repository
{
    public class ComicBookRepository : IComicBookRepository
    {
         private readonly AppDbContext _context;

        public ComicBookRepository(AppDbContext context)
        {
            _context = context;

        }

        public void Create(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void Delete(ComicBook comicBook)
        {
            _context.ComicBooks.Remove(comicBook);
            _context.SaveChanges();
        }

        public List<ComicBook> GetAll()
        {
            return _context.ComicBooks.Include(x => x.Publisher).ToList().OrderBy(x => x.Genre).ToList();
        }
       
        public List<ComicBook> GetByAvailableQuantity(ComicBookFilter filter)
        {
            var result = _context.ComicBooks.Include(x => x.Publisher)
                .Where(x => x.AvailableQuantity < filter.Max && x.AvailableQuantity > filter.Min)
                .OrderByDescending(x => x.Price).ToList();
            return result;
        }
        
        public List<ComicBook> GetByGenre(string genre)
        {
            return _context.ComicBooks.Include(x => x.Publisher).Where(x => x.Genre == genre).OrderByDescending(x => x.Price).ToList();
        }

        public ComicBook GetById(int id)
        {
            return _context.ComicBooks.Include(x => x.Publisher).FirstOrDefault(x => x.Id == id);
        }
      
 

        public void Update(ComicBook comicBook)
        {
            
            _context.Entry(comicBook).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
