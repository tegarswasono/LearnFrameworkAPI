using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Common
{
    public class GeneralDatatableRequestModel
    {
        public string? SortBy { get; set; }
        public bool Descending { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 20;
    }
    public class GeneralDatatableResponseModel<T> where T : class
    {
        public string? SortBy { get; set; }
        public bool Descending { get; set; }
        public int Page { get; set; }
        public int RowsPerPage { get; set; }
        public int RowsNumber { get; set; }
        public List<T> Result { get; set; } = [];

        public static GeneralDatatableResponseModel<T> Dto(List<T> result, GeneralDatatableRequestModel param, int rowsNumber)
        {
            return new GeneralDatatableResponseModel<T>()
            {
                SortBy = param.SortBy,
                Descending = param.Descending,
                Page = param.Page,
                RowsPerPage = param.RowsPerPage,
                Result = result,
                RowsNumber = rowsNumber
            };
        }
    }
}
