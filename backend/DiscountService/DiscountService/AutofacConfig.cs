namespace DiscountService
{
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using RestSharp;
    using Services;
    using Services.ServiceAgent;

    public static class AutofacConfig
    {
        public static void RegisterAutofacDependencies(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            containerBuilder.RegisterType<DiscountService>().As<IDiscountService>();
            containerBuilder.RegisterType<DatabaseAgent>().As<IDatabaseAgent>();
            containerBuilder.RegisterType<DiscountMetadataServiceAgent>().As<IDiscountMetadataServiceAgent>();

            var metadataServiceUri = configuration.GetSection("MetadataServiceUri").Get<string>();
            containerBuilder.RegisterInstance(new RestClient(metadataServiceUri)).As<IRestClient>().SingleInstance();
        }
    }
}