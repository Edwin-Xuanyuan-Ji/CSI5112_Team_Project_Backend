using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

[Authorize]
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
    public async Task<List<Question>> Get([FromQuery] string product_id)
    {
        var questions = await _QuestionsService.GetQuestionsByProduct(product_id);

        return questions;
    }

    [HttpPost("create")]
    public async Task<List<Question>> Post(Question newQuestion)
    {
        await _QuestionsService.CreateQuestion(newQuestion);

        return await _QuestionsService.GetQuestionsByProduct(newQuestion.product_id);
    }

    [HttpDelete("delete")]
    public async Task<List<Question>> Delete([FromBody] string[] ids)
    {
        List<Question> question = await _QuestionsService.GetQuestionsByID(ids[0]);
        
        await _QuestionsService.RemoveQuestion(ids);

        return await _QuestionsService.GetQuestionsByProduct(question[0].product_id);
    }
}