using System;

#nullable enable
namespace FoxholeBot.src.Discord.shemas.commands
{
    public abstract record SendCommandPayload<TCommand, TArgs>
        where TCommand : Enum;

    public record SendCommandPayloadStandard<TCommand, TArgs>
    : SendCommandPayload<TCommand, TArgs>
    where TCommand : Enum
    {
        public required TCommand Cmd { get; init; }
        public TArgs? Args { get; init; }
        public object[]? Transfer { get; init; }
    }

    public sealed record SendCommandPayloadSubscription<TArgs>
    : SendCommandPayload<Commands, TArgs>
    {
        public required Commands Cmd { get; init; } // must be SUBSCRIBE or UNSUBSCRIBE
        public TArgs? Args { get; init; }
        public required string Evt { get; init; }
    }
}
