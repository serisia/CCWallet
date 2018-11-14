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
    public class XPChain : CurrencyBase, ICurrency
    {
        public static XPChain Instance { get; } = new XPChain();

        public override string CryptoCode { get; } = "XPC";
        // TODO : SET CORRECT DATA
        //string ICurrency.Name { get; } = "eXPerience Chain";
        string ICurrency.Name { get; } = "testnet chain";
        // TODO : SET CORRECT DATA
//        string ICurrency.IconUrl { get; } = "https://raw.githubusercontent.com/xpc-wg/xpchain/0.17-xpc/src/qt/res/icons/xpchain.png";
        string ICurrency.IconUrl { get; } = "https://user-images.githubusercontent.com/4088274/48312879-a0f71e80-e5f8-11e8-9a96-49022ade0ef1.png";
        string ICurrency.MessageMagic { get; } = "XPChain Signed Message:\n";
        // TODO : SET CORRECT DATA
        //int ICurrency.BIP44CoinType { get; } = 0x70000001; // Mainet is undefined
        int ICurrency.BIP44CoinType { get; } = 0x00000001; // Testnet
        int ICurrency.TransactionConfirms { get; } = 6;
        int ICurrency.BaseAmountUnit { get; } = 10000;
        // TODO : SET CORRECT DATA
        decimal ICurrency.MinAmount { get; } = 0.01m;
        // TODO : SET CORRECT DATA
        decimal ICurrency.MaxAmount { get; } = 210000000000m;
        // TODO : SET CORRECT DATA
        decimal ICurrency.MinRainAmount { get; } = 100m;
        Money ICurrency.MinTxFee { get; } = Money.Coins(0.1m);
        int ICurrency.MaxRainUsers { get; } = int.MaxValue;
        bool ICurrency.SupportSegwit { get; } = true;

        private XPChain()
        {
        }

        protected override NetworkBuilder CreateMainnet()
        {
            return new NetworkBuilder()
                .SetConsensus(new Consensus()
                {
                    SupportSegwit = true,
                })
                .SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 76 })
                .SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 28 })
                .SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
                .SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
                .SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
                .SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, "xpc")
                .SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, "xpc")
                .SetMagic(0xfc87bac0)
                .SetPort(8798)
                .SetRPCPort(8762)
                .SetName("xpc-main")
                .AddAlias("xpc-mainnet")
                .AddAlias("xpchain-main")
                .AddAlias("xpchain-mainnet")
                .AddDNSSeeds(new[]
                {
                    // TODO : SET CORRECT DATA
                    new DNSSeedData("seed1","seed1.xpchain.io"),
                    new DNSSeedData("seed2","seed2.xpchain.io"),
                    new DNSSeedData("seed3","seed3.xpchain.io"),
                })
                .AddSeeds(new NetworkAddress[0])
                // TODO : SET CORRECT DATA
                .SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000001c0ee41fb6b792f9d4ad6a295812747aebc30972c326cfed6b9a62f27c283ccb808bd57ffff0f1ee9fb03000101000000b508bd57010000000000000000000000000000000000000000000000000000000000000000ffffffff1304ffff001d020f270a58502047656e65736973ffffffff010000000000000000000000000000");
        }

        protected override NetworkBuilder CreateTestnet()
        {
            return new NetworkBuilder()
                .SetConsensus(new Consensus()
                {
                    SupportSegwit = true,
                })
                .SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 138 })
                .SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 88 })
                .SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
                .SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
                .SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
                .SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, "txpc")
                .SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, "txpc")
                .SetMagic(0xfc87bbc1)
                .SetPort(18798)
                .SetRPCPort(18762)
                .SetName("xpc-test")
                .AddAlias("xpc-testnet")
                .AddAlias("xpchain-test")
                .AddAlias("xpchain-testnet")
                .AddSeeds(new NetworkAddress[0]);
        }

        protected override NetworkBuilder CreateRegtest()
        {
            // The currency side does not implement it.
            return new NetworkBuilder()
                .SetConsensus(new Consensus())
                .SetName("xpc-reg")
                .AddAlias("xpc-regtest")
                .AddAlias("xpchain-reg")
                .AddAlias("xpchain-regtest");
        }
    }
}
