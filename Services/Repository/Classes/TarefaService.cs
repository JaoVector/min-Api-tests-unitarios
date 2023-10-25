using Domain.Model;
using Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Classes
{
    public class TarefaService : ITarefaService
    {
        private readonly IRepository<Tarefa> _repository;

        public TarefaService(IRepository<Tarefa> repository)
        {
            _repository = repository;
        }

        public void AddTarefa(Tarefa tarefa)
        {
            _repository.Add(tarefa);
        }

        public void AtualizaStatus(Tarefa entiy)
        {
            _repository.UpdateTarefa(entiy);
        }

        public void AtualizaTarefa(Tarefa entiy)
        {
            _repository.UpdateTarefa(entiy);
        }

        public IQueryable<Tarefa> ConsultaTarefas(int skip, int take)
        {
            return _repository.GetAll(skip, take);
        }

        public IQueryable<Tarefa> ConsultaTarefasAbertas(int skip, int take)
        {
            return _repository.GetTarefasAbertas(skip, take);
        }

        public IQueryable<Tarefa> ConsultaTarefasConcluidas(int skip, int take)
        {
            return _repository.GetTarefasConcluidas(skip, take);
        }

        public IQueryable<Tarefa> ConsultaTarefasExcluidas(int skip, int take)
        {
            return _repository.GetTarefasExcluidas(skip, take);
        }

        public IQueryable<Tarefa> ConsultaTarefasAtrasadas(int skip, int take)
        {
            return _repository.GetTarefasAtrasadas(skip, take);
        }

        public void DeleteTarefa(Tarefa tarefa)
        {
            _repository.Delete(tarefa);
        }

        public async Task<Tarefa> GetTarefaPorId(Expression<Func<Tarefa, bool>> expression)
        {
            return await _repository.GetById(expression);
        }
    }
}
