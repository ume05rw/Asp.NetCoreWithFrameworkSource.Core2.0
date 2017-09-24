// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderIISExtensions
    {
        // These are defined as ASPNETCORE_ environment variables by IIS's AspNetCoreModule.
        private static readonly string ServerPort = "PORT";
        private static readonly string ServerPath = "APPL_PATH";
        private static readonly string PairingToken = "TOKEN";
        private static readonly string IISAuth = "IIS_HTTPAUTH";

        /// <summary>
        /// Configures the port and base path the server should listen on when running behind AspNetCoreModule.
        /// The app will also be configured to capture startup errors.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseIISIntegration(this IWebHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            // Check if `UseIISIntegration` was called already
            if (hostBuilder.GetSetting(nameof(UseIISIntegration)) != null)
            {
                return hostBuilder;
            }

            var port = hostBuilder.GetSetting(ServerPort) ?? Environment.GetEnvironmentVariable($"ASPNETCORE_{ServerPort}");
            var path = hostBuilder.GetSetting(ServerPath) ?? Environment.GetEnvironmentVariable($"ASPNETCORE_{ServerPath}");
            var pairingToken = hostBuilder.GetSetting(PairingToken) ?? Environment.GetEnvironmentVariable($"ASPNETCORE_{PairingToken}");
            var iisAuth = hostBuilder.GetSetting(IISAuth) ?? Environment.GetEnvironmentVariable($"ASPNETCORE_{IISAuth}");

            if (!string.IsNullOrEmpty(port) && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(pairingToken))
            {
                // Set flag to prevent double service configuration
                hostBuilder.UseSetting(nameof(UseIISIntegration), true.ToString());

                var enableAuth = false;
                if (string.IsNullOrEmpty(iisAuth))
                {
                    // back compat with older ANCM versions
                    enableAuth = true;
                }
                else
                {
                    // Lightup a new ANCM variable that tells us if auth is enabled.
                    foreach (var authType in iisAuth.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.Equals(authType, "anonymous", StringComparison.OrdinalIgnoreCase))
                        {
                            enableAuth = true;
                            break;
                        }
                    }
                }

                var address = "http://localhost:" + port;
                hostBuilder.CaptureStartupErrors(true);

                hostBuilder.ConfigureServices(services =>
                {
                    // Delay register the url so users don't accidently overwrite it.
                    hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, address);
                    hostBuilder.PreferHostingUrls(true);
                    services.AddSingleton<IStartupFilter>(new IISSetupFilter(pairingToken, new PathString(path)));
                    services.Configure<ForwardedHeadersOptions>(options =>
                    {
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                    });
                    services.Configure<IISOptions>(options =>
                    {
                        options.ForwardWindowsAuthentication = enableAuth;
                    });
                    services.AddAuthenticationCore();
                });
            }

            return hostBuilder;
        }
    }
}
