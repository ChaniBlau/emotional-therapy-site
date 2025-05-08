CREATE TABLE [dbo].[EmptyAppointments] (
    [Code]        INT        IDENTITY (1, 1) NOT NULL,
    [Date]        DATE       NOT NULL,
    [Time]        TIME (7)   NOT NULL,
    [TherapistId] NCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Code] ASC),
    CONSTRAINT [FK_EmptyAppointments_Therapists] FOREIGN KEY ([TherapistId]) REFERENCES [dbo].[Therapists] ([Id]) ON DELETE CASCADE
);

