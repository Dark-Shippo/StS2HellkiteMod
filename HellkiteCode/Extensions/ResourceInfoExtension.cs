using Hellkite.HellkiteCode.Structs;

namespace Hellkite.HellkiteCode.Extensions;

public static class ResourceInfoExtension
{
    public class HellkiteResourceInfo
    {
        public required FireUp FireUpSpent { get; init; }
        public required FireUp FireUpValue { get; init; }
    }
}