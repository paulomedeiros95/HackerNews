using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Santander.HackerNews.Application.Services;
using Santander.HackerNews.Domain.Dtos;

namespace Santander.HackerNews.Tests;

public class StoryServiceTests
{
    [Fact]
    public async Task GetBestStoriesAsync_ShouldReturnRequestedNumberOfStories()
    {
        // Arrange
        var cacheMock = new MemoryCache(new MemoryCacheOptions());
        var mockStories = new List<StoryDto>
        {
            new("Story 1", "https://example.com/1", "author1", "2023-10-12T13:43:01+00:00", 100, 10),
            new("Story 2", "https://example.com/2", "author2", "2023-10-12T14:43:01+00:00", 90, 20),
            new("Story 3", "https://example.com/3", "author3", "2023-10-12T15:43:01+00:00", 80, 30)
        };

        cacheMock.Set("BestStoriesCache", mockStories);
        var service = new StoryService(cacheMock);

        // Act
        var result = await service.GetBestStoriesAsync(2, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().Title.Should().Be("Story 1");
    }

    [Fact]
    public async Task GetBestStoriesAsync_ShouldReturnEmpty_WhenNIsZeroOrNegative()
    {
        // Arrange
        var cacheMock = new MemoryCache(new MemoryCacheOptions());
        var service = new StoryService(cacheMock);

        // Act
        var result = await service.GetBestStoriesAsync(0, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}