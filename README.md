# ToDo App - PoC de testes de integração com xUnit

Pequena API de ToDo List para PoC de setup de testes de integração utilizando xUnit

## Motivação

Para entender melhor como controlar o ciclo de criação, população e limpeza das tabelas do banco de dados nos testes de integração utilizando xUnit, criei essa PoC para testar alguns possíveis arranjos no Setup da aplicação e verificar qual a melhor maneira de fazer o ciclo de limpeza entre os testes e o Dispose dos recursos no final dos testes. Neste caso utilizei o recurso IClassFixture do xUnit

## Ferramentas e bibliotecas do projeto

- .Net 5
- xUnit
- AutoBogus
- EF Core 5
- Respawn

## Antes de rodar o projeto

Para executar esta aplicação é necessário ter um container com o banco de dados PostgreSQL rodando na máquina com as tabelas que serão utilizadas pela aplicação. Para preparar a infraestrura para rodar a aplicação e os testes siga as instruções a abaixo.

### Baixando e executando os containeres

Para subir essa infraestrutura há um docker-compose na raiz do projeto que baixa e roda o container. Para executá-lo, utilize o seguinte comando na raiz do projeto

```shell
docker-compose up -d
```

e ao final do execução teremos dois containeres em execução, o banco de dados e uma aplicação para gereneciamento do postgres

```
⠿ Network todoapp_postgres-network  Created
⠿ Container postgres-db             Started
⠿ Container pgadmin                 Started
```

### Executando as migrations

Antes de executar a aplicação é necessário executar as migrations para que as tabelas utilizadas na aplicação estejam prontas no PostgreSQL. Para isso é necessário ter a ferramenta do dotnet para executar comandos do EF Core. Caso já tenha a ferramenta instalada é só pular para o passo de execução das migrations. Para instalar a ferramenta utilize o seguinte comando:

```shell
dotnet tool install --global dotnet-ef
```

Com a ferramenta instalada, execute o seguinte comando na raiz do projeto para executar as migrations:

```shell
dotnet ef database update --project ./src/ToDoApp.csproj
```

### Executando os testes

Para executar os testes do projeto é importante que o container com o banco de dados esteja em execução. Utilize o seguinte comando para executar os testes

```shell
dotnet test
```
