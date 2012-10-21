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
using System.Collections.Generic;
using System.Net;

using RestSharp;
using Newtonsoft.Json.Linq;

namespace Heyns.ZumoClient
{
    /// <summary>
    /// Wraps all operations in a fluent style Api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T> : IMobileServicesTable<T> where T : new()
    {
        private readonly IRestClient _httpClient;
        private readonly string _typeName;
        internal Table(IRestClient httpClient)
        {
            if (httpClient == null) 
                throw new ArgumentNullException("httpClient");
            
            _httpClient = httpClient;
            _typeName = typeof (T).Name;
        }

        /// <summary>
        /// Gets by id and returns the deserialized type back
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<TKey>(TKey id)
        {
            var request = new RestRequest(string.Format("{0}/{1}", ConstructTableUri(), id), Method.GET) 
            { RequestFormat = DataFormat.Json }; 
            var response = _httpClient.Execute<T>(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            return response.Data;
        }

        /// <summary>
        /// Get all the specified items in the entities table
        /// </summary>
        /// <returns><![CDATA[IEnumerable<T>]]></returns>
        public IEnumerable<T> Get()
        {
            var request = new RestRequest(ConstructTableUri(), Method.GET) 
            { RequestFormat = DataFormat.Json }; 
            var response = _httpClient.Execute<List<T>>(request);
            if(response.StatusCode != HttpStatusCode.OK)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            return response.Data;
        }

        /// <summary>
        /// Internal method used to execute OData styled queries against the data store
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<T> Get(string query)
        {
            var request = new RestRequest(string.Format("{0}?{1}",ConstructTableUri(), query), Method.GET) { RequestFormat = DataFormat.Json };
            var response = _httpClient.Execute<List<T>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            return response.Data;
        }

        /// <summary>
        /// Will insert the item into the Windows Azure MobileServices table for this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        public T Insert(T entity)
        {
            var request = new RestRequest(ConstructTableUri(), Method.POST) { RequestFormat = DataFormat.Json };
            var response = InsertOrUpdate(entity, request, true);
            if (response.StatusCode != HttpStatusCode.Created)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            return response.Data;
        }

        /// <summary>
        /// Will update the item into the Windows Azure MobileServices table for this entity
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        public T Update<TKey>(TKey id, T entity)
        {
            var request = new RestRequest(string.Format("{0}/{1}", ConstructTableUri(), id), Method.PATCH) { RequestFormat = DataFormat.Json };
            var response = InsertOrUpdate(entity, request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ZumoException(response.StatusDescription, response.StatusCode);
            return entity;
        }

        /// <summary>
        /// Deletes the Entity with this id from the DB
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        public void Delete<TKey>(TKey id)
        {
            var request = new RestRequest(string.Format("{0}/{1}", ConstructTableUri(), id), Method.DELETE) 
            { RequestFormat = DataFormat.Json };
            var response = _httpClient.Execute(request);
            if (response.StatusCode != HttpStatusCode.NoContent)
                throw new ZumoException(response.StatusDescription, response.StatusCode);

        }

        private string ConstructTableUri()
        {
            return string.Format("tables/{0}", _typeName);
        }

        private string ConstructPayload(T entity, bool removeIdKeyProperty = false)
        {
            var json = JObject.FromObject(entity);
            json["id"] = json["Id"];
            json.Remove("Id");
            if (removeIdKeyProperty)
                json.Remove("id");
            
            return json.ToString();
        }

        private IRestResponse<T> InsertOrUpdate(T entity, IRestRequest request, bool insert = false)
        {
            var data = ConstructPayload(entity, insert);
            request.AddParameter(string.Empty, data, ParameterType.RequestBody);
            var response = _httpClient.Execute<T>(request);
            return response;
        }
    }
}