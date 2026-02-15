using FoxholeBot.src.Discord.shemas;
using FoxholeBot.src.Discord.shemas.commands;

namespace FoxholeBot.src.Discord.commands
{
    public sealed record AuthenticateCommand : ICommandStandard<Commands,AuthenticateRequest,AuthenticateResponse>
    {
        public AuthenticateCommand(AuthenticateRequest args)
        {
            request = new SendCommandPayloadStandard<Commands, AuthenticateRequest>() { Cmd = Commands.AUTHENTICATE, Args = args };
        }
        public AuthenticateCommand()
        {
            request = new SendCommandPayloadStandard<Commands, AuthenticateRequest>() { Cmd = Commands.AUTHENTICATE, Args = new AuthenticateRequest() };
        }
    }
}
