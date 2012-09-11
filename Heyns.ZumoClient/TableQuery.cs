using System;
using System.Collections.Generic;

using RestSharp;

namespace Heyns.ZumoClient
{
    public class TableQuery<T> : IMobileServicesTableQuery<T> where T : new()
    {
        private int _top;
        private int _skip;
        private string _orderby;
        private string _filter;
        private string _select;
        private readonly Table<T> _table;
        private readonly IRestClient _httpClient;

        internal TableQuery(IRestClient httpClient)
        {
            if (httpClient == null) 
                throw new ArgumentNullException("httpClient");
            _httpClient = httpClient;

            _table = new Table<T>(_httpClient);
        }

        public IEnumerable<T> ExecuteQuery()
        {
            var query = this.ToString();
            return !string.IsNullOrWhiteSpace(query) ? _table.Get(query) : null;
        }

        public IMobileServicesTableQuery<T> Top(int top)
        {
            this._top = top;
            return this;
        }

        public IMobileServicesTableQuery<T> Skip(int skip)
        {
            this._skip = skip;
            return this;
        }

        public IMobileServicesTableQuery<T> OrderBy(string orderby)
        {
            this._orderby = orderby;
            return this;
        }

        public IMobileServicesTableQuery<T> Filter(string filter)
        {
            this._filter = filter;
            return this;
        }

        public IMobileServicesTableQuery<T> Select(string select)
        {
            this._select = select;
            return this;
        }
        
        public override string ToString()
        {
            // stop Array.Copy after 3 items
            var query = new List<string>(5);
            if(_top > 0)
            {
                query.Add("$top=" + _top);
            }
            if (_skip > 0)
            {
                query.Add("$skip=" + _skip);
            }
            if (!string.IsNullOrWhiteSpace(_filter))
            {
                query.Add("$filter=" + _filter);
            }
            if (!string.IsNullOrWhiteSpace(_select))
            {
                query.Add("$select=" + _select);
            }
            if (!string.IsNullOrWhiteSpace(_orderby))
            {
                query.Add("$orderby=" + _orderby);
            }

            return query.Count > 0 ? string.Join("&", query) : null;
        }
    }
}
