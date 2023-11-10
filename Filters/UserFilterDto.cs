using aaa_aspdotnet.src.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace aaa_aspdotnet.Filters
{
    public class UserFilterDto
    {
        [FromQuery(Name = "page")]
        [SwaggerParameter("Page number", Required = false)]
        public int? Page { get; set; }

        [FromQuery(Name = "pageSize")]
        [SwaggerParameter("Page size", Required = false)]
        public int? PageSize { get; set; }

        [FromQuery(Name = "search")]
        [SwaggerParameter("Search keyword", Required = false)]
        public string Search { get; set; }

        [FromQuery(Name = "searchColumns")]
        [SwaggerParameter("Columns to search", Required = false)]
        public string SearchColumns { get; set; }

        [FromQuery(Name = "orderByColumn")]
        [SwaggerParameter("Column to order by", Required = false)]
        public string OrderByColumn { get; set; }

        [FromQuery(Name = "sortDirection")]
        [SwaggerParameter("Sort direction (ASC/DESC)", Required = false)]
        public ESortDirection? SortDirection { get; set; }

    }

}
