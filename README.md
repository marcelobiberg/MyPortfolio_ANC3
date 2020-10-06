Portfólio para desenvolvedores
===================================================

Este projeto foi desenvolvido como portfólio pessoal para profissionais de tecnologia onde é possível adicionar projetos e informações sobre o desenvolvedor. Este projeto conta com autenticação de usuário ( Identity ) e foi escrito com ASP.NET Core 3.1.

## Configurar o exemplo
* Alterar a connectionString em appsettings 
![image](https://user-images.githubusercontent.com/13973962/95159356-331be900-0774-11eb-98be-4a073ebc2e02.png)

* Abrir o CMD dentro da pasta do projeto e executar os comandos abaixo
```
dotnet restore
dotnet tool restore
dotnet ef database update -c ApplicationDbContext -p ../MyPortfolio/MyPortfolio.csproj -s Web.csproj
```

## Tecnologias
* ASP.NET Core 3.1.2
* EF Core 3.1.2
* Identity Core 3.1.2
* BuildBundlerMinifier 3.2.435
* SQL Server
* Bootstrap 3.2.435
* Jquery 3.3.1

## Acesso ao projeto
* [Portfólio link](http://marcelobiberg2-001-site1.ftempurl.com/)

## Reportar Bugs
Para relatar Bugs usar o sistemas de [Issues](https://github.com/marcelobiberg/MyPortfolio_ANC3/issues)

