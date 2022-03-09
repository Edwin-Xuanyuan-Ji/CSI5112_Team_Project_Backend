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

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Question>>> Get(string product_id)
    {
        var questions = await _QuestionsService.GetQuestionsByProduct(product_id);

        if (questions is null)
        {
            return NotFound();
        }

        return questions;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Question newQuestion)
    {
        await _QuestionsService.CreateQuestion(newQuestion);

        return CreatedAtAction(nameof(Get), new { id = newQuestion.question_id }, newQuestion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Question = await _QuestionsService.GetQuestionsByID(id);

        if (Question is null)
        {
            return NotFound();
        }

        await _QuestionsService.RemoveQuestion(id);

        return NoContent();
    }
}