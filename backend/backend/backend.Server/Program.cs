using backend.Services.Auth;

namespace backend.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register services (DbContext, DI, JWT)
            RegisterServices(builder);
            ConfigureJwt(builder);

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Vite default
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            ConfigureMiddleware(app);

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRefreshTokenStore, RefreshTokenStore>();
            builder.Services.AddScoped<IKeycloakService, KeycloakService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            //    // DbContext
            //    builder.Services.AddDbContext<AuthDbContext>(options =>
            //        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //    // Services & Repositories
            //    builder.Services.AddScoped<IUserRepository, UserRepository>();
            //    builder.Services.AddScoped<SsoService>();
        }

        private static void ConfigureJwt(WebApplicationBuilder builder)
        {
            //    var jwtSecret = builder.Configuration["Jwt:Secret"];
            //    builder.Services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
            //        };
            //    });

            //    builder.Services.AddAuthorization();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
