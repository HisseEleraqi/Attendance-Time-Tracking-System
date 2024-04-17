using AttendenceSystem.IRepo;

namespace AttendenceSystem.Repo
{
    public class NotificationService : INotificationService
    {
        private readonly IStudentRepo studentRepo;
        private int _pendingStudentCount;

        public NotificationService(IStudentRepo _studentRepo)
        {
            studentRepo = _studentRepo;
            _pendingStudentCount = 0;
        }

        public async Task CheckPendingStudentsAsync()
        {
            var pendingStudents = await studentRepo.GetPendingStudentsAsync();
            _pendingStudentCount = pendingStudents.Count();
            // Trigger event or update UI to notify admin about pending students
        }

        public async Task<int> GetPendingStudentCountAsync()
        {
            var pendingStudents = await studentRepo.GetPendingStudentsAsync();
            _pendingStudentCount = pendingStudents.Count();
            return _pendingStudentCount;
        }
    }

}
