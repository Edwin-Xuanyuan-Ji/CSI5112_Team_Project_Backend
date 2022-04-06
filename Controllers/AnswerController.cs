using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

//[Authorize]
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
    public async Task<List<Answer>> Get([FromQuery]string answer_id) =>
        await _AnswersService.GetAnswerByID(answer_id);

    [HttpGet("by_question")]
    public async Task<List<Answer>> Gets([FromQuery]string question_id) =>
        await _AnswersService.GetAnswerByQuestion(question_id);

    [HttpPost("create")]
    public async Task<List<Answer>> Post([FromBody] Answer newAnswer)
    {
        await _AnswersService.CreateAnswer(newAnswer);

        return await _AnswersService.GetAnswerByQuestion(newAnswer.question_id);
    }

    [HttpPut("update")]
    public async Task<List<Answer>> Update([FromQuery]string answer_id, Answer updatedAnswer)
    {
        var Answer = await _AnswersService.GetAnswerByID(answer_id);

        updatedAnswer.answer_id = Answer[0].answer_id;

        await _AnswersService.UpdateAnswer(answer_id, updatedAnswer);

        return await _AnswersService.GetAllAnswers();
    }

    [HttpDelete("delete")]
    public async Task<List<Answer>> Delete([FromBody] string[] ids)
    {
        List<Answer> answer = await _AnswersService.GetAnswerByID(ids[0]);
        
        await _AnswersService.RemoveAnswer(ids);

        return await _AnswersService.GetAnswerByQuestion(answer[0].question_id);
    }
}