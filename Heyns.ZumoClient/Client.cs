using System;

using RestSharp;
namespace Heyns.ZumoClient
{
    /// <summary>
    /// The Client used to interact with the Windows Azure MobileServices Api
    /// </summary>
    public class Client
    {
        private readonly RestClient _httpClient;

        /// <summary>
        /// The constructor that requires the base Windows Azure endpoint for MobileServices and the Api Key 
        /// </summary>
        /// <param name="mobileServicesUri"></param>
        /// <param name="apiKey"></param>
        public Client(string mobileServicesUri, string apiKey)
        {
            if (mobileServicesUri == null) 
                throw new ArgumentNullException("mobileServicesUri");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException("apiKey");

            this._httpClient = new RestClient(mobileServicesUri);
            _httpClient.AddDefaultHeader("X-ZUMO-APPLICATION", apiKey);
            _httpClient.AddDefaultHeader("Accept", "application/json");
        }

        /// <summary>
        /// The main entry point into the Api that allows for fluent style calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Table<T> GetTable<T>()
            where T : new()
        {
            return new Table<T>(_httpClient);
        }
    }
}