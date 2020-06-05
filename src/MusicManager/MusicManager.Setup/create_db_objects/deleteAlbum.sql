CREATE PROCEDURE deleteAlbum @id INT
AS
BEGIN
	delete 
	FROM album
	WHERE id = @id
END