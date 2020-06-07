/*
Insert initial data for Tq Providers
*/

SET IDENTITY_INSERT [dbo].[TqProvider] ON

MERGE INTO [dbo].[TqProvider] AS Target 
USING (VALUES 
	(1, 1, 1, 1)
  )
  AS Source ([Id], [TqAwardingOrganisationId], [TlProviderId], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TqAwardingOrganisationId] <> Source.[TqAwardingOrganisationId])
	   OR (Target.[TlProviderId] <> Source.[TlProviderId]))
THEN 
UPDATE SET 
	[TqAwardingOrganisationId] = Source.[TqAwardingOrganisationId],
	[TlProviderId] = Source.[TlProviderId],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TqAwardingOrganisationId], [TlProviderId], [IsActive], [CreatedBy]) 
	VALUES ([Id], [TqAwardingOrganisationId], [TlProviderId], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqProvider] OFF
