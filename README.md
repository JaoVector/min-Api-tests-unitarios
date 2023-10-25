# API Mínima com Arquitetura Cebola e Testes Unitários
## Descrição do Projeto
Foi feita uma API Mínima para gerenciar tarefas, utilizando a Arquitetura Cebola e Implementando Testes Unitários. Essa API fornece um CRUD simples como
Criar uma tarefa, consultar, editar e excluir. O intuito do projeto é apresentar uma forma diferente de se construir uma API Mínima
sem aplicar todas as configurações dentro do arquivo Program.cs, aproveitando para apresentar o conceito da Arquitetura Cebola.

#
### Rotas da API:
+ POST https://localhost:7069/api/AddTarefas
+ GET https://localhost:7069/api/BuscaTarefas?skip=0&take=5
+ GET https://localhost:7069/api/BuscaPorId/{id}
+ PUT https://localhost:7069/api/AtualizaTarefa/{id}
+ DELETE https://localhost:7069/api/DeletaTarefa/{id}

#
### Rotas de Consultas e Atualização Personalizada
+ GET https://localhost:7069/api/BuscaTarefasAbertas?skip=0&take=5
+ GET https://localhost:7069/api/BuscaTarefasConcluidas?skip=0&take=5
+ GET https://localhost:7069/api/BuscaTarefasExcluidas?skip=0&take=5
+ GET https://localhost:7069/api/BuscaTarefasAtrasadas?skip=0&take=5
+ PUT https://localhost:7069/api/AtualizaStatus/{id}?status={status}

#
### Modelo de Dados
A Entidade tarefa possuí os seguintes campos:
+ __IdTarefa__ É o identificador único para cada ação.
+ __Nome__ É o título da Tarefa.
+ __Descricao__ É o descritivo do que deve ser feito na tarefa.
+ __DataAbertura__ Define a Data em Que foi Aberta a Tarefa.
+ __DataFechamento__ Define a data limite para fechar a Tarefa.
+ __Status__ É uma propriedade do tipo Enum para definir em qual Status se encontra a tarefa.
### OBS: Se a DataFechamento for menor que a data de Abertura o Status será "Atrasada".
#
### Modelo de Status
```C#
public enum TarefaEnum
{
    Aberta,
    Concluida,
    Excluida,
    Atrasada
}
```
#
### Corpo Requisição POST
```json
{
    "nome": "string",
    "descricao": "string",
    "dataFechamento": "DD/MM/YYYY"
}
```
### Corpo Requisição PUT
```json
{
    "nome": "string",
    "descricao": "string",
    "dataFechamento": "DD/MM/YYYY",
    "status": "string"
}
```

#
## Testes Unitários
### Funções Fact Utilizadas
+ BuscaTarefas_Sucesso_Returns()
+ PostTarefas_Sucesso_Returns()
+ BuscaTarefasAbertas_Sucesso_Returns()
+ BuscaTarefasConcluidas_Sucesso_Returns()
+ BuscaTarefasExcluidas_Sucesso_Returns()
+ BuscaTarefasAtrasadas_Sucesso_Returns()
#
### Funções Theory Utilizadas
+ BuscaTarefaPorId_Sucesso_Returns(int id)
+ AtualizaTarefa_Sucesso_Returns(int id)
+ DeletaTarefa_Sucesso_Returns(int id)
+ AtualizaStatusTarefa_Sucesso_ReturnsAsync(int id, string status)
#
### Configuração do Objeto Mock
> [!NOTE]
> Foi utilizado o Framework XUnit para os testes Unitários, a biblioteca Moq para Mockar os objetos e também uma lista de dados Fake (_tarefas) para simular o banco de dados.
#
+ Configurando o Objeto ITarefaService para receber dois tipos inteiros e retornar uma lista de objetos do tipo ReadTarefaDTO.
```C#
_service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());
```
+ Configurando o objeto para receber um tipo Tarefa e verificar se não existe uma Tarefa de mesmo ID para efetuar a inserção.
```C#
_service.Setup(x => x.AddTarefa(It.IsAny<Tarefa>()))
                .Callback((Tarefa tarefa) => 
                {
                    if(!_tarefas.Any(k => k.IdTarefa == tarefa.IdTarefa)) _tarefas.Add(tarefa);
                });
```
+ Configurando o objeto para receber um tipo delegate e retornar o objeto do tipo Tarefa que foi encontrado pela expressão na lista _tarefas.
```C#
_service.Setup(x => x.GetTarefaPorId(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> predicate) =>
                {
                    return _tarefas.Single(predicate.Compile());
                });
```
+ Objeto Mock que vai receber um tipo Tarefa para efetuar a atualização do status.
```C#
 _service.Setup(x => x.AtualizaTarefa(It.IsAny<Tarefa>())).Callback((Tarefa tarefa) => 
            {
                var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == tarefa.IdTarefa);
                if (consulta != null)
                {
                    consulta.Status = tarefa.Status;
                }
            });
```
+ Objeto Mock que vai receber um tipo Tarefa para efetuar a atualização da Tarefa
```C#
_service.Setup(x => x.AtualizaTarefa(It.IsAny<Tarefa>()))
                .Callback((Tarefa tarefa) => 
                {
                    var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == tarefa.IdTarefa);
                    if (consulta != null)
                    {
                        consulta.Nome = tarefa.Nome;
                        consulta.Descricao = tarefa.Descricao;
                        consulta.DataAbertura = tarefa.DataAbertura;
                        consulta.DataFechamento = tarefa.DataFechamento;
                        consulta.Status = tarefa.Status;
                    }
                });
```
+ Objeto Mock que recebe um objeto do tipo Tarefa e verifica se ele existe na lista _tarefas, se existir a tarefa será removida.
```C#
_service.Setup(x => x.DeleteTarefa(It.IsAny<Tarefa>()))
                .Callback((Tarefa tarefa) => 
                {
                    var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == tarefa.IdTarefa);
                    if (consulta != null) _tarefas.Remove(tarefa);
                });
```
#
### Referências
https://www.macoratti.net/20/05/net_onion1.htm

https://code-maze.com/onion-architecture-in-aspnetcore/

https://learn.microsoft.com/pt-br/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio
