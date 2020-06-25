using System.Collections.Generic;
using System;
namespace Opendata.Recalls.Models{

    [Serializable]
    public class Recall
    {
        /// <summary>
        /// Gets or sets the Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the RecallID
        /// </summary>
        public int RecallID { get; set; }

        /// <summary>
        /// Gets or sets the RecallNumber
        /// </summary>
        public string RecallNumber { get; set; }

        /// <summary>
        /// Gets or sets the RecallDate
        /// </summary>
        public DateTime? RecallDate { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the ConsumerContact
        /// </summary>
        public string ConsumerContact { get; set; }

        /// <summary>
        /// Gets or sets the LastPublishDate
        /// </summary>
        public DateTime? LastPublishDate { get; set; }

        /// <summary>
        /// Gets or sets the Products
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the Inconjunctions
        /// </summary>
        public List<Inconjunction> Inconjunctions { get; set; }

        /// <summary>
        /// Gets or sets the Images
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// Gets or sets the Injuries
        /// </summary>
        public List<Injury> Injuries { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturers
        /// </summary>
        public List<Manufacturer> Manufacturers { get; set; }

        /// <summary>
        /// Gets or sets the ManufacturerCountries
        /// </summary>
        public List<ManufacturerCountry> ManufacturerCountries { get; set; }

        /// <summary>
        /// Gets or sets the ProductUPCs
        /// </summary>
        public List<ProductUPC> ProductUPCs { get; set; }

        /// <summary>
        /// Gets or sets the Hazards
        /// </summary>
        public List<Hazard> Hazards { get; set; }

        /// <summary>
        /// Gets or sets the Remedies
        /// </summary>
        public List<Remedy> Remedies { get; set; }

        /// <summary>
        /// Gets or sets the Retailers
        /// </summary>
        public List<Retailer> Retailers { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Product" />
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the CategoryID
        /// </summary>
        public string CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfUnits
        /// </summary>
        public string NumberOfUnits { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Inconjunction" />
    /// </summary>
    [Serializable]
    public class Inconjunction
    {
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Image" />
    /// </summary>
    [Serializable]
    public class Image
    {
        /// <summary>
        /// Defines the _url
        /// </summary>
        private string _url;

        /// <summary>
        /// Gets or sets the URL
        /// </summary>
        public string URL
        {
            get
            {
                return _url;
            }

            set
            {
                _url = ReplaceHttpWithHttps(value);
            }
        }

        /// <summary>
        /// The ReplaceHttpWithHttps
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string ReplaceHttpWithHttps(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Uri uri = new Uri(url);
                if (uri.Scheme == Uri.UriSchemeHttp)
                {
                    url = uri.OriginalString.Replace("http", "https");
                    return url;
                }
            }

            return url;
        }
    }

    /// <summary>
    /// Defines the <see cref="Injury" />
    /// </summary>
    [Serializable]
    public class Injury
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Manufacturer" />
    /// </summary>
    [Serializable]
    public class Manufacturer
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CompanyID
        /// </summary>
        public string CompanyID { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ManufacturerCountry" />
    /// </summary>
    [Serializable]
    public class ManufacturerCountry
    {
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ProductUPC" />
    /// </summary>
    [Serializable]
    public class ProductUPC
    {
        /// <summary>
        /// Gets or sets the UPC
        /// </summary>
        public string UPC { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Hazard" />
    /// </summary>
    [Serializable]
    public class Hazard
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the HazardTypeID
        /// </summary>
        public string HazardTypeID { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Remedy" />
    /// </summary>
    [Serializable]
    public class Remedy
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Retailer" />
    /// </summary>
    [Serializable]
    public class Retailer
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CompanyID
        /// </summary>
        public string CompanyID { get; set; }
    }
}