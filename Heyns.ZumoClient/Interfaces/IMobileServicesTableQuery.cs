using System.Collections.Generic;

namespace Heyns.ZumoClient
{
    public interface IMobileServicesTableQuery<T> where T : new()
    {
        /// <summary>
        /// Executes the commands against the data store this allows the stringing of commands
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> ExecuteQuery();

        /// <summary>
        /// Take the top number of records
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        IMobileServicesTableQuery<T> Top(int top);
        
        /// <summary>
        /// Skip a number of records and take from there on
        /// </summary>
        /// <param name="skip"></param>
        /// <returns></returns>
        IMobileServicesTableQuery<T> Skip(int skip);
        
        /// <summary>
        /// Allows for ordering of the records retrieved
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        IMobileServicesTableQuery<T> OrderBy(string orderby);
        
        /// <summary>
        /// Filtering of the records in the data store
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IMobileServicesTableQuery<T> Filter(string filter);
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        IMobileServicesTableQuery<T> Select(string select);
    }
}