using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    /// <summary>
    /// This class inherits List so, it is basically a List<T> that has 4 properties
    /// Use these properties to determine the total size of the List object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">Object data passed by the Repo</param>
        /// <param name="pageNumber">Passed from Controller-> Repo-> here</param>
        /// <param name="pageSize">Passed from Controller-> Repo-> here</param>
        /// <returns>A fully created PageList<T> object</returns>
        public static async Task<PagedList<T>> CreateAsync
        (IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}