using System;
using System.Collections.Generic;
using System.Linq;
using API.DataEntities;
using API.DTOs;
using API.Helpers;
using API.Extensions;
using AutoMapper;
using Xunit;

namespace API.UnitTests.Tests;
    public class AutoMapperProfilesTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfilesTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_AppUser_To_MemberResponse()
        {
            // Arrange
            var user = new AppUser
            {
                Id = 1,
                UserName = "arenita",
                BirthDay = new DateOnly(1990, 1, 1),
                KnownAs = "Terminator",
                Gender = "female",
                City = "Aguascalientes",
                Country = "Mexico",
                Photos = new List<Photo>
                {
                    new Photo { Id = 1, Url = "http://example.com/photo1.jpg", IsMain = true },
                    new Photo { Id = 2, Url = "http://example.com/photo2.jpg", IsMain = false }
                }
            };

            // Act
            var result = _mapper.Map<MemberResponse>(user);

            // Assert
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.BirthDay.CalculateAge(), result.Age);
            Assert.Equal(user.KnownAs, result.KnownAs);
            Assert.Equal(user.Gender, result.Gender);
            Assert.Equal(user.City, result.City);
            Assert.Equal(user.Country, result.Country);
            Assert.Equal(user.Photos.FirstOrDefault(p => p.IsMain)?.Url, result.PhotoUrl);
        }

        [Fact]
        public void Should_Map_Photo_To_PhotoResponse()
        {
            // Arrange
            var photo = new Photo
            {
                Id = 1,
                Url = "http://example.com/photo.jpg",
                IsMain = true
            };

            // Act
            var result = _mapper.Map<PhotoResponse>(photo);

            // Assert
            Assert.Equal(photo.Id, result.Id);
            Assert.Equal(photo.Url, result.Url);
            Assert.Equal(photo.IsMain, result.IsMain);
        }

        [Fact]
        public void Should_Map_MemberUpdateRequest_To_AppUser()
        {
            // Arrange
            var updateRequest = new MemberUpdateRequest
            {
                Introduction = "Hello",
                LookingFor = "Friendship",
                Interests = "Reading, Sports, Gaming",
                City = "New York",
                Country = "United States"
            };

            // Act
            var result = _mapper.Map<AppUser>(updateRequest);

            // Assert
            Assert.Equal(updateRequest.Introduction, result.Introduction);
            Assert.Equal(updateRequest.LookingFor, result.LookingFor);
            Assert.Equal(updateRequest.Interests, result.Interests);
            Assert.Equal(updateRequest.City, result.City);
            Assert.Equal(updateRequest.Country, result.Country);
        }
    }
