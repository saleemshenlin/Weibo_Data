CREATE TABLE [dbo].[StatusLs] (
    [ID]                   NVARCHAR (128) NOT NULL,
    [CreatedAt]            NVARCHAR (MAX) NULL,
    [Text]                 NVARCHAR (MAX) NULL,
    [Source]               NVARCHAR (MAX) NULL,
    [ThumbnailPictureUrl]  NVARCHAR (MAX) NULL,
    [MiddleSizePictureUrl] NVARCHAR (MAX) NULL,
    [OriginalPictureUrl]   NVARCHAR (MAX) NULL,
    [RepostsCount]         INT            NOT NULL,
    [CommentsCount]        INT            NOT NULL,
    [AttitudeCount]        INT            NOT NULL,
    [Long]                 REAL           NOT NULL,
    [Lat]                  REAL           NOT NULL,
    [UserID]               NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.StatusLs] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.StatusLs_dbo.UserLs_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserLs] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[StatusLs]([UserID] ASC);

