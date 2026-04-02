using System;

public class Book
{
    public string DeweyNumber { get; }
    public string Title { get; }

    public Book(string deweyNumber, string title)
    {
        if (deweyNumber == null || deweyNumber.Length == 0)
            throw new ArgumentException("deweyNumber must be non-empty");
        if (title == null)
            throw new ArgumentException("title must not be null");

        DeweyNumber = deweyNumber;
        Title = title;
    }
}
