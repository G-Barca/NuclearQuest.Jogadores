using System.ComponentModel.DataAnnotations;
using NuclearQuest.Jogadores.Exceptions;
using NuclearQuest.Jogadores.Models;
using NuclearQuest.Jogadores.Repositories;

namespace NuclearQuest.Jogadores.Services;

public class JogadorService
{
    private readonly JogadorRepository _repositorio;

    public JogadorService(JogadorRepository repositorio)
    {
        _repositorio = repositorio;
    }

    // Cadastro
    
    public Jogador Cadastrar(string nome, string email)
    {
        var jogadores = _repositorio.ObterTodos();

        var jogador = new Jogador
        {
            Id = jogadores.Count == 0 ? 1 : jogadores.Max(j => j.Id) + 1,
            Nome = nome.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Pontuacao = 0,
            DataCadastro = DateTime.Now
        };

        Validar(jogador);

        // O email n pode se repetir
        
        if (jogadores.Any(j => j.Email.Equals(jogador.Email, StringComparison.OrdinalIgnoreCase)))
            throw new RegraNegocioException("Já existe um jogador cadastrado com este e-mail.");

        jogadores.Add(jogador);
        _repositorio.Salvar(jogadores);
        return jogador;
    }

    // listagem/ranking de todos 
    
    public List<Jogador> ListarRanking()
    {
        return _repositorio.ObterTodos()
            .OrderByDescending(j => j.Pontuacao)
            .ThenBy(j => j.Nome)
            .ToList();
    }

    // Busca 
    
    public List<Jogador> BuscarPorNome(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            throw new ValidacaoException("Informe parte do nome para buscar.");

        return _repositorio.ObterTodos()
            .Where(j => j.Nome.Contains(termo.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(j => j.Pontuacao)
            .ToList();
    }

    // Somar pontos e detectar lvl up
    
    public (Jogador Jogador, bool SubiuDeNivel) RegistrarPontos(int id, int pontos)
    {
        if (pontos <= 0)
            throw new RegraNegocioException("A pontuação a registrar deve ser maior que zero.");

        var jogadores = _repositorio.ObterTodos();
        var jogador = jogadores.FirstOrDefault(j => j.Id == id)
            ?? throw new RegraNegocioException($"Jogador com ID {id} não encontrado.");

        var nivelAnterior = jogador.Nivel;
        jogador.Pontuacao += pontos;
        Validar(jogador);

        _repositorio.Salvar(jogadores);
        return (jogador, jogador.Nivel > nivelAnterior);
    }

    // DataAnnotations (validations) da classe Jogador
    
    private static void Validar(Jogador jogador)
    {
        var resultados = new List<ValidationResult>();
        var contexto = new ValidationContext(jogador);

        if (!Validator.TryValidateObject(jogador, contexto, resultados, validateAllProperties: true))
            throw new ValidacaoException(resultados.Select(r => r.ErrorMessage ?? "Dado inválido."));
    }
}
