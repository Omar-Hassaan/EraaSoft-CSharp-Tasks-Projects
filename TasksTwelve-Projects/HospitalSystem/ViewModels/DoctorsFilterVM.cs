namespace HospitalSystem.ViewModels
{
    public class DoctorsFilterVM
    {
        public List<Doctor> Doctors { get; set; } = new();

        public string? SearchName { get; set; }
        public string? SearchSpecialization { get; set; }

        public List<string> Specializations { get; set; } = new();

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}