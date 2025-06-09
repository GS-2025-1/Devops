namespace Alagamenos.Dto;

public record SearchDto<T> (string? term, int? page, int totalItems, List<T> data) { }