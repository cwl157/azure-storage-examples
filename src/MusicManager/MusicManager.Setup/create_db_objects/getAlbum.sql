CREATE PROCEDURE getAlbum @id INT
as
BEGIN
	SELECT Id, Artist, Title, Year, Format, Store, Price, Location, Symbol
	FROM dbo.album
	WHERE Id = @id
END