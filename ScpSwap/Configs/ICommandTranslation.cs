namespace ScpSwap.Configs
{
    public interface ICommandTranslation
    {
        string Name { get; set; }
        string[] Aliases { get; set; }
        string Description { get; set; }
        
    }
}