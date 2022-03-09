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

    [HttpGet("all")]
    public async Task<List<Answer>> Get() =>
        await _AnswersService.GetAllAnswers();
    
    [HttpGet]
    public async Task<Answer> Get([FromQuery]string answer_id) =>
        await _AnswersService.GetAnswerByID(answer_id);

    [HttpGet("by_question")]
    public async Task<IActionResult> Gets([FromQuery]string question_id)
    {
        var answers = await _AnswersService.GetAnswerByQuestion(question_id);

        if (answers is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromBody] Answer newAnswer)
    {
        await _AnswersService.CreateAnswer(newAnswer);

        return CreatedAtAction(nameof(Get), new { id = newAnswer.answer_id }, newAnswer);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery]string answer_id, Answer updatedAnswer)
    {
        var Answer = await _AnswersService.GetAnswerByID(answer_id);

        if (Answer is null)
        {
            return NotFound();
        }

        updatedAnswer.answer_id = Answer.answer_id;

        await _AnswersService.UpdateAnswer(answer_id, updatedAnswer);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string[] id)
    {
        await _AnswersService.RemoveAnswer(id);

        return NoContent();
    }
}