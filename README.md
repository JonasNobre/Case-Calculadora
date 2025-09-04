# Apresentação da Solução do Case "Calculadora"

**De:** Jonas Costa Nobre

---

## 1. Introdução

Apresento aqui a minha solução detalhada para o case "Calculadora". Meu trabalho partiu do código-fonte original fornecido, com o objetivo de não apenas corrigir os problemas funcionais apontados, mas também de evoluir a arquitetura da aplicação para um padrão profissional, robusto, extensível e alinhado com as boas práticas de engenharia de software.

Neste documento, vou detalhar meu processo de raciocínio, explicando como identifiquei e resolvi cada problema do código original e, mais importante, justificando as decisões de design que tomei para aprimorar a estrutura e a qualidade da solução final.

## 2. Correções Funcionais do Código Original

A primeira etapa do meu trabalho foi garantir que a aplicação se comportasse como o esperado, corrigindo os três problemas centrais presentes no código do case.

### Problema 1: Loop Infinito no Processamento da Fila

* **No código original:** O loop de processamento utilizava o método `filaOperacoes.Peek()`, que apenas "observa" o primeiro item da fila, sem removê-lo. Isso fazia com que a mesma operação fosse processada repetidamente, e como a fila nunca ficava vazia, o programa entrava em um loop infinito.
* **Minha Solução:** Substituí `Peek()` por `Dequeue()`. O método `Dequeue()` lê e, crucialmente, **remove** o item do início da fila, garantindo que o loop processe cada item uma única vez e termine corretamente.

### Problema 2: Ausência da Funcionalidade de Divisão

* **No código original:** A aplicação não possuía a lógica para realizar cálculos de divisão.
* **Minha Solução:** Implementei a funcionalidade de divisão. Para isso, adicionei a validação de divisão por zero, uma exceção matemática que quebraria o programa. A minha implementação agora lança uma exceção (`DivideByZeroException`), permitindo que a aplicação trate esse erro de forma controlada, além de impedir que o usuário faça uma divisão por zero na entrada.

### Problema 3: Cálculo Incorreto por Overflow de Inteiro

* **No código original:** A operação `2147483647 + 2` falhava porque o tipo `int` não suporta o resultado.
* **Minha Solução:** Migrei todos os tipos de dados numéricos para `decimal`. Escolhi `decimal` por seu alcance muito maior, que resolve o problema de overflow, e por sua alta precisão, que evita os erros de arredondamento comuns em `double` ou `float`, sendo o tipo ideal para cálculos exatos.

## 3. Evolução da Arquitetura e Implementação de Boas Práticas

Com a aplicação funcional, meu foco passou a ser a qualidade do código. Uma aplicação profissional não deve apenas funcionar, mas ser fácil de manter, testar e expandir. A seguir, detalho cada melhoria implementada.

### Melhoria 1: Estrutura de Projeto em Camadas

A aplicação foi dividida em três projetos: `Calculadora.Core` (lógica de negócio), `Calculadora.ConsoleApp` (interface do usuário) e `Calculadora.Tests` (testes).

* **Por que fiz essa melhoria?**
    * **Separação de Responsabilidades (SoC):** Essa estrutura separa claramente a lógica de negócio (como calcular) da lógica de apresentação (como exibir menus). Isso torna o código muito mais organizado e fácil de encontrar.
    * **Reutilização:** A lógica de negócio no projeto `Core` pode ser reutilizada por qualquer outra interface (uma API web, um aplicativo desktop, etc.) sem nenhuma alteração.

### Melhoria 2: Criação do `CalculationService` para Orquestração

Criei uma nova classe, `CalculationService`, para gerenciar o fluxo de processamento da fila.

* **Por que fiz essa melhoria?**
    * **Princípio da Responsabilidade Única (SRP):** No código original, a classe `Program` fazia tudo. Agora, sua única responsabilidade é interagir com o usuário. A lógica de "pegar um item da fila, mandar calcular, tratar o erro e guardar o resultado" foi movida para o `CalculationService`, deixando cada classe com um propósito único e claro.

### Melhoria 3: Uso de `records` Imutáveis para Representar Operações

Substituí a classe `Operacoes` por um `record` imutável, `Operacao`.

* **Por que fiz essa melhoria?**
    * **Previsibilidade e Segurança:** No código original, a classe `Operacoes` era mutável e podia misturar dados de entrada com o resultado. A minha solução com `records` garante que um objeto de operação, uma vez criado, não pode mais ser alterado. O método de cálculo agora recebe a operação e retorna um resultado, sem "efeitos colaterais". Isso evita uma classe inteira de bugs e torna o código mais seguro e fácil de entender.

### Melhoria 4: Padrão de Estratégia (Strategy Pattern) com `Dictionary`

Substituí a estrutura `switch` por um `Dictionary` que mapeia operadores a funções de cálculo.

* **Por que fiz essa melhoria?**
    * **Princípio Aberto/Fechado (O do SOLID):** O `switch` original precisaria ser modificado toda vez que uma nova operação fosse adicionada. Com o `Dictionary`, para adicionar uma nova funcionalidade, basta adicionar uma nova entrada no dicionário, sem tocar no código de cálculo que já existe e está testado. Isso torna o sistema muito mais extensível e fácil de manter.

### Melhoria 5: Implementação de Interfaces para Desacoplamento (`ICalculadora`)

Criei a interface `ICalculadora` e fiz a classe `Calculadora` implementá-la. O `CalculationService` agora depende da interface, não da classe concreta.

* **Por que fiz essa melhoria?**
    * **Princípio da Inversão de Dependência (D do SOLID):** Isso desacopla o serviço da implementação específica da calculadora. Amanhã, poderíamos ter uma `CalculadoraCientifica` que também implementa `ICalculadora`, e poderíamos trocá-la no sistema sem quebrar nada. Essa flexibilidade é crucial em sistemas maiores e facilita muito os testes (podemos "simular" uma calculadora nos testes).

### Melhoria 6: Garantia de Qualidade com Testes Unitários

Criei um projeto de testes dedicado para validar toda a lógica de negócio.

* **Por que fiz essa melhoria?**
    * **Confiabilidade:** Testes automatizados garantem que a lógica de cálculo funciona corretamente para diversos cenários (números positivos, negativos, zero, erros, etc.).
    * **Segurança para o Futuro:** Eles funcionam como uma "rede de segurança". Se alguém fizer uma alteração que quebre uma funcionalidade existente, os testes falharão imediatamente, prevenindo a introdução de bugs (regressões).

## 4. Conclusão

Minha abordagem para este case foi evoluir o código fornecido, transformando-o em uma solução completa e profissional. Cada decisão, desde a correção de um bug simples até a reestruturação da arquitetura, foi tomada com o objetivo de criar um software que não apenas funciona, mas que é robusto, de fácil manutenção e que exemplifica a aplicação de princípios sólidos de engenharia de software.