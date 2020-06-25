namespace Opendata.Recalls.Repository
{
    using Elasticsearch.Net;
    using Nest;
    using Opendata.Recalls.Commands;
    using Opendata.Recalls.Models;
   
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ElasticSearchRepository" />
    /// </summary>
    public class ElasticSearchRepository : IRecallApiProxyRepository
    {
        /// <summary>
        /// Defines the config
        /// </summary>
        private IConnectionSettingsValues config;

        /// <summary>
        /// Defines the settings
        /// </summary>
        //private RepoSettings settings;

        /// <summary>
        /// Defines the _indexname
        /// </summary>
        private const string _indexname = "cpsc-resolvedrecall";

        /// <summary>
        /// Defines the _client
        /// </summary>
        private ElasticClient _client;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        //private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchRepository"/> class.
        /// </summary>
        /// <param name="_logger">The _logger<see cref="ILogger"/></param>
       

        /// <summary>
        /// The Init
        /// </summary>
        private void Init()
        {
         
            var connString = Environment.GetEnvironmentVariable("es_connection");
            var node = new Uri(connString);

            var connectionPool = new SniffingConnectionPool(new[] { node });

            config = new ConnectionSettings(new Uri(connString))
               .DefaultMappingFor<Recall>(i => i
               .IndexName(_indexname))
               .DisableDirectStreaming()
               .PrettyJson()
               .DisableAutomaticProxyDetection(false)
               //.BasicAuthentication(settings.UserName, settings.Password)
               .RequestTimeout(TimeSpan.FromSeconds(60));

            _client = new ElasticClient(config);
        }

        /// <summary>
        /// The RetrieveRecall
        /// </summary>
        /// <param name="command">The command<see cref="SearchCommand"/></param>
        /// <param name="limit">The limit<see cref="int"/></param>
        /// <returns>The <see cref="Task{SearchQueryResult}"/></returns>
        public async Task<SearchQueryResult> RetrieveRecall(SearchCommand command, int limit = 500)
        {
            string[] multiMatchFields = new string[] { "fullTextSearch", "injuries", "manufacturerCountries", "manufacturers", "products", "retailers", "recallNumber" };

            DateTime recallStartDate = string.IsNullOrEmpty(command.Data.RecallDateStart)
                ? DateTime.Now.AddYears(-50).Date
                : DateTime.Parse(command.Data.RecallDateStart).Date;

            DateTime recallDateEnd = string.IsNullOrEmpty(command.Data.RecallDateEnd)
               ? DateTime.Now.Date
               : DateTime.Parse(command.Data.RecallDateEnd).Date;
            var SearchResponse = await _client.SearchAsync<Recall>(s => s

              .Query(q =>
                    q.Bool(b => b
                     .Must(
                        mu => mu
                        .DateRange(r => r
                                .Field(f => f.RecallDate)
                                .GreaterThanOrEquals(command.Data.RecallDateStart)
                                .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd"))
                       ),
                        mu => mu
                        .MultiMatch(c => c
                            .Query(command.Data.SearchFor)
                            .Analyzer("standard")
                            .Slop(3)
                            .AutoGenerateSynonymsPhraseQuery(true)
                            ),
                         pname => pname
                        .MultiMatch(c => c
                            .Query(command.Data.ProductName)
                            .Analyzer("standard")
                            .Slop(3)
                            .AutoGenerateSynonymsPhraseQuery(true)
                            ),
                          mu => mu
                        .MultiMatch(c => c
                            .Query(command.Data.ProductModel)
                            .Analyzer("standard")
                            .Slop(3)
                            .AutoGenerateSynonymsPhraseQuery(true)
                            ),
                        term => term
                               .Term(f => f
                               .Field("manufacturers.name.keyword")
                                    .Value(command.Data.ManufacturerName)

                           ),

                          term => term
                               .Term(f => f
                               .Field("products.type.keyword")
                                    .Value(command.Data.ManufacturerName)
                           )

                        )

                      )
                    )
                .From(string.IsNullOrEmpty(command.Data.From) ? 0 : Convert.ToInt32(command.Data.From))
                .Size(string.IsNullOrEmpty(command.Data.Size) ? 50 : Convert.ToInt32(command.Data.Size))
                .Sort(ss => ss
                   .Descending(r => r.RecallDate)
                // .Ascending(SortSpecialField.Score)
                )

               );

            return new SearchQueryResult()
            {
                ResultCount = Convert.ToInt32(SearchResponse.HitsMetadata.Total),
                Recalls = ConvertSearchResponseToRecalls(SearchResponse).ToList()

            };
        }

        /// <summary>
        /// The ConvertSearchResponseToRecalls
        /// </summary>
        /// <param name="response">The response<see cref="ISearchResponse{Recall}"/></param>
        /// <returns>The <see cref="IEnumerable{Recall}"/></returns>
        private IEnumerable<Recall> ConvertSearchResponseToRecalls(ISearchResponse<Recall> response)
        {
            var recalls = response.Documents;
            var mappedRecalls = new List<Recall>();
            foreach (var item in recalls)
            {
                mappedRecalls.Add(new Opendata.Recalls.Models.Recall()
                {
                    RecallDate = item.RecallDate,
                    RecallID = item.RecallID,
                    RecallNumber = item.RecallNumber,
                    Title = item.Title,
                    Description = item.Description,
                    ConsumerContact = item.ConsumerContact,
                    Hazards = item.Hazards,
                    Images = item.Images,
                    Injuries = item.Injuries,
                    Inconjunctions = item.Inconjunctions,
                    LastPublishDate = item.LastPublishDate,
                    ManufacturerCountries = item.ManufacturerCountries,
                    Manufacturers = item.Manufacturers,
                    Products = item.Products,
                    ProductUPCs = item.ProductUPCs,
                    Category = item.Category,
                    Remedies = item.Remedies,
                    Retailers = item.Retailers,
                    URL = item.URL
                });
            }
            return recalls;
        }

        /// <summary>
        /// retreive latest recalls going back to x months. Current default is 4 months
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<SearchQueryResult> RetrieveLastest(int limit = 15)
        {
            var yearFromNow = DateTime.Today.AddMonths(-4).ToString("yyyy-MM-dd");
            var searchRes = await _client.SearchAsync<Recall>(s => s

              .Query(q =>
                    q.Bool(b => b
                     .Must(mu => mu
                        .DateRange(r => r
                                .Field(f => f.RecallDate)
                                .GreaterThanOrEquals(yearFromNow)
                                .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd"))
                       )
                    )
                )
                )


                .Sort(ss => ss
                   .Descending(r => r.RecallDate)

                )
                .Size(15)

                );
            var recalls = ConvertSearchResponseToRecalls(searchRes);
            return new SearchQueryResult()
            {
                ResultCount = Convert.ToInt32(searchRes.HitsMetadata.Total),
                Recalls = recalls.ToList().OrderByDescending(x => x.RecallDate).ToList()
            };
        }

        public Task<List<Recall>> RetrieveRecall(SearchCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
