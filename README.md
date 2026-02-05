
# Music Practice Room Booking System

**This project was created in 5 hours as part of the Programming in C# exam.**

## Overview
This is a booking system for a conservatory with 25 music practice rooms of various configurations:
- 15 small rooms (solo practice)
- 6 medium rooms (small ensembles)
- 4 large rooms (full bands or recital prep)

Room features include:
- Grand pianos
- Upright pianos
- Soundproofing (for drums/brass)

## Key Features
- **Student Quotas:**
	- Performance majors: 20 hours/week
	- Education majors: 10 hours/week
	- Minors: 5 hours/week
	- Usage is tracked and booking is blocked when quota is exhausted.
- **No-Show Tracking:**
	- Students who book but do not show are penalized with reduced quotas.
- **Equipment Matching:**
	- Booking flow matches room features to instrument requirements (e.g., pianists need pianos, drummers need soundproofing).
	- Recital prep requires a grand piano, not an upright.
- **Special Booking Rules:**
	- Recital prep bookings are blocked in the final 2 weeks before juries.
	- Students can reserve 3-hour blocks (instead of the usual 2-hour max) with instructor approval.
- **Graduation Requirements:**
	- Some programs require 200 logged practice hours; the system tracks this.

## Technologies Used
- C#
- ASP.NET Core (WebApp)
- Entity Framework Core (DAL)
- Layered architecture: Domain, BLL, DAL, WebApp

## Project Structure
- **Domain:** Core entities (Room, Booking, Person, etc.)
- **DAL:** Data access layer, EF Core context, migrations
- **BLL:** Business logic (booking rules, quotas, penalties)
- **WebApp:** ASP.NET Core web application

## Building & Running

### Prerequisites
- .NET 6.0 or higher
- Visual Studio, Visual Studio Code, or JetBrains Rider

### Web Application
```sh
cd Exam/WebApp
dotnet run
```

## Notes
- This project is a prototype and was developed under time constraints for an exam.
- Some features may be simplified or stubbed for demonstration purposes.

---

**Created for Programming in C# exam, January 2026.**
