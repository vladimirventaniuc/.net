create table CARTE(
   CarteId int IDENTITY(1,1) PRIMARY KEY,
   AutorId int FOREIGN KEY REFERENCES dbo.AUTOR(AutorId),
   Titlu nvarchar(50),
   GenId int FOREIGN KEY REFERENCES dbo.GEN(GenId)
);
