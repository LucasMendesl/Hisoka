using System;
using Hisoka.Configuration;
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

            HisokaConfiguration.ApplyConfig(option);
            mvcBuilder.AddMvcOptions(opt => opt.ModelBinderProviders.Insert(0, new HisokaModelBinderProvider()));
            
            return mvcBuilder;
        }
    }
}
