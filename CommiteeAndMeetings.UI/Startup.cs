using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.AuthenticationServices;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.Server;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Service.Sevices;
using HelperServices;
using HelperServices.Hubs;
using IHelperServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using CommiteeAndMeetings.Service.Helpers;
using System.Linq;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.AspNetCore.Server.IISIntegration;
using Models.ProjectionModels;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using IHelperServices;
using SessionServices = CommiteeAndMeetings.Service.Sevices.SessionServices;
using DbContexts.AuditDbContext;
using Models;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.BaseObjects.Repositories;
using CommiteeAndMeetings.UI.Filter;
using CommiteeAndMeetings.UI.Helpers;

namespace CommiteeAndMeetings.UI
{
    public class Startup
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            Configuration = configuration;
            Hosting.HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BearerTokensOptions>(options => Configuration.GetSection("BearerTokens").Bind(options));
            services.Configure<AppSettings>(options => Configuration.Bind(options));

           
            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true;
            //});

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            HttpClientHandler handlers = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            HttpClient client = new HttpClient(handlers);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            //Enable GZip Compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                // You Can use fastest compression
                options.Level = CompressionLevel.Optimal;
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddOptions();
            //services.Configure<IHelperServices.Models.AppSettings>(options => Configuration.Bind(options));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                          //.AllowCredentials();
                      });
            });
            //services.AddCors();
            // Using Cache with ServerState option
            services.AddDistributedMemoryCache();
            // Default IdleTimeout 20 Minutes
            services.AddSession(opt =>
            {
                opt.Cookie.IsEssential = true;
            });
            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            MasarContext.connectionString = Configuration.GetConnectionString("MainConnectionString");
            //services.AddEntityFrameworkSqlServer().AddDbContext<MasarContext>(options =>
            //{
            //    options.UseLazyLoadingProxies(false)
            //    .UseSqlServer(Configuration.GetConnectionString("MainConnectionString"), serverDbContextOptionsBuilder =>
            //    {
            //        int minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
            //        serverDbContextOptionsBuilder.CommandTimeout(minutes);
            //        serverDbContextOptionsBuilder.EnableRetryOnFailure();
            //        serverDbContextOptionsBuilder.EnableRetryOnFailure();
            //    });
            //});

            services.AddScoped<IUpdateTaskLogService, UpdateTaskLogService>();


            //var context = services.BuildServiceProvider()
            //         .GetService<MasarContext>();
            UnitOfWork<MasarContext> _unitOfWork = services.BuildServiceProvider()
                     .GetService<UnitOfWork<MasarContext>>();
            services.AddAutoMapper(_unitOfWork);

            #region Audit
            // Configure audit
            MasarContext.AuditEnabled = Configuration["AuditSettings:IsEnabled"].ToString() == "1" ? true : false;
            MasarContext.AuditUrl = Configuration["AuditSettings:BaseUrl"].ToString();
            MasarContext.DatabaseName = Configuration["AuditSettings:DatabaseName"].ToString();
            AuditContext.connectionString = EncryptHelper.Decrypt(Configuration["AuditSettings:connectionString"].ToString());
            #endregion

            services.AddSwaggerGen(action =>
            {

                action.MapType<FileContentResult>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "file" });
                action.MapType<object>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "any" });
                action.MapType<JToken>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "any" });
                action.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Committe And Meetings WebApi", Version = "v1" });
                action.EnableAnnotations();
                action.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
            });
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEntityFrameworkSqlServer().AddDbContext<MasarContext>(options =>
            {
                options.UseLazyLoadingProxies(true)
                .UseSqlServer(Configuration.GetConnectionString("MainConnectionString"), serverDbContextOptionsBuilder =>
                {
                    int minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                });
            });
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in
            services.AddUnitOfWork<MasarContext>(_httpContextAccessor);
            services.AddDataProtection().SetApplicationName("Masar4"); //Add this
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISMSTemplateRepository, SMSTemplateRepository>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasPermissionPolicy", policy =>
                    policy.Requirements.Add(new HasPermissionRequirment()));
                options.AddPolicy("AuthenticatedOnlyPolicy", policy =>
                    policy.Requirements.Add(new AuthenticatedOnlyRequirment()));
            });
            //services.AddHttpClient<MasarCommunicationClient>(c =>
            //{
            //    c.DefaultRequestHeaders.Add("Accept", "application/json");
            //    c.Timeout = new TimeSpan(0, 0, 30);
            //})
            services.AddTransient<IAuthorizationHandler, AuthenticatedOnlyFilter>();
            services.AddTransient<IAuthorizationHandler, AuthHandler>();

            services
               .AddAuthentication(options =>
               {
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.SaveToken = true;
                   cfg.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = Configuration["BearerTokens:Issuer"], // site that makes the token
                       ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
                       ValidAudience = Configuration["BearerTokens:Audience"], // site that consumes the token
                       ValidateAudience = false, // TODO: change this to avoid forwarding attacks
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])),
                       ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                       ValidateLifetime = true, // validate the expiration
                       ClockSkew = TimeSpan.FromMinutes(5) // tolerance for the expiration date
                   };
                   cfg.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           ILogger logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           logger.LogError("Authentication failed.", context.Exception);
                           return Task.CompletedTask;
                       },
                       OnTokenValidated = context =>
                       {
                           //  ITokenValidatorService tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                           // return tokenValidatorService.ValidateAsync(context);
                           return Task.CompletedTask;
                       },
                       OnChallenge = context =>
                       {
                           ILogger logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
                           return Task.CompletedTask;
                       },
                       OnMessageReceived = context =>
                       {
                           Microsoft.Extensions.Primitives.StringValues accessToken = context.Request.Query["access_token"];

                           // If the request is for our hub...
                           PathString path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) &&
                               (path.StartsWithSegments("/api/SignalR")))
                           {
                               // Read the token out of the query string
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               })
               .AddIdentityCookies();

            services.AddMvc(
                // for  Global Authorization
                o =>
                {
                    o.EnableEndpointRouting = false;
                    AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                    o.Filters.Add<GlobalExceptionFilter>();
                }
                ).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                })
                .AddDataAnnotationsLocalization()
                ;

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddScoped<IJobSchedulingService, JobSchedulingService>();
            services.AddScoped(typeof(IStringLocalizer), typeof(DbStringLocalizer));
            DbContextFactory.connectionString = Configuration.GetConnectionString("MainConnectionString");
            if (Convert.ToString(Configuration["HangFireSettings:IsEnabled"]).Equals("1"))
            {
                services.AddHangfire(configuration => configuration
                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseSqlServerStorage(Configuration.GetConnectionString("MainConnectionString"),
                   new SqlServerStorageOptions
                   {
                       CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                       SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                       QueuePollInterval = TimeSpan.Zero,
                       UseRecommendedIsolationLevel = true,
                       UsePageLocksOnDequeue = true,
                       DisableGlobalLocks = true,
                       PrepareSchemaIfNecessary = true,
                       SchemaName = "CommiteeHangfire"
                   }));

                // Add the processing server as IHostedService
                services.AddHangfireServer(options =>
                {
                    options.Queues = HangfireHelper.GetQueues();
                });




            }


            services.AddHelperServices();
            services.AddBusinessServices();
            services.AddScoped<IDataProtectService, DataProtectService>();
            services.AddControllers()
                //if Swagger comment next line
                .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()))
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddSignalR(options => options.EnableDetailedErrors = true);

            #region HangFire Configurations
            //services.AddHangfire(configuration => configuration
            //               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //               .UseSimpleAssemblyNameTypeSerializer()
            //               .UseRecommendedSerializerSettings()
            //               .UseSqlServerStorage(Configuration.GetConnectionString("MainConnectionString"),
            //               new SqlServerStorageOptions
            //               {
            //                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //                   QueuePollInterval = TimeSpan.Zero,
            //                   UseRecommendedIsolationLevel = true,
            //                   DisableGlobalLocks = true
            //               })
            //               .UseSerializerSettings(new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

            //services.AddHangfireServer(options =>
            //{
            //    options.Queues = HangfireHelper.GetQueues();
            //});
            #endregion

            // To use Username instead of ConnectionId in Communication 
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();



            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            #region //--< set uploadsize large files >----

            services.Configure<FormOptions>(options =>

            {

                options.ValueLengthLimit = int.MaxValue;

                options.MultipartBodyLengthLimit = int.MaxValue;

                options.MultipartHeadersLengthLimit = int.MaxValue;

            });

            #endregion  //--</ set uploadsize large files >----

        }

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHelperServices.ISessionServices SessionServices)
        //{

        //    app.UseWebSockets();
        //    app.UseForwardedHeaders();




        //    // string URL = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
        //    if (!env.IsDevelopment() && Configuration["SystemSettingOptions:SSO_LOGIN"].ToString() == "1")
        //    {
        //        app.Use(async (context, next) =>
        //        {
        //            //if (!context.User.Identity.IsAuthenticated)
        //            //{
        //            string auth = context.Request.Headers["Authorization"];
        //            if (System.IO.File.Exists("C:\\comlog.txt"))
        //            {
        //                using (StreamWriter sw = System.IO.File.AppendText("C:\\comlog.txt"))
        //                {
        //                    sw.WriteLine("From Masar " + auth);
        //                    //sw.WriteLine("SID2 =" + SID2);
        //                }
        //            }
        //            if (string.IsNullOrEmpty(auth))
        //            {
        //                await context.ChallengeAsync("Windows");
        //            }
        //            else
        //            {
        //                await next();
        //            }
        //            //}
        //            //else
        //            //{
        //            //    await next();
        //            //}
        //        });
        //    }
        //    app.UseRouting();
        //    app.UseCors("AllowAllHeaders");
        //    app.UseExceptionHandler(appBuilder =>
        //    {
        //        appBuilder.Use(async (context, next) =>
        //        {
        //            IExceptionHandlerFeature error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
        //            if (error != null && error.Error is SecurityTokenExpiredException)
        //            {
        //                context.Response.StatusCode = 401;
        //                context.Response.ContentType = "application/json";
        //                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        //                {
        //                    State = 401,
        //                    Msg = "Token Expired"
        //                }));
        //            }
        //            else if (error != null && error.Error != null)
        //            {
        //                context.Response.StatusCode = 500;
        //                context.Response.ContentType = "application/json";
        //                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        //                {
        //                    State = 500,
        //                    Msg = error.Error.Message
        //                }));
        //            }
        //            else
        //            {
        //                await next();
        //            }
        //        });
        //    });



        //    app.UseResponseCompression();
        //    app.UseSession();
        //    // app.UseCors(builder => builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build());

        //    app.UseSwagger(action =>
        //    {
        //    });
        //    app.UseSwaggerUI(action => { action.SwaggerEndpoint("/swagger/v1/swagger.json", "Masar WebApi"); });
        //    app.UseAuthentication();
        //    app.UseAuthorization();



        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseHsts();
        //    }


        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles(new StaticFileOptions()
        //    {
        //        ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>() {
        //            { ".xlf","application/x-msdownload"},
        //            { ".exe","application/octect-stream"},
        //        })
        //    });

        //    app.UseSpaStaticFiles();

        //    #region HangFire Jobs Scheduler
        //    if (Convert.ToString(Configuration["HangFireSettings:IsEnabled"]).Equals("1"))
        //    {
        //        app.UseHangfireServer();
        //        app.UseHangfireDashboard("/commiteehangfire", new DashboardOptions
        //        {
        //            Authorization = new[] { new HangFireAuthorizedFilter(SessionServices) },
        //        });


        //        //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        //        //RecurringJob.AddOrUpdate(() => Console.WriteLine("Every Min *********"), Cron.Minutely);

        //        //Register Jobs ...
        //        MasarContext _Db = new MasarContext();

        //        HangFireJobScheduler.RegisterSchedulerRecurringJobs(_Db.CommitteeMeetingSystemSetting
        //            .Where(x => x.SystemSettingCode == "TimeToExecuteFirstReminder" || x.SystemSettingCode == "TimeToExecuteSecondReminder"
        //            || x.SystemSettingCode == "AutomaticEscalationTime").ToListAsync().Result);
        //        // HangFireJobScheduler.RegisterSchedulerRecurringJobs(_Db.HangFireJobSchedulings.ToListAsync().Result);
        //        _Db.Dispose();
        //    }
        //    #endregion
        //    //if (Convert.ToString(Configuration["HangFireSettings:IsEnabled"]).Equals("1"))
        //    //{
        //    //    app.UseHangfireServer();
        //    //    app.UseHangfireDashboard("/hangfire", new DashboardOptions
        //    //    {
        //    //        Authorization = new[] { new HangFireAuthorizedFilter(SessionServices) },
        //    //    });


        //    //    //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        //    //    //RecurringJob.AddOrUpdate(() => Console.WriteLine("Every Min *********"), Cron.Minutely);

        //    //    //Register Jobs ...
        //    //    MainDbContext _Db = new MainDbContext();
        //    //    HangFireJobScheduler.RegisterSchedulerRecurringJobs(_Db.HangFireJobSchedulings.ToListAsync().Result);
        //    //    _Db.Dispose();
        //    //}
        //    //#endregion HangFire Jobs Scheduler
        //    //if (Convert.ToString(Configuration["SystemSettingOptions:CookieEnabled"]).Equals("1"))
        //    //{
        //    //    CookiePolicyOptions cpo = new CookiePolicyOptions();
        //    //    cpo.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
        //    //    cpo.Secure = CookieSecurePolicy.Always;
        //    //    app.UseCookiePolicy();
        //    //}

        //    app.UseStatusCodePages();
        //    app.UseDefaultFiles(); // so index.html is not required
        //    app.UseRequestLocalization(new RequestLocalizationOptions
        //    {
        //        DefaultRequestCulture = new RequestCulture("en"),
        //        SupportedCultures = new[] { new CultureInfo("en")/*, new CultureInfo("ar")*/ },
        //        SupportedUICultures = new[] { new CultureInfo("en")/*, new CultureInfo("ar")*/ }
        //    });

        //    app.UseFileServer(new FileServerOptions()
        //    {
        //        EnableDirectoryBrowsing = false
        //    });

        //    //SignalR routing
        //    app.UseEndpoints(options =>
        //    {
        //        //SignalRHub is Class
        //        options.MapHub<SignalRHub>("/api/SignalR");
        //       // options.MapHub<ChatHub>("/api/chatR");

        //    });

        //    app.UseMvc();
        //    app.UseSpa(spa =>
        //    {
        //        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        //        // see https://go.microsoft.com/fwlink/?linkid=864501

        //        spa.Options.SourcePath = "ClientApp";
        //        if (env.IsDevelopment())
        //        {
        //            spa.UseAngularCliServer(npmScript: "start");
        //            spa.Options.StartupTimeout = TimeSpan.FromMinutes(10);
        //            //spa.UseProxyToSpaDevelopmentServer("http://localhost:4444");
        //        }
        //    });

        //    if (env.IsDevelopment())
        //    {
        //        Task.Run(() =>
        //        {
        //            using (var handler = new HttpClientHandler() { Credentials = CredentialCache.DefaultCredentials })
        //            {
        //                using (HttpClient httpClient = new HttpClient(handler))
        //                {
        //                    string SourceDocumentAbsoluteUrl = Configuration["SwaggerToTypeScriptClientGeneratorSettings:SourceDocumentAbsoluteUrl"];
        //                    string OutputDocumentRelativePath = Configuration["SwaggerToTypeScriptClientGeneratorSettings:OutputDocumentRelativePath"];
        //                    using (Stream contentStream = httpClient.GetStreamAsync(SourceDocumentAbsoluteUrl).Result)
        //                    using (StreamReader streamReader = new StreamReader(contentStream))
        //                    {
        //                        string json = streamReader.ReadToEnd();
        //                        //NSwag.OpenApiDocument document = NSwag.OpenApiDocument.FromJsonAsync(json).Result;
        //                        //NSwag.CodeGeneration.TypeScript.TypeScriptClientGeneratorSettings settings = new NSwag.CodeGeneration.TypeScript.TypeScriptClientGeneratorSettings
        //                        var document = SwaggerDocument.FromJsonAsync(json).Result;
        //                        var settings = new NSwag.CodeGeneration.TypeScript.SwaggerToTypeScriptClientGeneratorSettings
        //                        {
        //                            OperationNameGenerator = new SwaggerClientOperationNameGenerator(),
        //                            ClassName = "SwaggerClient",
        //                            Template = TypeScriptTemplate.Angular,
        //                            RxJsVersion = 6.0M,
        //                            HttpClass = HttpClass.HttpClient,
        //                            InjectionTokenType = InjectionTokenType.InjectionToken,
        //                            BaseUrlTokenName = "API_BASE_URL",
        //                            WrapDtoExceptions = true
        //                        };

        //                        //TypeScriptClientGenerator generator = new TypeScriptClientGenerator(document, settings);
        //                        var generator = new SwaggerToTypeScriptClientGenerator(document, settings);
        //                        string code = generator.GenerateFile();
        //                        new FileInfo(OutputDocumentRelativePath).Directory.Create();
        //                        try
        //                        {
        //                            File.WriteAllText(OutputDocumentRelativePath, code);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw ex;
        //                        }
        //                    }
        //                }
        //            }
        //        });

        //    }

        //}
        //This method gets called by the runtime.Use this method to configure the HTTP request pipeline.

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHelperServices.ISessionServices SessionServices)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            // enable Web Sockets
            app.UseWebSockets();
            app.UseForwardedHeaders();

            if (!env.IsDevelopment() && Configuration["SystemSettingOptions:SSO_LOGIN"].ToString() == "1")
            {
                app.Use(async (context, next) =>
                {
                    //if (!context.User.Identity.IsAuthenticated)
                    //{
                    string auth = context.Request.Headers["Authorization"];
                    if (System.IO.File.Exists("C:\\comlog.txt"))
                    {
                        using (StreamWriter sw = System.IO.File.AppendText("C:\\comlog.txt"))
                        {
                            sw.WriteLine("From Committe" + auth);
                            //sw.WriteLine("SID2 =" + SID2);
                        }
                    }
                    if (string.IsNullOrEmpty(auth))
                    {
                        await context.ChallengeAsync("Windows");
                    }
                    else
                    {
                        await next();
                    }
                    //}
                    //else
                    //{
                    //    await next();
                    //}
                });
            }
            app.UseRouting();
            app.UseCors("AllowAllHeaders");
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    IExceptionHandlerFeature error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = 401,
                            Msg = "Token Expired"
                        }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = 500,
                            Msg = error.Error.Message
                        }));
                    }
                    else
                    {
                        await next();
                    }
                });
            });


            MasarContext.SessionService = SessionServices;

            app.UseResponseCompression();
            app.UseSession();


            // app.UseCors(builder => builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build());
            app.UseSwagger(action =>
            {

            });
            app.UseSwaggerUI(action => { action.SwaggerEndpoint("/swagger/v1/swagger.json", "Masar WebApi"); });

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }





            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>() {
                    { ".xlf","application/x-msdownload"},
                    { ".exe","application/octect-stream"},
                })
            });

            app.UseSpaStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/api/SignalR");
            });

            if (Convert.ToString(Configuration["HangFireSettings:IsEnabled"]).Equals("1"))
            {
                app.UseHangfireServer();
                app.UseHangfireDashboard("/commiteehangfire", new DashboardOptions
                {
                    Authorization = new[] { new HangFireAuthorizedFilter(SessionServices) },
                });

                //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
                //RecurringJob.AddOrUpdate(() => Console.WriteLine("Every Min *********"), Cron.Minutely);

                //Register Jobs ...
                MasarContext _Db = new MasarContext();

                HangFireJobScheduler.RegisterSchedulerRecurringJobs(_Db.CommitteeMeetingSystemSetting
                    .Where(x => x.SystemSettingCode == "TimeToExecuteFirstReminder" || x.SystemSettingCode == "TimeToExecuteSecondReminder"
                    || x.SystemSettingCode == "AutomaticEscalationTime").ToListAsync().Result);
                // HangFireJobScheduler.RegisterSchedulerRecurringJobs(_Db.HangFireJobSchedulings.ToListAsync().Result);
                _Db.Dispose();
            }
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    spa.Options.StartupTimeout = TimeSpan.FromMinutes(10);
                }
            });




            app.UseMvc();

            if (env.IsDevelopment())
            {

                Task.Run(() =>
                {
                    using (var handler = new HttpClientHandler() { Credentials = CredentialCache.DefaultCredentials })
                    {
                        using (HttpClient httpClient = new HttpClient(handler))
                        {
                            string SourceDocumentAbsoluteUrl = Configuration["SwaggerToTypeScriptClientGeneratorSettings:SourceDocumentAbsoluteUrl"];
                            string OutputDocumentRelativePath = Configuration["SwaggerToTypeScriptClientGeneratorSettings:OutputDocumentRelativePath"];
                            using (Stream contentStream = httpClient.GetStreamAsync(SourceDocumentAbsoluteUrl).Result)
                            using (StreamReader streamReader = new StreamReader(contentStream))
                            {
                                string json = streamReader.ReadToEnd();
                                //NSwag.OpenApiDocument document = NSwag.OpenApiDocument.FromJsonAsync(json).Result;
                                //NSwag.CodeGeneration.TypeScript.TypeScriptClientGeneratorSettings settings = new NSwag.CodeGeneration.TypeScript.TypeScriptClientGeneratorSettings
                                var document = SwaggerDocument.FromJsonAsync(json).Result;
                                var settings = new NSwag.CodeGeneration.TypeScript.SwaggerToTypeScriptClientGeneratorSettings
                                {
                                    OperationNameGenerator = new SwaggerClientOperationNameGenerator(),
                                    ClassName = "SwaggerClient",
                                    Template = TypeScriptTemplate.Angular,
                                    RxJsVersion = 6.0M,
                                    HttpClass = HttpClass.HttpClient,
                                    InjectionTokenType = InjectionTokenType.InjectionToken,
                                    BaseUrlTokenName = "API_BASE_URL",
                                    WrapDtoExceptions = true,
                                };

                                //TypeScriptClientGenerator generator = new TypeScriptClientGenerator(document, settings);
                                var generator = new SwaggerToTypeScriptClientGenerator(document, settings);
                                string code = generator.GenerateFile();
                                new FileInfo(OutputDocumentRelativePath).Directory.Create();
                                try
                                {
                                    File.WriteAllText(OutputDocumentRelativePath, code);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
                );

            }
        }
    }
}
