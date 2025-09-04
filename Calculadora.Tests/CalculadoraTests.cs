using System;
using Xunit;
using Calculadora.Core;

namespace Calculadora.Tests
{
    public class CalculadoraTests
    {
        private readonly ICalculadora _calculadora = new Core.Calculadora();

        [Theory]
        [InlineData(10, '+', 5, 15)]
        [InlineData(10, '-', 5, 5)]
        [InlineData(10, '*', 5, 50)]
        [InlineData(10, '/', 5, 2)]
        [InlineData(2147483647, '+', 2, 2147483649)]
        [InlineData(5, '/', 2, 2.5)]
        [InlineData(-10, '+', 5, -5)]
        [InlineData(-10, '-', -5, -5)]
        [InlineData(10, '*', -5, -50)]
        [InlineData(0, '/', 5, 0)]
        public void Calcular_OperacoesValidas_RetornaResultadoCorreto(decimal a, char op, decimal b, decimal esperado)
        {
            var operacao = new Operacao(a, op, b);
            var resultado = _calculadora.Calcular(operacao);
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Calcular_DivisaoPorZero_LancaDivideByZeroException()
        {
            var operacao = new Operacao(10, '/', 0);
            Assert.Throws<DivideByZeroException>(() => _calculadora.Calcular(operacao));
        }

        [Fact]
        public void Calcular_OperadorInvalido_LancaInvalidOperationException()
        {
            var operacao = new Operacao(10, '%', 5);
            Assert.Throws<InvalidOperationException>(() => _calculadora.Calcular(operacao));
        }

        [Fact]
        public void Calcular_MultiplicacaoGrande_LancaOverflowException()
        {
            // Arrange
            var operacao = new Operacao(decimal.MaxValue, '*', 2);

            // Act & Assert
            Assert.Throws<OverflowException>(() => _calculadora.Calcular(operacao));
        }
    }
}