using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypesDetails;

public class GetLeaveTypesDetailsHandler : IRequestHandler<GetLeaveTypesDetailsQuery, LeaveTypeDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<GetLeaveTypesDetailsHandler> _logger;

    public GetLeaveTypesDetailsHandler(
        IMapper mapper, 
        ILeaveTypeRepository leaveTypeRepository, 
        IAppLogger<GetLeaveTypesDetailsHandler> logger)
    {
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
    }

    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypesDetailsQuery request, CancellationToken cancellationToken)
    {
        //Query Database
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);

        //verify that record exists
        if (leaveType == null)
            throw new NotFoundException(nameof(leaveType), request.Id);

        //convert to DTO object
        var data = _mapper.Map<LeaveTypeDetailsDto>(leaveType);

        //retur DTO object
        _logger.LogInformation("Leave types were retrived successfully");
        return data;
    }
}
