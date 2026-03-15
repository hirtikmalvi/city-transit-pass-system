// src/utils/mockData.js

// Matches the PassTypes table
export const mockPassTypes = [
  {
    id: 1,
    name: "Daily Bus Pass",
    validity_days: 1,
    price: 50.00,
    max_trips_per_day: 4,
    transport_modes: ["BUS"]
  },
  {
    id: 2,
    name: "Weekly Metro Pass",
    validity_days: 7,
    price: 300.00,
    max_trips_per_day: null, // Unlimited
    transport_modes: ["METRO"]
  },
  {
    id: 3,
    name: "Monthly All-Access Pass",
    validity_days: 30,
    price: 1200.00,
    max_trips_per_day: null,
    transport_modes: ["BUS", "METRO", "FERRY"]
  }
];

// Matches the UserPasses table for "Charlie Commuter"
export const mockActivePass = {
  id: 1,
  user_id: 3,
  pass_type_id: 3,
  pass_name: "Monthly All-Access Pass",
  pass_code: "PASS-QR-987654321",
  purchase_date: new Date().toISOString(),
  expiry_date: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
  status: "Active"
};

// Matches the Trips table
export const mockTrips = [
  {
    id: 1,
    user_pass_id: 1,
    transport_mode: "METRO",
    route_info: "Blue Line - Station A to Station D",
    validated_at: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString() // 2 hours ago
  }
];