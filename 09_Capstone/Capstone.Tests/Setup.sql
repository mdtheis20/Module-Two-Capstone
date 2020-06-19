--Begin Transaction

Delete reservation
Delete site
Delete campground
Delete park


INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Acadia', 'Maine', '1919-02-26', 47389, 2563129, 'Covering most of Mount Desert Island and other coastal islands, Acadia features the tallest mountain on the Atlantic coast of the United States, granite peaks, ocean shoreline, woodlands, and lakes. There are freshwater, estuary, forest, and intertidal habitats.');
Declare @acadiaId int
Select @acadiaId = @@Identity

INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Arches',	'Utah', '1929-04-12', 76518,	1284767, 'This site features more than 2,000 natural sandstone arches, including the famous Delicate Arch. In a desert climate, millions of years of erosion have led to these structures, and the arid ground has life-sustaining soil crust and potholes, which serve as natural water-collecting basins. Other geologic formations are stone columns, spires, fins, and towers.');
Declare @archesId int
Select @archesId = @@Identity

INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (@acadiaId, 'Blackwoods', 1, 12, 35.00);

INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (@acadiaId, 'Seawall', 5, 9, 30.00);

--Rollback Transaction