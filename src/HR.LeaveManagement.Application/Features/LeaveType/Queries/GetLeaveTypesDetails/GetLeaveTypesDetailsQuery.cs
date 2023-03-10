using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypesDetails;

public record GetLeaveTypesDetailsQuery(int Id) : IRequest<LeaveTypeDetailsDto>;