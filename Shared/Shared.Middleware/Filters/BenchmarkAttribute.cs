﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shared.Middleware.Filters
{
    public class BenchmarkAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await next();

            stopWatch.Stop();
            context.HttpContext.Response.Headers.Add(
                "x-time-elapsed",
                stopWatch.Elapsed.ToString());
        }
    }
}
