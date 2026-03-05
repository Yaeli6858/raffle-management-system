/* =========================
   DISABLE FOREIGN KEYS
   ========================= */
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";


/* =========================
   CLEAN ALL TABLES
   ========================= */
DELETE FROM Winnings;
DELETE FROM Purchases;
DELETE FROM Gifts;
DELETE FROM Categories;
DELETE FROM Users;


/* =========================
   RESET IDENTITY
   ========================= */
DBCC CHECKIDENT ('Winnings', RESEED, 0);
DBCC CHECKIDENT ('Purchases', RESEED, 0);
DBCC CHECKIDENT ('Gifts', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);


/* =========================
   ENABLE FOREIGN KEYS
   ========================= */
EXEC sp_MSforeachtable "ALTER TABLE ? CHECK CONSTRAINT ALL";


/* =========================
   INSERT USERS
   Role:
   0 = Admin
   1 = Donor
   2 = User
   ========================= */
INSERT INTO Users
(Name, Email, Phone, City, Address, Password, Role, IsActive)
VALUES
-- Admins
('Devora', 'devora.video@gmail.com', '0500000001', 'carmiel', 'Main Street 1', '1234', 0, 1),
('Yaeli',  'porat4241@gmail.com',  '0500000002', 'Jerusalem', 'Main Street 2', '1234', 0, 1),

-- Donors (5)
('David Cohen',  'david@donor.com',  '0500000101', 'Tel Aviv',    'Herzl 10',   '1234', 1, 1),
('Sarah Levi',   'sarah@donor.com',  '0500000102', 'Haifa',       'Hagana 5',   '1234', 1, 1),
('Michael Ben',  'michael@donor.com','0500000103', 'Ramat Gan',   'Bialik 3',   '1234', 1, 1),
('Rachel Stern', 'rachel@donor.com', '0500000104', 'Petah Tikva', 'Weizman 8',  '1234', 1, 1),
('Daniel Katz',  'daniel@donor.com', '0500000105', 'Netanya',     'Ocean 2',    '1234', 1, 1),

-- Users
('John Smith',   'john@user.com',   '0500000201', 'Ashdod',     'Palm 4',   '1234', 2, 1),
('Emily Brown',  'emily@user.com',  '0500000202', 'Beer Sheva', 'Desert 7', '1234', 2, 1),
('Alex Green',   'alex@user.com',   '0500000203', 'Holon',      'Park 9',   '1234', 2, 1);


/* =========================
   INSERT CATEGORIES
   ========================= */
INSERT INTO Categories (Name)
VALUES
('Experiences'),
('Home'),
('Tech'),
('Home'),
('Gifts')

/* =========================
   INSERT GIFTS
   ========================= */
INSERT INTO Gifts (Description, CategoryId, Price, DonorId, ImageUrl, HasWinning)
VALUES
-- Food
('Breakfast', 1, 50, 3, '/uploads/gifts/breakfest.jpg', 0),

-- Toys
('Toys Collection', 4, 30, 4, '/uploads/gifts/bears.jpg', 0),

-- Travel
('Flight Ticket', 1, 300, 5, '/uploads/gifts/flight.jpg', 0),
('Alps Vacation',1, 800, 6, '/uploads/gifts/alps.jpg', 0),

-- Home
('Modern Kitchen Design', 2, 120, 7, '/uploads/gifts/kitchen.jpg', 0),
('Sofa Set', 2, 200, 6, '/uploads/gifts/Sofas.jpg', 0),

-- Technology
('Tech Gadgets', 3, 150, 5, '/uploads/gifts/tech.jpg', 0),
('Camera Kit', 3, 400, 7, '/uploads/gifts/camera.jpg', 0),

-- Surprise
('Mystery Surprise Box', 4, 40, 3, '/uploads/gifts/surprise.jpg', 0),
('Cash Gift', 1, 100, 7, '/uploads/gifts/cash.jpg', 0),

-- Extra
('Car', 1, 250, 6, '/uploads/gifts/car.jpg', 0),
('Makeup Set', 4, 90, 4, '/uploads/gifts/makeup.jpg', 0);
