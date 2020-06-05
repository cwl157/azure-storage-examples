CREATE PROCEDURE [dbo].[getAlbums]
AS
BEGIN
	SELECT Id, Artist, Title, Year, Format, Store, Price, Location, Symbol
	FROM dbo.album
END