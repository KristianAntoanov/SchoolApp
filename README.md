# SchoolApp
SchoolApp is a comprehensive web application for school management, featuring an electronic diary system, administrative functionalities, and communication tools. The system provides different access roles and features to facilitate interaction between teachers, parents, and administration.

## Features
- **Electronic Diary:** Complete system for managing grades, absences, and remarks with term and annual grade calculations
- **Admin Panel:** Powerful management tools for students, teachers, and system permissions
- **News System:** Multi-category news management with important announcements and student achievements
- **Gallery:** Image management system with album organization using Azure Blob Storage
- **Team Section:** Showcase of teaching staff and school management
- **Email Integration:** SendGrid implementation for registration confirmation and contact form

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core, Entity Framework Core
- **Languages:** C#, HTML, CSS, JavaScript (Bootstrap, Ajax, jQuery, Vanilla JavaScript)
- **Architecture:** MVC and Repository Pattern, Service Layer, Data Layer
- **Other Concepts:** ViewModels, Object-Oriented Programming (OOP) principles, Filters, Middlewares
- **Error Handling:** Robust error handling mechanisms for a smooth user experience, server-side and client-side validation
- **Security Measures:** SQL Injection prevention, XSS protection, CSRF prevention, parameter tampering prevention, AutoForgeryToken implementation
- **Dependency Injection:** Utilizes Dependency Injection and IoC Container for modular and scalable design
- **Data Storage:** MS SQL with EF Core for code-first approach, Azure Blob Storage for images
- **Authentication:** ASP.NET Identity for secure user authentication and role management
- **Cloud Services:** Azure Blob Storage, SendGrid Email Service

## Getting Started
To set up the project locally, follow these steps:

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any preferred .NET IDE
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Azure Account (for Blob Storage)
- SendGrid Account

### Installation
1. Clone the repository
2. Open the project in Visual Studio
3. Configure the following in secrets.json or appsettings.json:
   - Database connection string
   - Azure Blob Storage connection string
   - SendGrid API key
4. Apply migrations using `dotnet ef database update`
5. Run the project

### Usage
1. Navigate to http://localhost:your-port in your web browser
2. Register an account
3. Explore the features based on your role:
   - **Admin:** Full access to admin panel, user management, and content control
   - **Teacher:** Access to electronic diary, grade management, and absence tracking
   - **Parent:** View grades, absences, and school information

### Project Architecture
```
The project follows the architecture of ASP.NET Core Template by Nikolay Kostov, which separates the application into different layers:

- **Web Layer:** ASP.NET Core MVC
- **Service Layer:** Business logic implementation
- **Data Layer:** Repository pattern and database context
- **Common Layer:** Shared functionality and constants
- **ViewModels:** Data transfer objects for the views
- **Infrastructure:** Cross-cutting concerns and configurations

The template provides a robust foundation with built-in:
- Repository Pattern implementation
- Service Layer architecture
- Dependency Injection setup
- Identity configuration
- Data validation
       
```

### Contributing
Feel free to contribute to the project by submitting bug reports, feature requests, or pull requests. Follow the guidelines in the Contribution Guidelines.

### License
This project is licensed under the MIT License.

### Acknowledgments
- Special thanks to the .NET community
- Built using ASP.NET Core Template by Nikolay Kostov

### Live Demo

A live demo of BMW will be available in the future.
