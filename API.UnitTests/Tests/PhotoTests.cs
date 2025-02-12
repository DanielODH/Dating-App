using System;
using Xunit;
using API.DataEntities;

namespace API.UnitTests.Tests
{
    public class PhotoTests
    {
        [Fact]
        public void Should_Allow_Optional_PublicId()
        {
            // Arrange
            var photo = new Photo
            {
                Id = 1,
                Url = "http://example.com/photo1.jpg",
                IsMain = false,
                AppUserId = 2,
                AppUser = new AppUser { 
                    Id = 1, 
                    UserName = "arenita",
                    KnownAs = "Arenita",
                    Gender = "female",
                    City = "Aguascalientes",
                    Country = "Mexico"
                    }
            };

            // Assert
            Assert.Equal(1, photo.Id);
            Assert.Equal("http://example.com/photo1.jpg", photo.Url);
            Assert.False(photo.IsMain);
            Assert.Null(photo.PublicId);  // PublicId is optional
            Assert.Equal(2, photo.AppUserId);
            Assert.NotNull(photo.AppUser);
            Assert.Equal("arenita", photo.AppUser.UserName);
        }
    }
}
