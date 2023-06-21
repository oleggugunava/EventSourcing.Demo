using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using EventSourcing.Demo.Models;
using System.Net;
using System.Text.Json;

namespace EventSourcing.Demo.Repositories;

public interface ILedgerProjectionRepository
{
    Task<bool> CreateAsync(LedgerProjection ledger);

    Task<LedgerProjection?> GetAsync(string id);

    Task<bool> UpdateAsync(LedgerProjection ledger);

    Task<bool> DeleteAsync(string id);
}

public class LedgerProjectionRepository : ILedgerProjectionRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName = "ledger-projection";

    public LedgerProjectionRepository(IAmazonDynamoDB dynamoDB)
    {
        _dynamoDb = dynamoDB;
    }
    public async Task<bool> CreateAsync(LedgerProjection ledger)
    {
        ledger.UpdatedAt = DateTime.UtcNow;
        var ledgerAsJson = JsonSerializer.Serialize(ledger);
        var ledgerAsAttributes = Document.FromJson(ledgerAsJson).ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = ledgerAsAttributes,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }


    public async Task<LedgerProjection?> GetAsync(string id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id } },
                { "sk", new AttributeValue { S = id } },
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<LedgerProjection>(itemAsDocument.ToJson());
    }

    public async Task<bool> UpdateAsync(LedgerProjection ledger)
    {
        ledger.UpdatedAt = DateTime.UtcNow;
        var ledgerAsJson = JsonSerializer.Serialize(ledger);
        var ledgerAsAttributes = Document.FromJson(ledgerAsJson).ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = ledgerAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

}
