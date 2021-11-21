using Ankh.Data;

namespace Ankh.Handlers;

public sealed class DirectoryHandler {
    
    public static DirectoryData Update(DirectoryData before, DirectoryData after) {
        before.Records.UnionWith(after.Records);
        before.Count = before.Records.Count;
        return before;
    }
}