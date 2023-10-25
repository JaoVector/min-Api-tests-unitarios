using Domain.Model;
using System.Linq.Expressions;

namespace Services.Repository.Interfaces
{
    public interface ITarefaService
    {
        void AddTarefa(Tarefa tarefa);
        Task<Tarefa> GetTarefaPorId(Expression<Func<Tarefa, bool>> expression);
        void DeleteTarefa(Tarefa tarefa);
        IQueryable<Tarefa> ConsultaTarefas(int skip, int take);
        IQueryable<Tarefa> ConsultaTarefasAbertas(int skip, int take);
        IQueryable<Tarefa> ConsultaTarefasConcluidas(int skip, int take);
        IQueryable<Tarefa> ConsultaTarefasExcluidas(int skip, int take);
        IQueryable<Tarefa> ConsultaTarefasAtrasadas(int skip, int take);
        void AtualizaTarefa(Tarefa entiy);
    }
}
