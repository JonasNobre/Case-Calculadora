using System.Collections.Generic;

namespace Calculadora.Core
{
    public interface ICalculationService
    {
        List<ResultadoOperacao> ProcessarFila(Queue<Operacao> filaOperacoes);
    }
}
