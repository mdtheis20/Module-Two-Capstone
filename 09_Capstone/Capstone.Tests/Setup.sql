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
Declare @blackwoodsId int
Select @blackwoodsId = @@Identity
--INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (@acadiaId, 'Seawall', 5, 9, 30.00);
--Declare @seawallId int
--Select @seawallId = @@IDENTITY

INSERT INTO site (site_number, campground_id, max_occupancy, accessible, max_rv_length, utilities) VALUES (1, @blackwoodsId, 8, 1, 35, 1);
Declare @blackwoodsSite1Id int
Select @blackwoodsSite1Id = @@Identity
INSERT INTO site (site_number, campground_id, max_occupancy, accessible, max_rv_length, utilities) VALUES (2, @blackwoodsId, 8, 1, 35, 1);
Declare @blackwoodsSite2Id int
Select @blackwoodsSite2Id = @@Identity
--INSERT INTO site (site_number, campground_id, max_occupancy, accessible, max_rv_length, utilities) VALUES (1, @seawallId, 8, 1, 35, 1);
--Declare @seawallSite3Id int
--Select @seawallSite3Id = @@Identity

INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@blackwoodsSite1Id, 'Smith Family Reservation', '2020-06-23', '2020-06-30');
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@blackwoodsSite2Id, 'Lockhart Family Reservation', '2020-07-07', '2020-07-10');
Declare @nextReservationId int
Select @nextReservationId = @@Identity + 1
--INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@seawallSite3Id, 'Jones Reservation', '2020-06-30', '2020-07-04');

Select @blackwoodsId AS blackwoodsId
--Select @seawallId AS seawallId
--Select @blackwoodsSite1Id AS blackwoodsSite1Id
--Select @nextReservationId AS nextReservationId
--SELECT * FROM reservation WHERE site_id = @blackwoodsSite1Id
--Select @blackwoodsSite2Id AS blackwoodsSite2Id
--Rollback Transaction