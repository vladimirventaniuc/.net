create table IMPRUMUT(
   ImprumutId int IDENTITY(1,1) PRIMARY KEY,
   CarteId int FOREIGN KEY REFERENCES dbo.AUTOR(AutorId),
   CititorId int FOREIGN KEY REFERENCES dbo.CITITOR(CititorId),
   DataImprumut date not null,
   DataScadenta date not null,
   DataRestituire date
);