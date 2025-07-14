using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Skopia.Api.Middleware.Filters
{
    public class CustomSchemaNameFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            schema.Title = type.Name switch
            {
                "ProjectResponseDTO" => "Projeto (Resposta) - Contém as informações detalhadas de um projeto retornado pela API.",
                "ProjectRequestDTO" => "Projeto (Requisição) - Utilizado para envio de dados ao criar um novo projeto.",
                "ProjectBasicInfoResponseDTO" => "Projeto (Info Básica) - Exibe apenas os dados essenciais de um projeto.",
                "TaskRequestDTO" => "Tarefa (Requisição) - Representa os dados necessários para registrar uma nova tarefa.",
                "TaskResponseDTO" => "Tarefa (Resposta) - Contém os detalhes completos de uma tarefa recuperada pela API.",
                "TaskUpdateRequestDTO" => "Tarefa (Atualização) - Utilizado para atualizar parcialmente os dados de uma tarefa existente.",
                "UserInfoResponseDTO" => "Usuário (Informações) - Retorna dados básicos do usuário vinculado a projetos ou tarefas.",
                _ => schema.Title
            };
        }
    }
}