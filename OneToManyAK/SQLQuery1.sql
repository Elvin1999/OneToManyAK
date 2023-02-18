CREATE DATABASE OneToManyDb
GO
USE OneToManyDb

GO

CREATE TABLE Groups(
[GroupId] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
[Title] NVARCHAR(30) NOT NULL
)

GO

CREATE TABLE Students(
[StudentId] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
[Firstname] NVARCHAR(30) NOT NULL,
[Age] INT NOT NULL,
[GroupId] INT FOREIGN KEY REFERENCES Groups([GroupId])
)

GO

INSERT INTO Groups([Title])
VALUES('2201_az'),('12012_az')

GO

INSERT INTO Students([Firstname],[Age],[GroupId])
VALUES('John',24,1),('Leyla',32,1),
('Mike',33,2),('Jessica',86,2)

SELECT * FROM Students