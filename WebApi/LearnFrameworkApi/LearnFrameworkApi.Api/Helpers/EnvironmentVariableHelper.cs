namespace LearnFrameworkApi.Api.Helpers
{
    public static class EnvironmentVariableHelper
    {
        public static string StripeDomain(IConfiguration configuration)
        {
            return configuration.GetValue<string>(@"Stripe:Domain")!;
        }
        public static string StripeUsername(IConfiguration configuration)
        {
            return configuration.GetValue<string>(@"Stripe:Username")!;
        }
        public static int StripePort(IConfiguration configuration)
        {
            return configuration.GetValue<int>(@"Stripe:Port")!;
        }
    }
}
