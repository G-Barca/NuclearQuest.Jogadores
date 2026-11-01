using System.Text.Encodings.Web;
using System.Text.Json;
using NuclearQuest.Jogadores.Models;

namespace NuclearQuest.Jogadores.Repositories;

// JSON

public class JogadorRepository
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
    };

    private readonly string _caminho;

    public JogadorRepository(string caminho)
    {
        _caminho = caminho;
    }

    public List<Jogador> ObterTodos()
    {
        if (!File.Exists(_caminho))
            return new List<Jogador>();

        var json = File.ReadAllText(_caminho);
        if (string.IsNullOrWhiteSpace(json))
            return new List<Jogador>();

        return JsonSerializer.Deserialize<List<Jogador>>(json, Opcoes) ?? new List<Jogador>();
    }

    public void Salvar(List<Jogador> jogadores)
    {
        var pasta = Path.GetDirectoryName(_caminho);
        if (!string.IsNullOrEmpty(pasta))
            Directory.CreateDirectory(pasta);

        File.WriteAllText(_caminho, JsonSerializer.Serialize(jogadores, Opcoes));
    }
}
