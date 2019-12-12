/*
Insert initial data for TqPathways
*/

SET IDENTITY_INSERT [dbo].[TqPathway] ON

MERGE INTO [dbo].[TqPathway] AS Target 
USING (VALUES 
  (1, N'DSP', N'Design, Surveying and Planning', 1),
  (2, N'Edu', N'Education', 2),
  (3, N'DPDD', N'Digital Production, Design and Development', 3)
  )
  AS Source ([Id], [Code], [Name], [RouteId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Code] <> Source.[Code] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[RouteId] <> Source.[RouteId])) 
THEN 
UPDATE SET 
	[Code] = Source.[Code],
	[Name] = Source.[Name],
	[RouteId] = Source.[RouteId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Code], [Name], [RouteId], [CreatedBy]) 
	VALUES ([Id], [Code], [Name], [RouteId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqPathway] OFF
