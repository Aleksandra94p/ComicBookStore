using Aleksandra_Petrovic_Test.Interfaces;
using Aleksandra_Petrovic_Test.Models.DTO;
using Aleksandra_Petrovic_Test.Repository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Aleksandra_Petrovic_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;

        public PublishersController(IPublisherRepository publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
            {
          var result = _publisherRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<PublisherDTO>>(result));
          
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id) 
            {
             if (id < 0)
            {
                return BadRequest();
            }
            var result = _publisherRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PublisherDTO>(result));
            }

        [Route("potrazi")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetByName(string name)
            {
            if (string.IsNullOrEmpty(name)) 
                {
                return BadRequest();
                }
            var result = _publisherRepository.GetByName(name);
            return Ok(_mapper.Map<IEnumerable<PublisherDTO>>(result));
            }

       
    }
}
