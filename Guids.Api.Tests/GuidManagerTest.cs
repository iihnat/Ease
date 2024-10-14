using Guids.Api.Managers;
using Guids.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Guids.Api.Repository;
using Guids.Api.Models.Dtos;
using Microsoft.Extensions.Configuration;

namespace Guids.Api.Tests
{
    public class GuidManagerTest
    {
        private readonly Mock<IGuidMetadataRepository> _guidRepositoryMock;
        private readonly IGuidManager _guidManager;
        private readonly Mock<IDistributedCache> _cacheMock;

        public GuidManagerTest()
        {
            _guidRepositoryMock = new Mock<IGuidMetadataRepository>();
            _cacheMock = new Mock<IDistributedCache>();
            var configurationMock = new Mock<IConfiguration>();
            _guidManager = new GuidManager(configurationMock.Object, _guidRepositoryMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task CreateGuid_ShouldReturnNewGuid()
        {
            // Arrange
            var user = "test_user";
            var guidMetadata = new GuidMetadata
            {
                Guid = Guid.NewGuid().ToString().ToUpper().Replace("-", ""),
                User = user,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _guidRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<GuidMetadata>())).Returns(Task.CompletedTask);

            // Act
            var createGuidRq = new CreateGuidRq
            {
                User = guidMetadata.User,
                Expires = DateTime.UtcNow.AddDays(30),
            };
            await _guidManager.CreateGuid(createGuidRq);

            // Assert
            _guidRepositoryMock.Verify(repo => repo.AddAsync(It.Is<GuidMetadata>(g => g.User == user)), Times.Once);
        }

        [Fact]
        public async Task GetGuidByGuidId_ShouldReturnGuid()
        {
            // Arrange
            var guidMetadata = new GuidMetadata 
            { 
                Guid = "020E12A4D9D54157ABE31103A7689499",
                User = "user1",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            _guidRepositoryMock.Setup(repo => repo.GetByGuidIdAsync(It.IsAny<string>())).ReturnsAsync(guidMetadata);
            _cacheMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), default)).ReturnsAsync((byte[])null);

            // Act
            var result = await _guidManager.GetById(guidMetadata.Guid);

            // Assert
            Assert.Equal("020E12A4D9D54157ABE31103A7689499", result.Value.Guid);
            Assert.Equal("user1", result.Value.User);
        }

        [Fact]
        public async Task UpdateGuid_ShouldUpdateMetadata()
        {
            // Arrange
            var time = DateTime.UtcNow;
            var guidMetadata = new GuidMetadata 
            { 
                Guid = "020E12A4D9D54157ABE31103A7689499",
                User = "user1" 
            };

            _guidRepositoryMock.Setup(repo => repo.GetByGuidIdAsync(It.IsAny<string>())).ReturnsAsync(guidMetadata);

            // Act
            var updateGuidMetadataRq = new UpdateGuidMetadataRq
            {
                User = guidMetadata.User,
                Expires = time
            };

            var result = await _guidManager.UpdateGuidMetadata(guidMetadata.Guid, updateGuidMetadataRq);

            // Assert
            Assert.Equal("020E12A4D9D54157ABE31103A7689499", result.Value.Guid);
            Assert.Equal("user1", result.Value.User);
            Assert.Equal(time, result.Value.Expires);
        }

        [Fact]
        public async Task DeleteGuid_ShouldDeleteGuid()
        {
            // Arrange
            var guidId = "020E12A4D9D54157ABE31103A7689499";
            var guidMetadata = new GuidMetadata 
            { 
                Guid = "020E12A4D9D54157ABE31103A7689499",
                User = "user1",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            _guidRepositoryMock.Setup(repo => repo.DeleteAsync(guidId)).Returns(Task.CompletedTask);
            _guidRepositoryMock.Setup(repo => repo.GetByGuidIdAsync(It.IsAny<string>())).ReturnsAsync(guidMetadata);

            // Act
            await _guidManager.Delete(guidId);

            // Assert
            _guidRepositoryMock.Verify(repo => repo.DeleteAsync(guidId), Times.Once());
        }
    }
}
