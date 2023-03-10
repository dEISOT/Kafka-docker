using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    tokenSource.Cancel();
};

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = args[0] ?? "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Latest
};

var consumer = new ConsumerBuilder<Null, string>(consumerConfig)
    .Build();

const string topic = "test-topic";
consumer.Subscribe(topic);

while (!tokenSource.IsCancellationRequested)
{
    var message = consumer.Consume(new TimeSpan(0,0,1));
    if(message != null)
    {  
        Console.WriteLine(message.TopicPartitionOffset + " " + message.Value);
    }

}

consumer.Close();