namespace Integration
{
    public enum BuildTrigger
    {
        Invalid,
        SemVerTag,
        PullRequest,
        MasterBranch,
        UnknownBranchOrTag
    }
}