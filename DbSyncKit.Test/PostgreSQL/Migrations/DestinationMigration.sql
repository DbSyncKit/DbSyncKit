DELETE FROM PlaylistTrack WHERE TrackId IN ( SELECT TrackId FROM Track WHERE AlbumId > 345 );
DELETE FROM Track WHERE AlbumId > 345;
DELETE FROM Album WHERE AlbumId > 345;

DELETE FROM InvoiceLine WHERE InvoiceId IN (SELECT InvoiceId FROM Invoice WHERE CustomerId = 59);
DELETE FROM Invoice WHERE CustomerId = 59;
DELETE FROM Customer  WHERE CustomerId = 59;

UPDATE Album
SET Title = REPLACE(REPLACE(Title, 'a', 'z'), 'i', 'p')
WHERE AlbumID BETWEEN 1 AND 5;

UPDATE Artist
SET Name = REPLACE(REPLACE(Name, 'a', 'z'), 'i', 'p')
WHERE ArtistID BETWEEN 1 AND 5;

UPDATE Customer
SET Address = REPLACE(REPLACE(Address, 'a', 'z'), 'i', 'p')
WHERE CustomerID BETWEEN 1 AND 20;

UPDATE Employee
SET Email = REPLACE(REPLACE(Email, 'a', 'z'), 'i', 'p')
WHERE EmployeeID BETWEEN 1 AND 3;

UPDATE Genre
SET Name = REPLACE(REPLACE(Name, 'a', 'z'), 'i', 'p')
WHERE GenreID BETWEEN 7 AND 9;

UPDATE Invoice
SET BillingAddress = REPLACE(REPLACE(BillingAddress, 'a', 'z'), 'i', 'p')
WHERE InvoiceID BETWEEN 9 AND 50;

UPDATE Track
SET Name = REPLACE(REPLACE(Name, 'a', 'z'), 'i', 'p')
WHERE TrackID BETWEEN 100 AND 300;