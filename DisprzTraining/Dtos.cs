namespace DisprzTraining.Dtos
{
    public record ItemDto(Guid id, DateTime startDate, DateTime endDate, string appointment);
    public record PostItemDto(DateTime startDate, DateTime endDate, string appointment);
}