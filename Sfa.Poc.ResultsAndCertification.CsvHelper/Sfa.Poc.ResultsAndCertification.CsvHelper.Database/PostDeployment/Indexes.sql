IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('ResultsAndCertification_1Million.TqRegistrationProfile') AND NAME ='IX_TqRegistrationProfile_Uln')
CREATE INDEX IX_TqRegistrationProfile_Uln ON [ResultsAndCertification_1Million].[dbo].[TqRegistrationProfile](UniqueLearnerNumber);

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('ResultsAndCertification_1Million.TqRegistrationPathway') AND NAME ='IX_TqRegistrationPathway_RegistrationProfileId')
CREATE NONCLUSTERED INDEX IX_TqRegistrationPathway_RegistrationProfileId ON [ResultsAndCertification_1Million].[dbo].[TqRegistrationPathway]([TqRegistrationProfileId])

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('ResultsAndCertification_1Million.TqRegistrationSpecialism') AND NAME ='IX_TqRegistrationSpecialism_All')
CREATE NONCLUSTERED INDEX IX_TqRegistrationSpecialism_All ON [ResultsAndCertification_1Million].[dbo].[TqRegistrationSpecialism] ([TqRegistrationPathwayId])
INCLUDE ([Id],[TlSpecialismId],[StartDate],[EndDate],[Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy])
