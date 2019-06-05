using Discord;
using Discord.Commands;
using NBitcoin;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot.Modules
{
    [Name("xp")]
    public class XPCoinModule : CurrencyModuleBase
    {
        protected override Network Network => Currencies.XPCoin.Instance.Mainnet;
        
        public override async Task CommandHelpAsync(string command = null) => await base.CommandHelpAsync(command);
        public override async Task CommandBalanceAsync() => await base.CommandBalanceAsync();
        public override async Task CommandDepositAsync() => await base.CommandDepositAsync();
        public override async Task CommandTipAsync(decimal amount, IUser user, params string[] comment) => await base.CommandTipAsync(amount, user, comment);
        public override async Task CommandWithdrawAsync(decimal amount, string address) => await base.CommandWithdrawAsync(amount, address);
        public override async Task CommandWithdrawAllAsync(string address) => await base.CommandWithdrawAllAsync(address);
        public override async Task CommandRainAsync(decimal amount, params string[] comment) => await base.CommandRainAsync(amount, comment);
        public override async Task CommandSignMessageAsync([Remainder] string message) => await base.CommandSignMessageAsync(message);
    }
}
