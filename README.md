Declutter - Personal Journal App
A beautiful, intuitive digital journal application designed to help you capture thoughts, organize reflections, and preserve memories in a secure, private environment.

Live Demo: https://declutter-akhchsg3frg8degj.canadacentral-01.azurewebsites.net

✨ Features
🎨 Beautiful Interface
Elegant Design: Paper-like journal aesthetic with warm, inviting colors

Responsive Layout: Works seamlessly on desktop, tablet, and mobile devices

Smooth Animations: Subtle transitions and hover effects for enhanced UX

📝 Journal Management
Rich Text Editor: Powered by TinyMCE for formatted journal entries

Tag System: Organize entries with customizable tags

Search & Filter: Easily find past entries by title, content, or tags

Modal Views: View entries in beautiful modal popups without page navigation

🔒 Privacy & Security
User Authentication: Secure login system with ASP.NET Core Identity

Private Entries: All journal entries are private to each user

Data Encryption: Secure data storage and transmission

Privacy-Focused: No data sharing with third parties

⚡ Modern Technology Stack
ASP.NET Core 6.0: Robust backend framework

Entity Framework Core: Modern ORM for database operations

Bootstrap 5: Responsive frontend framework

Toastr Notifications: Beautiful alert system for user feedback

Font Awesome: Comprehensive icon library

🚀 Getting Started
Prerequisites
.NET 6.0 SDK

SQL Server (Express edition is sufficient)

Git

Installation
Clone the Repository

bash
git clone https://github.com/Cynthia-Motaung/declutter.git
cd declutter-journal
Configure Database

Update the connection string in appsettings.json:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DeclutterDb;Trusted_Connection=true;MultipleActiveResultSets=true"
}
Apply Database Migrations

bash
dotnet ef database update
Run the Application

bash
dotnet run
Open in Browser

Navigate to https://localhost:7000 (or the port shown in your terminal)

First Time Setup
Register a new account using the sign-up form

Verify your email address (if email confirmation is enabled)

Start creating your first journal entry!

📖 How to Use
Creating Entries
Click "Create New Entry" from the homepage or navigation bar

Add a title for your journal entry

Use the rich text editor to write your content

Select relevant tags or create new ones

Click "Create" to save your entry

Managing Entries
View: Click "View" to read entries in a beautiful modal

Edit: Click "Edit" to modify existing entries

Delete: Click "Delete" and confirm to remove entries

Organize: Use tags to categorize and filter your entries

Tags System
Create custom tags for different categories (e.g., #reflection, #goals, #memories)

Filter entries by tags for better organization

Manage tags during entry creation or editing

🛠️ Technology Stack
Backend
ASP.NET Core 6.0 - Web framework

Entity Framework Core - ORM and data access

ASP.NET Core Identity - Authentication and authorization

SQL Server - Database management

Frontend
Bootstrap 5 - Responsive UI framework

jQuery - DOM manipulation and AJAX

TinyMCE - Rich text editor

Toastr - Notification system

Font Awesome - Icons and UI elements

Development Tools
Visual Studio 2022 / VS Code - IDE

Git - Version control

SQL Server Management Studio - Database management

🔧 Configuration
App Settings
The application can be configured through appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connection string here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
Environment Settings
Development: Debug mode enabled, detailed errors

Production: Optimized for performance, secure settings

🤝 Contributing
We welcome contributions to improve Declutter! Here's how you can help:

Fork the repository

Create a feature branch (git checkout -b feature/amazing-feature)

Commit your changes (git commit -m 'Add amazing feature')

Push to the branch (git push origin feature/amazing-feature)

Open a Pull Request

Development Guidelines
Follow ASP.NET Core best practices

Maintain consistent coding style

Write meaningful commit messages

Test your changes thoroughly

📄 License
This project is licensed under the MIT License - see the LICENSE.md file for details.

🆘 Support
If you encounter any issues or have questions:

Check the FAQ section below

Search existing GitHub Issues

Create a new issue with detailed information

FAQ
Q: How do I reset my password?
A: Use the "Forgot Password" link on the login page.

Q: Can I export my journal entries?
A: Export functionality is planned for a future release.

Q: Is my data backed up?
A: Regular database backups are recommended for production deployments.

🚀 Future Enhancements
Mobile app version

Entry export functionality (PDF, Word)

Advanced search with filters

Entry templates

Mood tracking

Entry reminders and prompts

Data analytics and insights

Social sharing (optional)

Offline capability

🙏 Acknowledgments
Bootstrap for the responsive UI framework

TinyMCE for the rich text editor

Font Awesome for the beautiful icons

Toastr for notifications

