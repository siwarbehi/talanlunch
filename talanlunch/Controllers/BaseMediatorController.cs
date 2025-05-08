/*using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TalanLunch.API.Controllers;

[ApiController]
public abstract class BaseMediatorController : ControllerBase
{
    private readonly IMediator _mediator;

    protected BaseMediatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> HandleMediatorRequest(ApiRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Function))
            return BadRequest("Invalid function name or request body.");

        try
        {
            Type? requestType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(request.Function, StringComparison.OrdinalIgnoreCase));

            if (requestType == null)
                return BadRequest($"Invalid request type: {request.Function}");

            object commandOrQuery = JsonSerializer.Deserialize(
                JsonSerializer.Serialize(request.Parameters),
                requestType,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? throw new ArgumentException("Invalid request parameters");

            var result = await _mediator.Send(commandOrQuery);
            return result == null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error processing request: {ex.Message}");
        }
    }
}*/