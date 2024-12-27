SoftManager

SoftManager é um sistema completo de gestão e tarefas.

Visão Geral
SoftManager é uma aplicação web desenvolvida usando .NET Core, com uma arquitetura baseada em camadas para facilitar a manutenção, escalabilidade e extensibilidade. Ele inclui funcionalidades como agendamento de tarefas, acompanhamento e controle.

Funcionalidades Principais
Gestão de tarefas: Gerenciamento de tarefas por usuário.
Autenticação Segura: Utilize o sistema de Identity para autenticação de usuários com suporte a multi-tenant.
Personalização: Permite personalizar segmentos de produtos de acordo com as necessidades dos usuários.
Envio de E-mails: Integração com serviços de e-mail para notificações e autenticação.
Tecnologias Utilizadas
.NET Core: Framework usado para desenvolver a aplicação backend.
ASP.NET Core: Para a criação de APIs e interface web.
Entity Framework Core: Para acesso a dados e mapeamento de entidades.
Tailwind CSS: Framework CSS para estilização e responsividade.
sqlServer: Banco de dados utilizado para persistência de dados.
Identity: Gerenciamento de autenticação e autorização de usuários.
Estrutura do Projeto
Presentation: Contém as camadas responsivas pela interface do usuário (Views, Razor Pages).
Application: Implementa os serviços e regras de negócios.
Domain: Contém as entidades e modelos principais do sistema.
Infrastructure: Gerencia a persistência usando Entity Framework Core e sqlServer.
Instalação
Requisitos
Visual Studio 2022 ou superior
Node.js e npm para desenvolvimento frontend
SqlServer para banco de dados

Configure o banco de dados conforme especificado no appsettings.json:

json
Copy code
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SoftManagerDb;User Id=seu_usuario;Password=sua_senha;"
}
Execute o projeto:

Funcionalidades do Sistema
Autenticação e Registro
A aplicação utiliza autenticação segura através do Identity, permitindo login e gerenciamento de usuários.
Gerenciamento de Tarefas
Relatórios e Estatísticas

Contribuições
Contribuições para o projeto são bem-vindas! Sinta-se à vontade para abrir Pull Requests, reportar bugs ou sugerir melhorias.