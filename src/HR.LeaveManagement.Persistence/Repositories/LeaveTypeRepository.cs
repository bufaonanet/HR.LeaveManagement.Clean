using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(HrDatabaseContext context)
        : base(context) { }

    public async Task<bool> IsLeaveTypeNameUnique(string name)
    {
        return await _context.LeaveTypes.Where(t => t.Name == name).CountAsync() == 1;
    }

    public async Task<bool> IsLeaveTypeNotExistWithThisName(string name)
    {
        return await _context.LeaveTypes.AnyAsync(q => q.Name == name) == false;
    }
}