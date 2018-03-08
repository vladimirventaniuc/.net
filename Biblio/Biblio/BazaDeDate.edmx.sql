
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/06/2018 17:27:08
-- Generated from EDMX file: C:\Users\Vladimir\source\repos\Biblio\Biblio\BazaDeDate.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TestPerson];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CarteAutor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarteSet] DROP CONSTRAINT [FK_CarteAutor];
GO
IF OBJECT_ID(N'[dbo].[FK_ImprumutCarte]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarteSet] DROP CONSTRAINT [FK_ImprumutCarte];
GO
IF OBJECT_ID(N'[dbo].[FK_ImprumutCititor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImprumutSet] DROP CONSTRAINT [FK_ImprumutCititor];
GO
IF OBJECT_ID(N'[dbo].[FK_ReviewImprumut]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReviewSet] DROP CONSTRAINT [FK_ReviewImprumut];
GO
IF OBJECT_ID(N'[dbo].[FK_CarteGen]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarteSet] DROP CONSTRAINT [FK_CarteGen];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[GenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GenSet];
GO
IF OBJECT_ID(N'[dbo].[AutorSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AutorSet];
GO
IF OBJECT_ID(N'[dbo].[CarteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarteSet];
GO
IF OBJECT_ID(N'[dbo].[ImprumutSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImprumutSet];
GO
IF OBJECT_ID(N'[dbo].[CititorSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CititorSet];
GO
IF OBJECT_ID(N'[dbo].[ReviewSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReviewSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'GenSet'
CREATE TABLE [dbo].[GenSet] (
    [GenId] int IDENTITY(1,1) NOT NULL,
    [Descriere] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AutorSet'
CREATE TABLE [dbo].[AutorSet] (
    [AutorId] int IDENTITY(1,1) NOT NULL,
    [Nume] nvarchar(20)  NOT NULL,
    [Prenume] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CarteSet'
CREATE TABLE [dbo].[CarteSet] (
    [CarteId] int IDENTITY(1,1) NOT NULL,
    [Titlu] nvarchar(50)  NOT NULL,
    [AutorId] int  NOT NULL,
    [Autor_AutorId] int  NOT NULL,
    [GenId_GenId] int  NOT NULL
);
GO

-- Creating table 'ImprumutSet'
CREATE TABLE [dbo].[ImprumutSet] (
    [ImprumutId] int IDENTITY(1,1) NOT NULL,
    [DataImprumut] datetime  NOT NULL,
    [DataScadenta] datetime  NOT NULL,
    [DataRestituire] datetime  NOT NULL,
    [CarteId] int  NOT NULL,
    [CititorId_CitirorId] int  NOT NULL
);
GO

-- Creating table 'CititorSet'
CREATE TABLE [dbo].[CititorSet] (
    [CitirorId] int IDENTITY(1,1) NOT NULL,
    [Nume] nvarchar(15)  NOT NULL,
    [Prenume] nvarchar(15)  NOT NULL,
    [Adresa] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Stare] smallint  NOT NULL
);
GO

-- Creating table 'ReviewSet'
CREATE TABLE [dbo].[ReviewSet] (
    [ReviewId] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [ImprumutId_ImprumutId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [GenId] in table 'GenSet'
ALTER TABLE [dbo].[GenSet]
ADD CONSTRAINT [PK_GenSet]
    PRIMARY KEY CLUSTERED ([GenId] ASC);
GO

-- Creating primary key on [AutorId] in table 'AutorSet'
ALTER TABLE [dbo].[AutorSet]
ADD CONSTRAINT [PK_AutorSet]
    PRIMARY KEY CLUSTERED ([AutorId] ASC);
GO

-- Creating primary key on [CarteId] in table 'CarteSet'
ALTER TABLE [dbo].[CarteSet]
ADD CONSTRAINT [PK_CarteSet]
    PRIMARY KEY CLUSTERED ([CarteId] ASC);
GO

-- Creating primary key on [ImprumutId] in table 'ImprumutSet'
ALTER TABLE [dbo].[ImprumutSet]
ADD CONSTRAINT [PK_ImprumutSet]
    PRIMARY KEY CLUSTERED ([ImprumutId] ASC);
GO

-- Creating primary key on [CitirorId] in table 'CititorSet'
ALTER TABLE [dbo].[CititorSet]
ADD CONSTRAINT [PK_CititorSet]
    PRIMARY KEY CLUSTERED ([CitirorId] ASC);
GO

-- Creating primary key on [ReviewId] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [PK_ReviewSet]
    PRIMARY KEY CLUSTERED ([ReviewId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Autor_AutorId] in table 'CarteSet'
ALTER TABLE [dbo].[CarteSet]
ADD CONSTRAINT [FK_CarteAutor]
    FOREIGN KEY ([Autor_AutorId])
    REFERENCES [dbo].[AutorSet]
        ([AutorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarteAutor'
CREATE INDEX [IX_FK_CarteAutor]
ON [dbo].[CarteSet]
    ([Autor_AutorId]);
GO

-- Creating foreign key on [CititorId_CitirorId] in table 'ImprumutSet'
ALTER TABLE [dbo].[ImprumutSet]
ADD CONSTRAINT [FK_ImprumutCititor]
    FOREIGN KEY ([CititorId_CitirorId])
    REFERENCES [dbo].[CititorSet]
        ([CitirorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ImprumutCititor'
CREATE INDEX [IX_FK_ImprumutCititor]
ON [dbo].[ImprumutSet]
    ([CititorId_CitirorId]);
GO

-- Creating foreign key on [ImprumutId_ImprumutId] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [FK_ReviewImprumut]
    FOREIGN KEY ([ImprumutId_ImprumutId])
    REFERENCES [dbo].[ImprumutSet]
        ([ImprumutId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReviewImprumut'
CREATE INDEX [IX_FK_ReviewImprumut]
ON [dbo].[ReviewSet]
    ([ImprumutId_ImprumutId]);
GO

-- Creating foreign key on [GenId_GenId] in table 'CarteSet'
ALTER TABLE [dbo].[CarteSet]
ADD CONSTRAINT [FK_CarteGen]
    FOREIGN KEY ([GenId_GenId])
    REFERENCES [dbo].[GenSet]
        ([GenId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarteGen'
CREATE INDEX [IX_FK_CarteGen]
ON [dbo].[CarteSet]
    ([GenId_GenId]);
GO

-- Creating foreign key on [CarteId] in table 'ImprumutSet'
ALTER TABLE [dbo].[ImprumutSet]
ADD CONSTRAINT [FK_ImprumutId]
    FOREIGN KEY ([CarteId])
    REFERENCES [dbo].[CarteSet]
        ([CarteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ImprumutId'
CREATE INDEX [IX_FK_ImprumutId]
ON [dbo].[ImprumutSet]
    ([CarteId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------