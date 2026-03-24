-- QUERY TRUNCATED
-- ==========================================
-- MAAS APPLICATION - DUMMY DATA SCRIPT


-- Drop existing tables to ensure a clean slate if the script is run multiple times
DROP TABLE IF EXISTS Trips CASCADE;
DROP TABLE IF EXISTS UserPasses CASCADE;
DROP TABLE IF EXISTS PassType_TransportModes CASCADE;
DROP TABLE IF EXISTS PassTypes CASCADE;
DROP TABLE IF EXISTS TransportModes CASCADE;
DROP TABLE IF EXISTS Users CASCADE;

-- ==========================================
-- 1. CREATE TABLES
-- ==========================================

-- Users Table: Handles authentication and role-based access [cite: 33, 56]
CREATE TABLE Users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    mobile VARCHAR(15) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- TransportModes Table: Defines available transit types [cite: 60]
CREATE TABLE TransportModes (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    code VARCHAR(20) UNIQUE NOT NULL
);

-- PassTypes Table: The catalog of available passes [cite: 57]
-- Note: transport_modes is handled by the junction table below for proper normalization
CREATE TABLE PassTypes (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    validity_days INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    max_trips_per_day INT
);

-- PassType_TransportModes: Junction table to normalize the many-to-many relationship
CREATE TABLE PassType_TransportModes (
    pass_type_id INT REFERENCES PassTypes(id) ON DELETE CASCADE,
    transport_mode_id INT REFERENCES TransportModes(id) ON DELETE CASCADE,
    PRIMARY KEY (pass_type_id, transport_mode_id)
);

-- UserPasses Table: Tracks passes purchased by commuters [cite: 58]
CREATE TABLE UserPasses (
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(id) ON DELETE CASCADE,
    pass_type_id INT REFERENCES PassTypes(id) ON DELETE CASCADE,
    pass_code VARCHAR(100) UNIQUE NOT NULL,
    purchase_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expiry_date TIMESTAMP NOT NULL,
    status VARCHAR(20) NOT NULL 
);

-- Trips Table: Logs each successful pass validation [cite: 59]
CREATE TABLE Trips (
    id SERIAL PRIMARY KEY,
    user_pass_id INT REFERENCES UserPasses(id) ON DELETE CASCADE,
    validated_by INT REFERENCES Users(id), 
    transport_mode VARCHAR(50) NOT NULL,
    route_info TEXT,
    validated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================

-- Clean existing data (safe to re-run)
TRUNCATE TABLE Trips, UserPasses, PassType_TransportModes, PassTypes, TransportModes, Users 
RESTART IDENTITY CASCADE;


-- ==========================================
-- 1. USERS
-- 3 Roles: Admin, Validator, Commuter
-- Password for all = "Password@123" (hashed)
-- ==========================================

INSERT INTO Users (name, mobile, email, password_hash, role) VALUES
-- Admins
('Alice Admin',       '9000000001', 'alice@transit.city',   '$2a$11$dummyhash.Admin1', 'Admin'),
('Raj Manager',       '9000000002', 'raj@transit.city',     '$2a$11$dummyhash.Admin2', 'Admin'),

-- Validators (conductors/gate staff)
('Bob Conductor',     '9000000003', 'bob@transit.city',     '$2a$11$dummyhash.Val1',   'Validator'),
('Priya Gate Staff',  '9000000004', 'priya@transit.city',   '$2a$11$dummyhash.Val2',   'Validator'),
('Suresh Inspector',  '9000000005', 'suresh@transit.city',  '$2a$11$dummyhash.Val3',   'Validator'),

-- Commuters
('Charlie Commuter',  '9000000006', 'charlie@example.com',  '$2a$11$dummyhash.Com1',   'Commuter'),
('Divya Patel',       '9000000007', 'divya@example.com',    '$2a$11$dummyhash.Com2',   'Commuter'),
('Eshan Mehta',       '9000000008', 'eshan@example.com',    '$2a$11$dummyhash.Com3',   'Commuter'),
('Fatima Khan',       '9000000009', 'fatima@example.com',   '$2a$11$dummyhash.Com4',   'Commuter'),
('Gaurav Shah',       '9000000010', 'gaurav@example.com',   '$2a$11$dummyhash.Com5',   'Commuter');

-- Verify: SELECT * FROM Users;


-- ==========================================
-- 2. TRANSPORT MODES
-- ==========================================

INSERT INTO TransportModes (name, code) VALUES
('City Bus',    'BUS'),
('Metro Train', 'METRO'),
('Ferry',       'FERRY'),
('Tram',        'TRAM');

-- Verify: SELECT * FROM TransportModes;


-- ==========================================
-- 3. PASS TYPES
-- Covers: Daily, Weekly, Monthly + combos
-- ==========================================

INSERT INTO PassTypes (name, validity_days, price, max_trips_per_day) VALUES
-- Single mode passes
('Daily Bus Pass',          1,   50.00,    4),      -- id=1: Bus only, max 4 trips/day
('Daily Metro Pass',        1,   60.00,    4),      -- id=2: Metro only, max 4 trips/day
('Weekly Bus Pass',         7,   250.00,   NULL),   -- id=3: Bus only, unlimited trips
('Weekly Metro Pass',       7,   300.00,   NULL),   -- id=4: Metro only, unlimited trips
('Monthly Bus Pass',        30,  800.00,   NULL),   -- id=5: Bus only, unlimited trips
('Monthly Metro Pass',      30,  950.00,   NULL),   -- id=6: Metro only, unlimited trips

-- Combo passes
('Weekly Metro+Bus Pass',   7,   450.00,   NULL),   -- id=7: Metro + Bus
('Monthly Metro+Bus Pass',  30,  1200.00,  NULL),   -- id=8: Metro + Bus
('Monthly All-Access Pass', 30,  1800.00,  NULL);   -- id=9: Bus + Metro + Ferry + Tram

-- Verify: SELECT * FROM PassTypes;


-- ==========================================
-- 4. PASS TYPE → TRANSPORT MODE LINKS
-- Junction table mapping
-- ==========================================

INSERT INTO PassType_TransportModes (pass_type_id, transport_mode_id) VALUES
-- Daily Bus Pass (id=1) → BUS only
(1, 1),

-- Daily Metro Pass (id=2) → METRO only
(2, 2),

-- Weekly Bus Pass (id=3) → BUS only
(3, 1),

-- Weekly Metro Pass (id=4) → METRO only
(4, 2),

-- Monthly Bus Pass (id=5) → BUS only
(5, 1),

-- Monthly Metro Pass (id=6) → METRO only
(6, 2),

-- Weekly Metro+Bus Pass (id=7) → BUS + METRO
(7, 1),
(7, 2),

-- Monthly Metro+Bus Pass (id=8) → BUS + METRO
(8, 1),
(8, 2),

-- Monthly All-Access Pass (id=9) → BUS + METRO + FERRY + TRAM
(9, 1),
(9, 2),
(9, 3),
(9, 4);

-- Verify junction:
-- SELECT pt.name, tm.code FROM PassType_TransportModes ptm
-- JOIN PassTypes pt ON pt.id = ptm.pass_type_id
-- JOIN TransportModes tm ON tm.id = ptm.transport_mode_id
-- ORDER BY pt.id;


-- ==========================================
-- 5. USER PASSES (Purchased passes)
-- Covers: Active, Expired, different users
-- ==========================================

INSERT INTO UserPasses (user_id, pass_type_id, pass_code, purchase_date, expiry_date, status) VALUES

-- Charlie (user 6) — Active Monthly All-Access
(6, 9, 'PASS-CHARLIE001', 
    CURRENT_TIMESTAMP - INTERVAL '5 days', 
    CURRENT_TIMESTAMP + INTERVAL '25 days', 
    'Active'),

-- Charlie (user 6) — Expired Daily Bus Pass (for testing expired scenario)
(6, 1, 'PASS-CHARLIE002', 
    CURRENT_TIMESTAMP - INTERVAL '3 days', 
    CURRENT_TIMESTAMP - INTERVAL '2 days', 
    'Expired'),

-- Divya (user 7) — Active Weekly Metro Pass
(7, 4, 'PASS-DIVYA0001', 
    CURRENT_TIMESTAMP - INTERVAL '2 days', 
    CURRENT_TIMESTAMP + INTERVAL '5 days', 
    'Active'),

-- Divya (user 7) — Active Daily Bus Pass (max 4 trips/day — for limit testing)
(7, 1, 'PASS-DIVYA0002', 
    CURRENT_TIMESTAMP, 
    CURRENT_TIMESTAMP + INTERVAL '1 day', 
    'Active'),

-- Eshan (user 8) — Active Monthly Metro+Bus Pass
(8, 8, 'PASS-ESHAN0001', 
    CURRENT_TIMESTAMP - INTERVAL '10 days', 
    CURRENT_TIMESTAMP + INTERVAL '20 days', 
    'Active'),

-- Fatima (user 9) — Active Weekly Bus Pass
(9, 3, 'PASS-FATIMA001', 
    CURRENT_TIMESTAMP - INTERVAL '1 day', 
    CURRENT_TIMESTAMP + INTERVAL '6 days', 
    'Active'),

-- Gaurav (user 10) — Expired Monthly Metro Pass (for testing)
(10, 6, 'PASS-GAURAV001', 
    CURRENT_TIMESTAMP - INTERVAL '35 days', 
    CURRENT_TIMESTAMP - INTERVAL '5 days', 
    'Expired');

-- Verify: 
SELECT up.pass_code, u.name, pt.name, up.status, up.expiry_date
FROM UserPasses up
JOIN Users u ON u.id = up.user_id
JOIN PassTypes pt ON pt.id = up.pass_type_id;

SELECT *
FROM UserPasses;


-- ==========================================
-- 6. TRIPS (Validation history)
-- Covers: Normal trips, different modes, routes
-- ==========================================

INSERT INTO Trips (user_pass_id, validated_by, transport_mode, route_info, validated_at) VALUES

-- Charlie's trips on Monthly All-Access (user_pass_id = 1)
(1, 3, 'METRO', 'Blue Line - Station A to Station D',     CURRENT_TIMESTAMP - INTERVAL '4 days'),
(1, 4, 'BUS',   'Route 42 - City Centre to Airport',      CURRENT_TIMESTAMP - INTERVAL '3 days'),
(1, 3, 'METRO', 'Red Line - Central to North End',        CURRENT_TIMESTAMP - INTERVAL '2 days'),
(1, 5, 'FERRY', 'River Route 1 - East Wharf to West Bay', CURRENT_TIMESTAMP - INTERVAL '1 day'),
(1, 3, 'BUS',   'Route 15 - Market to University',        CURRENT_TIMESTAMP - INTERVA