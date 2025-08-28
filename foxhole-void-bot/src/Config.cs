

using DotNetEnv;
using System;
using System.IO;
#nullable enable
namespace FoxholeBot
{
    public class Config
    {
        static bool init = false;
        public static void Init()
        {
            Env.Load();
            init = true;
        }
        public static string GetBotToken()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            if(!init)
                Init();
            string? token = Environment.GetEnvironmentVariable("BOT_TOKEN");
            if (token == null)
                Console.Error.WriteLine("Token was not found");
            return token ?? "NOT FOUND";
        }
    }
}