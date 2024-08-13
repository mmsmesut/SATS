CREATE TABLE mlshop.Ssstudents (
    StudentID SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Email VARCHAR(150) UNIQUE NOT NULL,
    EnrollmentDate DATE NOT NULL
);