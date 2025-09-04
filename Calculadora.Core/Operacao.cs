namespace Calculadora.Core
{
    public record Operacao(decimal ValorA, char Operador, decimal ValorB)
    {
        public override string ToString() => $"{ValorA} {Operador} {ValorB}";
    }

    // Record para encapsular o resultado de uma operação processada.
    public record ResultadoOperacao(Operacao Operacao, decimal Resultado, string MensagemErro = null);
}
