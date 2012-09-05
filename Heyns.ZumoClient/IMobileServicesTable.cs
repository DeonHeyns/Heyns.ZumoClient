using System.Collections.Generic;

namespace Heyns.ZumoClient
{
    public interface IMobileServicesTable<T> where T : new()
    {
        /// <summary>
        /// Gets by id and returns the deserialized type back
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<TKey>(TKey id);

        /// <summary>
        /// Get all the specified items in the entities table
        /// </summary>
        /// <returns><![CDATA[IEnumerable<T>]]></returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Will insert the item into the Windows Azure MobileServices table for this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        T Insert(T entity);

        /// <summary>
        /// Will update the item into the Windows Azure MobileServices table for this entity
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        T Update<TKey>(TKey id, T entity);

        /// <summary>
        /// Deletes the Entity with this id from the DB
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        void Delete<TKey>(TKey id);
    }
}