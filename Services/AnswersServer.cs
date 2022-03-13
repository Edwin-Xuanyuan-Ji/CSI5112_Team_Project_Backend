using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class AnswersService
{
    private readonly IMongoCollection<Answer> _answersCollection;

    public AnswersService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _answersCollection = mongoDatabase.GetCollection<Answer>(
            csi5112BackEndDataBaseSettings.Value.AnswersCollectionName);
    }

    public async Task<List<Answer>> GetAllAnswers() =>
        await _answersCollection.Find(_ => true).ToListAsync();

    
    public async Task<List<Answer>> GetAnswerByID(string id) =>
        await _answersCollection.Find(x => x.answer_id == id).ToListAsync();

    public async Task<List<Answer>> GetAnswerByQuestion(string id) =>
        await _answersCollection.Find(x => x.question_id == id).ToListAsync();

    public async Task CreateAnswer(Answer newAnswer) =>
        await _answersCollection.InsertOneAsync(newAnswer);

    public async Task UpdateAnswer(string id, Answer updatedAnswer) =>
        await _answersCollection.ReplaceOneAsync(x => x.answer_id == id, updatedAnswer);

    public async Task RemoveAnswer(string[] id) =>
        await _answersCollection.DeleteManyAsync(x => id.Contains(x.answer_id));
}