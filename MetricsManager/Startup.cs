using MetricsManager.Models;

using MetricsManager.DAL;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MetricsManager
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
			services.AddControllers(); ConfigureSqlLiteConnection(services);
			services.AddScoped<IMetricsRepository<CPUMetric>>();
			services.AddScoped<IMetricsRepository<DotNetMetric>>();
			services.AddScoped<IMetricsRepository<HDDMetric>>();
			services.AddScoped<IMetricsRepository<NetworkMetric>>();
			services.AddScoped<IMetricsRepository<RAMMetric>>();
			var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
			var mapper = mapperConfiguration.CreateMapper();
			services.AddSingleton(mapper);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void ConfigureSqlLiteConnection(IServiceCollection services)
		{
			const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
			var connection = new SQLiteConnection(connectionString);
			connection.Open();
			PrepareSchema(connection);
		}

		private void PrepareSchema(SQLiteConnection connection)
		{
			using (var command = new SQLiteCommand(connection))
			{
				// ?????? ????? ????? ??????? ??? ??????????
				// ??????? ??????? ? ????????? ???? ??? ?????????? ? ???? ??????
				command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
				// ?????????? ?????? ? ???? ??????
				command.ExecuteNonQuery();


				command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
				command.ExecuteNonQuery();
			}
		}

	}
}
