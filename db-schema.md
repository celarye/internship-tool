# Internschip Tool

Tool voor het beheer van stages:

## Features

- Login.
  - By all users.
- Register companies and a company employee.
  - By logged out users.
- Register company employees for a specific company.
  - By company employees.
- Adding internship proposals.
  - By company employees.
- Approving internship proposals.
  - By admins.
- Asigning approved internship proposals to students.
  - By company employees and admins.
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
- Username
- First name
- Second name
- Password (hashed using argon2-id)
- User type (FK)

### Admins

- ID (PK,FK)
- Username (FK)
- First name (FK)
- Second name (FK)
- Password (hashed using argon2-id) (FK)
- User type (FK)

### Students

- ID (PK,FK)
- Username (FK)
- First name (FK)
- Second name (FK)
- Password (hashed using argon2-id) (FK)
- User type (FK)
- Education (FK)
- Curriculum vitae file path
- Preferred internship

### Education

- ID (PK)
- Name

### Education Courses
- Education ID (PK,FK)
- Course (PK, FK)

### Course

- ID (PK)
- Name

### Teachers

- ID (PK,FK)
- Username (FK)
- First name (FK)
- Second name (FK)
- User type (FK)
- Password (hashed using argon2-id) (FK)

### Teacher Courses

- Teacher ID (PK,PK)
- Course ID (PK, FK)

### Company Employee

- ID (PK,FK)
- Username (FK)
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
- Approved
- Location
- Duration
- Course ID (FK)

### Internship Supervisors

- Internship ID (PK,FK)
- Teacher ID (PK,FK)

### Internship Mentors

- Internship ID (PK,FK)
- Company employee ID (PK,FK)

### Internship Candidates

- Internship ID (PK,FK)
- Student ID (PK,FK)
- Approved

### Internship Evaluation

- ID (PK)
- Internship ID (FK)
- Progress
- Score
- Note

