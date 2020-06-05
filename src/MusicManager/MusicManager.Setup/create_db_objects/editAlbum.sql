
CREATE procedure editAlbum (@id INT, @artist NVARCHAR(512), @title NVARCHAR(2048), @year INT, @format NVARCHAR(512), @store NVARCHAR(512), @price DECIMAL(5,2), @location NVARCHAR(512), @symbol NVARCHAR(8))
AS
BEGIN
	update album
	set artist = @artist,
	title = @title,
	year = @year,
	Format = @format,
	store = @store,
	Price = @price,
	Location = @location,
	Symbol = @symbol
	where id = @id
END