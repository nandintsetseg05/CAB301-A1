using System;

public class Borrower
{
    public string StudentId { get; }
    public string Name { get; }

    public Borrower(string studentId, string name)
    {
        if (studentId == null || studentId.Length == 0)
            throw new ArgumentException("studentId must be non-empty");
        if (name == null)
            throw new ArgumentException("name must not be null");

        StudentId = studentId;
        Name = name;
    }
}
