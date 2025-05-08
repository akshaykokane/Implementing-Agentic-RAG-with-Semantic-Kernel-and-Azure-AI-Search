using AgenticRAG.Rag.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.Services;

namespace AgenticRAG.Controllers;

[ApiController]
[Route("rag/api/")]
public class RagController : ControllerBase
{
    private readonly ILogger<RagController> _logger;

    private readonly RagService _ragService;

    public RagController(ILogger<RagController> logger, RagService ragService)
    {
        _logger = logger;
        _ragService = ragService;
    }

    [HttpPost("/traditionalRag")]
    public async Task<Response> TraditionalRagAsync(Request request)
    {
        var result = await _ragService.GenerateResponseUsingTraditionalRag(request.prompt);

        Response response = new Response()
        {
            Content = result
        };

        return response; 
    }

    [HttpPost("/agenticRag")]
    public async Task<Response> AgenticRagAsync(Request request)
    {
        var result = await _ragService.GenerateResponseUsingAgenticRag(request.prompt);

        Response response = new Response()
        {
            Content = result
        };

        return response;
    }
}

