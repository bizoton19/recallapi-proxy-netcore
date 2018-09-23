using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Net;
using Opendata.Recalls.Models;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.Net.Http;
using Opendata.Recalls.Commands;

namespace Opendata.Recalls.Repository
{
    public class RecallApiProxyRepository : IRecallApiProxyRepository
    {
        public async Task<List<Recalls.Models.Recall>> RetrieveRecall(SearchCommand command)
        {
            string SERVICE_ROOT = "https://www.saferproducts.gov/RestWebServices/Recall?format=json" ;//Environment.GetEnvironmentVariable("CPSCAPIUrlRoot");


            StringBuilder uriBuilder = new StringBuilder(SERVICE_ROOT);

            List<Recalls.Models.Recall> recList = null;
            List<Recalls.Models.Recall> finalPO = null;

            if (!String.IsNullOrEmpty(command.SearchFor))
            {
                    StringBuilder uriBuilder1 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder1.AppendFormat("&RecallTitle={0}", command.SearchFor);
                    Task<List<Recall>> poList1 = RecallList(uriBuilder1.ToString());

                    StringBuilder uriBuilder2 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder2.AppendFormat("&ProductName={0}", command.SearchFor);
                    var poList2 = RecallList(uriBuilder2.ToString());

                    StringBuilder uriBuilder3 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder3.AppendFormat("&Hazard={0}", command.SearchFor);
                    var poList3 = RecallList(uriBuilder3.ToString());

                    StringBuilder uriBuilder4 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder4.AppendFormat("&ManufacturerCountry={0}", command.SearchFor);
                    var poList4 = RecallList(uriBuilder4.ToString());

                    StringBuilder uriBuilder5 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder5.AppendFormat("&Manufacturer={0}", command.SearchFor);
                    var poList5 = RecallList(uriBuilder5.ToString());

                    StringBuilder uriBuilder6 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder6.AppendFormat("&RecallNumber={0}", command.SearchFor);
                    var poList6 = RecallList(uriBuilder6.ToString());
                    finalPO = poList1.Result.Union(poList1.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList2.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList3.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList4.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList5.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList6.Result, new RecallComparer()).ToList();

            }

            if (!String.IsNullOrEmpty(command.ProductName) ||
                 !String.IsNullOrEmpty(command.ManufacturerName) ||
                 !String.IsNullOrEmpty(command.ProductType) ||
                 !String.IsNullOrEmpty(command.ProductModel) ||
                 !String.IsNullOrEmpty(command.RecallDateEnd) ||
                 !String.IsNullOrEmpty(command.RecallDateStart))
            {
                if (!String.IsNullOrEmpty(command.ProductName))
                {
                    uriBuilder.AppendFormat("&ProductName={0}", command.ProductName);
                }

                if (!String.IsNullOrEmpty(command.ManufacturerName))
                {
                    uriBuilder.AppendFormat("&manufacturer={0}", command.ManufacturerName);
                }

                if (!String.IsNullOrEmpty(command.ProductType))
                {
                    uriBuilder.AppendFormat("&ProductType={0}", command.ProductType);
                }

                if (!String.IsNullOrEmpty(command.ProductModel))
                {
                    uriBuilder.AppendFormat("&RecallDescription={0}", command.ProductModel);
                }
                if (!String.IsNullOrEmpty(command.RecallDateEnd))
                {
                    uriBuilder.AppendFormat("&RecallDateEnd={0:yyyy‐MM‐dd}", command.RecallDateEnd);
                }

                if (!String.IsNullOrEmpty(command.RecallDateStart))
                {
                    uriBuilder.AppendFormat("&RecallDateStart={0:yyyy‐MM‐dd}", command.RecallDateStart);
                }

                recList = await RecallList(uriBuilder.ToString());
            }

            if (finalPO == null)
            {
                return recList.OrderByDescending(x => x.RecallDate).ToList();
            }
            else if (recList == null)
            {
                return finalPO.OrderByDescending(x => x.RecallDate).ToList();
            }
            else
            {
                finalPO = finalPO.Intersect(recList, new RecallComparer()).ToList();
                return finalPO.OrderByDescending(x => x.RecallDate).ToList(); 
            }
        }        

        public static async Task<List<Recalls.Models.Recall>> RecallList( String uriBuilder)
        {
            List<Recalls.Models.Recall> PoList = null;
            using (HttpClient getClient = new HttpClient())
            {
                try
                {
                    // ensure desired encoding is used
                    getClient.DefaultRequestHeaders.Clear();
                    getClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("UTF8"));
                    string jsonResult = await getClient.GetStringAsync(uriBuilder);
                    PoList = JsonConvert.DeserializeObject<List<Recalls.Models.Recall>>(jsonResult);                   
                }
                catch (Exception ex)
                {
                    //log.Info("Failed to Get RecallList", ex.Message);
                    string errorMessage = ex.Message;
                }
                //log.Info("End Get RecallList");
            }
            return PoList;
        }
    }
}