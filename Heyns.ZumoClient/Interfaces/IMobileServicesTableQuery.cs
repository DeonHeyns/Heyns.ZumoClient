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

using System.Collections.Generic;

namespace Heyns.ZumoClient
{
    public interface IMobileServicesTableQuery<T> : IEnumerable<T>
        where T : new()
    {
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