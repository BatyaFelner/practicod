using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthServer.Models;  
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using TodoApi;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// options =>{

//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Name = "Authorization",
//         Description = "Bearer Authentication with JWT Token",
//         Type = SecuritySchemeType.Http
//     });
//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//         Reference = new OpenApiReference
//                 {
//                     Id = "Bearer",
//                     Type = ReferenceType.SecurityScheme
//                 }
//             },
//             new List<string>()
//         }
//     });
// });


// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["JWT:Issuer"],
//             ValidAudience = builder.Configuration["JWT:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//         };
//     });
 //builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAll");

//app.UseAuthentication(); 
//app.UseAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();

// app.MapPost("/api/login", async (ToDoDbContext db, [FromBody] LoginModel loginModel) =>
// {
//     var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.PasswordHash == loginModel.PasswordHash);
//     if (user is not null)
//     {
//         var jwt = CreateJWT(user);
//         return Results.Ok(jwt);
//     }
//     return Results.Unauthorized();
// });

// app.MapPost("/api/register", async (ToDoDbContext db, [FromBody] LoginModel loginModel) =>
// {
//     var name = loginModel.Email.Split("@")[0];
//     var lastId = db.Users.Max(u => u.Id) ?? 0;
//     var newUser = new User { Id = lastId + 1, Email = loginModel.Email, PasswordHash = loginModel.PasswordHash };
//     db.Users.Add(newUser);
//     await db.SaveChangesAsync();
//     var jwt = CreateJWT(newUser);
//     return Results.Ok(jwt);
// });

//  object CreateJWT(User user)
// {
//   var claims = new List<Claim>()
// {
//     new Claim("id", user.Id.ToString()),  
//     new Claim("Email", user.Email),      
//     new Claim("PasswordHash", user.PasswordHash), 
// };

//     var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]));
//     var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
//     var tokenOptions = new JwtSecurityToken(
//         issuer: builder.Configuration["JWT:Issuer"],
//         audience: builder.Configuration["JWT:Audience"],
//         claims: claims,
//         expires: DateTime.Now.AddMinutes(60),
//         signingCredentials: signinCredentials
//     );
//     var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
//     return new { Token = tokenString };
// }

app.MapGet("/", () => "helo!!");
app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());

app.MapPost("/items", async (ToDoDbContext db,   string Name) =>
{
    Item i = new Item();
    i.Name = Name;
    i.IsComplete = false;
    db.Items.Add(i);
    await db.SaveChangesAsync();
    return i; 
});


app.MapPut("/items/{id}", async (ToDoDbContext db, int id,  bool IsComplete) =>
{
    var item = await db.Items.FindAsync(id);
    if (item != null)
    {
        item.IsComplete = IsComplete;
        await db.SaveChangesAsync();
        return true; 
    }
    return false;
});
app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item != null)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync();
       return true; 
        }

    return false;
});


app.Run();
