using AutoMapper;
using Contracts;
using Elastic.Clients.Elasticsearch;
using FilterService.Models;
using MassTransit;

namespace FilterService.Consumers
{
    public class GameCreatedFilterConsumer(IMapper mapper,ElasticsearchClient elasticsearchClient,IConfiguration configuration) : IConsumer<GameCreated>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
        private readonly string indexName = configuration.GetValue<string>("indexName");

        public async Task Consume(ConsumeContext<GameCreated> context)
        {
            Console.WriteLine("Consuming filter service for created game ---> "+context.Message.Name);
            var objDTO = _mapper.Map<GameFilterItem>(context.Message);
            objDTO.GameId = context.Message.Id.ToString();
            var elasticSearch = await _elasticsearchClient.IndexAsync(objDTO,x => x.Index(indexName));

            if(!elasticSearch.IsValidResponse)
            {
                Console.WriteLine("Consuming proccess is not valid");
            }

        }
    }
}
