﻿using Discord;
using Discord.Commands;
using NBitcoin;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot.Modules
{
    [Name("xpchain")]
    public class XPChainModule : CurrencyModuleBase
    {
        protected override Network Network => Currencies.XPChain.Instance.Testnet;
        
        public override async Task CommandHelpAsync(string command = null) => await base.CommandHelpAsync(command);
        public override async Task CommandBalanceAsync() => await base.CommandBalanceAsync();
        public override async Task CommandDepositAsync() => await base.CommandDepositAsync();
        public override async Task CommandTipAsync(IUser user, decimal amount, params string[] comment) => await base.CommandTipAsync(user, amount, comment);
        public override async Task CommandWithdrawAsync(string address, decimal amount) => await base.CommandWithdrawAsync(address, amount);
        public override async Task CommandRainAsync(decimal amount, params string[] comment) => await base.CommandRainAsync(amount, comment);
        //Bech32 Address is not supported for sign message in Core wallet
        //public override async Task CommandSignMessageAsync([Remainder] string message) => await base.CommandSignMessageAsync(message);
    }
}
