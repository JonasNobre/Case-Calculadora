using System;
using System.Collections.Generic;

namespace Calculadora.Core
{
    public class CalculationService : ICalculationService
    {
        private readonly ICalculadora _calculadora;

        public CalculationService(ICalculadora calculadora)
        {
            _calculadora = calculadora;
        }

        public List<ResultadoOperacao> ProcessarFila(Queue<Operacao> filaOperacoes)
        {
            var resultados = new List<ResultadoOperacao>();

            while (filaOperacoes.Count > 0)
            {
                Operacao operacao = filaOperacoes.Dequeue();
                try
                {
                    decimal resultado = _calculadora.Calcular(operacao);
                    resultados.Add(new ResultadoOperacao(operacao, resultado));
                }
                // Capturando OverflowException também
                catch (Exception ex) when (ex is DivideByZeroException || ex is InvalidOperationException || ex is OverflowException)
                {
                    resultados.Add(new ResultadoOperacao(operacao, 0, ex.Message));
                }
            }
            return resultados;
        }
    }
}
