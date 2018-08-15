
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/14/2018 02:23:44
-- Generated from EDMX file: C:\Projects\project0_7\project0_7\Banking.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Banking];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BusinessAccounts_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BusinessAccounts] DROP CONSTRAINT [FK_BusinessAccounts_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_CheckingAccounts_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CheckingAccounts] DROP CONSTRAINT [FK_CheckingAccounts_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_Loans_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Loans] DROP CONSTRAINT [FK_Loans_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_TermDeposits_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TermDeposits] DROP CONSTRAINT [FK_TermDeposits_Customers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BusinessAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessAccounts];
GO
IF OBJECT_ID(N'[dbo].[CheckingAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CheckingAccounts];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Loans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Loans];
GO
IF OBJECT_ID(N'[dbo].[TermDeposits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TermDeposits];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BusinessAccounts'
CREATE TABLE [dbo].[BusinessAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NULL,
    [Active] bit  NOT NULL,
    [Balance] decimal(19,4)  NOT NULL
);
GO

-- Creating table 'CheckingAccounts'
CREATE TABLE [dbo].[CheckingAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NULL,
    [Active] bit  NOT NULL,
    [Balance] decimal(19,4)  NOT NULL,
    [InterestRate] decimal(7,5)  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Address1] nvarchar(50)  NOT NULL,
    [Address2] nvarchar(50)  NULL,
    [City] nvarchar(50)  NOT NULL,
    [Zip] int  NOT NULL,
    [Phone] nvarchar(12)  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Loans'
CREATE TABLE [dbo].[Loans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NULL,
    [Active] bit  NOT NULL,
    [Balance] decimal(19,4)  NOT NULL,
    [InterestRate] decimal(7,5)  NOT NULL,
    [InstallmentPeriodDuration] int  NOT NULL,
    [InstallmentAmount] decimal(19,4)  NOT NULL,
    [InstallmentNextDue] datetime  NOT NULL
);
GO

-- Creating table 'TermDeposits'
CREATE TABLE [dbo].[TermDeposits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NULL,
    [Active] bit  NOT NULL,
    [Balance] decimal(19,4)  NOT NULL,
    [InterestRate] decimal(7,5)  NOT NULL
);
GO

-- Creating table 'Transactions1'
CREATE TABLE [dbo].[Transactions1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountId] nvarchar(max)  NOT NULL,
    [AccountType] nvarchar(max)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [BusinessAccount_Id] int  NOT NULL,
    [CheckingAccount_Id] int  NOT NULL,
    [Loan_Id] int  NOT NULL,
    [TermDeposit_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'BusinessAccounts'
ALTER TABLE [dbo].[BusinessAccounts]
ADD CONSTRAINT [PK_BusinessAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CheckingAccounts'
ALTER TABLE [dbo].[CheckingAccounts]
ADD CONSTRAINT [PK_CheckingAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Loans'
ALTER TABLE [dbo].[Loans]
ADD CONSTRAINT [PK_Loans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TermDeposits'
ALTER TABLE [dbo].[TermDeposits]
ADD CONSTRAINT [PK_TermDeposits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [AccountId], [AccountType] in table 'Transactions1'
ALTER TABLE [dbo].[Transactions1]
ADD CONSTRAINT [PK_Transactions1]
    PRIMARY KEY CLUSTERED ([Id], [AccountId], [AccountType] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'BusinessAccounts'
ALTER TABLE [dbo].[BusinessAccounts]
ADD CONSTRAINT [FK_BusinessAccounts_Customers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BusinessAccounts_Customers'
CREATE INDEX [IX_FK_BusinessAccounts_Customers]
ON [dbo].[BusinessAccounts]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'CheckingAccounts'
ALTER TABLE [dbo].[CheckingAccounts]
ADD CONSTRAINT [FK_CheckingAccounts_Customers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CheckingAccounts_Customers'
CREATE INDEX [IX_FK_CheckingAccounts_Customers]
ON [dbo].[CheckingAccounts]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Loans'
ALTER TABLE [dbo].[Loans]
ADD CONSTRAINT [FK_Loans_Customers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Loans_Customers'
CREATE INDEX [IX_FK_Loans_Customers]
ON [dbo].[Loans]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'TermDeposits'
ALTER TABLE [dbo].[TermDeposits]
ADD CONSTRAINT [FK_TermDeposits_Customers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TermDeposits_Customers'
CREATE INDEX [IX_FK_TermDeposits_Customers]
ON [dbo].[TermDeposits]
    ([UserId]);
GO

-- Creating foreign key on [BusinessAccount_Id] in table 'Transactions1'
ALTER TABLE [dbo].[Transactions1]
ADD CONSTRAINT [FK_BusinessAccountTransactions]
    FOREIGN KEY ([BusinessAccount_Id])
    REFERENCES [dbo].[BusinessAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BusinessAccountTransactions'
CREATE INDEX [IX_FK_BusinessAccountTransactions]
ON [dbo].[Transactions1]
    ([BusinessAccount_Id]);
GO

-- Creating foreign key on [CheckingAccount_Id] in table 'Transactions1'
ALTER TABLE [dbo].[Transactions1]
ADD CONSTRAINT [FK_CheckingAccountTransactions]
    FOREIGN KEY ([CheckingAccount_Id])
    REFERENCES [dbo].[CheckingAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CheckingAccountTransactions'
CREATE INDEX [IX_FK_CheckingAccountTransactions]
ON [dbo].[Transactions1]
    ([CheckingAccount_Id]);
GO

-- Creating foreign key on [Loan_Id] in table 'Transactions1'
ALTER TABLE [dbo].[Transactions1]
ADD CONSTRAINT [FK_LoanTransactions]
    FOREIGN KEY ([Loan_Id])
    REFERENCES [dbo].[Loans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LoanTransactions'
CREATE INDEX [IX_FK_LoanTransactions]
ON [dbo].[Transactions1]
    ([Loan_Id]);
GO

-- Creating foreign key on [TermDeposit_Id] in table 'Transactions1'
ALTER TABLE [dbo].[Transactions1]
ADD CONSTRAINT [FK_TermDepositTransactions]
    FOREIGN KEY ([TermDeposit_Id])
    REFERENCES [dbo].[TermDeposits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TermDepositTransactions'
CREATE INDEX [IX_FK_TermDepositTransactions]
ON [dbo].[Transactions1]
    ([TermDeposit_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------