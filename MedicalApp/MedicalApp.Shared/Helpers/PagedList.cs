using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.Shared.Helpers
{
    public class PagedList<T> : List<T>
    {
        public List<T> Data { get; private set; }
        public int RecordsTotal { get; private set; }
        public int RecordsFiltered { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            RecordsTotal = count;
            RecordsFiltered = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Data = items;
            AddRange(items);
        }
        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int? pageNumber, int? pageSize)
        {
            int pNumber = pageNumber ?? 1;
            if (pNumber <= 0) pNumber = 1;

            int pSize = pageSize ?? 10;
            if (pSize <= 0) pSize = 10;

            var count = await source.CountAsync();
            var items = await source.Skip((pNumber - 1) * pSize).Take(pSize).ToListAsync();
            return new PagedList<T>(items, count, pNumber, pSize);
        }
    }
}
