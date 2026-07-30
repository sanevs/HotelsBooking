# HotelsBookingWebApi
Clone and run the project to test the API endpoints. Run on https://localhost:{port}/index.html

The project is built using .NET 10.0 and C# 14.0.

<h1>Swagger API:</h1> https://app.swaggerhub.com/apis/interfax-a93/hotels-booking-api/v1

<h1> Hotels: </h1>

<h2>GET</h2>
/api/Hotels/search
Search hotel by name

<h2>GET</h2>
/api/Hotels/available-rooms
Get available rooms in the hotel for the specified period

<h2>POST</h2>
/api/Hotels/book-room
Book room at hotel name with date range and guest count

<h2>GET</h2>
/api/Hotels/bookings/{id}
Get booking by ID


<h1>Initializer:</h1>

<h2>POST</h2>
/api/Initializer/seed
Initialize the database with test data.

<h2>DELETE</h2>
/api/Initializer/reset
Clear (reset) the database.

========

<h1>AI Copilot generated code for a hotel booking web API.</h1>

<h2>1. Models and DbContext generated on my prompt:</h2>

Hotels: id (PK), name 
RoomType: id (PK), name, capacity (int)
Rooms: id (PK), hotelId (FK Hotels), roomTypeId (FK RoomType) 
Bookings: id (PK), roomId (FK Rooms), startDate, endDate

<h2>2. API Controllers: hotels controller, initializer controller.</h2>

<h2>3. Swagger API documentation.</h2>
