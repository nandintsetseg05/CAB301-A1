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

        mKeys     = new string[tableCapacity];
        mCounts   = new int[tableCapacity];
        mOccupied = new bool[tableCapacity];
        mCapacity = tableCapacity;
        mSize     = 0;
    }

    // Polynomial rolling hash — does NOT use string.GetHashCode().
    // Multiplier 31 is a standard prime that distributes strings well.
    // All arithmetic is done in long to avoid int overflow before the final mod.
    private int Hash(string key)
    {
        long hash = 0;
        for (int i = 0; i < key.Length; i++)
            hash = (hash * 31 + key[i]) % mCapacity;
        return (int)hash;   // always in [0, mCapacity - 1]
    }

    // Returns true if the given key is present in the table.
    public bool Contains(string key)
    {
        int idx   = Hash(key);
        int start = idx;

        do
        {
            if (!mOccupied[idx])
                return false;          // hit an empty slot — key not present
            if (mKeys[idx] == key)
                return true;
            idx = (idx + 1) % mCapacity;
        }
        while (idx != start);

        return false;
    }

    // Returns the count for the given key, or -1 if not present.
    public int GetCount(string key)
    {
        int idx   = Hash(key);
        int start = idx;

        do
        {
            if (!mOccupied[idx])
                return -1;
            if (mKeys[idx] == key)
                return mCounts[idx];
            idx = (idx + 1) % mCapacity;
        }
        while (idx != start);

        return -1;
    }

    // Adds the key with count 1 if absent, otherwise increments its count.
    // Throws InvalidOperationException if the table is full and the key is new.
    public void Increment(string key)
    {
        int idx   = Hash(key);
        int start = idx;

        do
        {
            if (!mOccupied[idx])
            {
                // Empty slot — insert new key here
                if (mSize >= mCapacity)
                    throw new InvalidOperationException("DeweyCountTable is full; cannot insert new key.");

                mKeys[idx]     = key;
                mCounts[idx]   = 1;
                mOccupied[idx] = true;
                mSize++;
                return;
            }

            if (mKeys[idx] == key)
            {
                mCounts[idx]++;
                return;
            }

            idx = (idx + 1) % mCapacity;
        }
        while (idx != start);

        // Completed a full loop without finding the key or an empty slot
        throw new InvalidOperationException("DeweyCountTable is full; cannot insert new key.");
    }

    // Returns the key with the highest count, or null if the table is empty.
    // If two keys tie, either may be returned.
    public string GetMostFrequentKey()
    {
        string bestKey   = null;
        int    bestCount = 0;

        for (int i = 0; i < mCapacity; i++)
        {
            if (mOccupied[i] && mCounts[i] > bestCount)
            {
                bestCount = mCounts[i];
                bestKey   = mKeys[i];
            }
        }

        return bestKey;   // null when mSize == 0
    }
}