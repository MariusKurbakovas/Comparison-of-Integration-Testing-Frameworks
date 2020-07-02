namespace DiscountMetadata
{
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using Services;

    public static class AutofacConfig
    {
        public static void RegisterAutofacDependencies(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            var discountMetadatasFilePath = configuration.GetSection("DiscountMetadatasFilePath").Get<string>();
            containerBuilder.RegisterInstance(new MetadataService(discountMetadatasFilePath)).As<IMetadataService>();
        }
    }
}