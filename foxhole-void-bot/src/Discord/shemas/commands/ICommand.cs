using System;

namespace FoxholeBot.src.Discord.shemas.commands
{
    public record ICommandStandard<TCommand,TArgs,TResult>
        where TCommand : Enum
        where TResult : class
    {
        public SendCommandPayloadStandard<TCommand, TArgs> request;
    }

}
