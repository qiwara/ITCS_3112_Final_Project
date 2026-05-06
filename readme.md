# Room Booking System

---

## Requirements Checklist

**Console Input/Output** — [Program.cs](./Program.cs) throughout; all menus use `Console.ReadLine()` for input and `Console.WriteLine()` for output.

**Inheritance #1** — [Room (abstract)](./src/Domain/Room.cs) is the base class; [Classroom](./src/Domain/Classroom.cs), [Lab](./src/Domain/Lab.cs), and [StudyRoom](./src/Domain/StudyRoom.cs) all extend it.

**Inheritance #2** — [User (abstract)](./src/Domain/User.cs) is the base class; [Admin](./src/Domain/Admin.cs) and [Student](./src/Domain/Student.cs) extend it.

**Interface #1** — [IUserInterface](./src/Contracts/IUserInterface.cs) defines the menu contract; implemented by [AdminInterface](./src/Services/AdminInterface.cs) and [StudentInterface](./src/Services/StudentInterface.cs).

**Interface #2** — [IBookingService](./src/Contracts/IBookingService.cs) defines booking business logic; implemented by [BookingService](./src/Services/BookingService.cs).

**Interface #3** — [ILoginService](./src/Contracts/ILoginService.cs) defines authentication logic; implemented by [LoginService](./src/Services/LoginService.cs).

**Polymorphism** — `Room.DisplayDetails()` is declared abstract at [Room.cs](./src/Domain/Room.cs) line 18. Each subclass overrides it: Classroom (line 9), Lab (line 12), StudyRoom (line 7) — each prints type-specific fields.

**Struct** — [Record](./src/Domain/Record.cs) is a type (`struct`) holding `BookingId`, `UserId`, and `TimeStamp`.

**Enum** — [BookingStatus](./src/Domain/BookingStatus.cs): `Available`, `Booked`, `Incomplete`.

**Data Structures**\
`List<Room>` in [RoomRepo](./src/Repositories/RoomRepo.cs) line 8;\
`List<Booking>` in BookingRepo line 8;\
`Dictionary<int, string>` in [AdminInterface](./src/Services/AdminInterface.cs) lines 7–12.

---

## Design Patterns

### Factory Method — Creational
**File:** [src/Domain/RoomFactory.cs](./src/Domain/RoomFactory.cs)\
**Rationale:** The application needs to create `Room` subclass instances based on user input (a string like `"classroom"`, `"lab"`, or `"studyroom"`). Without a factory, `Program.cs` would need a switch statement coupled to every concrete type. `RoomFactory.CreateRoom()` centralizes that logic — adding a new room type only requires changing the factory, not the UI code.

---

## SOLID Reflection

- **S (Single Responsibility)** — Repos handle data storage, Services handle business logic, Domains handle data models, and `Program.cs` handles UI. Each class has one reason to change.
- **O (Open/Closed)** — New room types can be added by extending `Room` and adding a case to `RoomFactory` without modifying any existing classes. The main programs responsibilities is the one remaining O/C gap.
- **L (Liskov Substitution)** — All `Room` subclasses are used interchangeably via the `Room` base type (stored in `List<Room>`, passed to `Booking`), and all `User` subclasses are used via `User`.
- **I (Interface Segregation)** — Separate interfaces for repos (`IRoomRepo`, `IBookingRepo`, `IUserRepo`) and services (`IBookingService`, `ILoginService`) so classes only depend on the methods they actually use.
- **D (Dependency Inversion)** — Services receive repo interfaces via constructor injection (e.g. `BookingService(IBookingRepo)`, `LoginService(IUserRepo)`), so high level logic never depends on concrete implementations.

**Known areas for refactoring:** `Program.cs` is doing too much UI work directly and could be split into dedicated handler classes. `IRoomService` and `RoomService` still need implementation. Still need to implement User factory or refactor how Users work for simplicity
