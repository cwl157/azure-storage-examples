CREATE PROCEDURE dbo.addAlbum (@artist NVARCHAR(512), @title NVARCHAR(2048), @year INT, @format NVARCHAR(512), @store NVARCHAR(512), @price DECIMAL(5,2), @location NVARCHAR(512), @symbol NVARCHAR(8))
AS
BEGIN
    insert into album(artist, title, year, format, store, price, location, Symbol)
    values (@artist, @title, @year, @format, @store, @price, @location, @symbol)
END