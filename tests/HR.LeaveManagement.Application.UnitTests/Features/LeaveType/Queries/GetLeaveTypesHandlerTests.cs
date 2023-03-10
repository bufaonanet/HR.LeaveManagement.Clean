using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveType.Queries;

public class GetLeaveTypesHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILeaveTypeRepository> _mockRepo;
    private readonly Mock<IAppLogger<GetLeaveTypesHandler>> _mockLogger;

    public GetLeaveTypesHandlerTests()
    {
        _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<LeaveTypeProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockLogger = new Mock<IAppLogger<GetLeaveTypesHandler>>();
    }

    [Fact]
    public async Task GetLeaveTypeListTest()
    {
        //Arragne
        var handler = new GetLeaveTypesHandler(_mapper,
            _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);

        //Assert
        result.ShouldBeOfType<List<LeaveTypeDto>>();
        result.Count.ShouldBe(3);
    }
}
