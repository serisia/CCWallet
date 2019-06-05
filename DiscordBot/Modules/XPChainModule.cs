using CCWallet.DiscordBot.Utilities.Discord;
using Discord;
using Discord.Commands;
using NBitcoin;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot.Modules
{
    [Name("xpchain")]
    public class XPChainModule : CurrencyModuleBase
    {
        protected override Network Network => Currencies.XPChain.Instance.Mainnet;
        
        public override async Task CommandHelpAsync(string command = null) => await base.CommandHelpAsync(command);
        public override async Task CommandBalanceAsync() => await base.CommandBalanceAsync();
        public override async Task CommandDepositAsync() => await base.CommandDepositAsync();
        public override async Task CommandTipAsync(decimal amount, IUser user, params string[] comment) => await base.CommandTipAsync(amount, user, comment);
        public override async Task CommandMultipAsync(decimal amount, params IUser[] users) => await base.CommandMultipAsync(amount, users);
        public override async Task CommandWithdrawAsync(decimal amount, string address) => await base.CommandWithdrawAsync(amount, address);
        public override async Task CommandWithdrawAllAsync(string address) => await base.CommandWithdrawAllAsync(address);
        public override async Task CommandRainAsync(decimal amount, params string[] comment) => await base.CommandRainAsync(amount, comment);
        public override async Task CommandSplashAsync(decimal amount, IRole role, params string[] comment) => await base.CommandSplashAsync(amount, role, comment);
        //Bech32 Address is not supported for sign message in Core wallet
        //public override async Task CommandSignMessageAsync([Remainder] string message) => await base.CommandSignMessageAsync(message);

        // compatibility for old argument
        [Command(BotCommand.Tip)]
        [RequireContext(ContextType.Guild | ContextType.Group)]
        [RequireBotPermission(ChannelPermission.SendMessages | ChannelPermission.AddReactions | ChannelPermission.EmbedLinks)]
        public async Task CommandTipAsync(IUser user, decimal amount, params string[] comment) => await base.CommandTipAsync(amount, user, comment);

        [Command(BotCommand.Withdraw)]
        [RequireContext(ContextType.DM | ContextType.Group | ContextType.Guild)]
        [RequireBotPermission(ChannelPermission.SendMessages | ChannelPermission.AddReactions | ChannelPermission.EmbedLinks)]
        public async Task CommandWithdrawAsync(string address, decimal amount) => await base.CommandWithdrawAsync(amount, address);

        [Command(BotCommand.Splash)]
        [RequireContext(ContextType.Guild)]
        [RequireBotPermission(ChannelPermission.SendMessages | ChannelPermission.AddReactions | ChannelPermission.EmbedLinks)]
        public async Task CommandSplashAsync(IRole role, decimal amount, params string[] comment) => await base.CommandSplashAsync(amount, role, comment);
    }
}
