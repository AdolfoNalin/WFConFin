using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace WFConFin.Models
{
    public class PaginationResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public long TotalLine { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public PaginationResponse(IEnumerable<T> data, long totalLine, int skip, int take)
        {
            Data = data;
            TotalLine = totalLine;
            Skip = skip;
            Take = take;
        }
    }
}