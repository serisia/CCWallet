﻿using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot
{
    class Program
    {
        private static ManualResetEvent WaitHandle { get; } = new ManualResetEvent(false);
        private static IServiceProvider ServiceProvider { get; set; }

        static async Task Main(string[] args)
        {
            try
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                ServiceProvider = ConfigureServices().BuildServiceProvider();

                SetupCommandHandlingService();
                SetupCultureService();
                SetupWalletService();

                await StartDiscord();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
#if DEBUG
                Console.WriteLine("Press Any Key To Exit...");
                Console.Read();
#endif
                Environment.Exit(1);
            }

            WaitHandle.WaitOne();
        }

        private static IServiceCollection ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<Services.CommandHandlingService>()
                .AddSingleton<Services.ConfigureService>()
                .AddSingleton<Services.CultureService>()
                .AddSingleton<Services.PreferenceService>()
                .AddSingleton<Services.WalletService>()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    DefaultRetryMode = RetryMode.AlwaysRetry,
                    AlwaysDownloadUsers = true,
                }));
        }

        private static void SetupCommandHandlingService()
        {
            var command = ServiceProvider.GetRequiredService<Services.CommandHandlingService>();

            Task.WaitAll(new[]
            {
//                command.AddCommandService("!xpcoin").AddModuleAsync<Modules.XPCoinModule>(),
                command.AddCommandService("!xpc").AddModuleAsync<Modules.XPChainModule>(),
                command.AddCommandService("!ccwallet").AddModuleAsync<Modules.CCWalletModule>(),
            });
        }

        private static void SetupCultureService()
        {
            var culture = ServiceProvider.GetRequiredService<Services.CultureService>();

            culture.AddTranslation<Translations.ja>();
            culture.AddTranslation<Translations.ko>();
        }

        private static void SetupWalletService()
        {
            var wallet = ServiceProvider.GetRequiredService<Services.WalletService>();

            wallet.AddCurrency(Currencies.XPCoin.Instance);
            wallet.AddCurrency(Currencies.XPChain.Instance);
        }

        private static async Task StartDiscord()
        {
            var config = ServiceProvider.GetRequiredService<Services.ConfigureService>();
            var discord = ServiceProvider.GetRequiredService<DiscordSocketClient>();

            await discord.LoginAsync(TokenType.Bot, config.DiscordToken);
            await discord.StartAsync();
        }

        private static async void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                var discord = ServiceProvider.GetService<DiscordSocketClient>();

                if (discord.LoginState == LoginState.LoggedIn ||
                    discord.LoginState == LoginState.LoggingIn)
                {
                    await discord.LogoutAsync();
                }

                if (discord.ConnectionState == ConnectionState.Connected ||
                    discord.ConnectionState == ConnectionState.Connecting)
                {
                    await discord.StopAsync();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            e.Cancel = true;
            WaitHandle.Set();
        }
    }
}
