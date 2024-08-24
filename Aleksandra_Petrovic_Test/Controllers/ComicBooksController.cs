using Aleksandra_Petrovic_Test.Interfaces;
using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aleksandra_Petrovic_Test.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComicBooksController : ControllerBase
    {
        private readonly IComicBookRepository _comicBookRepository;
        private readonly IMapper _mapper;

        public ComicBooksController(IComicBookRepository comicBookRepository, IMapper mapper)
        {
            _comicBookRepository = comicBookRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
            {
            var result = _comicBookRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ComicBookDTO>>(result));
         
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById(int id) 
            { 
             if (id < 0)
            {
                return BadRequest("Invalid Id!");
            }
            var result = _comicBookRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ComicBookDTO>(result));
            }

        [Route("pronadji")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetByGenre(string genre) 
            {
            if (string.IsNullOrEmpty(genre)) 
                {
                return BadRequest();
                }
            var result = _comicBookRepository.GetByGenre(genre);
            return Ok(_mapper.Map<IEnumerable<ComicBookDTO>>(result));
            }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create(ComicBook comicBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _comicBookRepository.Create(comicBook);
            
            return CreatedAtAction("GetById", new { id = comicBook.Id }, _mapper.Map<ComicBookDTO>(comicBook));

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Update(int id, ComicBook comicBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != comicBook.Id)
            {
                return BadRequest("Invalid Id!");
            }

            try
            {
                _comicBookRepository.Update(comicBook);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(_mapper.Map<ComicBookDTO>(comicBook));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            var explorer = _comicBookRepository.GetById(id);
            if (explorer == null)
            {
                return BadRequest();
            }
            _comicBookRepository.Delete(explorer);
            return NoContent();
        }

        [Route("/api/pretraga")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetByAvailableQuantity(ComicBookFilter filter) 
            {
              if (filter == null)
            {
                filter = new ComicBookFilter()
                {
                    Max = int.MaxValue,
                    Min = int.MinValue
                };
            }
            if (filter.Min < 0 || filter.Max < filter.Min || filter.Max < 0)
            {
                return BadRequest();
            }
            var explorers = _comicBookRepository.GetByAvailableQuantity(filter);
            return Ok(_mapper.Map<IEnumerable<ComicBookDTO>>(explorers));
            }


    }
}
