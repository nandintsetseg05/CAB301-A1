using System;

public class DeweyCountTable
{
    private string[] mKeys;
    private int[] mCounts;
    private bool[] mOccupied;
    private int mCapacity;
    private int mSize;

    public DeweyCountTable(int tableCapacity)
    {
        if (tableCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(tableCapacity), "capacity must be at least 1");

        mKeys = new string[tableCapacity];
        mCounts = new int[tableCapacity];
        mOccupied = new bool[tableCapacity];
        mCapacity = tableCapacity;
        mSize = 0;
    }

    // Not yet implemented - you must implement it.
    // You must implement your own hash function.
    // Do NOT use string.GetHashCode().
    private int Hash(string key)
    {
        throw new NotImplementedException("DeweyCountTable.Hash() not implemented");
    }

    // Not yet implemented - you must implement it.
    public bool Contains(string key)
    {
        throw new NotImplementedException("DeweyCountTable.Contains() not implemented");
    }

    // Not yet implemented - you must implement it.
    public int GetCount(string key)
    {
        throw new NotImplementedException("DeweyCountTable.GetCount() not implemented");
    }

    // Not yet implemented - you must implement it.
    public void Increment(string key)
    {
        // Recommended behaviour if the table is full and key is not already present:
        // throw new InvalidOperationException(...)
        throw new NotImplementedException("DeweyCountTable.Increment() not implemented");
    }

    // Not yet implemented - you must implement it.
    // In the completed system, Library.MostBorrowedSubject() should call this method.
    // A correct implementation may scan the table arrays internally.
    public string GetMostFrequentKey()
    {
        throw new NotImplementedException("DeweyCountTable.GetMostFrequentKey() not implemented");
    }
}
