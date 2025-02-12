using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers;
using API.Data;
using API.DataEntities;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.UnitTests.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UsersController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithMembersList()
        {
            // Arrange
            var members = new List<MemberResponse>
            {
                new MemberResponse { Id = 1, UserName = "arenita", Gender = "female" },
                new MemberResponse { Id = 2, UserName = "perlita", Gender = "female" }
            };
            _mockRepo.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(members);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMembers = Assert.IsAssignableFrom<IEnumerable<MemberResponse>>(okResult.Value);
            Assert.Equal(2, returnedMembers.Count());
        }

        [Fact]
        public async Task GetByUsernameAsync_ReturnsNotFound_WhenMemberDoesNotExist()
        {
            // Arrange
            var username = "juan";
            _mockRepo.Setup(repo => repo.GetMemberAsync(username)).ReturnsAsync((MemberResponse)null);

            // Act
            var result = await _controller.GetByUsernameAsync(username);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByUsernameAsync_ReturnsOkResult_WithMember()
        {
            // Arrange
            var username = "calamardo";
            var expectedMember = new MemberResponse { UserName = username };

            _mockRepo.Setup(repo => repo.GetMemberAsync(username))
                 .ReturnsAsync(expectedMember);

            // Act
            var result = await _controller.GetByUsernameAsync(username);

            // Assert
            var okResult = Assert.IsType<ActionResult<MemberResponse>>(result);
            Assert.Equal(expectedMember, okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenUsernameIsNullInToken()
        {
            // Arrange
            var updateRequest = new MemberUpdateRequest { City = "New York" };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            // Act
            var result = await _controller.UpdateUser(updateRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No username found in token", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "juan";
            _mockRepo.Setup(repo => repo.GetMemberAsync(username)).ReturnsAsync((MemberResponse)null);

            // Act
            var result = await _controller.GetByUsernameAsync(username);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task UpdateUser_Should_Return_NoContent_When_Update_Succeeds()
        {
            // Arrange
            var request = new MemberUpdateRequest
            {
                City = "New York"
            };
            var username = "arenita";
            var user = new AppUser { 
                UserName = username, 
                KnownAs = "Arenita",
                Gender = "female",
                City = "Greenbush",
                Country = "Martinique"
                };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, username)
                    }))
                }
            };

            _mockRepo.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.SaveAllAsync()).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(repo => repo.Update(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_Should_Return_BadRequest_When_User_Not_Found()
        {
            // Arrange
            var request = new MemberUpdateRequest();
            var username = "unknownUser";

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, username)
                    }))
                }
            };

            _mockRepo.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.UpdateUser(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Could not find user", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_Should_Return_BadRequest_When_Save_Fails()
        {
            // Arrange
            var request = new MemberUpdateRequest();
            var username = "arenita";
            var user = new AppUser { 
                UserName = username, 
                KnownAs = "Arenita",
                Gender = "female",
                City = "Greenbush",
                Country = "Martinique"
                };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, username)
                    }))
                }
            };

            _mockRepo.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.SaveAllAsync()).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Update user failed!", badRequestResult.Value);
        }
    }
}
