using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Aleksandra_Petrovic_Test.Interfaces
{
    public interface IComicBookRepository
    {
        List<ComicBook> GetAll();

        ComicBook GetById(int id);
        List<ComicBook> GetByGenre(string genre);

        void Create(ComicBook comicBook);
        void Update(ComicBook comicBook);
        void Delete(ComicBook comicBook);
        List<ComicBook> GetByAvailableQuantity(ComicBookFilter filter);

       // List<PublisherReportDTO> GetReport(int limit);

    }
}
