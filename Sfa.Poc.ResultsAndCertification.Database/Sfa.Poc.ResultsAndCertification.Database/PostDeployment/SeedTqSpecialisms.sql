/*
Insert initial data for TqSpecialisms
*/

SET IDENTITY_INSERT [dbo].[TqSpecialism] ON

MERGE INTO [dbo].[TqSpecialism] AS Target 
USING (VALUES 
  (1, N'SDC', N'Surveying and design for construction and the built environment', 1),
  (2, N'CE', N'Civil Engineering', 1),
  (3, N'BSD', N'Building services design', 1),
  (4, N'HMAS', N'Hazardous materials analysis and surveying', 1),
  (5, N'EYEC', N'Early years education and childcare', 2),
  (6, N'AT', N'Assisting teaching', 2),
  (7, N'SMSFHE', N'Supporting and mentoring students in further and higher education', 2),
  (8, N'DPDD', N'Digital Production, Design and Development', 3)
  )
  AS Source ([Id], [Code], [Name], [PathwayId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Code] <> Source.[Code] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[PathwayId] <> Source.[PathwayId])) 
THEN 
UPDATE SET 
	[Code] = Source.[Code],
	[Name] = Source.[Name],
	[PathwayId] = Source.[PathwayId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Code], [Name], [PathwayId], [CreatedBy]) 
	VALUES ([Id], [Code], [Name], [PathwayId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqSpecialism] OFF
