using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FilterService.Models;

namespace FilterService.Services
{
    public class FilterGameService(ElasticsearchClient elasticsearchClient, IConfiguration configuration) : IFilterGameService
    {
        private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
        private readonly string _indexName = configuration.GetValue<string>("indexName");

        public async Task<List<GameFilterItem>> SearchAsync(GameFilterItem gameFilterItem)
        {
            // If no filters are provided, return all items
            if (gameFilterItem == null) return await SearchAllAsync();

            var listQuery = new List<Action<QueryDescriptor<GameFilterItem>>>();

            // Price range filter
            AddPriceFilter(gameFilterItem, listQuery);

            // System requirements filters (Minimum & Recommended)
            AddSystemRequirementsFilter(gameFilterItem, listQuery);

            // If no query was added, use a match-all query
            if (listQuery.Count == 0)
            {
                listQuery.Add(q => q.MatchAll());
            }

            return await CalculateResultSet(listQuery);
        }

        private static void AddPriceFilter(GameFilterItem gameFilterItem, List<Action<QueryDescriptor<GameFilterItem>>> listQuery)
        {
            if (gameFilterItem.MinPrice > 0 || gameFilterItem.MaxPrice > 0)
            {
                listQuery.Add(q => q.Range(m => m
                    .NumberRange(f => f
                        .Field(a => a.Price)
                        .Gte(gameFilterItem.MinPrice > 0 ? Convert.ToDouble(gameFilterItem.MinPrice) : null)
                        .Lte(gameFilterItem.MaxPrice > 0 ? Convert.ToDouble(gameFilterItem.MaxPrice) : null)
                    )));
            }
        }

        private static void AddSystemRequirementsFilter(GameFilterItem gameFilterItem, List<Action<QueryDescriptor<GameFilterItem>>> listQuery)
        {
            if (!string.IsNullOrEmpty(gameFilterItem.MinimumSystemRequirement) && gameFilterItem.MinimumSystemRequirement != "string")
            {
                string minRequirement = $"*{gameFilterItem.MinimumSystemRequirement.ToLower()}*";
                listQuery.Add(q => q.Wildcard(m => m.Field("minimumSystemRequirement").Value(minRequirement)));
            }

            if (!string.IsNullOrEmpty(gameFilterItem.RecommendedSystemRequirement) && gameFilterItem.RecommendedSystemRequirement != "string")
            {
                string recRequirement = $"*{gameFilterItem.RecommendedSystemRequirement.ToLower()}*";
                listQuery.Add(q => q.Wildcard(m => m.Field("recommendedSystemRequirement").Value(recRequirement)));
            }
        }

        private async Task<List<GameFilterItem>> SearchAllAsync()
        {
            var listQuery = new List<Action<QueryDescriptor<GameFilterItem>>> { q => q.MatchAll() };
            return await CalculateResultSet(listQuery);
        }

        private async Task<List<GameFilterItem>> CalculateResultSet(List<Action<QueryDescriptor<GameFilterItem>>> listQuery)
        {
            var searchResponse = await _elasticsearchClient.SearchAsync<GameFilterItem>(s => s
                .Index(_indexName)
                .Query(q => q.Bool(b => b.Should(listQuery.ToArray())))
            );

            Console.WriteLine(searchResponse.DebugInformation);
            return [.. searchResponse.Documents];
        }
    }
}
