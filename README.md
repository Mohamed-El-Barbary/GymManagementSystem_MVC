# GymManagementSystem 🏋️‍♂️

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**GymManagementSystem** is a comprehensive **Gym Management System** designed for administrators to manage **Members, Trainers, Plans, Memberships, Sessions, and Schedules**.  
This system focuses on **role-based access**, **membership management**, **session tracking**, and **trainer management**.

---

## 👥 System Roles

### 1. SuperAdmin
- Full control over the system.
- Can manage:
  - Members (Add, Update, Delete, View)
  - Trainers (Add, Update, Delete, View)
  - Sessions & Schedules
  - Membership Plans
  - Admins (Add, Update, Delete)
- Can **activate/deactivate** any entity.

### 2. Admin
- Limited control:
  - Manage Sessions, Plans, Memberships, Schedules
  - Cannot manage other Admins
- Responsible for **daily gym operations**.

---

## 🏃‍♂️ System Features

### Members
- Stores personal and health information:
  - Name, Email, Gender, Date of Birth, Phone, Address
  - Health: Height, Weight, Blood Type
- Operations:
  - Add, Update, Delete, View
  - Track membership history and attendance

### Trainers
- Stores trainer information:
  - Name, Email, Phone, Specialties, Address, Date of Birth
- Operations:
  - Add, Update, Delete, View

### Plans
- Types: Basic, Standard, Premium, Annual
- Features:
  - Activate / Deactivate / Edit
- Assign members to **one plan only**

### Memberships
- Assign members to plans
- Prevent duplicate memberships
- Track status: Active, Expired, Cancelled

### Sessions
- Status: Upcoming, Ongoing, Completed
- Categories: Yoga, CrossFit, General Fitness, Boxing
- Operations:
  - Book (Upcoming sessions)
  - Take Attendance (Ongoing sessions)
  - Complete (Completed sessions)

### Session Schedule
- Schedule **Upcoming & Ongoing** sessions
- Automatic status updates based on time
- Integrates members, trainers, and attendance

---

## ⚙️ System Architecture

- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript  
- **Backend:** ASP.NET Core MVC (.NET 9)  
- **Authentication & Authorization:** ASP.NET Core Identity  
- **Database:** SQL Server  
- **ORM:** Entity Framework Core  
- **Mapping:** AutoMapper  
- **Pattern:** Repository + Unit of Work

---

## 🔄 Workflow

1. SuperAdmin sets up Admins, Trainers, Plans.  
2. Admin manages Members, Sessions, and Schedules.  
3. Members join plans and book sessions.  
4. Trainers conduct sessions and record attendance.  
5. System tracks membership, session status, and updates automatically.

---

## 📌 Notes
- **Role-based access control** ensures data security.  
- **Single-plan restriction** avoids membership conflicts.  
- **Automatic session tracking** reduces manual errors.  
- System designed for scalability and maintainability.

---

## ⭐ Contributing
1. Fork the repository  
2. Create your feature branch (`git checkout -b feature/YourFeature`)  
3. Commit your changes (`git commit -m 'Add YourFeature'`)  
4. Push to the branch (`git push origin feature/YourFeature`)  
5. Open a Pull Request  

---

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
