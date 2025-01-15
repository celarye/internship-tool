# Internschip Tool

Tool voor het beheer van stages:

## Features

- Login. DONE
  - By all users.
- Register companies and a company employee. DONE
  - By logged out users.
- Register company employees for a specific company. DONE
  - By company employees.
- Adding internship proposals.
  - By company employees.
- Approving internship proposals.
  - By admins.
- Asigning approved internship proposals to students.
  - By company employees and admins.
- Add CVs to students.
  - By students.
- Adding internship evaluations.
  - By teachers and company employees.

## Tables

- Users
  - Admins.
  - Students
  - Teachers
  - Company Employees
    - Companies
- Internship Proposals
- Internship Evaluations

### Users

- ID (PK)
- Email
- First name
- Second name
- Password (hashed)
- User type

### Admins

- ID (PK,FK)
- Email (FK)
- First name (FK)
- Second name (FK)
- Password (hashed using argon2-id) (FK)
- User type (FK)

### Students

- ID (PK,FK)
- Email (FK)
- First name (FK)
- Second name (FK)
- Password (hashed using argon2-id) (FK)
- User type (FK)
- Curriculum vitae file path
- Preferred internship

### Teachers

- ID (PK,FK)
- Email (FK)
- First name (FK)
- Second name (FK)
- User type (FK)
- Password (hashed using argon2-id) (FK)

### Company Employee

- ID (PK,FK)
- Email (FK)
- First name (FK)
- Second name (FK)
- Password (hashed using argon2-id) (FK)
- User type (FK)
- Company ID (FK)

### Company

- ID (PK)
- Name
- Company Manager (FK)

### Internships

- ID (PK)
- Company ID (FK)
- Title
- Description
- Internship mentor (FK)
- Internship teacher (FK)
- Internship student (FK)

### Internship Candidates

- Internship ID (PK,FK)
- Student ID (PK,FK)

### Internship Evaluation

- ID (PK)
- Internship ID (FK)
- Mentor evalaution 1
- Teacher evalaution 1
- Mentor evalaution 2
- Teacher evalaution 2
- Overal score
- Note

