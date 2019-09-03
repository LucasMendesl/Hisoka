using System;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hisoka.AspNetCore
{
    /// <summary>
    /// ModelBinder para interceptar um request em busca dos parametros de querystring para execução de um filtro
    /// </summary>
    public class HisokaModelBinder : IModelBinder
    {
        /// <summary>
        /// Método responsável por realizar o bind dos parametros da querystring para a classe <see cref="QueryFilter" />
        /// </summary>
        /// <param name="bindingContext">bind contendo os dados da requisição</param>
        /// <returns>retorna uma instância de <see cref="QueryFilter" /></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(ResourceQueryFilter))
                throw new ArgumentNullException(nameof(bindingContext));

            var nameValueCollection = HttpUtility.ParseQueryString(bindingContext.HttpContext.Request.QueryString.Value);

            var queryFilter = new ResourceQueryFilter(
                                              GetFilter(nameValueCollection),
                                              nameValueCollection[QueryFilterOptions.OrderByQueryAlias],
                                              nameValueCollection[QueryFilterOptions.SelectFieldsQueryAlias],
                                              GetPaging(nameValueCollection));

            bindingContext.Result = ModelBindingResult.Success(queryFilter);
            return Task.CompletedTask;
        }

        private string GetFilter(NameValueCollection queryString)
        {
            var excludeParameters = new[]
            {
                QueryFilterOptions.OrderByQueryAlias,
                QueryFilterOptions.PageSizeQueryAlias,
                QueryFilterOptions.PageNumberQueryAlias,
                QueryFilterOptions.SelectFieldsQueryAlias
            };

            return string.Join(",", queryString.AllKeys.Where(key => !excludeParameters.Contains(key)).Select(s => $"{s}={queryString[s]}"));
        }

        private Paginate GetPaging(NameValueCollection queryString)
        {
            int pageNumber, pageSize;
            int.TryParse(queryString[QueryFilterOptions.PageNumberQueryAlias], out pageNumber);
            int.TryParse(queryString[QueryFilterOptions.PageSizeQueryAlias], out pageSize);

            return new Paginate(pageNumber, pageSize);
        }
    }
}
