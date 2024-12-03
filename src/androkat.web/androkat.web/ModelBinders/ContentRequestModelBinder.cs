#nullable enable
using androkat.infrastructure.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Threading.Tasks;

namespace androkat.web.ModelBinders;

public class ContentRequestModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var query = bindingContext.HttpContext.Request.Query;

        if (query.TryGetValue("tipus", out var tipusValue) && query.TryGetValue("id", out var idValue) && int.TryParse(tipusValue, out var tipus))
        {
            var model = new ContentRequest
            {
                Tipus = tipus,
                Id = idValue
            };
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Failed();
        return Task.CompletedTask;
    }
}

public class ContentRequestModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(ContentRequest))
        {
            return new BinderTypeModelBinder(typeof(ContentRequestModelBinder));
        }
        return null;
    }
}
