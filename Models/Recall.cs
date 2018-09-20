using System.Collections.Generic;
using System;
namespace Opendata.Recalls.Models{
public class Recall
    {
        public int RecallID { get; set; }
        public string RecallNumber { get; set; }
        public DateTime? RecallDate { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string ConsumerContact { get; set; }
        public DateTime? LastPublishDate { get; set; }
        public List<Product> Products { get; set; }
        public List<Inconjunction> Inconjunctions { get; set; }
        public List<Image> Images { get; set; }
        public List<Injury> Injuries { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        public List<ManufacturerCountry> ManufacturerCountries { get; set; }
        public List<ProductUPC> ProductUPCs { get; set; }
        public List<Hazard> Hazards { get; set; }
        public List<Remedy> Remedies { get; set; }
        public List<Retailer> Retailers { get; set; }
    }
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string CategoryID { get; set; }
        public string NumberOfUnits { get; set; }
    }
    public class Inconjunction
    {
        public string Country { get; set; }
    }
    public class Image
    {
        public string URL { get; set; }
    }
    public class Injury
    {
        public string Name { get; set; }
    }
    public class Manufacturer
    {
        public string Name { get; set; }
        public string CompanyID { get; set; }
    }
    public class ManufacturerCountry
    {
        public string Country { get; set; }
    }
    public class ProductUPC
    {
        public string UPC { get; set; }
    }
    public class Hazard
    {
        public string Name { get; set; }
        public string HazardTypeID { get; set; }
    }
    public class Remedy
    {
        public string Name { get; set; }
    }
    public class Retailer
    {
        public string Name { get; set; }
        public string CompanyID { get; set; }
    }
}