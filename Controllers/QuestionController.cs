using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly QuestionsService _QuestionsService;

    public QuestionsController(QuestionsService QuestionsService) =>
        _QuestionsService = QuestionsService;

    [HttpGet("all")]
    public async Task<List<Question>> Get() =>
        await _QuestionsService.GetAllQuestions();

    [HttpGet]
    public async Task<ActionResult<List<Question>>> Get([FromQuery] string product_id)
    {
        var questions = await _QuestionsService.GetQuestionsByProduct(product_id);

        if (questions is null)
        {
            return NotFound();
        }

        return questions;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post(Question newQuestion)
    {
        await _QuestionsService.CreateQuestion(newQuestion);

        return CreatedAtAction(nameof(Get), new { id = newQuestion.question_id }, newQuestion);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string[] ids)
    {
        await _QuestionsService.RemoveQuestion(ids);

        return NoContent();
    }
}