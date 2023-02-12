using System.Collections.Concurrent;
using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class DownloadingArchivesPage : CancellableTaskDialogPage
{
    private readonly EventHandler _cancelHandler;
    private readonly IProgress<ProgressEventArgs> _progress;
    private readonly ConcurrentQueue<(DateTime time, long value)> _progressMarkers = new();

    public DownloadingArchivesPage(EventHandler cancelHandler)
        : this()
    {
        _cancelHandler = cancelHandler;
    }

    public DownloadingArchivesPage()
    {
        Heading = "Производится загрузка обновлений...";
        Icon = TaskDialogIcon.Information;
        ProgressBar = new TaskDialogProgressBar
        {
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            State = TaskDialogProgressBarState.Normal
        };

        var noMoreUpdates = false;

        _progress = new Progress<ProgressEventArgs>(args =>
        {
            try
            {
                if (!noMoreUpdates)
                {
                    _progressMarkers.Enqueue((DateTime.Now, args.Current));
                    ProgressBar.Value = args.Percentage;
                    var (average, moment) = CalculateSpeed();
                    Text = $"{args.Current:n0} / {args.Total:n0}\n" +
                           $"Средняя скорость: {average:n0} байт/с\n" +
                           $"Мгновенная скорость: {moment:n0} байт/с\n";
                }
            }
            catch
            {
                noMoreUpdates = true;
            }
        });

        _buttonCancel.Click += buttonCancel_Click;
    }

    private void buttonCancel_Click(object? sender, EventArgs e)
    {
        _cancelHandler?.Invoke(this, EventArgs.Empty);
    }

    private (long average, long moment) CalculateSpeed()
    {
        try
        {
            var first = _progressMarkers.FirstOrDefault();
            var last = _progressMarkers.LastOrDefault();
            var previousSecond = _progressMarkers
                .LastOrDefault(x => x.time < last.time.Subtract(TimeSpan.FromSeconds(1)));

            var average = Convert.ToInt64((last.value - first.value) / (last.time - first.time).TotalSeconds);
            var moment =
                Convert.ToInt64((last.value - previousSecond.value) / (last.time - previousSecond.time).TotalSeconds);
            return (average, moment);
        }
        catch
        {
            // For empty queue.
            return (0, 0);
        }
    }

    public void ProgressHandler(object? sender, ProgressEventArgs e)
    {
        _progress.Report(e);
    }
}