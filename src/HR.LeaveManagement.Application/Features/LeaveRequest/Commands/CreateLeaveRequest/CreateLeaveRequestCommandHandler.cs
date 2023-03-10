using AutoMapper;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
{
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IUserService _userService;

    public CreateLeaveRequestCommandHandler(
        IEmailService emailService,
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository,
        ILeaveRequestRepository leaveRequestRepository,
        IUserService userService,
        ILeaveAllocationRepository leaveAllocationRepository)
    {
        _emailService = emailService;
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _userService = userService;
        _leaveAllocationRepository = leaveAllocationRepository;
    }

    public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid Leave Request", validationResult);

        // Get requesting employee's id
        var employeeId = _userService.UserId;

        // Check on employee's allocation
        var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

        // if allocations aren't enough, return validation error with message
        if(allocation is null)
        {
            validationResult.Errors.Add(new ValidationFailure(
                nameof(request.LeaveTypeId),"You do not have any allocations for this leave type."));
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        int dayRequested = (int)(request.EndDate - request.StartDate).TotalDays;
        if(dayRequested > allocation.NumberOfDays)
        {
            validationResult.Errors.Add(new ValidationFailure(
                nameof(request.LeaveTypeId), "You do not have enough days for this request."));
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        // Create leave request
        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        
        leaveRequest.RequestingEmployeeId = employeeId;
        leaveRequest.DateRequested = DateTime.Now;

        await _leaveRequestRepository.CreateAsync(leaveRequest);

        try
        {
            // send confirmation email
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                        $"has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            //await _emailService.SendEmail(email);
        }
        catch (Exception)
        {
            /// Log or handler error
        }

        return Unit.Value;
    }
}