using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefaMinAPI.Tests.FakeData
{
    public static class FakeTarefas
    {
        public static List<Tarefa> TarefasFake() 
        {
            return new List<Tarefa>() 
            {
                new Tarefa() { IdTarefa = 1, Nome = "Entregar Trabalho de História", Descricao="Trabalho sobre a Segunda Guerra", DataAbertura=DateTime.Now, DataFechamento=DateTime.Parse("25/10/2023"), Status=TarefaEnum.Aberta },
                new Tarefa() { IdTarefa = 2, Nome = "Entregar Trabalho de Matematica", Descricao="Trabalho sobre Pitagoras", DataAbertura=DateTime.Parse("23/10/2023"), DataFechamento=DateTime.Parse("24/10/2023"), Status=TarefaEnum.Concluida },
                new Tarefa() { IdTarefa = 3, Nome = "Entregar Trabalho de Portugues", Descricao="Trabalho sobre Camoes", DataAbertura=DateTime.Parse("12/10/2023"), DataFechamento=DateTime.Parse("23/10/2023"), Status=TarefaEnum.Atrasada },
                new Tarefa() { IdTarefa = 4, Nome = "Entregar Trabalho de Filosofia", Descricao="Trabalho sobre a Vida de Platao", DataAbertura=DateTime.Parse("15/10/2023"), DataFechamento=DateTime.Parse("26/10/2023"), Status=TarefaEnum.Excluida },
                new Tarefa() { IdTarefa = 5, Nome = "Entregar Trabalho de Sociologia", Descricao="Trabalho sobre a Vida de Max Weber", DataAbertura=DateTime.Parse("15/09/2023"), DataFechamento=DateTime.Parse("26/09/2023"), Status=TarefaEnum.Atrasada },
                new Tarefa() { IdTarefa = 6, Nome = "Entregar Trabalho de Artes", Descricao="Trabalho sobre a vida de Michelangelo", DataAbertura=DateTime.Parse("23/10/2023"), DataFechamento=DateTime.Parse("28/10/2023"), Status=TarefaEnum.Concluida }
            };
        }

        public static List<Tarefa> TarefasFakeNull()
        {
            var tarefaNull = new List<Tarefa>();
            return tarefaNull;
        }
    }
}
