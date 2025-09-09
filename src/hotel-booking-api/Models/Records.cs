namespace hotel_booking_api.Models;

public record Hotel(int Id, string Name, string Location, string City, decimal Price, double Rating, string WebSiteUrl);

public record BookingRequest(string HotelName, string Arrival, string Departure);

public record BookingResponse(System.Guid BookingId, int HotelId, string HotelName, System.DateOnly Arrival, System.DateOnly Departure, int Nights, decimal TotalPrice, string Message);

// WeatherForecast removed — not used anymore
