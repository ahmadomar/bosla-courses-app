using CoursesApp.Data;

namespace CoursesApp.Services
{
    public interface ITraineeService
    {
        Trainee Create(Trainee trainee);
    }
    public class TraineeService : ITraineeService
    {
        private readonly Courses_DBEntity courses_DBEntity;
        public TraineeService()
        {
            courses_DBEntity = new Courses_DBEntity();
        }

        public Trainee Create(Trainee trainee)
        {
            courses_DBEntity.Trainees.Add(trainee);

            int savingResult = courses_DBEntity.SaveChanges();
            if(savingResult > 0)
            {
                return trainee;
            }
            return null;
        }
    }
}