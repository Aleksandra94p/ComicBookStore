using Aleksandra_Petrovic_Test.Controllers;
using Aleksandra_Petrovic_Test.Interfaces;
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
    public class PublisherControllerTest
    {
        [Fact]
        public void GetById_InvalidId_ReturnsNotFound()
        {

            var mockRepo = new Mock<IPublisherRepository>();
            mockRepo.Setup(x => x.GetById(5));
            var controller = new PublishersController(mockRepo.Object, null);
            var result = controller.GetById(5) as NotFoundResult;
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }


    }
}
