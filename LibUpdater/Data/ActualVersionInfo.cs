using System;

namespace LibUpdater.Data;

internal class ActualVersionInfo : IActualVersionInfo
{
    public Version Version { get; set; } = new(0, 0, 0, 0);
    public string Path { get; set; } = "<path>";
    public string Description { get; set; } = "<default description>";
}