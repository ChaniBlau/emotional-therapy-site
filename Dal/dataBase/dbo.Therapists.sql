CREATE TABLE [dbo].[therapists] (
    [Id]               INT          NOT NULL,
    [firstName]        VARCHAR (50) NOT NULL,
    [lastName]         VARCHAR (50) NOT NULL,
    [addressId]        VARCHAR (50) NULL,
    [yearsOfSeniority] INT          NULL,
    [salaryPerMonth]   FLOAT (53)   NULL,
    [typeOfGroupId]    INT          NULL,
    [numOfCourses]     INT          NOT NULL,
    [numOfStudents]    INT          NOT NULL,
    [email]            VARCHAR (50) NULL,
    [phoneNumber]      NCHAR (10)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

