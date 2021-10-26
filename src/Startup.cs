using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Repositories;

namespace ToDoApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApiVersioning(p =>
			{
				p.DefaultApiVersion = new ApiVersion(1, 0);
				p.ReportApiVersions = true;
				p.AssumeDefaultVersionWhenUnspecified = true;
			});
			services.AddVersionedApiExplorer(p =>
			{
				p.GroupNameFormat = "'v'VVV";
				p.SubstituteApiVersionInUrl = true;
			});
			services.AddControllers();
			services.AddDbContext<AppDbContext>();
			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
					}

					options.DocExpansion(DocExpansion.List);
				});
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
