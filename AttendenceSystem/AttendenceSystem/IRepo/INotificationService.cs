using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface INotificationService
    {
        Task CheckPendingStudentsAsync();
        Task<int> GetPendingStudentCountAsync();
    }
}
