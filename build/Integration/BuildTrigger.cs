namespace Integration
{
    public enum BuildTrigger
    {
        Invalid = 0,
        SemVerTag,
        PullRequest,
        MasterBranch,
        UnknownBranchOrTag
    }
}