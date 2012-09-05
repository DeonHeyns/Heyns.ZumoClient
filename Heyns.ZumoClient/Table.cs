using System;
using System.Collections.Generic;
using System.Net;

using RestSharp;

namespace Heyns.ZumoClient
{
    /// <summary>
    /// Wraps all operations in a fluent style Api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T>
                where T : new()
    {
        private readonly IRestClient _httpClient;
        public Table(IRestClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            _httpClient = httpClient;
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
        /// Will insert the item into the Windows Azure MobileServices table for this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        public T Insert(T entity)
        {
            var request = new RestRequest(ConstructTableUri(), Method.POST) { RequestFormat = DataFormat.Json };
            var response = InsertUpdate(request, entity);
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
            var response = InsertUpdate(request, entity);
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

        private static string ConstructTableUri()
        {
            return string.Format("tables/{0}", typeof (T).Name);
        }

        private static void ConstructPayload(T entity, IRestRequest request)
        {
            request.JsonSerializer = new ZumoClient.JsonSerializer();
            request.AddParameter(typeof(T).Name,request.JsonSerializer.Serialize(entity), ParameterType.RequestBody);
        }

        private IRestResponse<T> InsertUpdate(IRestRequest request, T entity)
        {
            ConstructPayload(entity, request);
            var response = _httpClient.Execute<T>(request);
            return response;
        }
    }
}