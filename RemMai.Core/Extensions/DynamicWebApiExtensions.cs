﻿using RemMai.DynamicWebApi;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RemMai.Extensions;
/// <summary>
/// 注册动态Api
/// <para></para>
/// PandaDynamicWebApi
/// <para></para>
/// Swagger
/// </summary>
public static class DynamicWebApiExtensions
{
    /// <summary>
    /// 注册动态Api
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicWebApiAndSwaggerGen(this IMvcBuilder mvcBuilder)
    {
        // 注册Swagger
        mvcBuilder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Panda OpenApi", Version = "v1" });

            // TODO：一定要返回true！
            options.DocInclusionPredicate((docName, description) => true);

            // TODO：不可省略，否则Swagger中文注释不显示。
            DirectoryInfo directoryInfo = new(AppContext.BaseDirectory);
            directoryInfo.GetFiles("*.xml").Select(e => e.FullName).ToList().ForEach(xmlFullName =>
            {
                // 添加控制器层注释，true表示显示控制器注释
                options.IncludeXmlComments(xmlFullName, true);
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new List<string>()
                    }
                });
        });

        // 注册PandaDynamicWebApi
        //services.AddDynamicWebApi(options =>
        //{
        //    RemMaiApp.ProjectAssemblies.ForEach(assembly =>
        //    {
        //        options.AssemblyDynamicWebApiOptions.Add(assembly, new AssemblyDynamicWebApiOptions());
        //    });
        //});

        mvcBuilder.AddDynamicWebApi();

        return mvcBuilder.Services;
    }
}

