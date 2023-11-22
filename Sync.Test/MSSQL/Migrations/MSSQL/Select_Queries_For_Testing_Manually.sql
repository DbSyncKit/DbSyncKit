SELECT * FROM SourceChinook..Album DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..Album SA WHERE DA.AlbumId = SA.AlbumId) 

SELECT * FROM SourceChinook..Track DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..Track SA WHERE DA.TrackId = SA.TrackId) 

SELECT * FROM SourceChinook..PlaylistTrack DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..PlaylistTrack SA WHERE DA.TrackId = SA.TrackId) 

SELECT * FROM SourceChinook..InvoiceLine DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..InvoiceLine SA WHERE DA.InvoiceId = SA.InvoiceId) 

SELECT * FROM SourceChinook..Invoice DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..Invoice SA WHERE DA.InvoiceId = SA.InvoiceId) 

SELECT * FROM SourceChinook..Customer DA
WHERE NOT Exists (SELECT 1 FROM DestinationChinook..Customer SA WHERE DA.CustomerId = SA.CustomerId) 