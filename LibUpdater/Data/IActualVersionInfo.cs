using System;

namespace LibUpdater.Data;

public interface IActualVersionInfo
{
    /// <summary>
    ///     Version of application in format "x.y.z.w".
    /// </summary>
    Version Version { get; }

    /// <summary>
    ///     Relative path to directory on server.
    /// </summary>
    string Path { get; }

    /// <summary>
    ///     Some description.
    /// </summary>
    string Description { get; }
}