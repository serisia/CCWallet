using CCWallet.DiscordBot.Utilities;
using CCWallet.DiscordBot.Utilities.Insight;
using NBitcoin;
using NBitcoin.Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CCWallet.DiscordBot.Currencies
{
    public abstract class CurrencyBase : NetworkSetBase, ICurrency
    {
        public override string CryptoCode { get; } = "";
        public string Name => throw new NotImplementedException();
        public string IconUrl => throw new NotImplementedException();
        public string MessageMagic => throw new NotImplementedException();
        public int BIP44CoinType => throw new NotImplementedException();
        int ICurrency.TransactionConfirms { get; } = 6;
        int ICurrency.CoinbaseConfirms { get; } = 101;
        int ICurrency.BaseAmountUnit { get; } = 100000000;
        decimal ICurrency.MinAmount { get; } = 0.000001m;
        decimal ICurrency.MaxAmount { get; } = 21000000m;
        decimal ICurrency.MinRainAmount { get; } = 0.01m;
        Money ICurrency.MinTxFee { get; } = Money.Coins(0.00001m);     // MIN_TX_FEE = COIN * 0.00001
        int ICurrency.MaxRainUsers { get; } = 5000;
        bool ICurrency.SupportSegwit { get; } = true;

        public virtual string FormatMoney(Money money, CultureInfo culture, bool symbol)
        {
            return ((ICurrency) this).FormatAmount(((ICurrency)this).ConvertMoneyUnitReverse(money).ToDecimal(MoneyUnit.BTC), culture, symbol);
        }

        public virtual string FormatAmount(decimal amount, CultureInfo culture, bool symbol)
        {
            int digits = 0;
            for (int tempVal = ((ICurrency)this).BaseAmountUnit; tempVal >= 10; tempVal /= 10) { digits++; }
            return amount.ToString("N" + digits, culture) + (symbol ? $" {CryptoCode}" : string.Empty);
        }

        Money ICurrency.ConvertMoneyUnit(Money amount)
        {
            long BitcoinBaseAmountUnit = new Money(1, MoneyUnit.BTC).Satoshi;
            if (((ICurrency) this).BaseAmountUnit > BitcoinBaseAmountUnit)
            {
                return amount * (((ICurrency)this).BaseAmountUnit / BitcoinBaseAmountUnit);
            }
            else
            {
                return amount / (BitcoinBaseAmountUnit / ((ICurrency)this).BaseAmountUnit);
            }
        }

        Money ICurrency.ConvertMoneyUnitReverse(Money amount)
        {
            long BitcoinBaseAmountUnit = new Money(1, MoneyUnit.BTC).Satoshi;
            if (((ICurrency)this).BaseAmountUnit > BitcoinBaseAmountUnit)
            {
                return amount / (((ICurrency)this).BaseAmountUnit / BitcoinBaseAmountUnit);
            }
            else
            {
                return amount * (BitcoinBaseAmountUnit / ((ICurrency)this).BaseAmountUnit);
            }
        }

        public virtual Money CalculateFee(TransactionBuilder builder, IEnumerable<UnspentOutput.UnspentCoin> unspents)
        {
            var tx = builder.BuildTransaction(true);
            var bytes = tx.GetVirtualSize();

            return ((ICurrency)this).ConvertMoneyUnit(((ICurrency)this).MinTxFee) * bytes / 1000 + 20; // +20 for proiority budget
        }

        public virtual TransactionBuilder GeTransactionBuilder()
        {
            return new TransactionBuilder();
        }

        public virtual TransactionCheckResult VerifyTransaction(Transaction tx)
        {
            return tx.Check();
        }

        protected override NetworkBuilder CreateMainnet()
        {
            return new NetworkBuilder()
                .SetConsensus(Consensus.Main);
        }
        protected override NetworkBuilder CreateTestnet()
        {
            return new NetworkBuilder()
                .SetConsensus(Consensus.TestNet);
        }
        protected override NetworkBuilder CreateRegtest()
        {
            return new NetworkBuilder()
                .SetConsensus(Consensus.RegTest);
        }
    }
}
