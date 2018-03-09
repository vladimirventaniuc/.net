create table CITITOR(
   CititorId int IDENTITY(1,1) PRIMARY KEY,
   Nume nvarchar(15),
   Prenume nvarchar(15),
   Adresa nvarchar(50),
   Email nvarchar(50) constraint CK_Email check( Email LIKE '%_@_%_.__%'), 
   Stare bit
);