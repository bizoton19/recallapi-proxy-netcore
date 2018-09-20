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

namespace Opendata.Recalls.Repository
{
    public class RecallRepository : IRecallRepository
    {
        public async Task<List<Recalls.Models.Recall>> RetrieveRecall(string searchfor, string productname, string manufacturername, string producttype, string productmodel, string recalldateend, string recalldatestart)
        {
            string SERVICE_ROOT = "https://www.saferproducts.gov/RestWebServices/Recall?format=json" ;//Environment.GetEnvironmentVariable("CPSCAPIUrlRoot");


            StringBuilder uriBuilder = new StringBuilder(SERVICE_ROOT);

            List<Recalls.Models.Recall> recList = null;
            List<Recalls.Models.Recall> finalPO = null;

            if (!String.IsNullOrEmpty(searchfor))
            {
                    StringBuilder uriBuilder1 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder1.AppendFormat("&RecallTitle={0}", searchfor);
                    Task<List<Recall>> poList1 = RecallList(uriBuilder1.ToString());

                    StringBuilder uriBuilder2 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder2.AppendFormat("&ProductName={0}", searchfor);
                    var poList2 = RecallList(uriBuilder2.ToString());

                    StringBuilder uriBuilder3 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder3.AppendFormat("&Hazard={0}", searchfor);
                    var poList3 = RecallList(uriBuilder3.ToString());

                    StringBuilder uriBuilder4 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder4.AppendFormat("&ManufacturerCountry={0}", searchfor);
                    var poList4 = RecallList(uriBuilder4.ToString());

                    StringBuilder uriBuilder5 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder5.AppendFormat("&Manufacturer={0}", searchfor);
                    var poList5 = RecallList(uriBuilder5.ToString());

                    StringBuilder uriBuilder6 = new StringBuilder(SERVICE_ROOT);
                    uriBuilder6.AppendFormat("&RecallNumber={0}", searchfor);
                    var poList6 = RecallList(uriBuilder6.ToString());
                    finalPO = poList1.Result.Union(poList1.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList2.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList3.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList4.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList5.Result, new RecallComparer()).ToList();
                    finalPO = finalPO.Union(poList6.Result, new RecallComparer()).ToList();

            }

            if (!String.IsNullOrEmpty(productname) ||
                 !String.IsNullOrEmpty(manufacturername) ||
                 !String.IsNullOrEmpty(producttype) ||
                 !String.IsNullOrEmpty(productmodel) ||
                 !String.IsNullOrEmpty(recalldateend) ||
                 !String.IsNullOrEmpty(recalldateend))
            {
                if (!String.IsNullOrEmpty(productname))
                {
                    uriBuilder.AppendFormat("&ProductName={0}", productname);
                }

                if (!String.IsNullOrEmpty(manufacturername))
                {
                    uriBuilder.AppendFormat("&manufacturer={0}", manufacturername);
                }

                if (!String.IsNullOrEmpty(producttype))
                {
                    uriBuilder.AppendFormat("&ProductType={0}", producttype);
                }

                if (!String.IsNullOrEmpty(productmodel))
                {
                    uriBuilder.AppendFormat("&RecallDescription={0}", productmodel);
                }
                if (!String.IsNullOrEmpty(recalldateend))
                {
                    uriBuilder.AppendFormat("&RecallDateEnd={0:yyyy‐MM‐dd}", recalldateend);
                }

                if (!String.IsNullOrEmpty(recalldateend))
                {
                    uriBuilder.AppendFormat("&RecallDateStart={0:yyyy‐MM‐dd}", recalldatestart);
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