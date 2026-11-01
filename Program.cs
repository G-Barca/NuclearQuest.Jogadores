using System.Text;
using NuclearQuest.Jogadores.Exceptions;
using NuclearQuest.Jogadores.Models;
using NuclearQuest.Jogadores.Repositories;
using NuclearQuest.Jogadores.Services;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var caminho = Path.Combine(AppContext.BaseDirectory, "data", "jogadores.json");
var servico = new JogadorService(new JogadorRepository(caminho));

var executando = true;
while (executando)
{
    ExibirMenu();
    var opcao = Console.ReadLine()?.Trim();

    // try/catch:
    
    try
    {
        switch (opcao)
        {
            case "1": Cadastrar(servico); break;
            case "2": Listar(servico); break;
            case "3": Buscar(servico); break;
            case "4": RegistrarPontos(servico); break;
            case "0": executando = false; break;
            default: Console.WriteLine("Opção inválida."); break;
        }
    }
    catch (ValidacaoException ex)
    {
        Console.WriteLine("Dados inválidos:");
        foreach (var erro in ex.Erros)
            Console.WriteLine($"  - {erro}");
    }
    catch (RegraNegocioException ex)
    {
        Console.WriteLine($"Não foi possível concluir: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro inesperado: {ex.Message}");
    }
}

Console.WriteLine("Até a próxima partida!");

// Funções


static void ExibirMenu()
{
    Console.WriteLine();
    Console.WriteLine("=== NuclearQuest | Jogadores e Ranking ===");
    Console.WriteLine("1 - Cadastrar jogador");
    Console.WriteLine("2 - Listar ranking");
    Console.WriteLine("3 - Buscar jogador por nome");
    Console.WriteLine("4 - Registrar pontos de uma partida");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha: ");
}

static void Cadastrar(JogadorService servico)
{
    Console.Write("Nome: ");
    var nome = Console.ReadLine() ?? "";
    Console.Write("E-mail: ");
    var email = Console.ReadLine() ?? "";

    var jogador = servico.Cadastrar(nome, email);
    Console.WriteLine($"Jogador cadastrado! ID {jogador.Id} - nível {jogador.Nivel}.");
}

static void Listar(JogadorService servico)
{
    ExibirTabela(servico.ListarRanking());
}

static void Buscar(JogadorService servico)
{
    Console.Write("Nome (ou parte): ");
    var termo = Console.ReadLine() ?? "";
    ExibirTabela(servico.BuscarPorNome(termo));
}

static void RegistrarPontos(JogadorService servico)
{
    var id = LerInteiro("ID do jogador: ");
    var pontos = LerInteiro("Pontos ganhos: ");

    var (jogador, subiu) = servico.RegistrarPontos(id, pontos);
    Console.WriteLine($"{jogador.Nome} agora tem {jogador.Pontuacao} pontos (nível {jogador.Nivel}).");
    if (subiu)
        Console.WriteLine("*** Subiu de nível! ***");
}

static int LerInteiro(string texto)
{
    Console.Write(texto);
    if (!int.TryParse(Console.ReadLine(), out var valor))
        throw new ValidacaoException("Informe um número inteiro válido.");
    return valor;
}

static void ExibirTabela(List<Jogador> jogadores)
{
    if (jogadores.Count == 0)
    {
        Console.WriteLine("Nenhum jogador encontrado.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"{"Pos",-5}{"ID",-5}{"Nome",-25}{"Pontos",-9}{"Nível",-14}");
    for (var i = 0; i < jogadores.Count; i++)
    {
        var j = jogadores[i];
        Console.WriteLine($"{i + 1,-5}{j.Id,-5}{j.Nome,-25}{j.Pontuacao,-9}{j.Nivel,-14}");
    }
}
