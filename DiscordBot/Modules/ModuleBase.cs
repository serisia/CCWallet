using CCWallet.DiscordBot.Services;
using CCWallet.DiscordBot.Utilities.Discord;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using NGettext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot.Modules
{
    public class ModuleBase : Discord.Commands.ModuleBase
    {
        public IServiceProvider Provider { get; set; }
        public Catalog Catalog { get; protected set; }

        protected override void BeforeExecute(CommandInfo command)
        {
            Catalog = Provider.GetService<CultureService>().GetCatalog(Context.Channel);
        }

        protected virtual string _(string text)
        {
            return Catalog.GetString(text);
        }

        protected virtual string _(string text, params object[] args)
        {
            return Catalog.GetString(text, args);
        }

        protected virtual void SetLanguage(string lang, bool remaind = true)
        {
            var culture = Provider.GetService<CultureService>();

            if (remaind)
            {
                culture.SetLanguage(Context.Channel, Context.Message, lang);
            }

            Catalog = culture.GetCatalog(lang);
        }

        protected virtual async Task ReplySuccessAsync(string message, Embed embed = null)
        {
            await Task.WhenAll(new List<Task>()
            {
                Context.Message.AddReactionAsync(BotReaction.Success),
                ReplyAsync($"{Context.User.Mention} {message}", false, embed),
            });
        }

        protected virtual async Task ReplyFailureAsync(string message, Embed embed = null)
        {
            await Task.WhenAll(new List<Task>()
            {
                Context.Message.AddReactionAsync(BotReaction.Failure),
                ReplyAsync($"{Context.User.Mention} {message}", false, embed),
            });
        }

        protected override async Task<IUserMessage> ReplyAsync(string message, bool isTTS = false, Embed embed = null, RequestOptions options = null)
        {
            Exception last = null;
            for(int i = 0; i < 3; i++){
                try{
                    return await base.ReplyAsync(message, isTTS, embed, options);
                }
                catch(Exception e)
                {
                    last = e;
                    if(i == 2){ throw; }
                }
            }
            throw last; // Unexpected throw
        }
    }
}
