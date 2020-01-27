CREATE TABLE [dbo].[tblAdvertisementItem]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [IdAdvertisement] INT NULL, 
    [Primary] TINYINT NULL, 
    [Image] NCHAR(10) NULL, 
    [Create] DATETIME2 NULL, 
    [Update] DATETIME2 NULL, 
    [Leadtext] VARCHAR(MAX) NULL, 
    [LinkTo] VARCHAR(50) NULL 

)
