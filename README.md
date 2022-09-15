# BookingService

This API has been developed in .NET Core 3.1. Its purpose is to enable hotel room reservation

The API uses JWT Bearer Token authentication

To be able to use it, first you'll have to sign up a user using /api/Account/SignUp

After having signed up you'll have to sign in in order to get the auth token using /api/Account/SignIn


# AllowAnonymou endpoints:

/api/HotelRoom/GetRooms - returns a list of available rooms

/api/HotelRoom/GetReservedDates - returns reserved dates for a specific room

# Secure endpoints

/api/Reservation/GetReservations - gets all reservations

/api/Reservation/CreateReservation - creates a reservation

/api/Reservation/ChangeReservation - edits a reservation

/api/Reservation/CancelReservation - deletes a reservation

#
For all secure endpoints you must add an Authorization header to the HTTP request like below

Bearer {token}

![image](https://user-images.githubusercontent.com/19296543/190497202-c926a79a-590c-46e8-ae0c-ab6b212a543b.png)

#

The application uses SQL Server database but you can change the commented line in StartUp.cs to enable in memory database for testing purposes

![image](https://user-images.githubusercontent.com/19296543/190497524-35d07042-1e25-47df-aee1-65140f500471.png)


