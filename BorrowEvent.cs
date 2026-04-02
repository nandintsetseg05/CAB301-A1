using System;

public class BorrowEvent
{
    public string StudentId { get; }
    public string DeweyNumber { get; }

    public BorrowEvent(string studentId, string deweyNumber)
    {
        if (studentId == null || studentId.Length == 0)
            throw new ArgumentException("studentId must be non-empty");
        if (deweyNumber == null || deweyNumber.Length == 0)
            throw new ArgumentException("deweyNumber must be non-empty");

        StudentId = studentId;
        DeweyNumber = deweyNumber;
    }
}
