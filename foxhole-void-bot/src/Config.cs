

using DotNetEnv;
using FoxholeBot.FireBase;
using System;
using System.IO;
using System.Security.Cryptography;
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
            if (!init)
                Init();
            string? token = Environment.GetEnvironmentVariable("BOT_TOKEN");
            if (token == null)
                Console.Error.WriteLine("Token was not found");
            return token ?? "NOT FOUND";
        }

        /// <summary>
        /// This function get the firebase host url/base host
        /// </summary>
        /// <returns></returns>


        public static FirebaseServiceAccount GetServiceAccount()
        {
            FirebaseServiceAccount account = new FirebaseServiceAccount();
            account.Type = GetAccountType();
            account.ProjectId = GetProjectId();
            account.PrivateKeyId = GetPrivateKeyId();
            account.PrivateKey = GetPrivateKey();
            account.ClientEmail = GetClientEmail();
            account.ClientId = GetClientId();
            account.AuthUri = GetAuthUri();
            account.AuthProviderCertUrl = GetAuthProvider();
            account.ClientCertUrl = GetClientAuthProvider();
            return account;
        }

        private static string GetEnvVariable(string name)
        {
            if (!init)
                Init();
            string? var = Environment.GetEnvironmentVariable(name);
            if (var == null)
                Console.Error.WriteLine($"{name} not found");
            return var ?? "NOT FOUND";
        }
        private static string GetAccountType()
        {
            return GetEnvVariable("FIREBASE_ACCOUNT_TYPE");
        }

        public static string GetProjectId()
        {
            return GetEnvVariable("FIREBASE_PROJECT_ID");
        }

        private static string GetPrivateKeyId()
        {
            return GetEnvVariable("FIREBASE_PRIVATE_KEY_ID");
        }
        private static string GetPrivateKey()
        {
            return GetEnvVariable("FIREBASE_PRIVATE_KEY").Replace("\\n", "\n").Trim();
        }
        private static string GetClientEmail()
        {
            return GetEnvVariable("FIREBASE_CLIENT_EMAIL");
        }
        private static string GetClientId()
        {
            return GetEnvVariable("FIREBASE_CLIENT_ID");
        }

        private static string GetAuthUri()
        {
            return GetEnvVariable("FIREBASE_AUTH_URI");
        }
        private static string GetAuthProvider()
        {
            return GetEnvVariable("FIREBASE_AUTH_PROVIDER_X509_CERT_URL");
        }
        private static string GetClientAuthProvider()
        {
            return GetEnvVariable("FIREBASE_CLIENT_X509_CERT_URL");
        }
        public static string GetFoxholeShardURL()
        {
            return GetEnvVariable("SHARD");
        }

        public static string GetAssetMode()
        {
            return GetEnvVariable("ASSET_MODE");
        }

        public static string GetBootstrapCSS()
        {
            return (GetEnvVariable("ASSET_MODE") == "LOCAL") ? "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" : "";
        }

        public static string GetAssetUri()
        {
            return (GetEnvVariable("ASSET_MODE") == "LOCAL") ? "https://github.com/lolrobbe2/VOID-tool/raw/refs/heads/main" : "github";
        }
        public static string GetDeployUri()
        {
            return GetEnvVariable("DEPLOY_HOOK"); 
        }
        public static string GetPassWord()
        {
            return GetEnvVariable("PASSWORD");
        }
    }
}