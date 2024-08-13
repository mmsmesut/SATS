using Microsoft.EntityFrameworkCore;
using SATS.Data.Entities;

namespace SATS.Data.Extensions
{
    public static class ModelBuilderSeedDataCreaterExtensions
    {
        public static void CreateSeedData(this ModelBuilder modelBuilder)
        {
            var dummyStudents = CreateDummyStudentData();
            modelBuilder.Entity<Student>().HasData(dummyStudents);

            var dummyCourses = CreateDummyCourseData();
            modelBuilder.Entity<Course>().HasData(dummyCourses);

            var dummyStudentCourses = CreateDummyStudentCourseData(dummyStudents, dummyCourses);
            modelBuilder.Entity<StudentCourse>().HasData(dummyStudentCourses);

            var dummyAttendances = CreateDummyAttendanceData(dummyStudentCourses);
            modelBuilder.Entity<Attendance>().HasData(dummyAttendances);
        }

        private static List<Student> CreateDummyStudentData()
        {
            var students = new List<Student>();
            var random = new Random();
            var names = GetNames();
            var lastNames = GetSurnames();
            var cities = GetCities();

            var name = "";
            var lastName = "";
            var city = "";

            for (int i = 1; i <= 50; i++)
            {
                name = names[random.Next(students.Count)];
                lastName = lastNames[random.Next(students.Count)];
                students.Add(new Student
                {
                    StudentId = i,
                    FirstName = name,
                    LastName = lastName,
                    BirthDate = DateTime.UtcNow.AddDays(-i * 365), // yaklaşık doğum tarihi
                    Email = $"{name}{lastName}@example.com",
                    City = city
                });
            }
            return students;
        }

        private static List<Course> CreateDummyCourseData()
        {
            var courses = new List<Course>();
            for (int i = 1; i <= 2; i++)
            {
                courses.Add(new Course
                {
                    CourseId = i,
                    CourseName = $"Ozz Akademi{i}",
                    CourseDescription = $"Description for Course{i}"
                });
            }
            return courses;
        }

        private static List<StudentCourse> CreateDummyStudentCourseData(List<Student> students, List<Course> courses)
        {
            var studentCourses = new List<StudentCourse>();
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var studentId = students[random.Next(students.Count)].StudentId;
                var courseId = courses[random.Next(courses.Count)].CourseId;

                studentCourses.Add(new StudentCourse
                {
                    StudentCourseId = i + 1,
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-random.Next(30))
                });
            }

            return studentCourses;
        }

        private static List<Attendance> CreateDummyAttendanceData(List<StudentCourse> studentCourses)
        {
            var attendanceRecords = new List<Attendance>();
            var random = new Random();

            foreach (var studentCourse in studentCourses)
            {
                for (int i = 1; i <= 100; i++) // 10 attendance records per studentCourse
                {
                    attendanceRecords.Add(new Attendance
                    {
                        AttendanceId = attendanceRecords.Count + 1,
                        StudentCourseId = studentCourse.StudentCourseId,
                        AttendanceDate = DateTime.UtcNow.AddDays(-i),
                        Status = random.Next(2) == 0 ? "Present" : "Absent"
                    });
                }
            }

            return attendanceRecords;
        }


        static string[] GetNames()
        {
            string[] turkishNames = new string[]
            {
            "İsmail", "Muhammed", "Hlid", "Elif", "Abbas", "Zeynep", "Hasan", "Fatma", "İsmail", "Emine",
            "Mustafa", "Hatice", "Murat", "Aylin", "Orhan", "Sevgi", "Kemal", "Özlem", "Can", "Nur",
            "Haluk", "Sibel", "Hakan", "Derya", "Ömer", "Meryem", "Burak", "Selin", "Serkan", "Aysel",
            "Gül", "Tolga", "Büşra", "Yasin", "İpek", "Arda", "Cengiz", "Hülya", "Engin", "Vildan",
            "Barış", "Pinar", "Kadir", "Nazlı", "Ebru", "Emre", "Şirin", "Sinan", "Ceren", "Caner",
            "Nihan", "Yavuz", "Esra", "Alper", "Gamze", "Cem", "Özgül", "İbrahim", "Gülçin", "Volkan",
            "Ege", "Suna", "Aydın", "Gözde", "Levent", "Gülay", "Lale", "Onur", "Banu", "Serdar",
            "Melek", "Özgür", "İrem", "Halime", "Seda", "Emrah", "Gökçe", "Tarkan", "İdil", "Erdal",
            "Ahu", "Mert", "Aslı", "Yağmur", "Ekin", "Mahmut", "Şeyma", "Cemil", "Melis", "Nursel",
            "Yelda", "Cemil", "Nihal", "İlker", "Gülseren", "Melih", "Dilan", "Cansu", "Müge", "Selçuk",
            "Zeki", "Zehra", "Bahar", "Rıza", "Gülhan", "Hüseyin", "Arzu", "Özge", "Emrah", "Süleyman",
            "Oya", "Pelin", "Asuman", "Fikret", "Bahar", "Meryem", "Elvan", "Nuri", "Aysun", "Seda",
            };

            return turkishNames;
        }

        static string[] GetSurnames()
        {
            string[] turkishSurnames = new string[]
            {
            "Yılmaz", "Kaya", "Demir", "Çelik", "Yurt", "Kurt", "Arslan", "Koç", "Aydın", "Yalçın",
            "Akman", "Kara", "Duman", "Çakır", "Kara", "Gündüz", "Özkan", "Aksu", "Gül", "Çetin",
            "Erdoğan", "Çetin", "Acar", "Toprak", "Karaoğlu", "Uysal", "Kara", "Kaya", "Erdem",
            "Yavuz", "Kaya", "Ekinci", "Akkaya", "Çakmak", "Karaca", "Arı", "Özdemir", "Ege", "Bozkurt",
            "Akçay", "Görgülü", "Okan", "Ergin", "Koçak", "Yalçın", "Gündoğdu", "Kara", "Demirtaş", "Çelik",
            "Akgün", "Demir", "Bozdağ", "Özkan", "Yılmaz", "Kaya", "Yurt", "Karahan", "Balcı", "Oğuz",
            "Aydın", "Özkan", "Arslan", "Görkem", "Balkan", "Acar", "Yüksek", "Çoruh", "Çetin", "Gül",
            "Araz", "Arslan", "Sönmez", "Çelik", "Kısa", "Duru", "Aydemir", "Ceylan", "Kara", "Öz"
            };

            return turkishSurnames;
        }

        static string[] GetCities()
        {
            string[] turkishCities = new string[]
            {
            "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın",
            "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa",
            "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan", "Erzurum",
            "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Iğdır", "Isparta", "İstanbul", "İzmir",
            "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli", "Kırşehir", "Kilis",
            "Konya", "Kütahya", "Malatya", "Manisa", "Mardin", "Mersin", "Muğla", "Muş", "Nevşehir", "Niğde",
            "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Şanlıurfa", "Şırnak",
            "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak", "Aksaray",
            "Bayburt", "Karabük", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karaman", "Kırıkkale", "Osmaniye", "Düzce"
            };

            return turkishCities;
        }
    }
}
