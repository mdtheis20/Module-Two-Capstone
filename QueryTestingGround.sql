ALTER AUTHORIZATION ON DATABASE::npcampground TO [sa]

SELECT * FROM campground
SELECT * FROM park
SELECT * FROM reservation
SELECT * FROM site
declare @campground_id int
select @campground_id = 2
declare @from_date date
select @from_date = '2020-06-18'
declare @to_date date
select @to_date = '2020-06-23'
SELECT *
	FROM site
	LEFT JOIN reservation ON site.site_id = reservation.site_id
	WHERE campground_id = @campground_id AND (reservation_id IS NULL OR from_date >= @to_date OR to_date <= @from_date)