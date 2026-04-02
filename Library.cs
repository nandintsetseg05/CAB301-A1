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

    // Starter implementation — unchanged.
    // Marks catalogue unsorted on successful add.
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

    // Starter implementation — unchanged.
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

    // Completed: now increments mSubjectCounts on every successful borrow event.
    // This keeps the auxiliary table current so MostBorrowedSubject() never rescans.
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

        // Incremental update — O(1) expected, keeps table in sync.
        mSubjectCounts.Increment(deweyNumber);

        return true;
    }

    // Starter implementation — unchanged.
    // Linear scan; works regardless of catalogue sort state.
    public bool ContainsBook(string deweyNumber)
    {
        for (int i = 0; i < mBookCount; i++)
        {
            if (mBooks[i].DeweyNumber == deweyNumber)
                return true;
        }

        return false;
    }

    // Completed: merge sort — O(b log b) worst case.
    // Chosen because it is comparison-based, array-friendly, and achieves the
    // optimal O(b log b) lower bound for comparison sorts.
    public void SortCatalogue()
    {
        Book[] temp = new Book[mBookCount];
        MergeSort(mBooks, temp, 0, mBookCount - 1);
        mIsCatalogueSorted = true;
    }

    // Completed: binary search — O(log b) worst case.
    // Only valid when catalogue is sorted; throws immediately otherwise.
    public int GetBookIndex(string deweyNumber)
    {
        if (!mIsCatalogueSorted)
            throw new InvalidOperationException(
                "Catalogue is not sorted. Call SortCatalogue() before using GetBookIndex().");

        int low  = 0;
        int high = mBookCount - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            int cmp = string.CompareOrdinal(mBooks[mid].DeweyNumber, deweyNumber);

            if (cmp == 0)
                return mid;
            else if (cmp < 0)
                low = mid + 1;
            else
                high = mid - 1;
        }

        return -1;
    }

    // Completed: O(e + m log m) — collects m matches in O(e), sorts them by
    // studentId in O(m log m), then finds a run of length >= 3 in O(m).
    public string FindBorrowLimitViolator(string deweyNumber)
    {
        if (!ContainsBook(deweyNumber))
            return null;

        // Phase 1 — collect all matching events in O(e)
        BorrowEvent[] matching = new BorrowEvent[mBorrowEventCount];
        int matchingCount = 0;

        for (int i = 0; i < mBorrowEventCount; i++)
        {
            if (mBorrowEvents[i].DeweyNumber == deweyNumber)
                matching[matchingCount++] = mBorrowEvents[i];
        }

        // Phase 2 — sort by studentId in O(m log m)
        BorrowEvent[] temp = new BorrowEvent[matchingCount];
        MergeSortEvents(matching, temp, 0, matchingCount - 1);

        // Phase 3 — single linear scan for any run of length >= 3 in O(m)
        if (matchingCount < 3)
            return null;

        int runLength = 1;
        for (int i = 1; i < matchingCount; i++)
        {
            if (matching[i].StudentId == matching[i - 1].StudentId)
            {
                runLength++;
                if (runLength >= 3)
                    return matching[i].StudentId;
            }
            else
            {
                runLength = 1;
            }
        }

        return null;
    }

    // Completed: delegates to the incrementally-maintained table — O(t).
    // No rescan of mBorrowEvents occurs here.
    public string MostBorrowedSubject()
    {
        return mSubjectCounts.GetMostFrequentKey();
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    // Recursive merge sort over Book[] by DeweyNumber (ascending ordinal order).
    private void MergeSort(Book[] arr, Book[] temp, int left, int right)
    {
        if (left >= right)
            return;

        int mid = (left + right) / 2;
        MergeSort(arr, temp, left, mid);
        MergeSort(arr, temp, mid + 1, right);
        MergeBooks(arr, temp, left, mid, right);
    }

    private void MergeBooks(Book[] arr, Book[] temp, int left, int mid, int right)
    {
        // Copy the sub-array into temp
        for (int i = left; i <= right; i++)
            temp[i] = arr[i];

        int l = left, r = mid + 1, k = left;

        while (l <= mid && r <= right)
        {
            if (string.CompareOrdinal(temp[l].DeweyNumber, temp[r].DeweyNumber) <= 0)
                arr[k++] = temp[l++];
            else
                arr[k++] = temp[r++];
        }

        while (l <= mid)  arr[k++] = temp[l++];
        while (r <= right) arr[k++] = temp[r++];
    }

    // Recursive merge sort over BorrowEvent[] by StudentId (ascending ordinal order).
    private void MergeSortEvents(BorrowEvent[] arr, BorrowEvent[] temp, int left, int right)
    {
        if (left >= right)
            return;

        int mid = (left + right) / 2;
        MergeSortEvents(arr, temp, left, mid);
        MergeSortEvents(arr, temp, mid + 1, right);
        MergeEvents(arr, temp, left, mid, right);
    }

    private void MergeEvents(BorrowEvent[] arr, BorrowEvent[] temp, int left, int mid, int right)
    {
        for (int i = left; i <= right; i++)
            temp[i] = arr[i];

        int l = left, r = mid + 1, k = left;

        while (l <= mid && r <= right)
        {
            if (string.CompareOrdinal(temp[l].StudentId, temp[r].StudentId) <= 0)
                arr[k++] = temp[l++];
            else
                arr[k++] = temp[r++];
        }

        while (l <= mid)   arr[k++] = temp[l++];
        while (r <= right) arr[k++] = temp[r++];
    }
}