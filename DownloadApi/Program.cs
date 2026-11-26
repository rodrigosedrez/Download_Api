
using Download.CrossCutting.IoC;
using Download.Infrastructure.Context;
using Download.Infrastructure.Identity;
using Dwonload.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024; // 5 GB

});

var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(';') ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});



var cloudflareKey = builder.Configuration["Cloudflare:AccessKeyId"];
var jwtKey = builder.Configuration["Jwt:Key"];

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options => {
    // Se a variável estiver nula (erro de config), o Redis vai falhar no startup
    options.Configuration = redisConnectionString;
});


// Remove limite global do Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null; // sem limite
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = long.MaxValue; // ou um valor grande
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    // Definição do esquema de segurança
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Description = "Insira o token Jwt no campo abaixo. Exemplo: Bearer {seu_token}"
    });

    // Requisito de segurança

});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDependencyInjectionAPI(builder.Configuration); // Add this line to register your services
builder.Services.AddDependencyInjectionAPIServices(builder.Configuration); // Add this line to register your services

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<DownloadsDBContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{

    var jwtKey = builder.Configuration["Jwt:Key"];
    var jwtIssuer = builder.Configuration["Jwt:Issuer"];


    if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
    {
        throw new Exception("A Jwt:Key deve ter pelo menos 32 caracteres.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = false,

        ValidateLifetime = true,
        RequireExpirationTime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),


        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});


var baseUrl = "https://api.yoursite.com";



builder.Logging.ClearProviders();
builder.Logging.AddConsole();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




// Sitemap por plataforma (ROMs)
app.MapMethods("/sitemap-{platformSlug}-{page:int}.xml", new[] { "GET", "HEAD" },
    async (string platformSlug, int page, DownloadsDBContext db) =>
    {
        int pageSize = 45000;
        var baseUrl = "https://www.yoursite.com";

        var itens = await db.GameModels
            .AsNoTracking()
            .Where(p => p.PlataformSlug == platformSlug)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new { p.PlataformSlug, p.Slug, p.CreatedAt })
            .ToListAsync();

        if (!itens.Any()) return Results.NotFound();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var item in itens)
        {
            var url = $"{baseUrl}/roms/{item.PlataformSlug}/{item.Slug}";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url)}</loc>");
            sb.AppendLine($"    <lastmod>{item.CreatedAt:yyyy-MM-ddTHH:mm:sszzz}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });

// Índice principal
app.MapMethods("/sitemap.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl = "https://api.yoursite.com";
        int pageSize = 45000;

        var platformStats = await db.GameModels
            .GroupBy(p => p.PlataformSlug)
            .Select(g => new { Slug = g.Key, Count = g.Count() })
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");





        foreach (var platform in platformStats)
        {
            var totalPages = (int)Math.Ceiling((double)platform.Count / pageSize);
            for (int i = 1; i <= totalPages; i++)
            {
                sb.AppendLine("  <sitemap>");
                sb.AppendLine($"    <loc>https://api.yoursite.com/sitemap-{platform.Slug}-{i}.xml</loc>");
                sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("  </sitemap>");
            }


        }



        sb.AppendLine("</sitemapindex>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });
// Índice principal v2
app.MapMethods("/sitemap-v2.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl = "https://api.yoursite.com";
        int pageSize = 45000;

        var platformStats = await db.GameModels
            .GroupBy(p => p.PlataformSlug)
            .Select(g => new { Slug = g.Key, Count = g.Count() })
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");





        foreach (var platform in platformStats)
        {
            var totalPages = (int)Math.Ceiling((double)platform.Count / pageSize);
            for (int i = 1; i <= totalPages; i++)
            {
                sb.AppendLine("  <sitemap>");
                sb.AppendLine($"    <loc>https://api.yoursite.com/sitemap-{platform.Slug}-{i}.xml</loc>");
                sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("  </sitemap>");
            }


        }



        sb.AppendLine("</sitemapindex>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });


// Sitemap de emuladores
app.MapMethods("/sitemap-emulators.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl = "https://api.yoursite.com";
        var emuladores = await db.Emulators.ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var e in emuladores)
        {
            var url = $"{baseUrl}/emulators/{e.Slug}";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(url)}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });

app.MapMethods("/sitemap-principais.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl = "https://api.yoursite.com";
        var platformStats = await db.GameModels
             .GroupBy(p => p.PlataformSlug)
             .Select(g => new { Slug = g.Key, Count = g.Count() })
             .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");


        var url1 = "https://www.yoursite.com/games";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url1)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");

        var url2 = "https://www.yoursite.com/emulators";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url2)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");

        var url3 = "https://www.yoursite.com/post";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url3)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");
        foreach (var e in platformStats)
        {
            var url4 = $"https://www.yoursite.com/games/{e.Slug}/1";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url4)}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });

// Sitemap de posts
app.MapMethods("/sitemap-post.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl = "https://api.yoursite.com";
        var posts = await db.PostModels.ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var post in posts)
        {
            var url = $"{baseUrl}/post/{post.Slug}";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(url)}</loc>");
            sb.AppendLine($"    <lastmod>{post.CreatedAt:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });


var baseUrl2 = "https://yoursite.net";


app.MapMethods("/sitemapdotnet-{platformSlug}-{page:int}.xml", new[] { "GET", "HEAD" },
    async (string platformSlug, int page, DownloadsDBContext db) =>
    {
        int pageSize = 45000;
        var baseUrl2 = "https://www.yoursite.net";

        var itens = await db.GameModels
            .AsNoTracking()
            .Where(p => p.PlataformSlug == platformSlug)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new { p.PlataformSlug, p.Slug, p.CreatedAt })
            .ToListAsync();

        if (!itens.Any()) return Results.NotFound();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var item in itens)
        {
            var url = $"{baseUrl2}/roms/{item.PlataformSlug}/{item.Slug}";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url)}</loc>");
            sb.AppendLine($"    <lastmod>{item.CreatedAt:yyyy-MM-ddTHH:mm:sszzz}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });


app.MapMethods("/sitemapdotnet.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {
        var baseUrl2 = "https://api.yoursite.net";
        int pageSize = 45000;

        var platformStats = await db.GameModels
            .GroupBy(p => p.PlataformSlug)
            .Select(g => new { Slug = g.Key, Count = g.Count() })
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var platform in platformStats)
        {
            var totalPages = (int)Math.Ceiling((double)platform.Count / pageSize);
            for (int i = 1; i <= totalPages; i++)
            {
                sb.AppendLine("  <sitemap>");
                sb.AppendLine($"    <loc>https://api.yoursite.net/sitemapdotnet-{platform.Slug}-{i}.xml</loc>");
                sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("  </sitemap>");
            }


        }

        sb.AppendLine("</sitemapindex>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });


app.MapMethods("/sitemap-principaisdotnet.xml", new[] { "GET", "HEAD" },
    async (DownloadsDBContext db) =>
    {

        var platformStats = await db.GameModels
             .GroupBy(p => p.PlataformSlug)
             .Select(g => new { Slug = g.Key, Count = g.Count() })
             .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");


        var url1 = "https://www.yoursite.net/games";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url1)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");

        var url2 = "https://www.yoursite.net/emulators";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url2)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");

        var url3 = "https://www.yoursite.net/post";
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url3)}</loc>");
        sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("  </url>");
        foreach (var e in platformStats)
        {
            var url4 = $"https://www.yoursite.net/games/{e.Slug}/1";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Net.WebUtility.HtmlEncode(url4)}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Results.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    });

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DownloadsDBContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    int retryCount = 0;
    while (retryCount < 10)
    {
        try
        {
            // ALTERAÇÃO AQUI: Use MigrateAsync em vez de EnsureCreatedAsync
            await context.Database.MigrateAsync();

            await SeedData.SeedAsync(context);
            logger.LogInformation("Banco de dados atualizado e Seed executado com sucesso.");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            logger.LogError(ex, $"Tentativa {retryCount}/10 falhou. Erro ao aplicar migrations...");
            await Task.Delay(8000);
        }
    }
}

app.Run();

