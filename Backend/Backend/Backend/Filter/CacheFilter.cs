namespace Backend.Filter
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Caching.Memory;

    public class CacheActionFilter : IActionFilter
    {
        private readonly IMemoryCache _memoryCache;
        private readonly int _durationInSeconds;

        public CacheActionFilter(IMemoryCache memoryCache, int durationInSeconds = 30)
        {
            _memoryCache = memoryCache;
            _durationInSeconds = durationInSeconds;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            string cacheKey = context.HttpContext.Request.Path.ToString();

           
            if (_memoryCache.TryGetValue(cacheKey, out IActionResult cachedResult))
            {
                context.Result = cachedResult; 
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
            if (context.Result is ObjectResult objectResult && context.HttpContext.Response.StatusCode==200)
            {
                string cacheKey = context.HttpContext.Request.Path.ToString();
                _memoryCache.Set(cacheKey, objectResult, TimeSpan.FromSeconds(_durationInSeconds));
            }
        }
    }
}
