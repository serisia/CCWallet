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
        string ICurrency.Name { get; } = "eXPerience Chain";
        string ICurrency.IconUrl { get; } = "https://user-images.githubusercontent.com/4088274/50381599-5c1dd780-06ce-11e9-844e-78fda9a297ac.png";
        string ICurrency.MessageMagic { get; } = "XPChain Signed Message:\n";
        int ICurrency.BIP44CoinType { get; } = 0x0000018e; // 398 https://github.com/satoshilabs/slips/blob/master/slip-0044.md
        int ICurrency.TransactionConfirms { get; } = 6;
        int ICurrency.BaseAmountUnit { get; } = 10000;
        decimal ICurrency.MinAmount { get; } = 0.01m;
        decimal ICurrency.MaxAmount { get; } = 210000000000m;
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
                    new DNSSeedData("seed1","seed1.xpchain.io"),
                    new DNSSeedData("seed2","seed2.xpchain.io"),
                    new DNSSeedData("seed3","seed3.xpchain.io"),
                })
                .AddSeeds(new NetworkAddress[0])
                .SetGenesis("04000000000000000000000000000000000000000000000000000000000000000000000039970109c415a3a32349ec98b9dc3d2a81c17af16f2e891cd52d202c6610a6dabc23cf5bffff001d119b7dd80101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4a04ffff001d01044258706320646576656c6f70657273206172652050726574747920437574652e204f6620636f757273652069742069732061206a6f6b652e20224e6f7722207965742effffffff0120a1070000000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000");
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
