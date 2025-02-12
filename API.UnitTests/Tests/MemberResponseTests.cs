using System;
using System.Collections.Generic;
using API.DTOs;
using Xunit;

namespace API.UnitTests.Tests;
    public class MemberResponseTests
    {
        [Fact]
        public void MemberResponse_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = 1;
            var userName = "arenita";
            var age = 68;
            var photoUrl = "https://example.com/photo.jpg";
            var knownAs = "Arenita";
            var created = DateTime.Now.AddYears(-2);
            var lastActive = DateTime.Now;
            var gender = "Female";
            var introduction = "Sunt esse aliqua ullamco in incididunt consequat commodo. Nisi ad esse elit ipsum commodo fugiat est ad. Incididunt nostrud incididunt nostrud sit excepteur occaecat.";
            var interests = "Sit sit incididunt proident velit.";
            var lookingFor = "Dolor anim cupidatat occaecat aliquip et Lorem ut elit fugiat. Mollit eu pariatur est sunt. Minim fugiat sit do dolore eu elit ex do id sunt. Qui fugiat nostrud occaecat nisi est dolor qui fugiat laborum cillum. Occaecat consequat ex mollit commodo ad irure cillum nulla velit ex pariatur veniam cupidatat. Officia veniam officia non deserunt mollit.";
            var city = "Greenbush";
            var country = "Martinique";
            var photos = new List<PhotoResponse>
            {
                new PhotoResponse { Id = 1, Url = "https://randomuser.me/api/portraits/women/54.jpg", IsMain = true },
                new PhotoResponse { Id = 2, Url = "https://randomuser.me/api/portraits/women/50.jpg", IsMain = false }
            };

            // Act
            var member = new MemberResponse
            {
                Id = id,
                UserName = userName,
                Age = age,
                PhotoUrl = photoUrl,
                KnownAs = knownAs,
                Created = created,
                LastActive = lastActive,
                Gender = gender,
                Introduction = introduction,
                Interests = interests,
                LookingFor = lookingFor,
                City = city,
                Country = country,
                Photos = photos
            };

            // Assert
            Assert.Equal(id, member.Id);
            Assert.Equal(userName, member.UserName);
            Assert.Equal(age, member.Age);
            Assert.Equal(photoUrl, member.PhotoUrl);
            Assert.Equal(knownAs, member.KnownAs);
            Assert.Equal(created, member.Created);
            Assert.Equal(lastActive, member.LastActive);
            Assert.Equal(gender, member.Gender);
            Assert.Equal(introduction, member.Introduction);
            Assert.Equal(interests, member.Interests);
            Assert.Equal(lookingFor, member.LookingFor);
            Assert.Equal(city, member.City);
            Assert.Equal(country, member.Country);
            Assert.Equal(photos, member.Photos);
        }
    }
