using Aleksandra_Petrovic_Test.Controllers;
using Aleksandra_Petrovic_Test.Interfaces;
using Aleksandra_Petrovic_Test.Models;
using Aleksandra_Petrovic_Test.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aleksandra_Petrovic_Test_Test.Controllers
{
    public class ComicBooksControllerTest
    {
        [Fact]
        public void GetByID_ValidId_ReturnsOkObjectResult()
        {
            ComicBook comicBook = new ComicBook()
            {
                Id = 1,
                Name = "Test",
                Genre = "TestGenre",
                Price = 250,
                AvailableQuantity = 7,
                PublisherId = 1,
                Publisher = new Publisher() { Id = 1, Name = "TestPublisher", Year = 1996 }

            };

            ComicBookDTO comicBookDTO = new ComicBookDTO()
            {
                Id = 1,
                Name = "Test",
                Genre = "TestGenre",
                Price = 250,
                AvailableQuantity = 7,
                PublisherId = 1,
                PublisherName = "TestPublisher"
            };


            var mockRepository = new Mock<IComicBookRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(comicBook);

            var mapConf = new MapperConfiguration(cfg => cfg.AddProfile(new ComicBookProfile()));
            IMapper mapper = new Mapper(mapConf);
            var controller = new ComicBooksController(mockRepository.Object, mapper);

            var result = controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            ComicBookDTO dtoResult = (ComicBookDTO)result.Value;
            Assert.Equal(comicBookDTO, dtoResult);
        }

        [Theory]
        [InlineData(24, 1, "Invalid Id!")]
        [InlineData(1, 1, null)]
        public void Put_InvalidId_RetunsBadRequest(int routeId, int comicBookId, string expectedMessage)
        {

            ComicBook comicBook = new ComicBook()
            {
                Id = 1,
                Name = "Test",
                Genre = "TestGenre",
                Price = 250,
                AvailableQuantity = 7,
                PublisherId = 1,
                Publisher = new Publisher() { Id = 1, Name = "TestPublisher", Year = 1996 }
            };

            var mockRepository = new Mock<IComicBookRepository>();

            var controller = new ComicBooksController(mockRepository.Object, null);

            if (expectedMessage == null)
            {
                controller.ModelState.AddModelError("error", "error");
            }

            var actionResult = controller.Update(routeId, comicBook);

            var badReqRes = actionResult as BadRequestObjectResult;

            if (expectedMessage != null)
            {
                Assert.Equal(expectedMessage, badReqRes.Value);
            }
            else
            {
                Assert.IsType<SerializableError>(badReqRes.Value);
            }


        }

        [Fact]
        public void GetBySalary_ReturnsCollection()
        {
            List<ComicBookDTO> comicBooks = new List<ComicBookDTO>()
{
                new ComicBookDTO
                {
                   Id = 1,
                    Name = "Test",
                    Genre = "TestGenre",
                    Price = 250,
                    AvailableQuantity = 7,
                    PublisherId = 1,
                    PublisherName = "Test"
                },
                   new ComicBookDTO
                {
                    Id = 2,
                    Name = "Test1",
                    Genre = "TestGenre1",
                    Price = 259,
                    AvailableQuantity = 17,
                    PublisherId = 1,
                    PublisherName = "Test"

                }

                };


            var mockRepo = new Mock<IComicBookRepository>();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ComicBookProfile()));
            IMapper mapper = new Mapper(mapConfig);

            var controller = new ComicBooksController(mockRepo.Object, mapper);

            var actionResult = controller.GetByAvailableQuantity(new ComicBookFilter() { Min = 1, Max = 70 }) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<ComicBookDTO> listResults = (List<ComicBookDTO>)actionResult.Value;

            for (int i = 0; i < listResults.Count; i++)
            {
                Assert.Equal(listResults[i], comicBooks[i]);
            }

        }
    }
}
