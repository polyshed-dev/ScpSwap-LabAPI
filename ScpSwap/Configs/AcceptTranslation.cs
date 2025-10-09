namespace ScpSwap.Configs
{
    public class AcceptTranslation : ICommandTranslation
    {
        public string Name { get; set; } = "accept";
        public string[] Aliases { get; set; } = { "yes", "y" };
        public string Description { get; set; } = "Accepts an active swap request.";
    }
}