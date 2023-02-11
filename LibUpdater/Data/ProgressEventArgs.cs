using System;

namespace LibUpdater.Data;

public class ProgressEventArgs : EventArgs
{
    public object? DataItem;
    public long Total { get; set; } = 0;
    public long Current { get; set; } = 0;

    public int Percentage
    {
        get
        {
            var result = Convert.ToInt32(Total > 0 ? Current * 100 / Total : 0);

            if (result > 100) result = 100;

            return result;
        }
    }

    public Guid Id { get; set; }
}

public class ProgressEventArgs<T> : EventArgs
{
    public T? DataItem;
    public long Total { get; set; } = 0;
    public long Current { get; set; } = 0;

    public int Percentage
    {
        get
        {
            var result = Convert.ToInt32(Total > 0 ? Current * 100 / Total : 0);

            if (result > 100) result = 100;

            return result;
        }
    }

    public Guid Id { get; set; }
}