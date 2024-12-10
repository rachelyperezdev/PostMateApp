# PostMate App  

PostMate App is a social networking platform built using ASP.NET Core MVC (version 7). This application allows users to create, manage, and interact with posts, enabling them to connect and share with their friends.  

---

## **Key Features**  

### **Authentication**  
- **Login:**  
  - Redirect logged-in users attempting to access the login page to the home page.  
  - Displays error messages for incorrect credentials or inactive accounts.  
  - Allows users to reset their passwords via email.  

- **Registration:**  
  - New users can register with the following fields: Name, Last Name, Phone, Email, Profile Picture, Username, Password, and Confirm Password.  
  - Validates data for required fields, unique usernames, and matching passwords.  
  - Sends account activation emails to new users.  

---

### **Home (Posts)**  
- Displays all posts created by the logged-in user, sorted by recency.  
- Features for posts:  
  - Add comments and reply to specific comments (threaded discussions).  
  - Edit or delete posts (comments and replies are preserved).  
  - Create new posts, including text, images, and YouTube videos.  
- Displays user profile pictures next to actions.  

---

### **Friends**  
- View friends' posts, sorted by recency.  
- Manage friends:  
  - View a list of friends with names, profile pictures, and usernames.  
  - Add or remove friends:  
    - Add friends by entering their usernames.  
    - Displays appropriate messages if a user does not exist.  

---

### **Profile**  
- Edit personal information, including Name, Last Name, Phone, Email, Profile Picture, and Password.  
- Fields are pre-populated except for Password and Confirm Password.  
- Password changes require matching values.  
- Includes navigation back to the home page.  

---

### **Security**  
- Restricts access to authenticated users only for posts, friends, and profile sections.  
- Redirects unauthenticated users to the login page with an access error message.  

---

## **Technical Specifications**  

- **Architecture:** Onion Architecture (applied to 100% of the project).  
- **Validation:** ViewModels with server-side validation.  
- **Data Persistence:** Entity Framework with Code-First approach.  
- **Styling:** Bootstrap for user-friendly and responsive UI.  
- **Generic Patterns:** Repository and Service layers.  
- **Data Mapping:** AutoMapper for streamlined object mapping.  
- **Email Service:** Implemented in the shared layer for account activation and password resets.  

---

## Getting Started

Follow these instructions to set up the project locally.

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- * [Visual Studio](https://visualstudio.microsoft.com/) or any preferred IDE for [ASP.NET](http://asp.net/) Core development

### Installation

1. **Clone the repository**:

```bash
git clone https://github.com/rachelyperezdev/PostMateApp.git
cd PostMateApp
```

1. **Set up the database**:
- Update the `appsettings.json` file with your SQL Server connection string.
- Run the following commands to apply migrations and update the database:

```bash
dotnet ef database update
```

1. **Run the application**:

```bash
dotnet run
```
