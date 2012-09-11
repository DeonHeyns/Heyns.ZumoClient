using System;
using RestSharp;

namespace Heyns.ZumoClient
{
    /// <summary>
    /// The Client used to interact with the Windows Azure MobileServices Api
    /// </summary>
    public class MobileServicesClient
    {
        private readonly string _mobileServicesUri;
        private readonly string _apiKey;
        private readonly IRestClient _httpClient;
        
        /// <summary>
        /// The constructor that requires the base Windows Azure endpoint for MobileServices and the Api Key 
        /// </summary>
        /// <param name="mobileServicesUri"></param>
        /// <param name="apiKey"></param>
        public MobileServicesClient(string mobileServicesUri, string apiKey)
        {
            if (mobileServicesUri == null) 
                throw new ArgumentNullException("mobileServicesUri");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException("apiKey");

            _mobileServicesUri = mobileServicesUri;
            _apiKey = apiKey;

            _httpClient = new RestClient(mobileServicesUri);
            _httpClient.AddDefaultHeader("X-ZUMO-APPLICATION", apiKey);
            _httpClient.AddDefaultHeader("Accept", "application/json");
        }
        
        /// <summary>
        /// The main entry point into the Api that allows for fluent style calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IMobileServicesTable<T> GetTable<T>()
            where T : new()
        {
            return new Table<T>(_httpClient);
        }

        /// <summary>
        /// Allows for the data store to be queried using Filer, Top, Skip, Orderby and Select
        /// Call the execute method to execute the query against the data store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMobileServicesTableQuery<T> QueryTable<T>()
            where T : new()
        {
            return new TableQuery<T>(_httpClient);
        }
    }
}