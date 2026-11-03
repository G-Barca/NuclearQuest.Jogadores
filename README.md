# NuclearQuest.Jogadores

## Challenge FIAP - AMAZUL

## Integrantes do grupo

André Souza Matsuda RM 566733 | Guilherme Barca RM 568517 | Guilherme Oliveira RM 566842 | Kaliel Conceição RM 567587 | Paulo Henrique Muniz RM 567618 

## Sobre e como o .NET se aplica à solução do grupo

O projeto de java (https://github.com/G-Barca/NuclearQuest) é uma API que faz o CRUD de perguntas e registra respostas no banco de dados. Já o de .NET, complementa com um sistema de ranking e de cadastro de jogadores. O Java roda em H2 e o .NET em JSON. A integração está prevista para a Sprint 2, onde ambos devem usar o banco de dados Oracle.
Em resumo, o NuclearQuest é um quiz gamificado que apresenta, de forma simples e acessível, as aplicações e os benefícios da tecnologia nuclear.

## O que foi desenvolvido nesta Sprint

- Cadastro de jogadores, com validações
- Listagem do ranking e busca por nome
- Registro de pontos, com cálculo de nível (Iniciante, Pesquisador, Especialista, Mestre)
- Persistência em JSON

## Regras de negócio
 
Email único; sistema de niveis com pontuação; pontos devem ser maiores que zero e os erros são tratados com try/catch

## Como executar

- Instale o .NET 10 SDK
- Na pasta do projeto: 'dotnet run'
- Use o menu numérico (de 1 a 4, sendo 0 para sair).


## Arquitetura

Program.cs                      - menu e tratamento de erros
Services/JogadorService         - regras de negócio
Repositories/JogadorRepository  - leitura e gravação do JSON
Models/                         - entidade Jogador e níveis
Exceptions/                     - exceções de validação e de regra de negócio
data/jogadores.json             - dados de exemplo