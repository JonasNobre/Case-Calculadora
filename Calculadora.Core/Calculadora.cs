using System;
using System.Collections.Generic;

namespace Calculadora.Core
{
    public class Calculadora : ICalculadora
    {
        private readonly Dictionary<char, Func<decimal, decimal, decimal>> _operacoes;

        public Calculadora()
        {
            _operacoes = new Dictionary<char, Func<decimal, decimal, decimal>>
            {
                { '+', (a, b) => a + b },
                { '-', (a, b) => a - b },
                { '*', (a, b) => a * b },
                { '/', (a, b) => b == 0 ? throw new DivideByZeroException("Não é possível dividir por zero.") : a / b }
            };
        }

        public decimal Calcular(Operacao operacao)
        {
            if (_operacoes.TryGetValue(operacao.Operador, out var func))
            {
                // Usando um bloco checked para garantir que overflows matemáticos lancem exceções
                checked
                {
                    return func(operacao.ValorA, operacao.ValorB);
                }
            }

            throw new InvalidOperationException($"Operador '{operacao.Operador}' não é suportado.");
        }
    }
}
