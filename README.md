# github-featured-api
Construir uma nova API, utilizando a plataforma .NET, que deverá conectar na API do GitHub e disponibilizar as seguintes funcionalidades:
* a) Buscar e armazenar os repositórios destaques de 5 linguagens à sua escolha;
* b) Listar os repositórios encontrados;
* c) Visualizar os detalhes de cada repositório.

--- 

## Requisitos:
* Deve ser uma aplicação totalmente nova;
* Apenas será permitido o uso de bibliotecas open source no desenvolvimento do projeto;
* A solução deve estar em um repositório público do GitHub;
* Utilizar uma tecnologia de banco de dados à sua escolha para armazenar as informações coletadas;
* Utilizar boas práticas de desenvolvimento e versionamento de código;
* Documentar a API com Swagger;

---

## Instruções

### Utilizar a API pelo Docker Compose
Acesse o arquivo docker-compose.yml na pasta raiz e altere as variáveis de ambiente da API:
1. **ASPNETCORE_ENVIRONMENT**: Ambiente que será carregado na API. Exemplos: Development; Production.
2. **ConnectionStrings__AppDb**: Connection string do SQL Server.
3. **TrackedLanguages:0**, **TrackedLanguages:1**, etc.: São as linguagens dos repositórios que serão buscados no GitHub.
4. **GitHub__ApiKey**: Token de acesso a GitHub API.

Para iniciar a aplicação, execute o seguinte comando na pasta raiz do projeto:
```docker-compose up --build```

Quando o processo de build terminar, a API estará disponível na URL http://127.0.0.1:8080/.

Caso a variável ASPNETCORE_ENVIRONMENT esteja configurada como "Development", basta acessar http://127.0.0.1:8080/swagger/index.html para acessar o Swagger UI da API.

### Utilizar a API por Comando, Visual Studios, etc.
Acesse o arquivo "src\GitHubFeatured.API\appsettings.json" e adicione as variáveis de ambiente. Você pode utilizar o código abaixo ou o "appsettings.Development.json" como exemplo.
```
  "ConnectionStrings": {
    "AppDb": "Server=localhost,1433;Database=GitHub;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "TrackedLanguages": [ "C#", "PHP", "C++", "Dart", "JavaScript" ],
  "GitHub": {
    "ApiKey": "<your github api key>"
  }
```
1. **ConnectionStrings.AppDb**: Connection string do SQL Server.
2. **TrackedLanguages**: São as linguagens dos repositórios que serão buscados no GitHub.
3. **GitHub.ApiKey**: Token de acesso a GitHub API.

Após realizar as alterações, acesse a pasta "src\GitHubFeatured.API" e execute os seguintes comandos:
1. ```dotnet restore```
2. ```dotnet run```
Por fim, procure por "Now listening on" no console onde o comando foi executado para descobrir a URL e porta em que a API está sendo executada.

Caso a variável ASPNETCORE_ENVIRONMENT esteja configurada como "Development", basta acessar a URL da API + /swagger/index.html para acessar o Swagger UI da API.
Exemplo: http://127.0.0.1:8080/swagger/index.html

---

## Swagger
O Swagger da API pode ser encontrado no arquivo "swagger.json", mas também no Swagger UI da API caso a variável ASPNETCORE_ENVIRONMENT esteja configurada como "Development".