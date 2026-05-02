# 🏆 AI-Powered Internship & Skill Matching Platform

🌐 Live Demo:
https://internship-portal-n2fo.onrender.com

--------------------------------------------------

## 📌 Project Overview
This platform acts as a smart bridge between students and recruiters.
It is not just a job portal — it includes a Rule-based AI Matching Engine
and a Local LLM (Llama 3) integrated chatbot that helps students
create professional resumes.

--------------------------------------------------

## 🛠 Tech Stack (Deep Details)

### 1. Backend (The Brain)
- Framework: ASP.NET Core 9.0 MVC
- Architecture: Model-View-Controller (MVC)
- Security: BCrypt.Net (password hashing with salt)
- Authentication: Cookie-based + Role-Based Access Control (RBAC)

### 2. Frontend (The Face)
- UI Framework: Bootstrap 5 (custom SaaS-style components)
- Data Visualization: Chart.js
- Icons & Fonts: Bootstrap Icons, Google Fonts (Poppins)
- Notifications: SweetAlert2
- PDF Generation: html2pdf.js

### 3. Database (The Memory)
- System: PostgreSQL
- Provider: Aiven Cloud (Production) + Local PostgreSQL
- ORM: Entity Framework Core (Code-First Migrations)

### 4. Artificial Intelligence (The Intelligence)
- Matching Engine: Custom LINQ-based algorithm
- Techniques: Tokenization + Synonym Mapping
- Resume Bot: Ollama API + Llama 3 (offline LLM chatbot)

--------------------------------------------------

## 🚀 Key Features in Depth

### A. Role-Based Access Control (RBAC)

Student:
- Dashboard access
- Skill matching
- Internship history
- AI resume assistant

Recruiter:
- Job posting (CRUD)
- Applicant tracking
- Resume viewing
- Hiring analytics

--------------------------------------------------

### B. Smart Skill Matching Algorithm

The system compares internship required skills with student profile skills:

- Tokenization: Converts comma-separated strings into arrays
- Normalization: Lowercase mapping (case-insensitive)
- Synonym Mapping:
  JS = JavaScript
  .NET = C#
- Formula:
  Match Score = (Matches / Required Skills) * 100

--------------------------------------------------

### C. AI Resume Chatbot (Local LLM)

- The student interacts with Ollama (Llama 3)
- AI converts user details into a professional HTML resume
- Live preview is displayed
- Resume can be downloaded as a PDF and uploaded to the portal

--------------------------------------------------

### D. Data Insights & Analytics

On the recruiter dashboard:
- Total applicants
- Hiring status
- Top Talent (match score > 70%)
- Real-time charts and statistics

--------------------------------------------------

## 🏗 Database Schema (ER Summary)

Users:
UserId, FullName, Email, PasswordHash, Role

Internships:
InternshipId, Title, CompanyName, RequiredSkills, Description

StudentProfiles:
ProfileId, UserId, Skills, Education, ResumePath

Applications:
ApplicationId, InternshipId, StudentId, Status, MatchScore

--------------------------------------------------

## 💻 Local Setup Instructions

Prerequisites:
- .NET 9.0 SDK
- PostgreSQL + DBeaver
- Ollama (with llama3 model)

Steps:

1. Clone Repository
git clone <repo-url>

2. Configure Database
- Open appsettings.json
- Add your PostgreSQL password

3. Run Migrations
dotnet ef database update

4. Setup AI (Ollama)
ollama pull llama3

5. Run Project
dotnet run

--------------------------------------------------

## ☁️ Deployment Details

- Platform: Render.com (Docker container)
- Database: Aiven PostgreSQL (Cloud)
- DevOps: Environment variables used for secure DATABASE_URL management

--------------------------------------------------

## 🚀 Final Note

This project is not just an internship portal —
it is an AI-powered career assistant system
that makes the hiring process smarter and more efficient.
