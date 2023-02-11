using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

internal class JsonDecoder
{
    public IEnumerable<IArchiveItem> DecodeIndex(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var items = root.EnumerateArray();

        while (items.MoveNext())
        {
            var item = items.Current;

            var props = item.EnumerateObject();
            var archiveItem = new ArchiveItem();

            while (props.MoveNext())
            {
                var prop = props.Current;

                switch (prop.Name)
                {
                    case "path":
                    {
                        archiveItem.Path = prop.Value.GetString();
                        break;
                    }
                    case "sourceSize":
                    {
                        archiveItem.Size = prop.Value.GetInt64();
                        break;
                    }
                    case "sourceHash":
                    {
                        archiveItem.Hash = prop.Value.GetString();
                        break;
                    }
                    case "resultSize":
                    {
                        archiveItem.ArchiveSize = prop.Value.GetInt64();
                        break;
                    }
                    case "resultHash":
                    {
                        archiveItem.ArchiveHash = prop.Value.GetString();
                        break;
                    }
                }
            }

            yield return archiveItem;
        }
    }

    public async Task<IEnumerable<IArchiveItem>> DecodeIndexAsync(Stream jsonStream)
    {
        using var doc = await JsonDocument.ParseAsync(jsonStream);
        var root = doc.RootElement;

        var items = root.EnumerateArray();
        var result = new List<IArchiveItem>();

        while (items.MoveNext())
        {
            var item = items.Current;

            var props = item.EnumerateObject();
            var archiveItem = new ArchiveItem();

            while (props.MoveNext())
            {
                var prop = props.Current;

                switch (prop.Name)
                {
                    case "path":
                    {
                        archiveItem.Path = prop.Value.GetString();
                        break;
                    }
                    case "sourceSize":
                    {
                        archiveItem.Size = prop.Value.GetInt64();
                        break;
                    }
                    case "sourceHash":
                    {
                        archiveItem.Hash = prop.Value.GetString();
                        break;
                    }
                    case "resultSize":
                    {
                        archiveItem.ArchiveSize = prop.Value.GetInt64();
                        break;
                    }
                    case "resultHash":
                    {
                        archiveItem.ArchiveHash = prop.Value.GetString();
                        break;
                    }
                }
            }

            result.Add(archiveItem);
        }

        return result;
    }

    public IActualVersionInfo DecodeVersion(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var props = root.EnumerateObject();
        var versionInfo = new ActualVersionInfo();

        while (props.MoveNext())
        {
            var prop = props.Current;

            switch (prop.Name)
            {
                case "version":
                {
                    versionInfo.Version = new Version(prop.Value.GetString() ?? "0.0.0.0");
                    break;
                }
                case "path":
                {
                    versionInfo.Path = prop.Value.GetString();
                    break;
                }
                case "description":
                {
                    versionInfo.Description = prop.Value.GetString();
                    break;
                }
            }
        }

        return versionInfo;
    }

    public async Task<IActualVersionInfo> DecodeVersionAsync(Stream jsonStream)
    {
        using var doc = await JsonDocument.ParseAsync(jsonStream);
        var root = doc.RootElement;

        var props = root.EnumerateObject();
        var versionInfo = new ActualVersionInfo();

        while (props.MoveNext())
        {
            var prop = props.Current;

            switch (prop.Name)
            {
                case "version":
                {
                    versionInfo.Version = new Version(prop.Value.GetString() ?? "0.0.0.0");
                    break;
                }
                case "path":
                {
                    versionInfo.Path = prop.Value.GetString();
                    break;
                }
                case "description":
                {
                    versionInfo.Description = prop.Value.GetString();
                    break;
                }
            }
        }

        return versionInfo;
    }
}