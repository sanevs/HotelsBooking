# HotelsBookingWebApi
Clone and run the project to test the API endpoints. Run on https://localhost:{port}/index.html

The project is built using .NET 10.0 and C# 14.0.

Swagger API: https://app.swaggerhub.com/apis/interfax-a93/hotels-booking-api/v1

Hotels:
GET
/api/Hotels/search
Search hotel by name

GET
/api/Hotels/available-rooms
Get available rooms in the hotel for the specified period

POST
/api/Hotels/book-room
Book room at hotel name with date range and guest count

GET
/api/Hotels/bookings/{id}
Get booking by ID


Initializer:
POST
/api/Initializer/seed
Initialize the database with test data.

DELETE
/api/Initializer/reset
Clear (reset) the database.

========

AI Copilot generated code for a hotel booking web API.

1. Models and DbContext generated on my prompt:

Hotels: id (PK), name 
RoomType: id (PK), name, capacity (int)
Rooms: id (PK), hotelId (FK Hotels), roomTypeId (FK RoomType) 
Bookings: id (PK), roomId (FK Rooms), startDate, endDate

2. API Controllers: bookings controller and hotels controller.

3. Swagger API documentation.
