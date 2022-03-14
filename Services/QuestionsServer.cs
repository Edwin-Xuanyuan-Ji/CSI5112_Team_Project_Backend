using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class QuestionsService
{
    private readonly IMongoCollection<Question> _questionsCollection;

    public QuestionsService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _questionsCollection = mongoDatabase.GetCollection<Question>(
            csi5112BackEndDataBaseSettings.Value.QuestionsCollectionName);
    }

    public async Task<List<Question>> GetAllQuestions() =>
        await _questionsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Question>> GetQuestionsByID(string id) =>
        await _questionsCollection.Find(x => x.question_id == id).ToListAsync();

    public async Task<List<Question>> GetQuestionsByProduct(string id) =>
        await _questionsCollection.Find(x => x.product_id == id).ToListAsync();

    public async Task CreateQuestion(Question newQuestion) =>
        await _questionsCollection.InsertOneAsync(newQuestion);

    public async Task UpdateQuestion(string id, Question updatedQuestion) =>
        await _questionsCollection.ReplaceOneAsync(x => x.question_id == id, updatedQuestion);

    public async Task RemoveQuestion(string[] id) =>
        await _questionsCollection.DeleteOneAsync(x => id.Contains(x.question_id));
}