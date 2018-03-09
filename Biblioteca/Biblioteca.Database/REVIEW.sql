create table REVIEW(
   ReviewId int IDENTITY(1,1) PRIMARY KEY,
   ImprumutId int FOREIGN KEY REFERENCES dbo.IMPRUMUT(ImprumutId),
   Text nvarchar(max)
);