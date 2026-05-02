🏆 AI-Powered Internship & Skill Matching Platform
🌐 Live Demo: https://internship-portal-n2fo.onrender.com
📌 Project Overview
Yeh platform students aur recruiters ke darmiyan ek smart bridge hai. Yeh sirf ek job portal nahi hai, balkay isme ek Rule-based AI Matching Engine aur ek Local LLM (Llama 3) integrated chatbot hai jo students ko professional resume banane mein madad karta hai.
🛠 Tech Stack (Deep Details)
1. Backend (The Brain)
Framework: ASP.NET Core 9.0 MVC.
Architecture: Model-View-Controller (MVC) for clean separation of concerns.
Security: BCrypt.Net for cryptographic password hashing (one-way salt & hash).
Authentication: Cookie-based Authentication with Role-Based Access Control (RBAC).
2. Frontend (The Face)
UI Framework: Bootstrap 5 (with customized SaaS-style components).
Data Visualization: Chart.js for recruiter analytics and doughnut charts.
Icons & Fonts: Bootstrap Icons and Google Fonts (Poppins).
Notifications: SweetAlert2 for animated user feedback.
PDF Generation: html2pdf.js for client-side resume downloading.
3. Database (The Memory)
System: PostgreSQL (Relational Database).
Provider: Aiven Cloud (for production) and Local Windows PostgreSQL.
ORM: Entity Framework Core (EF Core) with Code-First Migrations.
4. Artificial Intelligence (The Intelligence)
Matching Engine: Custom LINQ-based algorithm with Tokenization and Synonym Mapping.
Resume Bot: Ollama API integrated with the Llama 3 model for offline conversational AI.
🚀 Key Features in Depth
A. Role-Based Access Control (RBAC)
Student: Dashboard access, skill matching, internship application history, and AI resume assistant.
Recruiter: Job posting management (CRUD), applicant tracking, resume viewing, and hiring analytics.
B. Smart Skill Matching Algorithm
System har internship ke required skills ko student ke profile skills se compare karta hai:
Tokenization: Comma-separated strings ko arrays mein convert karta hai.
Normalization: Case-sensitivity khatam karne ke liye lowercase mapping.
Synonym Mapping: Code recognize karta hai ke JS = JavaScript, .NET = C#, etc.
Intersection Formula: (Matches / Required) * 100 ke mutabiq real-time match score generate karta hai.
C. AI Resume Chatbot (Local LLM)
Agar student ke paas resume nahi hai, toh wo Ollama (Llama 3) ke saath chat karke apni details provide karta hai.
AI un details ko professional HTML Resume mein format karta hai jo Live Preview window mein nazar aata hai.
Student usse direct PDF format mein download karke portal par upload kar sakta hai.
D. Data Insights & Analytics
Recruiter dashboard par total applicants, hired status, aur "Top Talent" (match score > 70%) ke real-time stats cards aur charts show hote hain.
🏗 Database Schema (ER Summary)
Users Table: UserId, FullName, Email, PasswordHash, Role.
Internships Table: InternshipId, Title, CompanyName, RequiredSkills, Description.
StudentProfiles Table: ProfileId, UserId, Skills, Education, ResumePath.
Applications Table: ApplicationId, InternshipId, StudentId, Status, MatchScore.
💻 Local Setup Instructions
Prerequisites:
.NET 9.0 SDK
PostgreSQL & DBeaver
Ollama (with llama3 model)
Steps:
Clone the Repo: git clone <repo-url>
Database Configuration: appsettings.json mein apna local PostgreSQL password enter karein.
Run Migrations:
code
Bash
dotnet ef database update
Setup AI: Ensure Ollama is running and run ollama pull llama3.
Run Project:
code
Bash
dotnet run
☁️ Deployment Details
Platform: Hosted on Render.com using Docker containerization.
Database: Cloud-hosted Aiven PostgreSQL.
DevOps: Environment variables are used to securely manage DATABASE_URL between Local and Production environments.
