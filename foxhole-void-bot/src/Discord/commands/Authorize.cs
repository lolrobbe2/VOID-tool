using FoxholeBot.src.Discord.shemas;
using FoxholeBot.src.Discord.shemas.commands;

namespace FoxholeBot.src.Discord.commands
{
    public sealed record AuthorizeCommand : ICommandStandard<Commands, AuthorizeRequest, AuthorizeResponse>
    {
        public AuthorizeCommand(AuthorizeRequest args)
        {
            request = new SendCommandPayloadStandard<Commands, AuthorizeRequest>() { Cmd = Commands.AUTHORIZE, Args = args };
        }
        public AuthorizeCommand()
        {
            request = new SendCommandPayloadStandard<Commands, AuthorizeRequest>() { Cmd = Commands.AUTHORIZE, Args = new AuthorizeRequest() { clientId = "", scopes = [] } };
        }
        public AuthorizeCommand(string clientId, OAuthScope[] scopes)
        {
            request = new SendCommandPayloadStandard<Commands, AuthorizeRequest>() { Cmd = Commands.AUTHORIZE, Args = new AuthorizeRequest() { clientId = clientId, scopes = scopes } };
        }
    }
}
