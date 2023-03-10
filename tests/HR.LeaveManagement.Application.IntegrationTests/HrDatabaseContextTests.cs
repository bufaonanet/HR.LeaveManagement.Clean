using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HR.LeaveManagement.Application.IntegrationTests;

public class HrDatabaseContextTests
{
    private readonly HrDatabaseContext _hrDatabaseContext;
    private readonly IUserService _userService;

    public HrDatabaseContextTests(IUserService userService)
    {
        var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
            .UseInMemoryDatabase("MyInMemoryDb").Options;

        _userService = userService;
        _hrDatabaseContext = new HrDatabaseContext(dbOptions, _userService);
    }
    [Fact]
    public async void Save_SetDataCreatedValue()
    {
        //Arrage
        var leaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };

        //Act
        await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        //Assert
        leaveType.DateCreated.ShouldNotBeNull();
    }

    [Fact]
    public async void Save_SetDataModifiedValue()
    {
        //Arrage
        var leaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };

        //Act
        await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        //Assert
        leaveType.DateModified.ShouldNotBeNull();
    }
}