using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hisoka.AspNetCore
{
    /// <summary>
    /// Classe representando as extensões para o AspNet Core
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Método responsável por adicionar a configuração do hisoka
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IMvcBuilder AddHisoka(this IMvcBuilder mvcBuilder, Action<HisokaOptions> options = null)
        {
            var option = new HisokaOptions();
            options?.Invoke(option);
            QueryFilterOptions.ApplyConfig(option);

            mvcBuilder.AddMvcOptions(opt =>
            {
                opt.ModelBinderProviders.Insert(0, new HisokaModelBinderProvider());
            })
            .AddJsonOptions(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            });

            return mvcBuilder;
        }
    }
}
