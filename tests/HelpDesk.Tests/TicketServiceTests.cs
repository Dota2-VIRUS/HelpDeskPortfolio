using HelpDesk.Api.Data;
using HelpDesk.Api.Domain;
using HelpDesk.Api.Dtos;
using HelpDesk.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HelpDesk.Tests;

public class TicketServiceTests
{
    private static TicketService CreateService()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TicketService(new AppDbContext(options));
    }

    private static CreateTicketDto NewDto(string title = "Impressora sem toner") => new()
    {
        Title = title,
        Description = "A impressora do 2º andar parou de imprimir.",
        Priority = TicketPriority.High
    };

    [Fact]
    public async Task Create_SetsOpenStatusAndCreatedAt()
    {
        var service = CreateService();

        var created = await service.CreateAsync(NewDto());

        Assert.True(created.Id > 0);
        Assert.Equal(TicketStatus.Open, created.Status);
        Assert.NotEqual(default, created.CreatedAt);
        Assert.Null(created.UpdatedAt);
    }

    [Fact]
    public async Task GetAll_FiltersByStatus()
    {
        var service = CreateService();
        var a = await service.CreateAsync(NewDto("Chamado A"));
        await service.CreateAsync(NewDto("Chamado B"));

        await service.UpdateAsync(a.Id, new UpdateTicketDto
        {
            Title = a.Title,
            Description = a.Description,
            Priority = a.Priority,
            Status = TicketStatus.Resolved
        });

        var resolved = await service.GetAllAsync(TicketStatus.Resolved, null);
        var open = await service.GetAllAsync(TicketStatus.Open, null);

        Assert.Single(resolved);
        Assert.Single(open);
        Assert.Equal("Chamado A", resolved[0].Title);
    }

    [Fact]
    public async Task GetAll_SearchMatchesTitle()
    {
        var service = CreateService();
        await service.CreateAsync(NewDto("VPN não conecta"));
        await service.CreateAsync(NewDto("Impressora offline"));

        var result = await service.GetAllAsync(null, "VPN");

        Assert.Single(result);
        Assert.Equal("VPN não conecta", result[0].Title);
    }

    [Fact]
    public async Task Update_ReturnsNull_WhenTicketDoesNotExist()
    {
        var service = CreateService();

        var result = await service.UpdateAsync(999, new UpdateTicketDto
        {
            Title = "x y z",
            Description = "desc"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_RemovesTicket()
    {
        var service = CreateService();
        var created = await service.CreateAsync(NewDto());

        Assert.True(await service.DeleteAsync(created.Id));
        Assert.Null(await service.GetByIdAsync(created.Id));
        Assert.False(await service.DeleteAsync(created.Id));
    }
}
