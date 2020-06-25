
using System.ComponentModel.DataAnnotations;

namespace Opendata.Recalls.Commands{

public class SearchCommand{
    
   public Params Data { get; set; }

        /// <summary>
        /// Defines the <see cref="Params" />
        /// </summary>
        public class Params
        {
            /// <summary>
            /// Gets or sets the SearchFor
            /// </summary>
            public string SearchFor { get; set; }

            /// <summary>
            /// Gets or sets the ManufacturerName
            /// </summary>
            public string ManufacturerName { get; set; }

            /// <summary>
            /// Gets or sets the ProductName
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// Gets or sets the ProductType
            /// </summary>
            public string ProductType { get; set; }

            /// <summary>
            /// Gets or sets the ProductModel
            /// </summary>
            public string ProductModel { get; set; }

            /// <summary>
            /// Gets or sets the RecallDateEnd
            /// </summary>
            public string RecallDateEnd { get; set; }

            /// <summary>
            /// Gets or sets the RecallDateStart
            /// </summary>
            public string RecallDateStart { get; set; }

            /// <summary>
            /// Gets or sets the From
            /// </summary>
            public string From { get; set; }

            /// <summary>
            /// Gets or sets the Size
            /// </summary>
            public string Size { get; set; }
        }

              /// <summary>
        /// The IsValid
        /// </summary>
        /// <param name="value">The value<see cref="SearchCommand.Params"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool IsValid(SearchCommand.Params value)
        {
            return
                (string.IsNullOrEmpty(value.SearchFor)) ||
                (string.IsNullOrEmpty(value.ProductName)) ||
                (string.IsNullOrEmpty(value.ManufacturerName)) ||
                (string.IsNullOrEmpty(value.ProductModel)) ||
                (string.IsNullOrEmpty(value.ProductType)) ||
                (string.IsNullOrEmpty(value.RecallDateEnd)) ||
                (string.IsNullOrEmpty(value.RecallDateStart));
        }
}
     



}