using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly AnswersService _AnswersService;

    public AnswersController(AnswersService AnswersService) =>
        _AnswersService = AnswersService;

    [HttpGet("{id}")]
    public async Task<Answer> Get(string id) =>
        await _AnswersService.GetAnswerByID(id);

    [HttpGet("question/{id}")]
    public async Task<IActionResult> Gets(string product_id)
    {
        var answers = await _AnswersService.GetAnswerByQuestion(product_id);

        if (answers is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Answer newAnswer)
    {
        await _AnswersService.CreateAnswer(newAnswer);

        return CreatedAtAction(nameof(Get), new { id = newAnswer.answer_id }, newAnswer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Answer = await _AnswersService.GetAnswerByID(id);

        if (Answer is null)
        {
            return NotFound();
        }

        await _AnswersService.RemoveAnswer(id);

        return NoContent();
    }
}