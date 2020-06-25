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
        static HttpClient getClient = new HttpClient();

        public async Task<List<Recalls.Models.Recall>> RetrieveRecall(SearchCommand command)
        {
            string SERVICE_ROOT = "https://www.saferproducts.gov/RestWebServices/Recall?format=json" ;//Environment.GetEnvironmentVariable("CPSCAPIUrlRoot");
             
            StringBuilder uriBuilder = new StringBuilder(SERVICE_ROOT);

            IDictionary<string,Recalls.Models.Recall> recList = null;
            List<Recalls.Models.Recall> finalPO = null;
            //refactoring by having one dictionary
            //only add items to the dictionary if the key does not exist
            


            if (!String.IsNullOrEmpty(command.Data.SearchFor))
            {
                
                  
                    StringBuilder uriBuilder1 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder1.AppendFormat("&RecallTitle={0}", command.Data.SearchFor);
                    Task<IDictionary<string,Recall>> poList1 = RecallList(uriBuilder1.ToString());

                    StringBuilder uriBuilder2 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder2.AppendFormat("&ProductName={0}", command.Data.SearchFor);
                    poList1.Result.Union(RecallList(uriBuilder2.ToString()).Result);

                    StringBuilder uriBuilder3 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder3.AppendFormat("&Hazard={0}", command.Data.SearchFor);
                    poList1.Result.Union(RecallList(uriBuilder3.ToString()).Result);

                    StringBuilder uriBuilder4 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder4.AppendFormat("&ManufacturerCountry={0}", command.Data.SearchFor);
                    poList1.Result.Union(RecallList(uriBuilder4.ToString()).Result);

                    StringBuilder uriBuilder5 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder5.AppendFormat("&Manufacturer={0}", command.Data.SearchFor);
                    poList1.Result.Union(RecallList(uriBuilder5.ToString()).Result);

                    StringBuilder uriBuilder6 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder6.AppendFormat("&RecallNumber={0}", command.Data.SearchFor);
                    poList1.Result.Union(RecallList(uriBuilder6.ToString()).Result).ToList();
                    finalPO = poList1.Result.Values.ToList();
                    //finalPO = poList1.Result.Union(poList1.Result, new RecallComparer()).ToList();
                    //finalPO = finalPO.Union(poList2.Result, new RecallComparer()).ToList();
                    //finalPO = finalPO.Union(poList3.Result, new RecallComparer()).ToList();
                    //finalPO = finalPO.Union(poList4.Result, new RecallComparer()).ToList();
                    //finalPO = finalPO.Union(poList5.Result, new RecallComparer()).ToList();
                    //finalPO = finalPO.Union(poList6.Result, new RecallComparer()).ToList();

            }

            if (!String.IsNullOrEmpty(command.Data.ProductName) ||
                 !String.IsNullOrEmpty(command.Data.ManufacturerName) ||
                 !String.IsNullOrEmpty(command.Data.ProductType) ||
                 !String.IsNullOrEmpty(command.Data.ProductModel) ||
                 !String.IsNullOrEmpty(command.Data.RecallDateEnd) ||
                 !String.IsNullOrEmpty(command.Data.RecallDateStart))
            {
                if (!String.IsNullOrEmpty(command.Data.ProductName))
                {
                    uriBuilder.AppendFormat("&ProductName={0}", command.Data.ProductName);
                }

                if (!String.IsNullOrEmpty(command.Data.ManufacturerName))
                {
                    uriBuilder.AppendFormat("&manufacturer={0}", command.Data.ManufacturerName);
                }

                if (!String.IsNullOrEmpty(command.Data.ProductType))
                {
                    uriBuilder.AppendFormat("&ProductType={0}", command.Data.ProductType);
                }

                if (!String.IsNullOrEmpty(command.Data.ProductModel))
                {
                    uriBuilder.AppendFormat("&RecallDescription={0}", command.Data.ProductModel);
                }
                if (!String.IsNullOrEmpty(command.Data.RecallDateEnd))
                {
                    uriBuilder.AppendFormat("&RecallDateEnd={0:yyyy‐MM‐dd}", command.Data.RecallDateEnd);
                }

                if (!String.IsNullOrEmpty(command.Data.RecallDateStart))
                {
                    uriBuilder.AppendFormat("&RecallDateStart={0:yyyy‐MM‐dd}", command.Data.RecallDateStart);
                }

                recList = await RecallList(uriBuilder.ToString());
            }

            if (finalPO == null)
            {
                var t = recList.OrderByDescending(x=>x.Value.RecallDate)
                                .Select(x=>x.Value)
                                .ToList();
                return t;
            }
            else if (recList == null)
            {
                return finalPO.OrderByDescending(x => x.RecallDate).ToList();
            }
            else
            {
                finalPO.AddRange(recList.Values.ToList());
                return finalPO.OrderByDescending(x => x.RecallDate).ToList(); 
            }
        }        

        public static async Task<IDictionary<string,Recall>> RecallList( String uriBuilder)
        {
             IDictionary<string,Recall> poList = new Dictionary<string,Recalls.Models.Recall>();

            
            
                try
                {
                    // ensure desired encoding is used
                    getClient.DefaultRequestHeaders.Clear();
                    getClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("UTF8"));
                    string jsonResult = await getClient.GetStringAsync(uriBuilder);
                    poList = JsonConvert.DeserializeObject<List<Recalls.Models.Recall>>(jsonResult).ToDictionary(r=>r.RecallNumber,r=>r);                   
                }
                catch (Exception ex)
                {
                    //log.Info("Failed to Get RecallList", ex.Message);
                    string errorMessage = ex.Message;
                }
                //log.Info("End Get RecallList");
            
            return poList;
        }

       
    }
}