using aaa_aspdotnet.src.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace aaa_aspdotnet.src.Common.pagination
{

    public class PaginationFilterDto
    {
        [FromQuery(Name = "page")]
        [SwaggerParameter("Page Number", Required = true)]
        [DefaultValue(1)]

        public int Page { get; set; }

        [FromQuery(Name = "pageSize")]
        [SwaggerParameter("Page size", Required = true)]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [FromQuery(Name = "search")]
        [SwaggerParameter("Search keyword", Required = false)]

        public string? Search { get; set; }

        [FromQuery(Name = "searchColumns")]
        [SwaggerParameter("Columns to search", Required = false)]
        public string? SearchColumns { get; set; }

        [FromQuery(Name = "orderByColumn")]
        [SwaggerParameter("Column to order by", Required = false)]
        public string? OrderByColumn { get; set; }

        [FromQuery(Name = "sortDirection")]
        [SwaggerParameter("Sort direction (0=ASC/1=DESC)", Required = false)]
        public ESortDirection? SortDirection { get; set; } = ESortDirection.ASC;

    }

}
