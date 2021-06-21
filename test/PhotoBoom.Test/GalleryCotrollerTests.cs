
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhotoBoom.Contracts;
using PhotoBoom.Controllers;
using PhotoBoom.Models;
using Xunit;

namespace PhotoBoom.Test
{
    public class GalleryCotrollerTests
    {
        private readonly Mock<IGalleryRepository> _mockRepo;
        private readonly GalleryController _controller;
        private List<Gallery> gallery;
        public GalleryCotrollerTests()
        {

            _mockRepo = new Mock<IGalleryRepository>(MockBehavior.Loose);
            _controller = new GalleryController(_mockRepo.Object);
            var stream = File.OpenRead(@"./1.jpg");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./1.jpg"))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };
            gallery = new List<Gallery>(){
                new Gallery{
                    Id=1,
                    Title="Test",
                    Tags="#Unit#Integration#Test",
                    ImagePath="1.jpg",
                    ImageFile=file
        },
                 new Gallery{
                    Id=2,
                    Title="Test2",
                    Tags="#Unit2#Integration2#Test2",
                    ImagePath="2.jpg",
                    ImageFile=file
                },
            };
        }

        [Fact]
        public async void IndexActionExecutesReturnView()
        {
            var result = await _controller.Index();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void IndexActionExecuteReturnGalleryList()
        {
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(gallery);
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var galleryList = Assert.IsAssignableFrom<IEnumerable<Gallery>>(viewResult.Model);
            Assert.Equal<int>(2, galleryList.Count());
        }
        [Fact]
        public async void DetailsIdIsNullReturnBadRequest()
        {
            var result = await _controller.Details(null);
            var redirect = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, redirect.StatusCode);


        }
        [Fact]
        public async void DetailsInValidIdReturnNotFound()
        {
            Gallery glr = null;
            _mockRepo.Setup(repo => repo.GetItemAsync(0)).ReturnsAsync(glr);
            var result = await _controller.Details(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);

        }

        [Theory]
        [InlineData(2)]
        public async void DetailsValiIdReturnGallery(int Id)
        {
            Gallery glr = gallery.First(x => x.Id == Id);
            _mockRepo.Setup(repo => repo.GetItemAsync(Id)).ReturnsAsync(glr);
            var result = await _controller.Details(Id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultGallery = Assert.IsAssignableFrom<Gallery>(viewResult.Model);

            Assert.Equal(glr.Id, resultGallery.Id);
            Assert.Equal(glr.Title, resultGallery.Title);
            Assert.Equal(glr.Tags, resultGallery.Tags);
            Assert.Equal(glr.ImagePath, resultGallery.ImagePath);
        }

        [Fact]
        public void CreateActionExecutesReturnView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void CreateInValidModelStateReturnView()
        {
            _controller.ModelState.AddModelError("Title", "Title is required.");
            var result = await _controller.Create(gallery.First());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Gallery>(viewResult.Model);
        }

        [Fact]
        public async void CreateValidModelStateReturnRedirectToIndex()
        {
            var result = await _controller.Create(gallery.First());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal<string>("Index", redirect.ActionName);

        }
        [Fact]
        public async void CreateValidModeStateCreateMethodExecute()
        {

            Gallery glr = null;
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Gallery>())).Callback<Gallery>(x => glr = x);

            var result = await _controller.Create(gallery.First());
            _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Gallery>()), Times.Once());

            Assert.Equal(gallery.First().Id,glr.Id);
        }

        [Fact]
        public async void DeleteIdIsNullReturnBadRequest()
        {
            var result = await _controller.DeleteConfirmed(null);
            var redirect = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, redirect.StatusCode);


        }

         [Fact]
        public async void DeleteInValidIdReturnNotFound()
        {
            Gallery glr = null;
            _mockRepo.Setup(repo => repo.GetItemAsync(0)).ReturnsAsync(glr);
            var result = await _controller.DeleteConfirmed(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);

        }
        [Theory]
        [InlineData(2)]
        public async void DeleteValiIdReturnGallery(int Id)
        {
            Gallery glr = gallery.First(x => x.Id == Id);
            _mockRepo.Setup(repo => repo.GetItemAsync(Id)).ReturnsAsync(glr);
            var result = await _controller.DeleteConfirmed(Id);
           // var viewResult = Assert.IsType<ViewResult>(result);
           
             var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal<string>("Index", redirect.ActionName);
            

          
        }

    }
}