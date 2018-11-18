namespace CCWallet.DiscordBot.Utilities.Discord
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CommandHelpAttribute : System.Attribute
    {
        public string Sample;
        public string Description;

        public CommandHelpAttribute(string sample, string description)
        {
            this.Sample = sample;
            this.Description = description;
        }
    }
}
