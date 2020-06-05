CREATE TABLE [dbo].[album] (
    [Id]       INT             NOT NULL IDENTITY,
    [Artist]   NVARCHAR (512)  NULL,
    [Title]    NVARCHAR (2048) NULL,
    [Year]     INT             NULL,
    [Format]   NVARCHAR (512)  NULL,
    [Store]    NVARCHAR (512)  NULL,
    [Price]    DECIMAL (5, 2)  CONSTRAINT [df_Price] DEFAULT ((0.00)) NULL,
    [Location] NVARCHAR (512)  NULL,
    [Symbol] NVARCHAR(8) NULL
    CONSTRAINT [PK_Album_id] PRIMARY KEY CLUSTERED ([Id] ASC)
)