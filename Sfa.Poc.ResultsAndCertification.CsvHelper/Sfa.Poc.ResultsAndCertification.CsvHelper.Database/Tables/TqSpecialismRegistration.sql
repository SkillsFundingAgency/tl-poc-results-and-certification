CREATE TABLE [dbo].[TqSpecialismRegistration]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TqRegistrationId] INT NOT NULL,
    [TlSpecialismId] INT NOT NULL,
	[Status] INT NOT NULL DEFAULT 1,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqSpecialismRegistration] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqSpecialismRegistration_TqRegistration] FOREIGN KEY ([TqRegistrationId]) REFERENCES [TqRegistration]([Id]),
	CONSTRAINT [FK_TqSpecialismRegistration_TlSpecialism] FOREIGN KEY ([TlSpecialismId]) REFERENCES [TlSpecialism]([Id])
)
