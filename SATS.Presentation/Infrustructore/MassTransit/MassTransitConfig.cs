using MassTransit;
using SATS.Business.Consumers;

namespace SATS.Presentation.Infrustructore.MassTransit
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQOptions = configuration.GetSection("RabbitMQ");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<StudentConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOptions["Host"], "/", h => //cfg.Host : RabbitMQ sunucusuna bağlantı kurmak için gerekli ayarları yapar.,rabbitMQOptions["Host"] RabbitMQ sunucusunun adresini belirtir.
                    {
                        h.Username(rabbitMQOptions["Username"]); //, sunucunun bağlantı ayarlarını yapmanızı sağlar, örneğin kullanıcı adı ve şifre.
                        h.Password(rabbitMQOptions["Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
//"/" genellikle RabbitMQ'da varsayılan sanal ana bilgisayardır
/*
 1)Varsayılan Sanal Ana Bilgisayar ("/"):
cfg.Host("localhost", "/", h =>
{
    h.Username("guest");
    h.Password("guest");
});

 
 */
//2)Özel Sanal Ana Bilgisayar: my_vhost
/*
 cfg.Host("localhost", "my_vhost", h =>
{
    h.Username("guest");
    h.Password("guest");
});
 
 */