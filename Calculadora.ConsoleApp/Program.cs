using System;
using System.Collections.Generic;
using System.Linq;
using Calculadora.Core;

namespace Calculadora.ConsoleApp
{
    class Program
    {
        private static readonly ICalculadora _calculadora = new Core.Calculadora();
        private static readonly ICalculationService _calculationService = new CalculationService(_calculadora);
        
        private static readonly Queue<Operacao> _filaOperacoes = new Queue<Operacao>();
        private static readonly Stack<decimal> _pilhaResultados = new Stack<decimal>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear(); 
                
                Console.WriteLine("--- Calculadora Interativa ---");
                Console.WriteLine("1. Adicionar nova operação à fila");
                Console.WriteLine("2. Processar todas as operações da fila");
                Console.WriteLine("3. Ver operações na fila");
                Console.WriteLine("4. Sair");
                Console.Write("Escolha uma opção: ");

                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1": AdicionarOperacao(); break;
                    case "2": ProcessarFila(); break;
                    case "3": ImprimirFila("Operações Atualmente na Fila"); break;
                    case "4": Console.WriteLine("Encerrando o programa."); return;
                    default: 
                        Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey(); // Pausa para o usuário ver a mensagem de erro.
                        break;
                }
            }
        }

        static void AdicionarOperacao()
        {
            Console.Clear();
            Console.WriteLine("--- Adicionar Nova Operação ---");

            try
            {
                Console.Write("Digite o primeiro valor: ");
                decimal valorA = Convert.ToDecimal(Console.ReadLine());

                Console.Write("Digite o operador (+, -, *, /): ");
                char operador = Convert.ToChar(Console.ReadLine());
                
                if (!"+-*/".Contains(operador))
                {
                    Console.WriteLine("\nErro: Operador inválido. Use apenas +, -, *, /.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Digite o segundo valor: ");
                decimal valorB = Convert.ToDecimal(Console.ReadLine());
                
                //Validação de divisão por zero no momento da entrada.
                if (operador == '/' && valorB == 0)
                {
                    Console.WriteLine("\nErro: Divisão por zero não é permitida.");
                    Console.WriteLine("A operação não foi adicionada à fila.");
                }
                else
                {
                    _filaOperacoes.Enqueue(new Operacao(valorA, operador, valorB));
                    Console.WriteLine("\nOperação adicionada à fila com sucesso!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nErro: Valor em formato inválido. Tente novamente.");
            }
            
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void ProcessarFila()
        {
            Console.Clear();

            if (_filaOperacoes.Count == 0)
            {
                Console.WriteLine("A fila de operações está vazia.");
            }
            else
            {
                Console.WriteLine("--- Processando Fila de Operações ---");
                
                var resultadosProcessados = _calculationService.ProcessarFila(_filaOperacoes);

                foreach (var res in resultadosProcessados)
                {
                    if (res.MensagemErro != null)
                    {
                        Console.WriteLine($"Erro ao processar '{res.Operacao}': {res.MensagemErro}");
                    }
                    else
                    {
                        Console.WriteLine($"Cálculo: {res.Operacao} = {res.Resultado}");
                        _pilhaResultados.Push(res.Resultado);
                    }
                }
                
                Console.WriteLine("--------------------------------------");
                ImprimirPilha();
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void ImprimirFila(string titulo)
        {
            Console.Clear();
            Console.WriteLine($"--- {titulo} ---");

            if (_filaOperacoes.Count == 0) 
            {
                Console.WriteLine("Nenhuma operação na fila.");
            }
            else 
            {
                foreach (var op in _filaOperacoes) 
                {
                    Console.WriteLine($"- {op}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void ImprimirPilha()
        {
            Console.WriteLine("\n--- Pilha de Resultados (em ordem de cálculo) ---");
            if (_pilhaResultados.Count == 0)
            {
                Console.WriteLine("Nenhum resultado calculado com sucesso.");
            }
            else
            {
                foreach (var resultado in _pilhaResultados.Reverse())
                {
                    Console.WriteLine($"- {resultado}");
                }
            }
        }
    }
}

