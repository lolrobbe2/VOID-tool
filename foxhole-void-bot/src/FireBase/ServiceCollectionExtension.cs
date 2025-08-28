using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace FoxholeBot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFirebase(this IServiceCollection services)
        {
            // Register the configuration as a singleton
            // Register FirebaseClient as scoped (one per scope)
            services.AddScoped<FirestoreClient>(client =>
            {
                // Load your service account JSON
                FireBase.FirebaseServiceAccount account = Config.GetServiceAccount();

                var builder = new FirestoreClientBuilder
                {
                    JsonCredentials = JsonSerializer.Serialize(account)
                };
                return builder.Build();
            });
            services.AddScoped<FirestoreDb>(provider =>
            {
                var client = provider.GetRequiredService<FirestoreClient>();
                var account = Config.GetServiceAccount(); // Or inject Config if preferred

                return FirestoreDb.Create(account.ProjectId, client);
            });

            return services;
        }
    }
}
