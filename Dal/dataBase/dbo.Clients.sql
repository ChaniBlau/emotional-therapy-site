CREATE TABLE [dbo].[clients]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [firstName] VARCHAR(50) NOT NULL, 
    [lastName] VARCHAR(50) NOT NULL, 
    [address] NCHAR(10) NOT NULL, 
    [age] FLOAT NOT NULL, 
    [typeOfGroup] NCHAR(10) NULL, 
    [isPrivate] NCHAR(10) NULL, 
    [isPaid] NCHAR(10) NULL, 
    [teacherName] VARCHAR(50) NULL, 
    [level] NCHAR(10) NULL, 
    
)
