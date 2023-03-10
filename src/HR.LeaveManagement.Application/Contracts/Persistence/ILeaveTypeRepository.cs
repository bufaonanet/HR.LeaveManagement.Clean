using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence;

public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
{
    Task<bool> IsLeaveTypeNameUnique(string name);
    Task<bool> IsLeaveTypeNotExistWithThisName(string name);
}
