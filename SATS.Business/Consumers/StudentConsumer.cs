using MassTransit;

namespace SATS.Business.Consumers
{
    public class StudentConsumer : IConsumer<StudentMessage>
    {
        public async Task Consume(ConsumeContext<StudentMessage> context)
        {
            // Mesaj işleme kodu
            var message = context.Message;
            // İşleme işlemleri
        }
    }



    public class StudentMessage
    {
        public string Text { get; set; }
    }
}
