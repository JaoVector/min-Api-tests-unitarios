using AutoMapper;
using Contracts.DTOS;
using Services.Repository.Interfaces;
using Domain.Model;
using Microsoft.IdentityModel.Tokens;

namespace TarefasMinAPI.TarefaConfig
{
    public static class TarefaRoute
    {
        public static void TarefasRoutes(this WebApplication app) 
        {
            app.MapPost("api/AddTarefas", PostTarefas);
            app.MapGet("api/BuscaPorId/{id}", BuscaTarefaPorId);
            app.MapGet("api/BuscaTarefas", BuscaTarefas);
            app.MapGet("api/BuscaTarefasAbertas", BuscaTarefasAbertas);
            app.MapGet("api/BuscaTarefasConcluidas", BuscaTarefasConcluidas);
            app.MapGet("api/BuscaTarefasExcluidas", BuscaTarefasExcluidas);
            app.MapGet("api/BuscaTarefasAtrasadas", BuscaTarefasAtrasadas);
            app.MapPut("api/AtualizaStatus/{id}", AtualizaStatusTarefa);
            app.MapPut("api/AtualizaTarefa/{id}", AtualizaTarefa);
            app.MapDelete("api/DeletaTarefa/{id}", DeletaTarefa);

        }

        public static IResult PostTarefas(CreateTarefaDTO tarefaDto, ITarefaService service, IMapper mapper) 
        {
            try
            {
                Tarefa tarefa = mapper.Map<Tarefa>(tarefaDto);
                service.AddTarefa(tarefa);
                return Results.Created($"api/BuscaPorId/{tarefa.IdTarefa}", tarefa);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> BuscaTarefaPorId(int id, ITarefaService service, IMapper mapper) 
        {
            try
            {
                var consulta = await service.GetTarefaPorId(i => i.IdTarefa == id);
                ReadTarefaDTO tarefa = mapper.Map<ReadTarefaDTO>(consulta);
                return tarefa is null ? Results.NotFound($"Não encontrada tarefa com {id}") : Results.Ok(tarefa);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static IResult BuscaTarefas(ITarefaService service, IMapper mapper, int skip = 0, int take = 5) 
        {
            try
            {
                var consulta = service.ConsultaTarefas(skip, take);
                var tarefas = mapper.Map<List<ReadTarefaDTO>>(consulta);
                return tarefas.IsNullOrEmpty() ? Results.NotFound($"Tarefas Não encontradas") : Results.Ok(tarefas);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static IResult BuscaTarefasAbertas(ITarefaService service, IMapper mapper, int skip = 0, int take = 5)
        {
            try
            {
                var consulta = service.ConsultaTarefasAbertas(skip, take);
                var tarefas = mapper.Map<List<ReadTarefaDTO>>(consulta);
                return tarefas.IsNullOrEmpty() ? Results.NotFound($"Não Existem Tarefas com Status Aberta") : Results.Ok(consulta);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static IResult BuscaTarefasConcluidas(ITarefaService service, IMapper mapper, int skip = 0, int take = 5)
        {
            try
            {
                var consulta = service.ConsultaTarefasConcluidas(skip, take);
                var tarefas = mapper.Map<List<ReadTarefaDTO>>(consulta);
                return tarefas.IsNullOrEmpty() ? Results.NotFound($"Não Existem Tarefas com Status Concluida") : Results.Ok(tarefas);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static IResult BuscaTarefasExcluidas(ITarefaService service, IMapper mapper, int skip = 0, int take = 5)
        {
            try
            {
                var consulta = service.ConsultaTarefasExcluidas(skip, take);
                var tarefas = mapper.Map<List<ReadTarefaDTO>>(consulta);
                return tarefas.IsNullOrEmpty() ? Results.NotFound($"Não Existem Tarefas com Status Excluida") : Results.Ok(tarefas);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static IResult BuscaTarefasAtrasadas(ITarefaService service, IMapper mapper, int skip = 0, int take = 5)
        {
            try
            {
                var consulta = service.ConsultaTarefasAtrasadas(skip, take);
                var tarefas = mapper.Map<List<ReadTarefaDTO>>(consulta);
                return tarefas.IsNullOrEmpty() ? Results.NotFound($"Não Existem Tarefas com Status Atrasada") : Results.Ok(tarefas);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> AtualizaStatusTarefa(ITarefaService service, IMapper mapper, int id, string status) 
        {
            try
            {
                var consulta = await service.GetTarefaPorId(i => i.IdTarefa == id);

                if (consulta == null) return Results.NotFound($"Tarefa de Id: {id} não encontrada");

                var nstatus = VerificaStatus(status);
                consulta.Status = nstatus;
               
                service.AtualizaTarefa(consulta);
                return Results.Ok(consulta);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> AtualizaTarefa(ITarefaService service, IMapper mapper, int id, UpdateTarefaDTO tarefaDTO)
        {
            try
            {
                var consulta = await service.GetTarefaPorId(i => i.IdTarefa == id);

                if (consulta == null) return Results.NotFound($"Tarefa de Id: {id} não encontrada");


                var status = VerificaStatus(tarefaDTO.Status);

                consulta.Nome = tarefaDTO.Nome;
                consulta.Descricao = tarefaDTO.Descricao;
                consulta.DataFechamento = DateTime.Parse(tarefaDTO.DataFechamento);
                consulta.Status = status;
                

                service.AtualizaTarefa(consulta);
                return Results.Ok(consulta);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> DeletaTarefa(int id, ITarefaService service, IMapper mapper) 
        {
            var consulta = await service.GetTarefaPorId(i => i.IdTarefa == id);

            if (consulta == null) return Results.NotFound($"Tarefa de Id: {id} não encontrada");

            service.DeleteTarefa(consulta);

            return Results.NoContent();
        }

        private static TarefaEnum VerificaStatus(string status) 
        {
            switch (status.ToLower())
            {
                case "aberta":
                    return TarefaEnum.Aberta;
                case "concluida":
                    return TarefaEnum.Concluida;
                case "excluida":
                    return TarefaEnum.Excluida;
                case "atrasada":
                    return TarefaEnum.Atrasada;
                default:
                    return TarefaEnum.Aberta;
            }
        }

    }
}
