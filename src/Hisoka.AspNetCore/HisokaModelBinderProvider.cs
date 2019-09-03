using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Hisoka.AspNetCore
{
    public class HisokaModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Método responsável por obter o ModelBinder do queryFilter
        /// </summary>
        /// <param name="context">contexto do modelbinder</param>
        /// <returns>retorna o modelbinder de queryFilter</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(ResourceQueryFilter))
                return new BinderTypeModelBinder(typeof(HisokaModelBinder));

            return null;
        }
    }
}
