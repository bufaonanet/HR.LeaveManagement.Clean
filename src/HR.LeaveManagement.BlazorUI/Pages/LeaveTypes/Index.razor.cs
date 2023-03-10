using Blazored.Toast.Services;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveTypes;

public partial class Index
{

    [Inject]
    public ILeaveTypeService LeaveTypeService { get; set; }

    [Inject]
    public ILeaveAllocationService LeaveAllocationService{ get; set; }

    [Inject]
    IToastService toastService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<LeaveTypeVM> LeaveTypes { get; private set; }
    public string Message { get; set; } = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        LeaveTypes = await LeaveTypeService.GetLeaveTypes();
    }

    protected void DetailsLeaveType(int id)
    {
        NavigationManager.NavigateTo($"leavetypes/details/{id}");
    }

    protected void CreateLeaveType()
    {
        NavigationManager.NavigateTo("leavetypes/create");
    }

    protected void EditLeaveType(int id)
    {
        NavigationManager.NavigateTo($"leavetypes/edit/{id}");
    }

    protected async Task DeleteLeaveType(int id)
    {
        var response = await LeaveTypeService.DeleteLeaveType(id);
        if (response.Success)
        {
            toastService.ShowSuccess("Leave Type deleted sucessfully.");
            //StateHasChanged();
            await OnInitializedAsync();
        }
        else
        {
            Message = response.Message;
        }
    }

    protected void AllocateLeaveType(int id)
    {
        //TODO: use leabe allocation services here
        LeaveAllocationService.CreateLeaveAllocations(id);
    }
}