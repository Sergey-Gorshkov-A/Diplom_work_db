using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diplom
{
    public static class DbInitializer
    {
        private static Random random = new Random();

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ServerDbContext>();

            context.Database.EnsureCreated();

            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                context.Roles.Add(new Role { Name = "admin" });
                context.SaveChanges();
            }
            
            if (!context.Users.Any(u => u.Login == "admin"))
            {
                var adminRole = context.Roles.First(r => r.Name == "admin");
                context.Users.Add(new User { Login = "admin", Password = "pascal", Role = adminRole });
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "student" },
                    new Role { Name = "parent" },
                    new Role { Name = "teacher" }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var studentRole = context.Roles.First(r => r.Name == "student");
                var parentRole = context.Roles.First(r => r.Name == "parent");
                var teacherRole = context.Roles.First(r => r.Name == "teacher");

                context.Users.AddRange(
                    new User { Login = "student", Password = "123", Role = studentRole },
                    new User { Login = "parent", Password = "123", Role = parentRole },
                    new User { Login = "teacher", Password = "123", Role = teacherRole }
                );
                context.SaveChanges();
            }

            var defaultStudentUser = context.Users.First(u => u.Login == "student");
            var defaultParentUser = context.Users.First(u => u.Login == "parent");
            var defaultTeacherUser = context.Users.First(u => u.Login == "teacher");

            if (!context.Balances.Any())
            {
                context.Balances.AddRange(
                    new Balance { Money = 0 },
                    new Balance { Money = 0 },
                    new Balance { Money = 0 } 
                );
                context.SaveChanges();
            }

            var teacherBalance = context.Balances.First();
            var parentBalance = context.Balances.Skip(1).First();

            if (!context.Teachers.Any())
            {
                context.Teachers.Add(new Teacher
                {
                    UserId = defaultTeacherUser.Id,
                    Fio = "Учитель Учитель Учителевич",
                    BalanceId = teacherBalance.Id
                });
                context.SaveChanges();
            }

            if (!context.Parents.Any())
            {
                context.Parents.Add(new Parent
                {
                    UserId = defaultParentUser.Id,
                    Fio = "Родитель Родитель Родителевич",
                    BalanceId = parentBalance.Id
                });
                context.SaveChanges();
            }
            var defaultParent = context.Parents.First();

            if (!context.Groups.Any())
            {
                context.Groups.AddRange(
                    new Group { Name = "61" },
                    new Group { Name = "62" },
                    new Group { Name = "63" },
                    new Group { Name = "64" }
                );
                context.SaveChanges();
            }

            if (!context.Subjects.Any())
            {
                context.Subjects.AddRange(
                    new Subject { Name = "Математика", Price = 100 },
                    new Subject { Name = "Физика", Price = 80 },
                    new Subject { Name = "Информатика", Price = 120 }
                );
                context.SaveChanges();
            }

            if (!context.Students.Any())
            {
                var groups = context.Groups.ToList();
                foreach (var group in groups)
                {
                    int studentCount = random.Next(24, 31);
                    for (int i = 1; i <= studentCount; i++)
                    {
                        context.Students.Add(new Student
                        {
                            Fio = $"Ученик {i} группы {group.Name}",
                            GroupId = group.Id,
                            UserId = defaultStudentUser.Id,
                            ParentId = defaultParent.Id
                        });
                    }
                }
                context.SaveChanges();
            }

            if (!context.Days.Any())
            {
                for (int day = 1; day <= 30; day++)
                {
                    context.Days.Add(new Day
                    {
                        Year = "2025",
                        Month = "04",
                        DayNumber = day.ToString("D2")
                    });
                }
                context.SaveChanges();
            }

            if (!context.Logbooks.Any())
            {
                var groups = context.Groups.ToList();
                var subjects = context.Subjects.ToList();
                foreach (var group in groups)
                {
                    foreach (var subject in subjects)
                    {
                        context.Logbooks.Add(new Logbook
                        {
                            GroupId = group.Id,
                            SubjectId = subject.Id
                        });
                    }
                }
                context.SaveChanges();
            }

            if (!context.Lessons.Any())
            {
                var days = context.Days.ToList();
                var subjects = context.Subjects.ToList();
                foreach (var subject in subjects)
                {
                    foreach (var day in days)
                    {
                        context.Lessons.Add(new Lesson
                        {
                            SubjectId = subject.Id,
                            DayId = day.Id
                        });
                    }
                }
                context.SaveChanges();
            }

            var students = context.Students.ToList();
            var lessons = context.Lessons.Include(l => l.Subject).ToList();
            var logbooks = context.Logbooks.ToList();

            var logbookDict = logbooks.ToDictionary(l => (l.GroupId, l.SubjectId), l => l.Id);

            if (!context.Events.Any())
            {
                foreach (var student in students)
                {
                    foreach (var lesson in lessons)
                    {
                        int groupId = student.GroupId;
                        int subjectId = lesson.SubjectId;
                        if (!logbookDict.ContainsKey((groupId, subjectId)))
                            continue;

                        double r = random.NextDouble();
                        string presence;
                        if (r < 0.6) presence = "+";
                        else if (r < 0.8) presence = "Н";
                        else presence = "П";

                        context.Events.Add(new Event
                        {
                            StudentId = student.Id,
                            LessonId = lesson.Id,
                            Presence = presence
                        });
                    }
                }
                context.SaveChanges();
            }

            if (!context.Marks.Any())
            {
                foreach (var student in students)
                {
                    foreach (var lesson in lessons)
                    {
                        int groupId = student.GroupId;
                        int subjectId = lesson.SubjectId;
                        if (!logbookDict.TryGetValue((groupId, subjectId), out int logbookId))
                            continue;

                        var ev = context.Events.FirstOrDefault(e => e.StudentId == student.Id && e.LessonId == lesson.Id);
                        if (ev != null && ev.Presence == "+")
                        {
                            int markValue = random.Next(2, 6);
                            context.Marks.Add(new Mark
                            {
                                StudentId = student.Id,
                                LessonId = lesson.Id,
                                LogbookId = logbookId,
                                MarkValue = markValue
                            });
                        }
                    }
                }
                context.SaveChanges();
            }

            if (!context.Profiles.Any())
            {
                var allStudents = context.Students.ToList();
                foreach (var student in allStudents)
                {
                    context.Profiles.Add(new Profile
                    {
                        StudentId = student.Id,
                        Surname = $"Фамилия_{student.Id}",
                        FirstName = $"Имя_{student.Id}",
                        Patronymic = $"Отчество_{student.Id}",
                        Phone = $"+7(999)000-{student.Id:D3}",
                        Address = $"г. Город, ул. Улица, д. {student.Id}",
                        PersonalInfo = "Тестовые данные"
                    });
                }
                context.SaveChanges();
            }

            if (!context.Reviews.Any())
            {
                var teacherUser = context.Users.First(u => u.Login == "teacher");
                var allStudents = context.Students.ToList();
                string[] positiveTexts = { "Отлично учится", "Хорошие успехи", "Старательный ученик", "Проявляет интерес" };
                string[] negativeTexts = { "Мог бы лучше", "Пропускает занятия", "Не сдаёт домашние задания", "Требует внимания" };

                foreach (var student in allStudents)
                {
                    int reviewCount = random.Next(2, 5);
                    for (int i = 0; i < reviewCount; i++)
                    {
                        bool isPositive = random.NextDouble() < 0.7;
                        string text = isPositive
                            ? positiveTexts[random.Next(positiveTexts.Length)]
                            : negativeTexts[random.Next(negativeTexts.Length)];
                        context.Reviews.Add(new Review
                        {
                            Date = DateTime.Now.AddDays(-random.Next(1, 60)),
                            Text = text,
                            Points = isPositive ? 1 : -1,
                            UserId = teacherUser.Id,
                            StudentId = student.Id
                        });
                    }
                }
                context.SaveChanges();
            }
        }
    }
}