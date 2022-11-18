# Marvel Characters

##Como iniciar o projeto:
- Mudar a connectionstring para apontar para o seu banco;
- Rodar o comando de migration "Update-Database";


##Proposta do projeto:
- Utilizar a API da Marvel: https://developer.marvel.com;
- povoar um banco de dados (SQL Server, MongoDB, MySql, etc).
- Buscar de personagens através de endpoint(Possibilidade de filtros, Trazer a lista ordenada: Favoritos, Nome);
- Disponibilizar um endpoint para buscar o detalhamento do personagem;
- Favoritar personagens (no máximo 05);
- Desfavotirar personagens;
- Utilizar interface e injeção de dependência;
##Resolução do problema:
- Criado um endpoint para sincronizar os dados do banco de dados com a api, caso o banco esteja vazio;
- Como a api só permite buscar de 100 em 100 personagens foi implementado um recurso com while até percorrer todos os dados da API e cadastrar no nosso banco de dados;
- Criado um filtro com paginação para o endpoint de listar personagens;
- Criado um endpoint para trazer todos os dados do personagem pelo id dele no banco de dados;
- Criado um endpoint para favoritar personagem pelo seu id no banco de dados, se houver já 5 personagens favoritados, ele não deixaria favoritar mais;
- Criado um endpoint para desfavoritar o personagem pelo id.
- Foram criados no projeto 6 camadas no projeto (API, Application, Domain, Infra.Data, Infra.IoC e Tests) cada uma com sua responsabilidade;
