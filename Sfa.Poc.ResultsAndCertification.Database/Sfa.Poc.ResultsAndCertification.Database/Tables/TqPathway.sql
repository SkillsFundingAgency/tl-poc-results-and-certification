CREATE TABLE [dbo].[TqPathway]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] Nvarchar(100) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
	[RouteId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqPathway] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqPathway_TqRoute] FOREIGN KEY ([RouteId]) REFERENCES [TqRoute]([Id])
)
