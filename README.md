# QuizHub - Corporate Quiz Management Platform
QuizHub is an ASP.NET Core MVC project designed for corporate environments to streamline the creation, assignment, and participation in quizzes. The platform enables the training department to create and assign quizzes to employees based on their departments. It supports three user roles: Admin, Editor, and User, ensuring secure and efficient management of quiz processes. QuizHub aims to improve employee engagement and performance evaluation through interactive and department-specific assessments.


## Key Features
1. User-friendly interface for quiz creation and participation.
2. Secure user authentication and role-based access (Admin/Student).
3. Real-time quiz results and performance analytics.
4. Multiple-choice and true/false question types.
5. Responsive design for mobile and desktop users.

## Project Structure
Entities (Domain Layer)
Contains the core data models and entities of the application.
Example: Classes such as User, Quiz, Department, and Question are defined here.

Repositories (Data Access Layer)
Manages database interactions, including CRUD operations.
Provides database queries and connections using Entity Framework Core.

Services (Business Logic Layer)
Implements the application's business rules and logic.
Example: Handles quiz assignments, department-based filtering, and result analysis.

Presentation (UI and API Layer)
Includes the user interface and APIs for the application.
Provides Razor Pages for Admin, Editor, and User roles with role-specific views.
Processes API requests using the MVC pattern and delegates operations to the service layer.


##Technologies and Tools
Backend
Framework: ASP.NET Core MVC
Database: SQL Server
ORM: Entity Framework Core
Logging: SeriLog
Dependency Injection
Localization
Middleware

Frontend
UI Framework: Razor Pages and MVC Views
Styling: Bootstrap
Scripting: JavaScript, JQuery

Key Infrastructure Features
Authentication and Authorization
Custom Routing
Global Exception Handling



## Screenshots

<div style="display: flex; justify-content: space-between;">
  <img src="QuizHubPresentation/wwwroot/images/admindashboard.png" alt="Dashboard View" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/admin-quiz-settings.png" alt="Quiz Page" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/addingquiz.png" alt="Add Quiz" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/uploadingquiz.png" alt="Uploading Quiz " width="23%" />
</div>
<div style="display: flex; justify-content: space-between;">
  <img src="QuizHubPresentation/wwwroot/images/user-settings.png" alt="User Page" width="30%" />
  <img src="QuizHubPresentation/wwwroot/images/createuser.png" alt="Add User" width="30%" />
  <img src="QuizHubPresentation/wwwroot/images/assign.png" alt="Assign Quiz" width="30%" />
</div>
<div style="display: flex; justify-content: space-between;">
  <img src="QuizHubPresentation/wwwroot/images/home.png" alt="userhome" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/login.png" alt="login" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/register.png" alt="register" width="23%" />  
  <img src="QuizHubPresentation/wwwroot/images/profile.png" alt="userprofile" width="23%" />

</div>
<div style="display: flex; justify-content: space-between;">
  <img src="QuizHubPresentation/wwwroot/images/pendingquizzes.png" alt="pending" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/retakequizzes.png" alt="retake" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/solving.png" alt="solve" width="23%" />
  <img src="QuizHubPresentation/wwwroot/images/quizfailresult.png" alt="quizfailresult" width="23%" />
</div>
<div style="display: flex; justify-content: space-between;">
  <img src="QuizHubPresentation/wwwroot/images/db.png" alt="db" width="43%" />
  <img src="QuizHubPresentation/wwwroot/images/vs.png" alt="vs" width="43%" />
</div>
