using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services;

public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
{
    public LeaveAllocationService(
        IClient client,
        ILocalStorageService localStorageService)
        : base(client, localStorageService) { }

    public async Task<Response<Guid>> CreateLeaveAllocations(int leaveTypeId)
    {
        try
        {
            var response = new Response<Guid>();
            var createLeaveAllocationCommand = new CreateLeaveAllocationCommand { LeaveTypeId = leaveTypeId };

            await _client.LeaveAllocationsPOSTAsync(createLeaveAllocationCommand);
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }
}
