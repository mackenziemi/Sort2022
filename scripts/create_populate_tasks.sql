CREATE TABLE Tasks  (
    TaskId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(1024) NULL,
    IsCompleted BIT DEFAULT 0
)

INSERT INTO Tasks (Name, Description) VALUES ('Get Milk', 'Go to the store and get milk')
INSERT INTO Tasks (Name) VALUES ('Work on SORT Presentation')