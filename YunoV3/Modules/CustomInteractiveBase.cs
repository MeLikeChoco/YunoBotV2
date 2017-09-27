using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YunoV3.Modules
{
    public class CustomInteractiveBase : InteractiveBase<SocketCommandContext>
    {

        private IUserMessage _display;
        private CancellationTokenSource _cts;

        /// <summary>
        /// $"There were no {objects} found with `{input}`!"
        /// </summary>
        /// <param name="objects">The expected results (plural form)</param>
        /// <param name="input">The entered search term</param>
        /// <returns>Task</returns>
        public Task NoResultError(string objects, string input)
            => ReplyAsync($"There were no {objects} found with `{input}`!");

        /// <summary>
        /// $"There was nothing found with `{input}`!"
        /// </summary>
        /// <param name="input">The entered search term</param>
        /// <returns>Task</returns>
        public Task NoResultError(string input)
            => ReplyAsync($"There was nothing found with `{input}`!");

        /// <summary>
        /// Upload file to channel
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <param name="filename">The filename</param>
        /// <returns>Task</returns>
        public Task<RestUserMessage> UploadAsync(Stream stream, string filename)
            => Context.Channel.SendFileAsync(stream, filename);

        /// <summary>
        /// Send an embed message
        /// </summary>
        /// <param name="embed">The embed to send</param>
        /// <returns>Task</returns>
        public Task<RestUserMessage> SendEmbedAsync(Embed embed)
            => Context.Channel.SendMessageAsync("", embed: embed);
        
        /// <summary>
        /// Sends paginated message and waits for user input that matches the criteria
        /// Will throw error if original pager is deleted
        /// </summary>
        /// <param name="pager">The PaginatedMessage</param>
        /// <param name="criteria">The criteria needed</param>
        /// <param name="timeout">The amount of time the message should be live until deleted</param>
        /// <returns>Task</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<string> PagedReplyAndNextMessage(PaginatedMessage pager, Criteria<SocketMessage> criteria, TimeSpan? timeout = null)
        {

            _display = await PagedReplyAsync(pager);
            _cts = new CancellationTokenSource();

            Context.Client.MessageDeleted += CheckMessage;

            var response = await NextMessageAsync(criteria, timeout ?? TimeSpan.FromSeconds(60));

            _cts.Token.ThrowIfCancellationRequested();

            return response.Content;

        }

        private Task CheckMessage(Cacheable<IMessage, ulong> cache, ISocketMessageChannel channel)
        {

            if (cache.Id == _display.Id)
            {

                _cts.Cancel();
                Context.Client.MessageDeleted -= CheckMessage;
                return Task.CompletedTask;

            }
            else
                return Task.CompletedTask;

        }

    }
}
