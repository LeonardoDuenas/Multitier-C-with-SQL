IF db_id('College1en') IS NULL CREATE DATABASE College1en;
GO

USE College1en

CREATE TABLE Programs(ProgId VARCHAR (5) NOT NULL,
                    Prog_Name VARCHAR (50) NOT NULL,
                    PRIMARY KEY (ProgId));

CREATE TABLE Courses(CId VARCHAR (7) NOT NULL,
                    CName VARCHAR (50) NOT NULL,
                    ProgId VARCHAR (5) NOT NULL,
                    PRIMARY KEY (CId),
                    FOREIGN KEY (ProgId) REFERENCES
                    Programs(ProgId)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION);

CREATE TABLE Students(StId VARCHAR (10) NOT NULL,
                    StName VARCHAR (50) NOT NULL,
                    ProgId VARCHAR (5) NOT NULL,
                    PRIMARY KEY (StId),
                    FOREIGN KEY (ProgId) REFERENCES
                    Programs(ProgId)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE);

CREATE TABLE Enrollment(StId VARCHAR (10) NOT NULL,
                    CId VARCHAR (7) NOT NULL,
                    final_grade INT,
                    PRIMARY KEY (StId, CId),
                    FOREIGN KEY (StId) REFERENCES
                    Students(StId)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE,
                    FOREIGN KEY (CId) REFERENCES
                    Courses(CId)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE);

GO

INSERT INTO Programs
VALUES ('P1011', 'Programming'), 
('P1012', 'Fashion'),
('P1013', 'Management'),
('P1014', 'Tourism');

INSERT INTO Courses
VALUES ('C111001', 'CS', 'P1011'), 
('C111002', 'FKL', 'P1012'),
('C111003', 'MNG', 'P1011');

INSERT INTO Students
VALUES ('S20231081', 'Leonardo Duenas', 'P1011'), 
('S20231082', 'Marco Livias', 'P1012'),
('S20231083', 'Benjamin Esquivel', 'P1011'),
('S20231084', 'Arturo Tapia', 'P1012');

INSERT INTO Enrollment
VALUES ('S20231082', 'C111002', NULL),
('S20231081', 'C111001', NULL),
('S20231084', 'C111002', 90);

GO

--SELECT * FROM Courses;