using Discord.Addons.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace YunoV3.Objects.Criterions
{
    public class IntegerCriteria : ICriterion<SocketMessage>
    {

        private int _min;
        private int _max;

        public IntegerCriteria()
            : this(int.MinValue, int.MaxValue) { }

        public IntegerCriteria(int exactMatch)
            : this(exactMatch, exactMatch) { }

        public IntegerCriteria(int min, int max)
        {

            _min = min;
            _max = max;

        }

        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketMessage parameter)
        {

            var content = parameter.Content;

            return Task.FromResult(int.TryParse(content, out var integer) && integer <= _max && integer >= _min);

        }

    }
}
