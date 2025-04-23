CREATE TABLE Locations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Latitude FLOAT NOT NULL,
    Longitude FLOAT NOT NULL,
    Rating FLOAT
);