using System;

public class Library
{
    private Book[] mBooks;
    private int mBookCount;

    private Borrower[] mBorrowers;
    private int mBorrowerCount;

    private BorrowEvent[] mBorrowEvents;
    private int mBorrowEventCount;

    private DeweyCountTable mSubjectCounts;
    private bool mIsCatalogueSorted;

    public Library(int maxBooks, int maxBorrowers, int maxEvents, int tableCapacity)
    {
        if (maxBooks < 1 || maxBorrowers < 1 || maxEvents < 1 || tableCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(maxBooks), "all capacities must be at least 1");

        mBooks = new Book[maxBooks];
        mBookCount = 0;

        mBorrowers = new Borrower[maxBorrowers];
        mBorrowerCount = 0;

        mBorrowEvents = new BorrowEvent[maxEvents];
        mBorrowEventCount = 0;

        mSubjectCounts = new DeweyCountTable(tableCapacity);
        mIsCatalogueSorted = false;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool AddBook(string deweyNumber, string title)
    {
        if (mBookCount >= mBooks.Length)
            return false;

        for (int i = 0; i < mBookCount; i++)
        {
            if (mBooks[i].DeweyNumber == deweyNumber)
                return false;
        }

        mBooks[mBookCount] = new Book(deweyNumber, title);
        mBookCount++;
        mIsCatalogueSorted = false;
        return true;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool AddBorrower(string studentId, string name)
    {
        if (mBorrowerCount >= mBorrowers.Length)
            return false;

        for (int i = 0; i < mBorrowerCount; i++)
        {
            if (mBorrowers[i].StudentId == studentId)
                return false;
        }

        mBorrowers[mBorrowerCount] = new Borrower(studentId, name);
        mBorrowerCount++;
        return true;
    }

    // Starter implementation
    // This implementation is correct but incomplete with respect to the final assignment requirements.
    // Starter implementation does NOT update mSubjectCounts.
    // In the completed system, you should add the required table update.
    // The efficiency of this method is not considered during marking    
    public bool RecordBorrowEvent(string studentId, string deweyNumber)
    {
        if (mBorrowEventCount >= mBorrowEvents.Length)
            return false;

        bool borrowerExists = false;
        for (int i = 0; i < mBorrowerCount; i++)
        {
            if (mBorrowers[i].StudentId == studentId)
            {
                borrowerExists = true;
                break;
            }
        }

        if (!borrowerExists)
            return false;

        if (!ContainsBook(deweyNumber))
            return false;

        mBorrowEvents[mBorrowEventCount] = new BorrowEvent(studentId, deweyNumber);
        mBorrowEventCount++;
        return true;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool ContainsBook(string deweyNumber)
    {
        for (int i = 0; i < mBookCount; i++)
        {
            if (mBooks[i].DeweyNumber == deweyNumber)
                return true;
        }

        return false;
    }

    // Not yet implemented - you must implement it.
    public void SortCatalogue()
    {
        throw new NotImplementedException("Library.SortCatalogue() not implemented");
    }

    // Not yet implemented - you must implement it.
    // If the catalogue is not currently marked as sorted (including before the first
    // call to SortCatalogue), this method must throw InvalidOperationException.
    public int GetBookIndex(string deweyNumber)
    {
        throw new NotImplementedException("Library.GetBookIndex() not implemented");
    }

    // Starter implementation
    // This implementation is correct but does not meet the completed-system performance requirement.
    public string FindBorrowLimitViolator(string deweyNumber)
    {
        if (!ContainsBook(deweyNumber))
            return null;

        BorrowEvent[] matching = new BorrowEvent[mBorrowEventCount];
        int matchingCount = 0;

        for (int i = 0; i < mBorrowEventCount; i++)
        {
            if (mBorrowEvents[i].DeweyNumber == deweyNumber)
            {
                matching[matchingCount] = mBorrowEvents[i];
                matchingCount++;
            }
        }

        for (int i = 0; i < matchingCount; i++)
        {
            int count = 0;
            string studentId = matching[i].StudentId;

            for (int j = 0; j < matchingCount; j++)
            {
                if (matching[j].StudentId == studentId)
                    count++;
            }

            if (count >= 3)
                return studentId;
        }

        return null;
    }

    // Starter implementation
    // This implementation is correct but does not meet the completed-system requirement below.
    // In the completed system, you should replace this with a call to
    // mSubjectCounts.GetMostFrequentKey().
    public string MostBorrowedSubject()
    {
        if (mBorrowEventCount == 0)
            return null;

        string bestDewey = null;
        int bestCount = 0;

        for (int i = 0; i < mBorrowEventCount; i++)
        {
            string currentDewey = mBorrowEvents[i].DeweyNumber;
            int count = 0;

            for (int j = 0; j < mBorrowEventCount; j++)
            {
                if (mBorrowEvents[j].DeweyNumber == currentDewey)
                    count++;
            }

            if (count > bestCount)
            {
                bestCount = count;
                bestDewey = currentDewey;
            }
        }

        return bestDewey;
    }

    // Optional private helper methods may be added below this line.
    // Optional private helper fields may also be added below this line.
    // Do not add any new public methods.
}
