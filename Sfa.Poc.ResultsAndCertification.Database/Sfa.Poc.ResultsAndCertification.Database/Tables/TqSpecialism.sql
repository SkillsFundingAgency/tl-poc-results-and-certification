CREATE TABLE [dbo].[TqSpecialism]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] Nvarchar(100) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
	[PathwayId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqSpecialism] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqSpecialism_TqPathway] FOREIGN KEY ([PathwayId]) REFERENCES [TqPathway]([Id])
)
