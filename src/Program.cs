using FinalProject.Contracts;
using FinalProject.Domain;
using FinalProject.Repositories;
using FinalProject.Services;
using FinalProject.Services.SearchStrategies;

namespace FinalProject;

class Program
{
    static readonly IUserRepo _userRepo = new UserRepo();
    static readonly IRoomRepo _roomRepo = new RoomRepo();
    static readonly IBookingRepo _bookingRepo = new BookingRepo();
    static readonly IRecordRepo _recordRepo = new RecordRepo();
    static readonly ILoginService _loginService = new LoginService(_userRepo);
    static readonly IBookingService _bookingService = new BookingService(_bookingRepo, _roomRepo);
    static readonly IRecordService _recordService = new RecordService(_recordRepo, _bookingRepo);
    static readonly RoomFactory _roomFactory = new RoomFactory();

    static void Main(string[] args)
    {
        SeedRooms();
        SeedBookings();
        SeedRecords();

        while (true)
        {
            CheckExpiredClassrooms();
            Console.Clear();
            Console.WriteLine("=== Room Booking System ===\n");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign Up");
            Console.WriteLine("3. Exit");
            Console.Write("\n> ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    Session? session = DoLogin();
                    if (session != null) RunUserMenu(session);
                    break;
                case "2":
                    DoSignUp();
                    break;
                case "3":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void SeedRooms()
    {
        _roomRepo.AddRoom(new Classroom("Building A - Room 101", 30) { NumberOfWhiteBoards = 2 });
        _roomRepo.AddRoom(new Classroom("Building B - Room 202", 20) { NumberOfWhiteBoards = 1 });
        _roomRepo.AddRoom(new Classroom("Building C - Room 303", 20) { NumberOfWhiteBoards = 1} );
        _roomRepo.AddRoom(new Lab("Science Block - Lab 1", 24) { Equipments = "Microscopes, Bunsen Burners" });
        _roomRepo.AddRoom(new Lab("Tech Block - Lab 2", 20) { Equipments = "Computers, Soldering Kits" });
        _roomRepo.AddRoom(new Lab("Media Block - Lab 3", 20) { Equipments = "Camera, Microphone" });
        _roomRepo.AddRoom(new StudyRoom("Library - Study Room A", 8));
        _roomRepo.AddRoom(new StudyRoom("Library - Study Room B", 6));
        _roomRepo.AddRoom(new StudyRoom("Library - Study Room C", 8));
    }

    static void SeedBookings()
    {
        _bookingService.AddBooking(
            _bookingService.CreateBooking("Calc Study Group", "Building A - Room 101", Subject.Mathematics, DateTime.Now.AddDays(4), "niner@charlotte.edu"));
        _bookingService.AddBooking(
            _bookingService.CreateBooking("History Review", "Building B - Room 202", Subject.History, DateTime.Now.AddDays(5), "miner@charlotte.edu"));
        _bookingService.AddBooking(
            _bookingService.CreateBooking("Bio Lab Practicals", "Science Block - Lab 1", Subject.Biology, DateTime.Now.AddDays(6), "charlot2@charlotte.edu"));
        _bookingService.AddBooking(
            _bookingService.CreateBooking("CS Circuits Workshop", "Tech Block - Lab 2", Subject.ComputerScience, DateTime.Now.AddDays(7), "niner@charlotte.edu"));
        _bookingService.AddBooking(
            _bookingService.CreateBooking("Essay Peer Review", "Library - Study Room A", Subject.English, DateTime.Now.AddDays(3), "norm@charlotte.edu"));
        _bookingService.AddBooking(
            _bookingService.CreateBooking("Finance Flashcards", "Library - Study Room B", Subject.Finance, DateTime.Now.AddDays(3), "miner@charlotte.edu"));
    }

    static void SeedRecords()
    {
        Booking cancelledSeed = _bookingService.CreateBooking("Archived Cancelled Session", "Building C - Room 303", Subject.English, DateTime.Now.AddDays(2), "niner@charlotte.edu");
        _bookingService.AddBooking(cancelledSeed);
        cancelledSeed.Status = BookingStatus.Cancelled;
        cancelledSeed.Room.Booked = false;
        cancelledSeed.Room.CurrentBooking = null;
        _recordService.ArchiveBooking(cancelledSeed);
    }

    static void CheckExpiredClassrooms()
    {
        var expired = _bookingService.GetAllBookings()
            .Where(b => b.Room is Classroom
                     && DateTime.Now - b.ScheduledTime > TimeSpan.FromDays(2)
                     && b.Attendees.Count < b.Minimum)
            .ToList();

        foreach (var booking in expired)
        {
            booking.Status = BookingStatus.Expired;
            booking.Room.Booked = false;
            booking.Room.CurrentBooking = null;
            Console.WriteLine($"[System] Session \"{booking.Name}\" cancelled — minimum attendance not met within 2 days.");
        }

        ArchiveInactiveBookings();

        if (expired.Count > 0)
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }

    static void ArchiveInactiveBookings()
    {
        var inactive = _bookingService.GetAllBookings()
            .Where(b => b.Status == BookingStatus.Cancelled || b.Status == BookingStatus.Expired)
            .ToList();

        foreach (var booking in inactive)
        {
            _recordService.ArchiveBooking(booking);
        }
    }

    static Session? DoLogin()
    {
        Console.Clear();
        Console.WriteLine("=== Login ===\n");
        Console.Write("Email: ");
        string email = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        User? user = _loginService.Login(email, password);
        if (user == null)
        {
            Console.WriteLine("\nInvalid email or password. Press any key.");
            Console.ReadKey();
            return null;
        }

        Console.WriteLine($"\nWelcome back, {user.name}! Press any key.");
        Console.ReadKey();
        return new Session(user);
    }

    static void DoSignUp()
    {
        Console.Clear();
        Console.WriteLine("=== Sign Up ===\n");
        Console.Write("Name: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Email: ");
        string email = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";
        Console.Write("Role (student/admin): ");
        string role = Console.ReadLine()?.Trim().ToLower() ?? "student";

        User newUser = role == "admin"
            ? new Admin(name, email, password)
            : new Student(name, email, password);

        _userRepo.AddUser(newUser);
        Console.WriteLine($"\nAccount created for {name}. Press any key.");
        Console.ReadKey();
    }

    static void RunUserMenu(Session session)
    {
        if (session.CurrentUser is Admin)
            RunAdminMenu(session);
        else
            RunStudentMenu(session);
    }

    static void RunAdminMenu(Session session)
    {
        while (_loginService.IsLoggedIn(session))
        {
            CheckExpiredClassrooms();
            Console.Clear();
            Console.WriteLine($"=== Admin Menu — {session.CurrentUser.name} ===\n");
            Console.WriteLine("1. Create Session");
            Console.WriteLine("2. Search Sessions");
            Console.WriteLine("3. Search Records");
            Console.WriteLine("4. Cancel a Session");
            Console.WriteLine("5. View Rooms");
            Console.WriteLine("6. Add Room");
            Console.WriteLine("7. Sign Out");
            Console.Write("\n> ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": CreateSession(session.CurrentUser); break;
                case "2": SearchSessions(session.CurrentUser, canJoin: false); break;
                case "3": SearchRecords(); break;
                case "4": CancelSession(); break;
                case "5": ViewRooms(); break;
                case "6": AddRoom(); break;
                case "7": _loginService.Logout(session); break;
                default:
                    Console.WriteLine("Invalid option. Press any key.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void RunStudentMenu(Session session)
    {
        while (_loginService.IsLoggedIn(session))
        {
            CheckExpiredClassrooms();
            Console.Clear();
            Console.WriteLine($"=== Student Menu — {session.CurrentUser.name} ===\n");
            Console.WriteLine("1. Search Sessions");
            Console.WriteLine("2. Create Session");
            Console.WriteLine("3. My Sessions");
            Console.WriteLine("4. Sign Out");
            Console.Write("\n> ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": SearchSessions(session.CurrentUser, canJoin: true); break;
                case "2": CreateSession(session.CurrentUser); break;
                case "3": ViewMyBookings(session.CurrentUser); break;
                case "4": _loginService.Logout(session); break;
                default:
                    Console.WriteLine("Invalid option. Press any key.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void CreateSession(User creator)
    {
        Console.Clear();
        Console.WriteLine("=== Create Session ===\n");
        Console.Write("Session Name: ");
        string sessionName = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(sessionName))
        {
            Console.WriteLine("Session name cannot be empty. Press any key.");
            Console.ReadKey();
            return;
        }

        Console.Write("Room Type (classroom/lab/studyroom): ");
        string roomType = Console.ReadLine()?.Trim().ToLower() ?? "";

        var available = _roomRepo.GetAll()
            .Where(r => !r.Booked && r.GetType().Name.ToLower() == roomType)
            .ToList();

        if (available.Count == 0)
        {
            Console.WriteLine("\nNo available rooms of that type. Press any key.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Rooms:");
        for (int i = 0; i < available.Count; i++)
        {
            Console.Write($"  {i + 1}. ");
            available[i].DisplayDetails();
        }

        Console.Write("\nSelect Room #: ");
        if (!int.TryParse(Console.ReadLine(), out int roomIdx) || roomIdx < 1 || roomIdx > available.Count)
        {
            Console.WriteLine("Invalid selection. Press any key.");
            Console.ReadKey();
            return;
        }
        Room room = available[roomIdx - 1];

        var subjects = Enum.GetValues<Subject>();
        Console.WriteLine("\nSubjects:");
        for (int i = 0; i < subjects.Length; i++)
            Console.WriteLine($"  {i + 1}. {subjects[i]}");
        Console.Write("Select Subject #: ");
        if (!int.TryParse(Console.ReadLine(), out int subjectIdx) || subjectIdx < 1 || subjectIdx > subjects.Length)
        {
            Console.WriteLine("Invalid selection. Press any key.");
            Console.ReadKey();
            return;
        }
        Subject subject = subjects[subjectIdx - 1];

        Console.Write("Session Time (days from now): ");
        if (!int.TryParse(Console.ReadLine(), out int days) || days <= 0)
        {
            Console.WriteLine("Invalid input. Using default of 7 days.");
            days = 7;
        }
        DateTime bookingTime = DateTime.Now.AddDays(days);

        try
        {
            Booking booking = _bookingService.CreateBooking(
                sessionName, room.Location, subject, bookingTime, creator.getEmail());
            _bookingService.AddBooking(booking);
            Console.WriteLine($"\nSession \"{sessionName}\" created successfully! Press any key.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nCould not create session: {e.Message} Press any key.");
        }
        Console.ReadKey();
    }

    static void SearchSessions(User user, bool canJoin)
    {
        Console.Clear();
        Console.WriteLine("=== Search Sessions ===\n");
        Console.WriteLine("Search by:");
        Console.WriteLine("1. Room type");
        Console.WriteLine("2. Subject");
        Console.WriteLine("3. Availability (open sessions)");
        Console.WriteLine("4. All sessions");
        Console.Write("\n> ");

        IBookingSearchStrategy? strategy = null;
        bool searchAll = false;
        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                Console.Write("Enter room type (classroom/lab/studyroom): ");
                string roomType = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(roomType))
                {
                    Console.WriteLine("Room type cannot be empty. Press any key.");
                    Console.ReadKey();
                    return;
                }
                strategy = new RoomSearchStrategy(roomType);
                break;
            case "2":
                var subjects = Enum.GetValues<Subject>();
                Console.WriteLine("\nSubjects:");
                for (int i = 0; i < subjects.Length; i++)
                    Console.WriteLine($"  {i + 1}. {subjects[i]}");
                Console.Write("Select Subject #: ");
                if (!int.TryParse(Console.ReadLine(), out int subjectIdx) || subjectIdx < 1 || subjectIdx > subjects.Length)
                {
                    Console.WriteLine("Invalid selection. Press any key.");
                    Console.ReadKey();
                    return;
                }
                strategy = new SubjectSearchStrategy(subjects[subjectIdx - 1]);
                break;
            case "3":
                strategy = new AvailabilitySearchStrategy();
                break;
            case "4":
                searchAll = true;
                break;
            default:
                Console.WriteLine("Invalid option. Press any key.");
                Console.ReadKey();
                return;
        }

        var sessions = (searchAll
                ? _bookingService.GetAllBookings()
                : _bookingService.SearchBookings(strategy!))
            .Where(b => b.Status != BookingStatus.Expired)
            .ToList();

        if (sessions.Count == 0)
        {
            Console.WriteLine("\nNo matching sessions found. Press any key.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        for (int i = 0; i < sessions.Count; i++)
        {
            Booking b = sessions[i];
            string type = b.Room.GetType().Name;
            string confirmed = b.Status == BookingStatus.Confirmed ? " [CONFIRMED]" : "";
            string full = b.Attendees.Count >= b.Maximum ? " [FULL]" : "";
            Console.WriteLine($"  {i + 1}. \"{b.Name}\" — {type} @ {b.Room.Location}");
            Console.WriteLine($"       Subject: {b.Subject} | Attendees: {b.Attendees.Count}/{b.Maximum} (min {b.Minimum}){confirmed}{full}");
            Console.WriteLine($"       Deadline: {b.Deadline:yyyy-MM-dd HH:mm}");
        }

        if (!canJoin)
        {
            Console.WriteLine("\nPress any key.");
            Console.ReadKey();
            return;
        }

        Console.Write("\nEnter session # to join (or 0 to go back): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 0) return;
        if (choice < 1 || choice > sessions.Count)
        {
            Console.WriteLine("Invalid selection. Press any key.");
            Console.ReadKey();
            return;
        }

        Booking selected = sessions[choice - 1];
        string userEmail = user.getEmail();

        if (selected.Attendees.Contains(userEmail))
        {
            Console.WriteLine("\nYou have already joined this session. Press any key.");
            Console.ReadKey();
            return;
        }

        try
        {
            _bookingService.JoinBooking(selected.BookingId, userEmail);
            Console.WriteLine($"\nYou have joined \"{selected.Name}\"! Press any key.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nCould not join session: {e.Message} Press any key.");
        }

        Console.ReadKey();
    }

    static void ViewMyBookings(User user)
    {
        Console.Clear();
        Console.WriteLine("=== My Sessions ===\n");
        string email = user.getEmail();

        var mine = _bookingService.GetAllBookings()
            .Where(b => b.Attendees.Contains(email))
            .ToList();

        if (mine.Count == 0)
        {
            Console.WriteLine("You have not joined any sessions. Press any key.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < mine.Count; i++)
        {
            Booking b = mine[i];
            string confirmed = b.Status == BookingStatus.Confirmed ? " [CONFIRMED]" : " [OPEN]";
            Console.WriteLine($"{i + 1}. \"{b.Name}\" — {b.Room.GetType().Name} @ {b.Room.Location}");
            Console.WriteLine($"   Subject: {b.Subject} | Attendees: {b.Attendees.Count}/{b.Maximum}{confirmed}");
            Console.WriteLine($"   Deadline: {b.Deadline:yyyy-MM-dd HH:mm}");
        }

        Console.Write("\nEnter session # to leave (or 0 to go back): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 0) return;
        if (choice < 1 || choice > mine.Count)
        {
            Console.WriteLine("Invalid selection. Press any key.");
            Console.ReadKey();
            return;
        }

        Booking selected = mine[choice - 1];
        try
        {
            _bookingService.LeaveBooking(selected.BookingId, email);
            Console.WriteLine($"\nYou have left \"{selected.Name}\". Press any key.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nCould not leave session: {e.Message} Press any key.");
        }
        Console.ReadKey();
    }

    static void CancelSession()
    {
        Console.Clear();
        Console.WriteLine("=== Cancel Session ===\n");

        var sessions = _bookingService.GetAllBookings().ToList();
        if (sessions.Count == 0)
        {
            Console.WriteLine("No active sessions. Press any key.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < sessions.Count; i++)
        {
            Booking b = sessions[i];
            Console.WriteLine($"  {i + 1}. \"{b.Name}\" — {b.Room.GetType().Name} @ {b.Room.Location} | Attendees: {b.Attendees.Count}");
        }

        Console.Write("\nSelect session # to cancel (or 0 to go back): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 0) return;
        if (choice < 1 || choice > sessions.Count)
        {
            Console.WriteLine("Invalid selection. Press any key.");
            Console.ReadKey();
            return;
        }

        Booking toCancel = sessions[choice - 1];
        toCancel.Room.Booked = false;
        toCancel.Room.CurrentBooking = null;
        toCancel.Status = BookingStatus.Cancelled;
        ArchiveInactiveBookings();

        Console.WriteLine($"\nSession \"{toCancel.Name}\" cancelled. Press any key.");
        Console.ReadKey();
    }

    static void SearchRecords()
    {
        Console.Clear();
        Console.WriteLine("=== Search Records ===\n");
        Console.WriteLine("1. View all records");
        Console.WriteLine("2. Search by booking ID");
        Console.Write("\n> ");

        List<Record> records;
        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                records = _recordService.GetAllRecords();
                break;
            case "2":
                Console.Write("Enter booking ID: ");
                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    Console.WriteLine("Invalid booking ID. Press any key.");
                    Console.ReadKey();
                    return;
                }
                Record? record = _recordService.GetRecordById(bookingId);
                records = record.HasValue ? [record.Value] : [];
                break;
            default:
                Console.WriteLine("Invalid option. Press any key.");
                Console.ReadKey();
                return;
        }

        if (records.Count == 0)
        {
            Console.WriteLine("\nNo records found. Press any key.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        foreach (var record in records)
        {
            string scheduled = record.ScheduledTime?.ToString("yyyy-MM-dd HH:mm") ?? "N/A";
            Console.WriteLine($"Booking ID: {record.BookingId}");
            Console.WriteLine($"  Room: {record.RoomLocation}");
            Console.WriteLine($"  Status: {record.Status}");
            Console.WriteLine($"  Scheduled: {scheduled}");
            Console.WriteLine($"  Archived: {record.ArchivedTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine();
        }

        Console.WriteLine("Press any key.");
        Console.ReadKey();
    }

    static void ViewRooms()
    {
        Console.Clear();
        Console.WriteLine("=== All Rooms ===\n");

        var rooms = _roomRepo.GetAll();
        if (rooms.Count == 0)
        {
            Console.WriteLine("No rooms on record. Press any key.");
            Console.ReadKey();
            return;
        }

        foreach (Room r in rooms)
            r.DisplayDetails();

        Console.WriteLine("\nPress any key.");
        Console.ReadKey();
    }

    static void AddRoom()
    {
        Console.Clear();
        Console.WriteLine("=== Add Room ===\n");
        Console.Write("Location: ");
        string loc = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Capacity: ");
        string cap = Console.ReadLine()?.Trim() ?? "0";
        Console.Write("Type (classroom/lab/studyroom): ");
        string type = Console.ReadLine()?.Trim() ?? "";

        try
        {
            Room room = _roomFactory.CreateRoom(loc, cap, type);

            if (room is Classroom classroom)
            {
                Console.Write("Number of whiteboards: ");
                int.TryParse(Console.ReadLine(), out int wb);
                classroom.NumberOfWhiteBoards = wb;
            }
            else if (room is Lab lab)
            {
                Console.Write("Equipment: ");
                lab.Equipments = Console.ReadLine()?.Trim() ?? "";
            }

            _roomRepo.AddRoom(room);
            Console.WriteLine("\nRoom added successfully. Press any key.");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"\nError: {e.Message} Press any key.");
        }

        Console.ReadKey();
    }
}
