using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NuclearQuest.Jogadores.Models;

public class Jogador
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "A pontuação deve estar entre 0 e 100000.")]
    public int Pontuacao { get; set; }

    public DateTime DataCadastro { get; set; }

    // Gamificação (o nível é calculado a partir da pontuação do jgador)
    
    [JsonIgnore]
    public NivelJogador Nivel => Pontuacao switch
    {
        >= 600 => NivelJogador.Mestre,
        >= 300 => NivelJogador.Especialista,
        >= 100 => NivelJogador.Pesquisador,
        _ => NivelJogador.Iniciante
    };
}
