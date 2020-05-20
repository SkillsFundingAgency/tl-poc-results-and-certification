CREATE TABLE [dbo].[TqRegistration]
(
	[Id] INT IDENTITY(1,1) NOT NULL,    
	[UniqueLearnerNumber] BIGINT NOT NULL,
	[Firstname] NVARCHAR(50) NULL, 
	[Lastname] NVARCHAR(50) NULL, 
	[DateofBirth] DATE NULL,
	[TqProviderId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[Status] INT NOT NULL DEFAULT 1,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqRegistration] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqRegistration_TqProvider] FOREIGN KEY ([TqProviderId]) REFERENCES [TqProvider]([Id])
)
