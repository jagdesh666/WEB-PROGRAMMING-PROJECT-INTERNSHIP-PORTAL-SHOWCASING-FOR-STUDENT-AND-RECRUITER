# 🏆 AI-Powered Internship & Skill Matching Platform

🌐 **Live Demo:**  
https://internship-portal-n2fo.onrender.com  

---

## 📌 Project Overview
Yeh platform students aur recruiters ke darmiyan ek smart bridge hai.  
Yeh sirf ek job portal nahi hai, balkay isme ek **Rule-based AI Matching Engine** aur ek **Local LLM (Llama 3) integrated chatbot** hai jo students ko professional resume banane mein madad karta hai.

---

## 🛠 Tech Stack (Deep Details)

### 1. Backend (The Brain)
- **Framework:** ASP.NET Core 9.0 MVC  
- **Architecture:** Model-View-Controller (MVC)  
- **Security:** BCrypt.Net (password hashing with salt)  
- **Authentication:** Cookie-based + Role-Based Access Control (RBAC)  

### 2. Frontend (The Face)
- **UI Framework:** Bootstrap 5 (custom SaaS-style components)  
- **Data Visualization:** Chart.js  
- **Icons & Fonts:** Bootstrap Icons, Google Fonts (Poppins)  
- **Notifications:** SweetAlert2  
- **PDF Generation:** html2pdf.js  

### 3. Database (The Memory)
- **System:** PostgreSQL  
- **Provider:** Aiven Cloud (Production) + Local PostgreSQL  
- **ORM:** Entity Framework Core (Code-First Migrations)  

### 4. Artificial Intelligence (The Intelligence)
- **Matching Engine:** Custom LINQ-based algorithm  
- **Techniques:** Tokenization + Synonym Mapping  
- **Resume Bot:** Ollama API + Llama 3 (offline LLM chatbot)  

---

## 🚀 Key Features in Depth

### A. Role-Based Access Control (RBAC)
**Student:**
- Dashboard access  
- Skill matching  
- Internship history  
- AI resume assistant  

**Recruiter:**
- Job posting (CRUD)  
- Applicant tracking  
- Resume viewing  
- Hiring analytics  

---

### B. Smart Skill Matching Algorithm
System internships ke required skills ko student ke skills se compare karta hai:

- **Tokenization:** Comma-separated strings → arrays  
- **Normalization:** Lowercase mapping (case-insensitive)  
- **Synonym Mapping:**  
  - JS = JavaScript  
  - .NET = C#  
- **Formula:**  

Match Score = (Matches / Required Skills) * 100


---

### C. AI Resume Chatbot (Local LLM)
- Student Ollama (Llama 3) ke saath chat karta hai  
- AI unki details ko **professional HTML resume** mein convert karta hai  
- Live preview show hota hai  
- Resume PDF mein download karke upload kiya ja sakta hai  

---

### D. Data Insights & Analytics
Recruiter dashboard par:
- Total applicants  
- Hiring status  
- "Top Talent" (match score > 70%)  
- Real-time charts & stats  

---

## 🏗 Database Schema (ER Summary)

- **Users**  
`UserId, FullName, Email, PasswordHash, Role`

- **Internships**  
`InternshipId, Title, CompanyName, RequiredSkills, Description`

- **StudentProfiles**  
`ProfileId, UserId, Skills, Education, ResumePath`

- **Applications**  
`ApplicationId, InternshipId, StudentId, Status, MatchScore`

---

## 💻 Local Setup Instructions

### Prerequisites
- .NET 9.0 SDK  
- PostgreSQL + DBeaver  
- Ollama (with llama3 model)  

---

### Steps

#### 1. Clone Repository
```bash
git clone <repo-url>
2. Configure Database
Open appsettings.json
Add your local PostgreSQL password
3. Run Migrations
dotnet ef database update
4. Setup AI (Ollama)
ollama pull llama3
5. Run Project
dotnet run
☁️ Deployment Details
Platform: Render.com (Docker container)
Database: Aiven PostgreSQL (Cloud)
DevOps: Environment variables for secure DATABASE_URL management
