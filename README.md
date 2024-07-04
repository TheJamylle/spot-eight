## **SpotEight *- SpotMusic do Grupo 8***

**Introdução**

O projeto SpotEight é uma aplicação de streaming de músicas com gerenciamento de playlists e features relacionadas. Foi desenvolvido utilizando o framework .NET.

**Funcionalidades**

O projeto oferece as seguintes funcionalidades:

-   Rede Social
-   Gerenciamento de Músicas e Playlists
-   Compartilhamento entre Redes Sociais
-   E muito mais

**Tecnologias Utilizadas**

-   **Linguagem de Programação:** C#
-   **Framework:** .NET 8.0
-   **Base de Dados:** PostgreSQL
-   **Mensageria**: AWS SQS

**Requisitos de Sistema**

-   **Sistema operacional:** Windows 10 ou superior
-   **Versão do .NET:** .NET 6.0 ou superior

**Instalação**

1.  Clonar o repositório:
    
    Bash
    
    ```
    git clone https://github.com/TheJamylle/spot-eight-backend.git
    
    ```
    
    
2.  Restaurar as dependências:
    
    Bash
    
    ```
    cd spot-eight-backend
    dotnet restore
    
    ```
    
    
3.  Executar o projeto:
    
    Bash
    
    ```
    dotnet run
    
    ```
    

**Branches e Git Flow**

Este projeto utiliza o Git Flow para gerenciar o fluxo de trabalho de desenvolvimento. As branches principais são:

-   **master:** Branch principal do projeto, contendo a versão mais estável e pronta para produção.
-   **staging:** Branch utilizada para validação das alterações antes de serem mescladas na branch master.
-   **develop:** Branch de desenvolvimento principal, onde as novas funcionalidades são implementadas.

**Branches de Tarefa**

Para organizar as tarefas de desenvolvimento, utilizamos branches de tarefa com o seguinte padrão de nomeação:

-   `feat/<nome-da-funcionalidade>`: Utilizada para implementar novas funcionalidades.
-   `fix/<nome-do-bug>`: Utilizada para corrigir bugs.
-   `hotfix/<nome-do-problema>`: Utilizada para corrigir problemas urgentes que afetam a produção.
-   `chore/<nome-da-tarefa>`: Utilizada para tarefas de manutenção do código, como refatoração, documentação, etc.

**Fluxo de Trabalho**

O fluxo de trabalho geral segue estas etapas:

1.  **Criar uma branch de tarefa:** Crie uma branch de tarefa com o nome apropriado para sua tarefa.
2.  **Implementar a tarefa:** Implemente sua tarefa na branch de tarefa.
3.  **Testar a tarefa:** Teste sua tarefa completamente para garantir que funcione corretamente.
4.  **Criar um pull request:** Crie um pull request para mesclar sua branch de tarefa na branch develop.
5.  **Revisar o pull request:** Revise o pull request para garantir que o código esteja limpo, bem documentado e atenda aos padrões do projeto.
6.  **Mesclar o pull request:** Se o pull request for aprovado, mescle-o na branch develop.
7.  **Criar um pull request para staging:** Crie um pull request para mesclar a branch develop na branch staging.
8.  **Testar em staging:** Teste o código na branch staging para garantir que funcione corretamente no ambiente de staging.
9.  **Mesclar o pull request para staging:** Se o pull request for aprovado, mescle-o na branch staging.
10.  **Criar um pull request para master:** Crie um pull request para mesclar a branch staging na branch master.
11.  **Revisar e mesclar o pull request para master:** Revise o pull request para master e, se aprovado, mescle-o na branch master.
12.  **Implantar em produção:** Implemente a branch master em produção.
