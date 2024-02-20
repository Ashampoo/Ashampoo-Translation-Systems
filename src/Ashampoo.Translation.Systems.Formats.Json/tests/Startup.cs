﻿using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.Json.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().RegisterFormat<JsonFormat>();
    }
}