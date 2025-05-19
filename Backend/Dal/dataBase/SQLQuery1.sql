ALTER TABLE Clients
ADD CONSTRAINT FK_Clients_Therapists FOREIGN KEY (TherapistId)
REFERENCES Therapists(Id);

ALTER TABLE EmptyAppointments
ADD CONSTRAINT FK_EmptyAppointments_Therapists FOREIGN KEY (TherapistId)
REFERENCES Therapists(Id);

ALTER TABLE BusyAppointments
ADD CONSTRAINT FK_BusyAppointments_Therapists FOREIGN KEY (TherapistId)
REFERENCES Therapists(Id);

ALTER TABLE BusyAppointments
ADD CONSTRAINT FK_BusyAppointments_Clients FOREIGN KEY (ClientId)
REFERENCES Clients(Id);
