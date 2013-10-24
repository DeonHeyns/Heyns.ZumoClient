﻿//    Copyright 2012 Deon Heyns
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Globalization;
using System.Net;
using RestSharp;

namespace Heyns.ZumoClient
{
    /// <summary>
    /// The Client used to interact with the Windows Azure MobileServices Api
    /// </summary>
    public sealed class MobileServiceClient : IDisposable
    {

        private readonly string _mobileServicesUri;
        private IRestClient _httpClient;
        private const string LoginUrl = "login?mode=authenticationToken";
        private bool _disposed;
        /// <summary>
        /// The constructor that requires the base Windows Azure endpoint for MobileServices and the Api Key 
        /// </summary>
        /// <param name="mobileServicesUri"></param>
        /// <param name="apiKey"></param>
        /// <param name="masterKey"></param>
        public MobileServiceClient(string mobileServicesUri, string apiKey, bool masterKey = false)
        {
            if (mobileServicesUri == null) 
                throw new ArgumentNullException("mobileServicesUri");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException("apiKey");

            _mobileServicesUri = mobileServicesUri;

            _httpClient = new RestClient(mobileServicesUri);
            _httpClient.AddDefaultHeader("Accept", "application/json");
            _httpClient.AddDefaultHeader("Content-Type","application/json");

            _httpClient.AddDefaultHeader(masterKey ? "X-ZUMO-MASTER" : "X-ZUMO-APPLICATION", apiKey);
        }
        
        /// <summary>
        /// Authenticate this user against MobileServices
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <returns>MobileServicesUser</returns>
        internal MobileServicesUser Authenticate(string authenticationToken)
        {
            if(authenticationToken == null)
                throw  new ArgumentNullException("authenticationToken");
            if(string.IsNullOrWhiteSpace(authenticationToken))
                throw   new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The authenticationToken can not be null or empty"));

            var url = string.Concat(_mobileServicesUri, LoginUrl);

            var request = new RestRequest(url, Method.POST);
            request.AddParameter("authenticationToken", authenticationToken);
            var response = _httpClient.Execute<dynamic>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            
            return new MobileServicesUser(response.Data.user.userid);
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
        /// Allows for the data store to be queried using Filter, Top, Skip, OrderBy and Select
        /// Call the execute method to execute the query against the data store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMobileServicesTableQuery<T> QueryTable<T>()
            where T : new()
        {
            return new TableQuery<T>(_httpClient);
        }

        // Dispose is not called invoke the Destructor
        ~MobileServiceClient()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient = null;
                }
                _disposed = true;
            }
        }
    }
}
