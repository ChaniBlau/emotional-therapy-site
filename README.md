# 🧒 Emotional Therapy Site
A Child Therapy Clinic Appointment Management System
A dynamic appointment scheduling system with role-based permissions and an intuitive interface for patients, therapists, and admins.

## 🛠️ Technologies ##
Frontend: React + TypeScript

Backend: ASP.NET Core Web API (C#)

Database: SQL Server

DevOps & Deployment: Docker, Docker Compose, Google Cloud Run + Cloud Storage (in progress)

## ✨ Key Features ##
👤 User Roles and Permissions: Clients, Therapists, Admin

🗓️ Appointment Management: View, Schedule, and Cancel Appointments

🔍 Treatment Filtering: Filter appointments by treatment type

🔐 User Authentication: Login and registration functionality

🧩 Solid Architecture: DAL / BL / API following SOLID principles

## 🚀 Local Setup Instructions ##
Assuming you have .NET and Node.js installed:

bash
Copy
Edit
# Back-end:
cd Server
dotnet restore
dotnet run

# Front-end:
cd Client
npm install
npm start
