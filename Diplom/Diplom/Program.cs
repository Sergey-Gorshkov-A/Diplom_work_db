using Diplom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


// Серверная часть кода приложения
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        var relativePath = configuration.GetConnectionString("DefaultConnectionRelative");
        var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);
        var connectionString = $"Data Source={fullPath}";
        services.AddDbContext<ServerDbContext>(options =>
            options.UseSqlite(connectionString));

    })
    .Build();

//using (var scope = host.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    DbInitializer.Initialize(services);
//}

Task.Run(() => StartTcpServer());

Console.WriteLine("Сервер запущен. Нажмите любую клавишу для выхода...");
Console.ReadKey();

async Task StartTcpServer()
{
    int port = 8888;
    TcpListener listener = new TcpListener(IPAddress.Any, port);
    listener.Start();
    Console.WriteLine($"TCP-сервер запущен на порту {port}.");

    while (true)
    {
        var client = await listener.AcceptTcpClientAsync();
        _ = Task.Run(() => HandleClientAsync(client));
    }
}

async Task HandleClientAsync(TcpClient client)
{
    try
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        {
            string requestJson = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(requestJson))
            {
                await writer.WriteLineAsync("Empty request");
                return;
            }

            dynamic request;
            try
            {
                request = JsonConvert.DeserializeObject(requestJson);
            }
            catch (JsonException ex)
            {
                await writer.WriteLineAsync($"Invalid JSON: {ex.Message}");
                return;
            }

            if (request == null)
            {
                await writer.WriteLineAsync("Failed to deserialize request");
                return;
            }

            string action = request.Action?.ToString();
            string data = request.Data?.ToString();

            if (string.IsNullOrEmpty(action))
            {
                await writer.WriteLineAsync("Missing 'Action' field");
                return;
            }

            string response = ProcessRequest(action, data ?? "");
            await writer.WriteLineAsync(response);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
    }
}

// Обработка запросов
string ProcessRequest(string action, string data)
{
    using (var scope = host.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
        switch (action)
        {
            case "Login":
                var loginReq = JsonConvert.DeserializeObject<LoginRequest>(data);
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == loginReq.Login && u.Password == loginReq.Password);
                if (user == null)
                    return JsonConvert.SerializeObject(new { Success = false, Message = "Неверный логин или пароль" });

                int userId = user.Id;
                string role = user.Role.Name;
                object extraData = null;

                if (role == "student")
                {
                    var student = db.Students.FirstOrDefault(s => s.UserId == userId);
                    extraData = new { StudentId = student?.Id };
                }
                else if (role == "parent")
                {
                    var parent = db.Parents.FirstOrDefault(p => p.UserId == userId);
                    extraData = new { ParentId = parent?.Id };
                }
                else if (role == "teacher")
                {
                    var teacher = db.Teachers.FirstOrDefault(t => t.UserId == userId);
                    extraData = new { TeacherId = teacher?.Id };
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    Role = role,
                    UserId = userId,
                    Extra = extraData
                });

            case "GetSubjects":
                var subjects = db.Subjects.Select(s => new { s.Id, s.Name }).ToList();
                return JsonConvert.SerializeObject(subjects);

            case "GetStudents":
                var allStudents = db.Students.Select(s => new { s.Id, s.Fio }).ToList();
                return JsonConvert.SerializeObject(allStudents);

            case "GetLogbooksBySubject":
                if (!int.TryParse(data, out int subjectId)) return "error";
                var logbooks = db.Logbooks
                    .Where(l => l.SubjectId == subjectId)
                    .Select(l => new { l.Id, GroupName = l.Group.Name })
                    .ToList();
                return JsonConvert.SerializeObject(logbooks);

            case "GetJournalData":
                var req = JsonConvert.DeserializeObject<JournalRequest>(data);
                int logbookId = req.LogbookId;
                var logbook = db.Logbooks
                    .Include(l => l.Subject)
                    .Include(l => l.Group)
                    .FirstOrDefault(l => l.Id == logbookId);
                if (logbook == null) return "error";

                var students = db.Students
                    .Where(s => s.GroupId == logbook.GroupId)
                    .Select(s => new { s.Id, s.Fio })
                    .ToList();

                var lessons = db.Lessons
                    .Where(l => l.SubjectId == logbook.SubjectId)
                    .Include(l => l.Day)
                    .OrderBy(l => l.Day.Year)
                    .ThenBy(l => l.Day.Month)
                    .ThenBy(l => l.Day.DayNumber)
                    .Select(l => new { l.Id, Date = $"{l.Day.Year}-{l.Day.Month}-{l.Day.DayNumber}" })
                    .ToList();

                var studentIds = students.Select(s => s.Id).ToList();
                var lessonIds = lessons.Select(l => l.Id).ToList();

                var eventsPresence = db.Events
                    .Where(e => studentIds.Contains(e.StudentId) && lessonIds.Contains(e.LessonId))
                    .ToDictionary(e => $"({e.StudentId}, {e.LessonId})", e => e.Presence);

                var marks = db.Marks
                    .Where(m => studentIds.Contains(m.StudentId) && lessonIds.Contains(m.LessonId) && m.LogbookId == logbookId)
                    .Select(m => new { Key = $"({m.StudentId}, {m.LessonId})", Value = m.MarkValue ?? 0 })
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                return JsonConvert.SerializeObject(new
                {
                    students,
                    lessons,
                    eventsPresence,
                    marks
                });

            case "GetChildren":
                int parentId = int.Parse(data);
                var children = db.Students
                    .Where(s => s.ParentId == parentId)
                    .Select(s => new { s.Id, s.Fio })
                    .ToList();
                return JsonConvert.SerializeObject(children);

            case "GetChildData":
                var childReq = JsonConvert.DeserializeObject<ChildDataRequest>(data);

                var marksAll = db.Marks
                    .Where(m => m.StudentId == childReq.StudentId)
                    .Include(m => m.Lesson.Subject)
                    .GroupBy(m => m.Lesson.Subject.Name)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        Marks = g.Select(m => m.MarkValue).OrderBy(m => m).ToList()
                    })
                    .ToList();

                var attendance = db.Events
                    .Where(e => e.StudentId == childReq.StudentId)
                    .Include(e => e.Lesson.Day)
                    .Select(e => new { Date = $"{e.Lesson.Day.Year}-{e.Lesson.Day.Month}-{e.Lesson.Day.DayNumber}", e.Presence })
                    .ToList();

                var profile = db.Profiles.FirstOrDefault(p => p.StudentId == childReq.StudentId);

                var childReviews = db.Reviews.Where(r => r.StudentId == childReq.StudentId).Select(r => new { r.Date, r.Text, r.Points }).ToList();
                return JsonConvert.SerializeObject(new { Attendance = attendance, Profile = profile, Reviews = childReviews, MarksBySubject = marksAll });

            case "GetProfile":
                int sid = int.Parse(data);
                var prof = db.Profiles.FirstOrDefault(p => p.StudentId == sid);
                return JsonConvert.SerializeObject(prof ?? new Profile { StudentId = sid });

            case "UpdateProfile":
                var profileUpdate = JsonConvert.DeserializeObject<Profile>(data);
                var existing = db.Profiles.FirstOrDefault(p => p.StudentId == profileUpdate.StudentId);
                if (existing == null)
                    db.Profiles.Add(profileUpdate);
                else
                {
                    existing.Surname = profileUpdate.Surname;
                    existing.FirstName = profileUpdate.FirstName;
                    existing.Patronymic = profileUpdate.Patronymic;
                    existing.Phone = profileUpdate.Phone;
                    existing.Address = profileUpdate.Address;
                    existing.PersonalInfo = profileUpdate.PersonalInfo;
                }
                db.SaveChanges();
                return "ok";

            case "GetStudentMarksAndRating":
                int studentIdForRating = int.Parse(data);

                var marksBySubject = db.Marks
                    .Where(m => m.StudentId == studentIdForRating)
                    .Include(m => m.Lesson.Subject)
                    .GroupBy(m => m.Lesson.Subject.Name)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        Marks = g.Select(m => m.MarkValue).OrderBy(m => m).ToList()
                    })
                    .ToList();

                double? avgSum = 0;
                foreach (var subj in marksBySubject)
                {
                    if (subj.Marks.Any())
                        avgSum += subj.Marks.Average();
                }
                int reviewsSum = db.Reviews.Where(r => r.StudentId == studentIdForRating).Sum(r => r.Points);
                int ratingPoints = (int)(10 * avgSum) + 5 * reviewsSum;

                var allRatings = db.Students.Select(s => new
                {
                    StudentId = s.Id,
                    Points = (int)(10 * db.Marks.Where(m => m.StudentId == s.Id).GroupBy(m => m.Lesson.SubjectId)
                                  .Select(g => g.Average(m => m.MarkValue)).Sum() ?? 0)
                             + 5 * db.Reviews.Where(r => r.StudentId == s.Id).Sum(r => r.Points)
                }).OrderByDescending(r => r.Points).ToList();
                int place = allRatings.FindIndex(r => r.StudentId == studentIdForRating) + 1;

                var reviews = db.Reviews
                    .Where(r => r.StudentId == studentIdForRating)
                    .Include(r => r.User)
                    .Select(r => new { r.Date, r.Text, r.Points, Author = r.User.Login })
                    .ToList();

                return JsonConvert.SerializeObject(new
                {
                    MarksBySubject = marksBySubject,
                    RatingPoints = ratingPoints,
                    RatingPlace = place,
                    Reviews = reviews
                });

            case "AddReview":
                var reviewReq = JsonConvert.DeserializeObject<AddReviewRequest>(data);
                db.Reviews.Add(new Review
                {
                    Date = reviewReq.Date,
                    Text = reviewReq.Text,
                    Points = reviewReq.IsPositive ? 1 : -1,
                    UserId = reviewReq.UserId,
                    StudentId = reviewReq.StudentId
                });
                db.SaveChanges();
                return "ok";

            case "GetTeacherRating":
                var studentsRating = db.Students.Select(s => new
                {
                    s.Fio,
                    GroupName = s.Group.Name,
                    Points = (int)(10 * db.Marks.Where(m => m.StudentId == s.Id).GroupBy(m => m.Lesson.SubjectId)
                                  .Select(g => g.Average(m => m.MarkValue)).Sum() ?? 0)
                             + 5 * db.Reviews.Where(r => r.StudentId == s.Id).Sum(r => r.Points)
                }).OrderByDescending(r => r.Points).ToList();
                return JsonConvert.SerializeObject(studentsRating);

            //case "CalculateCost":
            //    var calcReq = JsonConvert.DeserializeObject<CalculateRequest>(data);
            //    int studentIdCalc = calcReq.StudentId;
            //    int subjectIdCalc = calcReq.SubjectId;

            //    var subject = db.Subjects.FirstOrDefault(s => s.Id == subjectIdCalc);
            //    if (subject == null) return "error: subject not found";
            //    int pricePerLesson = subject.Price;

            //    var lessonsIds = db.Lessons
            //        .Where(l => l.SubjectId == subjectIdCalc)
            //        .Select(l => l.Id)
            //        .ToList();

            //    var events = db.Events
            //        .Where(e => e.StudentId == studentIdCalc && lessonsIds.Contains(e.LessonId) && (e.Presence == "+" || e.Presence == "Н"))
            //        .ToList();

            //    int totalCost = events.Count * pricePerLesson;
            //    return JsonConvert.SerializeObject(new { TotalCost = totalCost, LessonCount = events.Count });

            case "GetStudentCostsBySubject":
                int studentIdForCosts = int.Parse(data);
                var paidLessonIds = db.PaidLessons.Where(p => p.StudentId == studentIdForCosts).Select(p => p.LessonId).ToList();
                var costs = db.Events
                    .Where(e => e.StudentId == studentIdForCosts && (e.Presence == "+" || e.Presence == "Н") && !paidLessonIds.Contains(e.LessonId))
                    .Include(e => e.Lesson.Subject)
                    .GroupBy(e => e.Lesson.Subject)
                    .Select(g => new
                    {
                        SubjectName = g.Key.Name,
                        LessonCount = g.Count(),
                        TotalCost = g.Count() * g.Key.Price
                    })
                    .ToList();
                return JsonConvert.SerializeObject(costs);

            case "GetParentBalance":
                int parentIdBalance = int.Parse(data);
                var parentBalance = db.Parents.Include(p => p.Balance).FirstOrDefault(p => p.Id == parentIdBalance);
                if (parentBalance?.Balance == null) return JsonConvert.SerializeObject(new { Balance = 0 });
                return JsonConvert.SerializeObject(new { Balance = parentBalance.Balance.Money });

            case "AddBalance":
                var addReq = JsonConvert.DeserializeObject<AddBalanceRequest>(data);
                var parentAdd = db.Parents.Include(p => p.Balance).FirstOrDefault(p => p.Id == addReq.ParentId);
                if (parentAdd?.Balance == null) return "error";
                parentAdd.Balance.Money += addReq.Amount;
                db.SaveChanges();
                return JsonConvert.SerializeObject(new { NewBalance = parentAdd.Balance.Money });

            case "PayAllPossibleLessons":
                int parentPayId = int.Parse(data);
                var parentPay = db.Parents.Include(p => p.Balance).FirstOrDefault(p => p.Id == parentPayId);
                if (parentPay?.Balance == null) return "error";
                int balance = parentPay.Balance.Money;
                var payable = db.Events
                    .Where(e => e.Student.ParentId == parentPayId && (e.Presence == "+" || e.Presence == "Н"))
                    .Include(e => e.Lesson.Subject)
                    .Include(e => e.Lesson.Day)
                    .Where(e => !db.PaidLessons.Any(p => p.StudentId == e.StudentId && p.LessonId == e.LessonId))
                    .OrderBy(e => e.Lesson.Day.Year)
                    .ThenBy(e => e.Lesson.Day.Month)
                    .ThenBy(e => e.Lesson.Day.DayNumber)
                    .ToList();
                var groups = payable.GroupBy(e => new { e.StudentId, e.Lesson.SubjectId, e.Lesson.Subject.Price });
                List<PaidLesson> toPay = new List<PaidLesson>();
                int remaining = balance;
                foreach (var g in groups)
                {
                    int price = g.Key.Price;
                    int count = g.Count();
                    int maxCanPay = Math.Min(count, remaining / price);
                    if (maxCanPay > 0)
                    {
                        var lessonsToPay = g.Select(e => e.Lesson).Take(maxCanPay).ToList();
                        foreach (var lesson in lessonsToPay)
                        {
                            toPay.Add(new PaidLesson
                            {
                                StudentId = g.Key.StudentId,
                                LessonId = lesson.Id,
                                ParentId = parentPayId,
                                PaymentDate = DateTime.Now
                            });
                        }
                        remaining -= maxCanPay * price;
                    }
                }
                if (toPay.Any())
                {
                    db.PaidLessons.AddRange(toPay);
                    parentPay.Balance.Money = remaining;
                    db.SaveChanges();
                    return JsonConvert.SerializeObject(new { PaidCount = toPay.Count, NewBalance = remaining });
                }
                return JsonConvert.SerializeObject(new { PaidCount = 0, NewBalance = balance });

            case "UpdateCell":
                var update = JsonConvert.DeserializeObject<CellUpdate>(data);

                var existingEvent = db.Events.FirstOrDefault(e => e.StudentId == update.StudentId && e.LessonId == update.LessonId);
                
                var existingMark = db.Marks.FirstOrDefault(m =>
                    m.StudentId == update.StudentId &&
                    m.LessonId == update.LessonId &&
                    m.LogbookId == update.LogbookId);


                if (update.Presence != null)
                {
                    if (update.Presence == "")
                    {
                        if (existingEvent != null)
                            db.Events.Remove(existingEvent);
                    }
                    else
                    {
                        if (existingEvent == null)
                        {
                            db.Events.Add(new Event
                            {
                                StudentId = update.StudentId,
                                LessonId = update.LessonId,
                                Presence = update.Presence
                            });
                        }
                        else
                        {
                            existingEvent.Presence = update.Presence;
                        }
                    }
                }

                if (update.Mark.HasValue)
                {
                    if (update.Mark.Value == 0)
                    {
                        if (existingMark != null)
                            db.Marks.Remove(existingMark);
                    }
                    else
                    {
                        if (existingMark == null)
                        {
                            db.Marks.Add(new Mark
                            {
                                StudentId = update.StudentId,
                                LessonId = update.LessonId,
                                LogbookId = update.LogbookId,
                                MarkValue = update.Mark.Value
                            });
                        }
                        else
                        {
                            existingMark.MarkValue = update.Mark.Value;
                        }
                    }
                }

                db.SaveChanges();
                return "ok";
            //Административные запросы
            case "GetUsers":
                var users = db.Users.Include(u => u.Role).Select(u => new { u.Id, u.Login, RoleName = u.Role.Name }).ToList();
                return JsonConvert.SerializeObject(users);

            case "GetRoles":
                var roles = db.Roles.Select(r => new { r.Id, r.Name }).ToList();
                return JsonConvert.SerializeObject(roles);

            case "CreateUser":
                var newUser = JsonConvert.DeserializeObject<CreateUserRequest>(data);
                var roleUser = db.Roles.FirstOrDefault(r => r.Name == newUser.RoleName);
                if (roleUser == null) return "error: role not found";
                var userToAdd = new User { Login = newUser.Login, Password = newUser.Password, RoleId = roleUser.Id };
                db.Users.Add(userToAdd);
                db.SaveChanges();
                if (newUser.RoleName == "student")
                {
                    db.Students.Add(new Student { UserId = userToAdd.Id, Fio = newUser.Login, GroupId = 1, ParentId = 1 });
                }
                else if (newUser.RoleName == "parent")
                {
                    db.Parents.Add(new Parent { UserId = userToAdd.Id, Fio = newUser.Login });
                }
                else if (newUser.RoleName == "teacher")
                {
                    db.Teachers.Add(new Teacher { UserId = userToAdd.Id, Fio = newUser.Login, BalanceId = 1 });
                }
                db.SaveChanges();
                return "ok";

            case "DeleteUser":
                int userIdDel = int.Parse(data);
                var userDel = db.Users.Find(userIdDel);
                if (userDel != null) db.Users.Remove(userDel);
                db.SaveChanges();
                return "ok";

            case "UpdateUserRole":
                var updateRole = JsonConvert.DeserializeObject<UpdateUserRoleRequest>(data);
                var userUp = db.Users.Find(updateRole.UserId);
                if (userUp != null)
                {
                    var newRole = db.Roles.First(r => r.Name == updateRole.NewRoleName);
                    userUp.RoleId = newRole.Id;
                    db.SaveChanges();
                }
                return "ok";

            case "GetGroups":
                var allGroups = db.Groups.Select(g => new { g.Id, g.Name }).ToList();
                return JsonConvert.SerializeObject(allGroups);

            case "AddGroup":
                var groupName = JsonConvert.DeserializeObject<AddGroupRequest>(data).Name;
                db.Groups.Add(new Group { Name = groupName });
                db.SaveChanges();
                return "ok";

            case "DeleteGroup":
                int groupIdDel = int.Parse(data);
                var groupDel = db.Groups.Find(groupIdDel);
                if (groupDel != null) db.Groups.Remove(groupDel);
                db.SaveChanges();
                return "ok";

            case "UpdateGroup":
                var updateNameGroupReq = JsonConvert.DeserializeObject<UpdateGroupRequest>(data);
                var groupToUpdate = db.Groups.Find(updateNameGroupReq.GroupId);
                if (groupToUpdate != null)
                {
                    groupToUpdate.Name = updateNameGroupReq.NewName;
                    db.SaveChanges();
                    return "ok";
                }
                return "error: group not found";

            case "AddSubject":
                var subjectReq = JsonConvert.DeserializeObject<AddSubjectRequest>(data);
                db.Subjects.Add(new Subject { Name = subjectReq.Name, Price = subjectReq.Price });
                db.SaveChanges();
                return "ok";

            case "DeleteSubject":
                int subjectIdDel = int.Parse(data);
                var subjectDel = db.Subjects.Find(subjectIdDel);
                if (subjectDel != null) db.Subjects.Remove(subjectDel);
                db.SaveChanges();
                return "ok";

            case "UpdateSubject":
                var updateSubjectReq = JsonConvert.DeserializeObject<UpdateSubjectRequest>(data);
                var subjectToUpdate = db.Subjects.Find(updateSubjectReq.SubjectId);
                if (subjectToUpdate != null)
                {
                    subjectToUpdate.Name = updateSubjectReq.NewName;
                    subjectToUpdate.Price = updateSubjectReq.NewPrice;
                    db.SaveChanges();
                    return "ok";
                }
                return "error: subject not found";

            case "GetLessons":
                var allLessons = db.Lessons
                    .Include(l => l.Subject)
                    .Select(l => new { l.Id, SubjectName = l.Subject.Name, DayId = l.DayId })
                    .ToList();
                return JsonConvert.SerializeObject(allLessons);

            case "GetDays":
                var days = db.Days.Select(d => new { d.Id, d.Year, d.Month, d.DayNumber }).ToList();
                return JsonConvert.SerializeObject(days);

            case "UpdateLessonDay":
                var updateLesson = JsonConvert.DeserializeObject<UpdateLessonDayRequest>(data);
                var lessonUpd = db.Lessons.Find(updateLesson.LessonId);
                if (lessonUpd != null)
                {
                    lessonUpd.DayId = updateLesson.DayId;
                    db.SaveChanges();
                }
                return "ok";

            case "RemoveLessonDay":
                int lessonIdRem = int.Parse(data);
                var lessonRem = db.Lessons.Find(lessonIdRem);
                if (lessonRem != null) 
                    lessonRem.DayId = null;
                db.SaveChanges();
                return "ok";

            case "GetStudentsWithGroups":
                var studentsWithGroups = db.Students
                    .Include(s => s.Group)
                    .Select(s => new { s.Id, s.Fio, GroupName = s.Group.Name, GroupId = s.GroupId })
                    .ToList();
                return JsonConvert.SerializeObject(studentsWithGroups);

            case "AddStudent":
                var addStudentReq = JsonConvert.DeserializeObject<AddStudentRequest>(data);

                var existingStudent = db.Students.FirstOrDefault(s => s.UserId == addStudentReq.UserId);
                if (existingStudent != null)
                    return JsonConvert.SerializeObject(new { Success = false, Message = "Этот пользователь уже является учеником" });

                var parentExists = db.Users.Any(u => u.Id == addStudentReq.ParentId && u.Role.Name == "parent");
                if (!parentExists)
                    return JsonConvert.SerializeObject(new { Success = false, Message = "Выбранный родитель не существует или не имеет роли 'parent'" });

                var newStudent = new Student
                {
                    UserId = addStudentReq.UserId,
                    Fio = addStudentReq.Fio,
                    GroupId = addStudentReq.GroupId,
                    ParentId = addStudentReq.ParentId
                };
                db.Students.Add(newStudent);
                db.SaveChanges();

                if (!db.Profiles.Any(p => p.StudentId == newStudent.Id))
                {
                    db.Profiles.Add(new Profile { StudentId = newStudent.Id });
                    db.SaveChanges();
                }

                return JsonConvert.SerializeObject(new { Success = true, Id = newStudent.Id, Fio = newStudent.Fio });

            case "DeleteStudent":
                int studentIdDel = int.Parse(data);
                var studentToDelete = db.Students.Include(s => s.User).FirstOrDefault(s => s.Id == studentIdDel);
                if (studentToDelete != null)
                {
                    var userToDelete = studentToDelete.User;
                    db.Students.Remove(studentToDelete);
                    if (userToDelete != null) db.Users.Remove(userToDelete);
                    db.SaveChanges();
                }
                return "ok";

            case "UpdateStudentGroup":
                var updateGroupReq = JsonConvert.DeserializeObject<UpdateStudentGroupRequest>(data);
                var studentToUpdate = db.Students.Find(updateGroupReq.StudentId);
                if (studentToUpdate != null)
                {
                    studentToUpdate.GroupId = updateGroupReq.NewGroupId;
                    db.SaveChanges();
                }
                return "ok";

            case "GetUsersByRole":
                string roleFilter = data;
                var usersByRole = db.Users
                    .Include(u => u.Role)
                    .Where(u => u.Role.Name == roleFilter)
                    .Select(u => new { u.Id, u.Login, RoleName = u.Role.Name })
                    .ToList();
                return JsonConvert.SerializeObject(usersByRole);

            case "GetParentsList":
                var parents = db.Parents.Select(p => new { p.Id, p.Fio }).ToList();
                return JsonConvert.SerializeObject(parents);

            default:
                return "unknown";
        }
    }
}

public class LoginRequest { public string Login { get; set; } public string Password { get; set; } }
public class ChildDataRequest { public int StudentId { get; set; } }
public class AddReviewRequest { public DateTime Date { get; set; } public string Text { get; set; } public bool IsPositive { get; set; } public int UserId { get; set; } public int StudentId { get; set; } }
public class AddBalanceRequest { public int ParentId { get; set; } public int Amount { get; set; } }
public class CreateUserRequest { public string Login { get; set; } public string Password { get; set; } public string RoleName { get; set; } }
public class UpdateUserRoleRequest { public int UserId { get; set; } public string NewRoleName { get; set; } }
public class AddGroupRequest { public string Name { get; set; } }
public class AddSubjectRequest { public string Name { get; set; } public int Price { get; set; } }
public class UpdateGroupRequest { public int GroupId { get; set; } public string NewName { get; set; } }
public class UpdateSubjectRequest { public int SubjectId { get; set; } public string NewName { get; set; } public int NewPrice { get; set; } }
public class UpdateLessonDayRequest { public int LessonId { get; set; } public int DayId { get; set; } }
public class AddStudentRequest
{
    public string Fio { get; set; }
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public int ParentId { get; set; }
}
public class UpdateStudentGroupRequest
{
    public int StudentId { get; set; }
    public int NewGroupId { get; set; }
}