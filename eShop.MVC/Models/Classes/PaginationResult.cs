namespace eShop.MVC.Models.Classes;

public record PaginationResult<T>
(
    int StartPage,
    int EndPage,
    int Length,
    IEnumerable<T> Items
);