/*
Insert initial data for TqRoutes
*/

SET IDENTITY_INSERT [dbo].[TqRoute] ON

MERGE INTO [dbo].[TqRoute] AS Target 
USING (VALUES 
  (1, N'Cons', N'Construction'),
  (2, N'EC', N'Education & Childcare'),
  (3, N'Dig', N'Digital')
  )
  AS Source ([Id], [Code], [Name]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Code] <> Source.[Code] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)) 
THEN 
UPDATE SET 
	[Code] = Source.[Code],
	[Name] = Source.[Name],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Code], [Name], [CreatedBy]) 
	VALUES ([Id], [Code], [Name], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqRoute] OFF
