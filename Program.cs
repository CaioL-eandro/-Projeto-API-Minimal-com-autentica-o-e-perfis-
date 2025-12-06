using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
    

if (app.Environment.IsDevelopment()) 

{
    app.UseSwagger();
    app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.Use(async(context,next)=>
{
    if (context.Request.Path.StartsWithSegments("/usuarios") && HttpMethods.IsGet(context.Request.Method))
    {
        var perfilHeader = context.Request.Headers["X-Perfil"].FirstOrDefault();
        if (!string.Equals(perfilHeader,"Admin",StringComparison.CurrentCultureIgnoreCase))
        {
            context.Response.StatusCode= StatusCodes.Status403Forbidden ;
            await context.Response.WriteAsync("Acesso permitido apenas para Administradores ");
            return;
        }
    }

    await next();
}
);
 var usuarios = new List<Usuario>();
 var tokens = new Dictionary<string,string>();

usuarios.Add(new Usuario
{
    Id = 1,
    Nome = "Admin Padrão",
    Email = "admin@sistema.com",
    SenhaHash = GerarHashSenha("123456"),
    Perfil = PerfilUsuario.Admin
});   /// perfil inicial para teste 


app.MapPost("/usuarios",([FromBody]RegisterRequest req )=>
{
    if(string.IsNullOrWhiteSpace(req.Nome)||
    string.IsNullOrWhiteSpace(req.Email)||
    string.IsNullOrWhiteSpace(req.Senha))
    {
        return Results.BadRequest("Nome, Email e senha são obrigatórios.");
    }
    // entender da linha da baixo ate o fim 
    if(req.Senha.Length<6 )
    {
        return Results.BadRequest("A senha deve ter pelo menos 6 caracters.");
    }
    if(!Enum.TryParse<PerfilUsuario>(req.Perfil,true,out var perfil))
    {
        return Results.BadRequest("Perfil inválido.Use 'Admin' ou 'User'.");
    }
    if (usuarios.Any(u => u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)))
    {
            return Results.Conflict("Ja existe um usuario com esse e-mail.");
    }
    var novoId = usuarios.Any()? usuarios.Max (u => u.Id)+ 1 :1; 
    var novoUsuario = new Usuario
    {
        Id= novoId,
        Nome = req.Nome,
        Email = req.Email,
        SenhaHash = GerarHashSenha(req.Senha),
        Perfil = perfil
    };

    usuarios.Add(novoUsuario);
    var dto = new UsuarioReadDto(
        novoUsuario.Id,
        novoUsuario.Nome,
        novoUsuario.Email,
        novoUsuario.Perfil.ToString()
    );
    return Results.Created($"/usuarios/{novoUsuario.Id}",dto);
});
app.MapPost("/login",([FromBody] LoginRequest req ) =>
{
    if (string.IsNullOrWhiteSpace(req.Email)||
    string.IsNullOrWhiteSpace(req.Senha))
    {
        return Results.BadRequest("Email e senha são obrigatorios.");
    }
    var senhaHash = GerarHashSenha(req.Senha);
    var usuario = usuarios.FirstOrDefault(u => u.Email.Equals(req.Email,StringComparison.OrdinalIgnoreCase)&& u.SenhaHash ==senhaHash);

    if(usuario is null)
{
    return Results.Unauthorized();
}
var token = Guid.NewGuid().ToString();
tokens [token]= usuario.Email;

var resp = new LoginResponse(token,usuario.Nome,usuario.Email,usuario.Perfil.ToString());
return Results.Ok(resp);});

app.MapGet("/usuarios", () =>
{
    var lista = usuarios
    .Select(u=> new UsuarioReadDto(
        u.Id,u.Nome,u.Email,u.Perfil.ToString()
    ));
    return Results.Ok(lista);
});




app.Run();
string GerarHashSenha(string senha )
{
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(senha);
    var hash =sha256.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
}

enum PerfilUsuario
{
    Admin,
    User
}
class Usuario
{
    public int Id {get;set;}
    public string Nome { get;set;}
    public string Email {get;set;}
    public string SenhaHash {get;set;}
    public PerfilUsuario Perfil{get;set;}
}
record RegisterRequest(string Nome, string Email,string Senha,string Perfil);
record LoginRequest(string Email,string Senha);

record LoginResponse(string Token, string Nome,string Email,string Perfil);
record UsuarioReadDto(int Id , string Nome,string Email, string Perfil);



/*Mandamos uma requisição pelo Swagger para o ASP.NET Core.
Ele identifica qual endpoint deve executar (pelo método POST e pela rota, como /login ou /register).
Com base nisso, ele transforma o JSON em um objeto do tipo correto (LoginRequest ou RegisterRequest).
Esse objeto é então usado pelo endpoint para validar os dados, criar um usuário na lista ou validar permissão/login.*/