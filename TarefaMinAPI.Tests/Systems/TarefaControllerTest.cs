using AutoMapper;
using Contracts.DTOS;
using Contracts.Profiles;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Moq;
using Services.Repository.Interfaces;
using System.Linq.Expressions;
using System.Web.Http.Results;
using TarefaMinAPI.Tests.FakeData;
using TarefasMinAPI.TarefaConfig;

namespace TarefaMinAPI.Tests.Systems
{
    public class TarefaControllerTest
    {
        private Mock<ITarefaService> _service;
        private List<Tarefa> _tarefas;
        private IMapper _mapper;

        public TarefaControllerTest()
        {

            var config = new MapperConfiguration(cfg =>
            { 
                cfg.AddProfile(new TarefaProfile()); 
            });

            _service = new Mock<ITarefaService>();
            _tarefas= FakeTarefas.TarefasFake();
            _mapper = config.CreateMapper();

            _service.Setup(x => x.GetTarefaPorId(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> predicate) =>
                {
                    return _tarefas.Single(predicate.Compile());
                });
        }

        [Fact]
        public void BuscaTarefas_Sucesso_Returns() 
        {
            _service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());

            var consulta = TarefaRoute.BuscaTarefas(_service.Object, _mapper);

            Assert.NotNull(consulta);
            Assert.IsAssignableFrom<IResult>(consulta);
        }

      
        [Fact]
        public void PostTarefas_Sucesso_Returns() 
        {
            _service.Setup(x => x.AddTarefa(It.IsAny<Tarefa>()))
                .Callback((Tarefa tarefa) => 
                {
                    if(!_tarefas.Any(k => k.IdTarefa == tarefa.IdTarefa)) _tarefas.Add(tarefa);
                });

            var tarefa = new CreateTarefaDTO() { Nome="Trabalho de SQL", Descricao="Trabalho sobre bancos relacionais", DataFechamento="25/10/2023" };

            TarefaRoute.PostTarefas(tarefa, _service.Object, _mapper);

            var tarefaConsulta = _tarefas.Any(x => x.Nome == tarefa.Nome);

            Assert.True(tarefaConsulta);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task BuscaTarefaPorId_Sucesso_Returns(int id) 
        {
            var consulta = await TarefaRoute.BuscaTarefaPorId(id, _service.Object, _mapper);

            Assert.NotNull(consulta);
        }

        [Fact]
        public void BuscaTarefasAbertas_Sucesso_Returns() 
        {
            _service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());

            var consulta = TarefaRoute.BuscaTarefasAbertas(_service.Object, _mapper);

            Assert.NotNull(consulta);
            Assert.IsAssignableFrom<IResult>(consulta);
        }

        [Fact]
        public void BuscaTarefasConcluidas_Sucesso_Returns()
        {
            _service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());

            var consulta = TarefaRoute.BuscaTarefasConcluidas(_service.Object, _mapper);

            Assert.NotNull(consulta);
            Assert.IsAssignableFrom<IResult>(consulta);
        }

        [Fact]
        public void BuscaTarefasExcluidas_Sucesso_Returns()
        {
            _service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());

            var consulta = TarefaRoute.BuscaTarefasExcluidas(_service.Object, _mapper);

            Assert.NotNull(consulta);
            Assert.IsAssignableFrom<IResult>(consulta);
        }

        [Fact]
        public void BuscaTarefasAtrasadas_Sucesso_Returns()
        {
            _service.Setup(x => x.ConsultaTarefas(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => _tarefas.Skip(skip).Take(take).AsQueryable());

            var consulta = TarefaRoute.BuscaTarefasAtrasadas(_service.Object, _mapper);

            Assert.NotNull(consulta);
            Assert.IsAssignableFrom<IResult>(consulta);
        }

        [Theory]
        [InlineData(2, "Excluida")]
        public async Task AtualizaStatusTarefa_Sucesso_ReturnsAsync(int id, string status) 
        {
            _service.Setup(x => x.AtualizaTarefa(It.IsAny<Tarefa>())).Callback((Tarefa tarefa) => 
            {
                var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == tarefa.IdTarefa);
                if (consulta != null)
                {
                    consulta.Status = tarefa.Status;
                }
            });

            await TarefaRoute.AtualizaStatusTarefa(_service.Object, _mapper, id, status);

            var tarefa = _tarefas.FirstOrDefault(x => x.IdTarefa == id);

            Assert.Equal(TarefaEnum.Excluida, tarefa.Status);
        }

        [Theory]
        [InlineData(4)]
        public async Task AtualizaTarefa_Sucesso_Returns(int id) 
        {
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

            var upTarefa = new UpdateTarefaDTO() { Nome = "Trabalho de Geografia", Descricao = "A Deriva Continental", DataFechamento = "31/10/2023", Status = "Aberta" };

            await TarefaRoute.AtualizaTarefa(_service.Object, _mapper, id, upTarefa);

            var atualizado = _tarefas.FirstOrDefault(x => x.IdTarefa == id);

            Assert.True(atualizado != null);
            Assert.Equal("Trabalho de Geografia", atualizado.Nome);
        }

        [Theory]
        [InlineData(5)]
        public async Task DeletaTarefa_Sucesso_Returns(int id) 
        {
            _service.Setup(x => x.DeleteTarefa(It.IsAny<Tarefa>()))
                .Callback((Tarefa tarefa) => 
                {
                    var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == tarefa.IdTarefa);
                    if (consulta != null) _tarefas.Remove(tarefa);
                });

            var consulta = _tarefas.FirstOrDefault(x => x.IdTarefa == id);

            await TarefaRoute.DeletaTarefa(id, _service.Object, _mapper);

            Assert.DoesNotContain(consulta, _tarefas);
        }
    }
}
