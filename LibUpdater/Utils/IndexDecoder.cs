using System;
using System.Collections.Generic;
using System.Text.Json;
using LibUpdater.Data;

namespace LibUpdater.Utils;

internal class IndexDecoder
{
    public IEnumerable<IArchiveItem> Decode(string json)
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
                Console.WriteLine($"{prop.Name}: {prop.Value}");

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
}