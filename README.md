# Worker Payroll and Attendance System – Integrated Web & Database Project

## 📘 Project Overview

This repository contains the implementation of a comprehensive payroll and attendance management system, developed as part of the **Third and Fourth Programming Assignments** for the course *Database Systems II* at Instituto Tecnológico de Costa Rica (ITCR). The system simulates real-world labor scenarios for a factory with rotating shifts, wage calculations, deductions, and reporting for both weekly and monthly periods.

The project includes:
- A **relational database** in SQL Server.
- A **web portal** for administrators and employees.
- **Stored procedures**, **event logging**, and **full data simulation** over multiple months.

## 🎯 Objective

The main objectives of this project are to:
- Model and implement a physical database that supports payroll and attendance control.
- Develop a **dual-role web application** (for administrators and employees).
- Simulate daily system operations (shifts, attendance, payroll) via XML over a multi-month timeline.
- Manage payroll logic, including rotating shifts, hourly wage computation, deductions, and legal contributions.
- Track user actions through detailed **event logging** and maintain transactional consistency.

## 🏭 System Description

The application simulates a **24/7 factory** with the following characteristics:
- Rotating weekly shifts: Morning, Evening, and Night (including weekends and holidays).
- Weekly payroll payments; monthly reports for government institutions.
- Complex logic for calculating:
  - Ordinary hours
  - Overtime (1.5x)
  - Double-time (2.0x for Sundays and holidays)
  - Fixed and percentage-based deductions (e.g., insurance, savings, garnishments).
- XML-driven data: input includes attendance, shift assignments, employee management, and payroll simulation.

## 🖥️ User Roles

### 👩‍💼 Administrator
Can:
- View and filter employees.
- Insert, edit, and logically delete employees.
- Impersonate employees.
- All changes are logged in a centralized event log.

### 👷 Employee
Can:
- View last 15 weekly payrolls (gross, deductions, net salary).
- View last 12 monthly payrolls.
- Click to inspect deduction breakdown and work details per day.
- Return to admin view if impersonated.

## ⚙️ Technical Features

- **Database**: Microsoft SQL Server 2014+.
- **Stored Procedures**: All CRUD, simulation, and queries.
- **Triggers**: Auto-assign mandatory deductions on employee insertion.
- **Transactional Logic**: Each employee’s daily or weekly operations are wrapped in a **single database transaction**.
- **Logging**: All actions are recorded in an `EventLog` table (with before/after data, parameters, and timestamps).
- **Web Interface**: Developed using a language/framework of choice.

## 📦 Data and Simulation

### Catalog XML
Includes:
- Document types
- Shift types
- Job positions
- Departments
- Deduction types
- Event types
- Public holidays
- System users

### Operation XML
Simulates daily operations over **at least 4 months**, including:
- Attendance records (with entry/exit times, even spanning across days).
- Employee insertion, deletion.
- Shift assignments for the upcoming week.
- Deduction assignment/removal.

Each simulated date processes all required operations and automatically computes:
- Hours worked (ordinary, overtime, double time)
- Weekly gross salary
- Weekly deductions (based on whether the month has 4 or 5 Thursdays)
- Monthly summary
- Salary net of deductions

## 📊 Deliverables

- Physical database schema (tables, FKs, triggers).
- Stored procedures and logic layer code.
- Web portal for both roles.
- Scripts for:
  - Catalog loading
  - Operation simulation (at least 4 months)
- Developer blog documenting progress and issues.
- Final results report: requirements matrix, metrics (e.g., LoC, hours, Git commits), graphs.


---
