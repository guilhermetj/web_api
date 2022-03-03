using Microsoft.AspNetCore.Mvc;
using crud_dotnet.Model;
using crud_dotnet.Repository;

namespace crud_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _repository.BuscaUsuarios();
            return usuarios.Any() ? Ok(usuarios) : NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var usuarios = await _repository.BuscaUsuario(id);

            return usuarios != null ? Ok(usuarios) : NotFound("Usuario não encontrado");
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            _repository.AdicionaUsuario(usuario);
            return await _repository.SaveChangesAsync() ? Ok("Usuario Adicionado com sucesso") : BadRequest("Erro ao salvar usúario");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario)
        {
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuario não encontrado");

            usuarioBanco.Nome = usuario.Nome ?? usuarioBanco.Nome;
            usuarioBanco.DataNascimento = usuario.DataNascimento != new DateTime() ? usuario.DataNascimento : usuarioBanco.DataNascimento;

            _repository.AtualizaUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync() ? Ok("Usuario Atualizado com sucesso") : BadRequest("Erro ao atualizar usúario");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuario não encontrado");

            _repository.DeletaUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync() ? Ok("Usuario deletado com sucesso") : BadRequest("Erro ao deletadar usúario");
        }
    }
}