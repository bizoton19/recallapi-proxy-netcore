namespace Opendata.Recalls.Repository
{
    using Opendata.Recalls.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SearchQueryResult" />
    /// </summary>
    [System.Serializable]
    public class SearchQueryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchQueryResult"/> class.
        /// </summary>
        public SearchQueryResult()
        {
            Recalls = new List<Recall>();
        }

        /// <summary>
        /// Gets or sets the ResultCount
        /// </summary>
        public int ResultCount { get; set; }

        //public int DisplayCount { get; set; }
        /// <summary>
        /// Gets or sets the Recalls
        /// </summary>
        public List<Recall> Recalls { get; set; }
    }
}
