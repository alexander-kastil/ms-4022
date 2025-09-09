using hotel_booking_api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register Swagger/OpenAPI generation and UI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as JSON endpoint and the Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- Hotel booking endpoints ---

var hotels = new List<Hotel>
{
    new Hotel(1, "Hotel Europa", "Unter den Linden 5", "Berlin", 140m, 4.4, "https://example.com/hotel-europa"),
    new Hotel(2, "Riverside Kyoto", "Shimogyo-ku 12-3", "Kyoto", 130m, 4.6, "https://example.com/riverside-kyoto"),
    new Hotel(3, "Alpine Lodge Zermatt", "Bahnhofstrasse 2", "Zermatt", 220m, 4.9, "https://example.com/alpine-lodge-zermatt"),
    new Hotel(4, "Baiyoke Sky Hotel", "Ratchaprarop Rd", "Bangkok", 160m, 4.5, "https://example.com/baiyoke-sky-hotel"),
    new Hotel(5, "Marina Bay Suites", "Marina Bay", "Singapore", 210m, 4.7, "https://example.com/marina-bay-suites"),
};

app.MapGet("/hotels", () => Results.Ok(hotels))
    .WithName("ListHotels");

app.MapPost("/book", (BookingRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.HotelName) || string.IsNullOrWhiteSpace(request.Arrival) || string.IsNullOrWhiteSpace(request.Departure))
    {
        return Results.BadRequest(new { error = "HotelName, Arrival and Departure are required" });
    }

    var hotel = hotels.FirstOrDefault(h => string.Equals(h.Name, request.HotelName, StringComparison.OrdinalIgnoreCase));
    if (hotel is null)
    {
        return Results.NotFound(new { error = "Hotel not found" });
    }

    if (!DateOnly.TryParse(request.Arrival, out var arrival) || !DateOnly.TryParse(request.Departure, out var departure))
    {
        return Results.BadRequest(new { error = "Invalid date format. Use YYYY-MM-DD." });
    }

    if (departure <= arrival)
    {
        return Results.BadRequest(new { error = "Departure must be after arrival" });
    }

    var nights = (departure.ToDateTime(TimeOnly.MinValue) - arrival.ToDateTime(TimeOnly.MinValue)).TotalDays;
    var totalPrice = hotel.Price * (decimal)nights;

    var message = $"Booking confirmed. You can now explore your hotel: {hotel.WebSiteUrl}";
    var booking = new BookingResponse(Guid.NewGuid(), hotel.Id, hotel.Name, arrival, departure, (int)nights, totalPrice, message);

    return Results.Ok(booking);
}).WithName("BookHotel");

app.Run();
