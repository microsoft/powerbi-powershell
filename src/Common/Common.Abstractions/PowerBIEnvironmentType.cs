namespace Microsoft.PowerBI.Common.Abstractions
{
    public enum PowerBIEnvironmentType
    {
#if DEBUG
        PPE = int.MaxValue,
#endif
        Public = 0
    }
}