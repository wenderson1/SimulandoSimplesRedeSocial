using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulandoRedeSocial.Data;
using SimulandoRedeSocial.Model;
using SimulandoRedeSocial.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimulandoRedeSocial.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .ToListAsync();


            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromBody] User model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();

                model.Password = "";
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possiível criar sua conta" });
            }
        }

        [HttpPut]
        [Route("{id}:int")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User model, [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Funcionário não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<User>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });
            }
        }


        [Authorize]
        public async Task<ActionResult<List<User>>> Delete(int id, [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Amigo removido com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover este amigo" });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("FriendshipRequest/{id}:int")]
        public async Task<ActionResult<Friends>> FriendshipRequest([FromServices] DataContext context, [FromServices] User user, [FromServices] Friends friends, int id)
        {
            friends.IdUserSent = user.Id;
            friends.IdUserReceived = id;
            friends.Accept = null;

            var userReceived = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Esta pessoa nao esta cadastrada no sistema" });

            try
            {
                user.Request.Add(friends);
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return Ok(new { message = "Solicitação enviada" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível enviar a solicitação" });
            }

        }

        [HttpPost]
        [Route("FriendshipResponse/{id}:int")]
        public async Task<ActionResult<Friends>> FriendshipResponse([FromServices] DataContext context, [FromServices] User user, [FromServices] Friends friends, int id, bool response)
        {

            friends.Accept = response;
            var userSent = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (friends.Accept == true)
            {
                user.Friend.Add(userSent);
                userSent.Friend.Add(user);
            }

            try
            {
                context.Entry<User>(user).State = EntityState.Modified;
                context.Entry<User>(userSent).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Erro" });
            }
        }


        [HttpPost]
        [Route("login")]

        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválida" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
    }
}
